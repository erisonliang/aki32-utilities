using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
using Aki32Utilities.ConsoleAppUtilities.Structure;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistory;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M054_SNAP_54_DrawGraphs_FromSingle(FileInfo originalWaveFile, List<(int PGV, double Amp)> PGVsAndAmpsList)
    {
        // ★★★★★ defs
        var outputDir = originalWaveFile.Directory!.GetChildDirectoryInfo(originalWaveFile.Name).CreateAndPipe();


        // ★★★★★ main
        var snap = new SNAPHelper.SNAPWaveData(originalWaveFile);
        var wave = snap.Accs;


        // ★★★★★ waves
        {
            var waveCsvFile = outputDir.GetChildFileInfo("Waves.csv");
            var waveImageFile = outputDir.GetChildFileInfo("Waves.png");
            var allWaves = FromCsv(waveCsvFile);

            var xLabel = "t [s]";
            var yLabel = "a [cm/s²]";

            var x = allWaves["t"];
            var plots = new List<PyPlotWrapper.IPlot>();

            var i = 0;
            foreach (var (PGV, Amp) in PGVsAndAmpsList)
            {
                var index = PGV.ToString();
                var y = allWaves[index].Select(x => x - i * 100).ToArray();

                plots.Add(new PyPlotWrapper.LinePlot(x, y)
                {
                    LegendLabel = index,
                    LineWidth = 1,
                    LineColor = null,
                    Marker = null,
                });

                i++;
            }

            new PyPlotWrapper.Figure
            {
                IsTightLayout = true,
                SubPlots = new List<PyPlotWrapper.SubPlot>()
                {
                    new PyPlotWrapper.SubPlot()
                    {
                        XLabel = xLabel,
                        YLabel = yLabel,
                        Title = null,
                        Plots = plots,
                    }
                }
            }.Run(waveImageFile, preview: false);

        }


        // ★★★★★ spectra
        {
            var SvsCsvFile = outputDir.GetChildFileInfo("Svs.csv");
            var SvsImageFile = outputDir.GetChildFileInfo("Svs.png");
            var allSvs = FromCsv(SvsCsvFile);

            allSvs.DrawGraph_OnPyPlot(SvsImageFile, "T", allSvs.Columns.Where(x => x != "T").ToArray(),
                type: ChartType.Line,
                xLabel: "T [s]",
                yLabel: "Sv [cm]",
                preview: true
                );
        }

    }
}
