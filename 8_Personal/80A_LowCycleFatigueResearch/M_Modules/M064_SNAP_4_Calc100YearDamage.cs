using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistory;
using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.PyPlotWrapper;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M064_SNAP_4_Calc100YearDamage(
        LINEController line,
        DirectoryInfo collectedResultsBaseDir,
        string targetEdgeName,
        string namingStrategy = "{wave}_D{DValue}_{edgeName}" // "{wave}_D{DValue}_{edgeName}"
        )
    {
        // init
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★★★★★★★★★★★★★★★ SNAP_4_PyCalc100YearDamage 開始！！！！", ConsoleColor.Blue);
        _ = line.SendMessageAsync(@$"★ SNAP_4_PyCalc100YearDamage 開始！！！！");
        PythonController.Initialize();



        // defs
        var np = PythonController.Import("numpy");

        var resultCsv = collectedResultsBaseDir.GetChildDirectoryInfo("ResultCsv");
        var metaFile = collectedResultsBaseDir.GetChildFileInfo("# PGV対応表.csv");
        var resultImage = collectedResultsBaseDir.GetChildDirectoryInfo("ResultImage").GetChildDirectoryInfo("100YearAnalysisImage").CreateAndPipe();

        var specificBeamMuAmpHistFile = resultCsv.GetChildFileInfo("# CollectedSpecificBeamMuAmpHist.csv");
        var df = FromCsv(specificBeamMuAmpHistFile);
        df.RenameColumn("Unnamed: 0", "MuAmpRange");


        var meta = FromCsv(metaFile);
        var Ds = meta["D"];
        var PGVs = meta["PGV"];
        var EPs_EQ_30y = meta["EPs_EQ_30y"]; // 超過確率は引くほうから順に並べる！！
        var NOs_EQ_100y_Overwrite = meta["NOs_EQ_100y"];
        var DIVISION_30y_to_1d = 10960; // 30years = 10960 days
        var DIVISION_100y_to_30y = 100 / 30d; // 30years = 10960 days

        var _100PercentCount = 0; // 100%なものは，後ろからこの量。
        for (int i = EPs_EQ_30y.Length - 1; i >= 0; i--)
        {
            if (EPs_EQ_30y[i] < 1)
                break;
            _100PercentCount++;
        }


        // ★★★★★ 関数定義たち

        string GetTargetIndex(int DValue, string edgeName) => namingStrategy.Replace("{DValue}", $"{DValue}").Replace("{edgeName}", $"{edgeName}");

        double[] GetDevidedEP(double[] EPs_EQ_original, int div)
        {
            return EPs_EQ_original.Select(EP_EQ_original => 1 - Math.Pow((1 - EP_EQ_original), (1d / div))).ToArray();
        }

        double[] GetExceedNumberOfOccurrences(double[] EPs_EQ_original, int div, double period_multiplier = 1)//  基本的には使わない。グラフのため。
        {
            var EPs_EQ_div = GetDevidedEP(EPs_EQ_original, div);
            var NOs_EQ_original = EPs_EQ_div.Select(PD_EQ_div => div * PD_EQ_div).ToArray();
            var NOs_EQ_period_multiplied = NOs_EQ_original.Select(NO => NO * period_multiplier).ToArray();

            return NOs_EQ_period_multiplied;
        }

        double[] GetNumberOfOccurrences(double[] EPs_EQ_original, int div, double period_multiplier = 1)
        {
            var EPs_EQ_div = GetDevidedEP(EPs_EQ_original, div);
            var PDs_EQ_div = (double[])EPs_EQ_div.Clone();

            for (int i = 1; i < EPs_EQ_div.Length; i++)
                PDs_EQ_div[i] = EPs_EQ_div[i] - EPs_EQ_div[i - 1];

            var NOs_EQ_original = PDs_EQ_div.Select(PD_EQ_div => div * PD_EQ_div).ToArray();
            var NOs_EQ_period_multiplied = NOs_EQ_original.Select(NO => NO * period_multiplier).ToArray();

            for (int i = 0; i < PGVs.Length; i++)
            {
                // 100% eq
                if (EPs_EQ_30y[i] == 1.0)
                    NOs_EQ_period_multiplied[i] = NOs_EQ_100y_Overwrite[i];
            }

            return NOs_EQ_period_multiplied;
        }

        double GetRequiredCyclesToCollapse(double halfMuLength, double C, double beta)
        {
            return Math.Pow(halfMuLength / C, -1 / beta);
        }

        double GetDamageOfOneCycle(double halfMuLength, double C = 4, double beta = 1 / 3d)
        {
            if (halfMuLength == 0)
                return 0;
            return 1 / GetRequiredCyclesToCollapse(halfMuLength, C, beta);
        }


        // ★★★★★ 損傷計算！！

        // 共通
        var EPs_1d = GetDevidedEP(EPs_EQ_30y, DIVISION_30y_to_1d);
        var NOs_EQ_100y = GetNumberOfOccurrences(EPs_EQ_30y, DIVISION_30y_to_1d, period_multiplier: DIVISION_100y_to_30y);
        var Mus = df["MuAmpRange"].Select(x => x / 2).ToArray(); // 全振幅 → 片振幅

        var NOs_Cycles_100y = EnumerableExtension.Range_WithCount(0, 0, df.DataRowCount).ToArray();
        var Damage_100y = EnumerableExtension.Range_WithCount(0, 0, df.DataRowCount).ToArray()[..^1];
        var df_Damage_EQ_100y_PGV = new TimeHistory();
        var df_Damage_Cycles_100y_PGV = new TimeHistory();


        for (int i = 0; i < meta.DataRowCount; i++)
        {
            // def
            var PGV = PGVs[i].ToString();
            var _NOs_EQ_100y = 0d;


            // ignore 100% eq
            if (EPs_1d[i] == 1.0 && NOs_EQ_100y_Overwrite[i] == 0)
            {
                df_Damage_EQ_100y_PGV[PGV] = new double[] { 0 };
                df_Damage_Cycles_100y_PGV[PGV] = new double[] { 0 };
                continue;
            }

            // add to sum
            _NOs_EQ_100y = NOs_EQ_100y[i];
            var _NOs_Mu_EQ = df[GetTargetIndex((int)Ds[i], targetEdgeName)].Select(x => x / 2).ToArray(); // 半サイクル → 1サイクル

            for (int j = 0; j < NOs_Cycles_100y.Length; j++)
                NOs_Cycles_100y[j] += _NOs_EQ_100y * _NOs_Mu_EQ[j];

            var _Damage_EQ = Enumerable
                .Zip(Mus[..^1], Mus[1..], _NOs_Mu_EQ)
                .Select(x =>
                {
                    (var muAmpMin, var muAmpMax, var num) = x;
                    return GetDamageOfOneCycle((muAmpMin + muAmpMax) / 2) * num;
                })
                .ToArray();

            for (int j = 0; j < Damage_100y.Length; j++)
                Damage_100y[j] += _Damage_EQ[j] * _NOs_EQ_100y;

            df_Damage_EQ_100y_PGV[PGV] = new double[] { _Damage_EQ.Sum() };
            df_Damage_Cycles_100y_PGV[PGV] = new double[] { _Damage_EQ.Sum() * _NOs_EQ_100y };
        }

        Console.WriteLine($"TotalDamage = {Damage_100y.Sum()}");


        // ★★★★★ 30年 超過確率
        {
            Console.WriteLine("\r\n★★★★★ 30年 超過確率");
            EPs_EQ_30y.WriteToConsole();

            new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "30年 超過確率",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "超過確率 [-]",
                    HasGrid = true,
                    YScale = "log",
                    Plot = new LinePlot(PGVs, EPs_EQ_30y)
                    {
                        LineColor = "blue",
                    }
                }

            }.Run(resultImage.GetChildFileInfo("30年 超過確率.png"), false);
        }

        // ★★★★★ 1日 超過確率
        {
            Console.WriteLine("\r\n★★★★★ 1日 超過確率");
            var EPs_EQ_1d = GetDevidedEP(EPs_EQ_30y, DIVISION_30y_to_1d);
            EPs_EQ_1d.WriteToConsole();

            new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "1日 超過確率",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "超過確率 [-]",
                    HasGrid = true,
                    YScale = "log",
                    Plot = new LinePlot(PGVs[..^_100PercentCount], EPs_EQ_1d[..^_100PercentCount])
                    {
                        LineColor = "blue",
                    }
                }

            }.Run(resultImage.GetChildFileInfo("1日 超過確率.png"), false);
        }

        // ★★★★★ 100年 発生回数比較
        {
            Console.WriteLine("\r\n★★★★★ 100年 発生回数");
            var divs = new int[] { 1, 2, 3, 10, 100, 1000, 10000 };

            var plots = new List<IPlot>();
            foreach (var div in divs)
            {
                var NOs_EQ_100y__ = GetNumberOfOccurrences(EPs_EQ_30y, div, period_multiplier: DIVISION_100y_to_30y);

                plots.Add(new LinePlot(PGVs[..^_100PercentCount], NOs_EQ_100y__[..^_100PercentCount])
                {
                    LineColor = null,
                    LegendLabel = $"{div}分割",
                });
            }

            new Figure
            {
                FigHeight = 8,
                FigWidth = 20,
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "100年 発生回数比較",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "地震発生回数 [回]",
                    HasGrid = true,
                    Plots = plots,
                }
            }.Run(resultImage.GetChildFileInfo("100年 発生回数比較.png"), false);

        }

        // ★★★★★ 100年 塑性率全振幅発生回数
        {
            Console.WriteLine("\r\n★★★★★ 100年 塑性率全振幅発生回数");
            var plots = new List<IPlot>();

            foreach (double i in np.logspace(-1, 1, num: 21))
            {
                var x = new double[] { 0.001, 1000 };
                var y = new double[] { 10 / i, 0.1 / i };
                plots.Add(new LinePlot(x, y)
                {
                    LineColor = "black",
                    Alpha = 0.5,
                    LineStyle = "-",
                });
            }

            plots.Add(new HorizontalBarPlot<double>(Mus, NOs_Cycles_100y)
            {
                Height = 0.1,
                Alignment = "edge",
            });

            new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLim = new(0.1, 1000),
                    YLim = new(0.1, 10),
                    XScale = "log",
                    YScale = "log",
                    Title = "100年 塑性率全振幅発生回数",
                    XLabel = "サイクル発生回数 N [回]",
                    YLabel = "塑性率片振幅 μ [-]",
                    HasGrid = true,
                    Plots = plots,
                }
            }.Run(resultImage.GetChildFileInfo("100年 塑性率全振幅発生回数.png"), false);

        }

        // ★★★★★ 100年 発生回数
        {
            Console.WriteLine("\r\n★★★★★ 100年 発生回数");
            NOs_EQ_100y.WriteToConsole();

            new Figure
            {
                FigHeight = 8,
                FigWidth = 20,
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "100年 発生回数",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "発生回数 [回]",
                    HasGrid = true,
                    Grid_Axis = "y",
                    YScale = "log",
                    Plot = new BarPlot<double>(PGVs, NOs_EQ_100y)
                    {
                        Width = 1,
                        Alignment = "edge",
                    }
                }

            }.Run(resultImage.GetChildFileInfo("100年 発生回数.png"), false);

        }

        // ★★★★★ 各地震動一回あたりの損傷
        {
            var damage_EQ_PGV = PGVs.Select(PGV => 100 * df_Damage_EQ_100y_PGV[PGV.ToString()][0]).ToArray();

            Console.WriteLine("\r\n★★★★★ 各地震動一回あたりの損傷");
            NOs_EQ_100y.WriteToConsole();

            new Figure
            {
                FigHeight = 8,
                FigWidth = 20,
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "各地震動一回あたりの損傷",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "損傷 [％]",
                    HasGrid = true,
                    Grid_Axis = "y",
                    Plot = new BarPlot<double>(PGVs, damage_EQ_PGV)
                    {
                        Width = 1,
                        Alignment = "edge",
                    }
                }

            }.Run(resultImage.GetChildFileInfo("各地震動一回あたりの損傷.png"), false);

        }

        // ★★★★★ 地震動の大きさの100年累積損傷への影響
        {
            var PGVDiffs = new double[PGVs.Length];
            for (int i = 0; i < PGVDiffs.Length; i++)
            {
                if (i == 0)
                    PGVDiffs[i] = 100000 - PGVs[i];
                else
                    PGVDiffs[i] = PGVs[i - 1] - PGVs[i];
            }

            var damage_Total_PGV = Enumerable
                .Zip(PGVs, PGVDiffs)
                .Select(x =>
                {
                    (var PGV, var PGVDiff) = x;
                    return 100 * df_Damage_Cycles_100y_PGV[PGV.ToString()][0] / PGVDiff;
                })
                .ToArray();

            Console.WriteLine("\r\n★★★★★ 地震動の大きさの100年累積損傷への影響");
            NOs_EQ_100y.WriteToConsole();

            new Figure
            {
                FigHeight = 8,
                FigWidth = 20,
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    Title = "地震動の大きさの100年累積損傷への影響",
                    XLabel = "最大地動速度 PGV [cm/s]",
                    YLabel = "損傷（PGVあたり） [％/PGV]",
                    HasGrid = true,
                    Grid_Axis = "y",
                    Plot = new BarPlot<double>(PGVs, damage_Total_PGV)
                    {
                        Width = 1,
                        Alignment = "edge",
                    }
                }

            }.Run(resultImage.GetChildFileInfo("地震動の大きさの100年累積損傷への影響.png"), false);

        }

        // ★★★★★ 100年 各階級による累積損傷への影響
        {
            Console.WriteLine("\r\n★★★★★ 100年 各階級による累積損傷への影響");

            var x = Mus[..^1];
            var y = Damage_100y.Select(x => x * 100).ToArray();

            new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    YLim = new(0.1, 10),
                    YScale = "log",
                    Title = "100年 各階級による累積損傷への影響",
                    XLabel = "損傷 [％]",
                    YLabel = "塑性率片振幅 μ [-]",
                    HasGrid = true,
                    Grid_Axis = "x",
                    Plot = new HorizontalBarPlot<double>(x, y)
                    {
                        Height = 0.1,
                        Alignment = "edge",
                    },
                }
            }.Run(resultImage.GetChildFileInfo("100年 各階級による累積損傷への影響.png"), false);

        }


        // post process
        PythonController.Shutdown();

    }
}
