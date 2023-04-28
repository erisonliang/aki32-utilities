using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M061_SNAP_1_CSCollect(
        string[] targetModels,
        string[] targetEQs,
        LINEController line,
        DirectoryInfo snapOutputDir,
        DirectoryInfo collectedResultsDir
        )
    {
        // init
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★★★★★★★★★★★★★★★ SNAP_1_CSCollect 開始！！！！", ConsoleColor.Blue);
        _ = line.SendMessageAsync(@$"★ SNAP_1_CSCollect 開始！！！！");

        var inputBasePath = snapOutputDir.FullName;
        var middleBasePath = $@"{inputBasePath}\途中計算";
        var middleDir = new DirectoryInfo($@"{middleBasePath}");
        if (middleDir.Exists)
            middleDir.Delete(true);

        // main
        foreach (var model in targetModels)
            foreach (var eq in targetEQs)
            {
                Console.WriteLine();
                ConsoleExtension.WriteLineWithColor($"★★★★★★★★★★★★★★★ {model} {eq} 開始！！！！", ConsoleColor.Blue);
                _ = line.SendMessageAsync(@$"★ {model} {eq} 開始！！！！");

                var input = new DirectoryInfo($@"{inputBasePath}\{model}\{eq}");
                var middle0 = new DirectoryInfo($@"{middleBasePath}\{model}_{eq}_first_collected");
                var middle9 = new DirectoryInfo($@"{middleBasePath}\{model}_{eq}_middle_result");

                // ★ 梁端

                // #M000 テスト
                {
                    //input
                    //    .CollectFiles(null, @"D.*\\D.*\.csv$")
                    //    .RenameFiles()
                    //    ;
                }

                // #M001 塑性変形頻度
                {
                    var middle1_r = new DirectoryInfo($@"{middleBasePath}\{model}_{eq}_rainflow_result");
                    var middle1_rb = new DirectoryInfo($@"{middleBasePath}\{model}_{eq}_rainflow_branch_result");

                    input
                        .CollectFiles(middle0, searchRegexen: $@"{eq}B\d*\.csv$")
                        // {eq}B.*\.csv$
                        //.CollectFiles(null, @".*B85.csv", @".*B86.csv", @".*B87.csv")
                        .RenameFiles()
                        .ExtractCsvColumnsForMany_Loop(null, 6,
                            ("i", new int[] { 0, 5 }, "t,μ"),
                            ("j", new int[] { 0, 11 }, "t,μ"))
                        .Rainflow_Loop(middle1_r, 4, 1 / 3d, true, true, middle1_rb)
                        ;

                    middle1_r
                        .RenameFiles()
                        .CollectCsvColumns(null, 3, null, -1)
                        .RenameFile($"損傷")
                        .MoveTo(middle9)
                        ;

                    middle1_rb
                        .RenameFiles()
                        .CollectCsvColumns(null, 0, null, 1)
                        .RenameFile($"塑性変形頻度")
                        .MoveTo(middle9)
                        ;
                }

                // ★ 仕上げ
                {
                    middle9
                        .Csvs2ExcelSheets(null)
                        .RenameFile($"{model}_{eq}")
                        .MoveTo(collectedResultsDir)
                        ;

                    // 大きすぎるから捨てとく。
                    middle0.Delete(true);
                }

            }

        // アーカイブ
        {

            // e-defence
            {
                //var baseDirF = @"F:\e-defenseモデル\model";
                //var baseDirC = @"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\77 別プロジェクト\SNAP解析\e-defenseモデル\calc";
                //var input = new DirectoryInfo($@"{baseDirF}");
                //var result = new DirectoryInfo($@"{baseDirC}\集計, all result");

                //// ★ 最大

                //// 最大値
                //{
                //    //input
                //    //    .CollectFiles(null, @"*.NAP-AVDQRFMList.csv")
                //    //    .RenameFiles()
                //    //    .CollectCsvColumns(null, 1)
                //    //    .RenameFile("最大値")
                //    //    .MoveTo(result)
                //    //    ;
                //}

                //// ★ 梁 （B3 (i11-j12) の，j端に注目）

                //// 梁端履歴ファイル with rainflow
                //{
                //    //input
                //    //    .CollectFiles(null, @"D7B17.csv") // 対象の梁端
                //    //    .RenameFiles()
                //    //    .ExtractCsvColumns_Loop(null, new int[] { 0, 6 }, 6, "t, mu")
                //    //    .Rainflow_Loop(null, 4, 1 / 3d)
                //    //    .CollectCsvColumns(null, 3)
                //    //    .RenameFile("梁端履歴（rainflow）")
                //    //    .MoveTo(result)
                //    //    ;
                //}

                //// 梁端履歴ファイル
                //{
                //    input
                //        .CollectFiles(null, @"D*B3.csv") // 対象の梁端
                //        .RenameFiles()
                //        .ExtractCsvColumns_Loop(null, new int[] { 0, 7, 8, 11 }, 6, "t,M[kN.m],Rotation[rad], mu")
                //        .MoveTo(result)
                //        ;
                //}

                //// ★ 層

                //// 層間変形履歴
                //{
                //    //// 必要csvだけ持ってきて，全部11行目だけ取り出す！
                //    //input
                //    //    .CollectFiles(null, @"D7F2.csv") // 対象の層
                //    //    .RenameFiles()
                //    //    .CollectCsvColumns(null, 11, 0, 6)
                //    //    .RenameFile("層間変形履歴")
                //    //    .MoveTo(result)
                //    //    ;
                //}

                //// ★ 仕上げ
                //{
                //    //result
                //    //    .Csvs2ExcelSheets(null)
                //    //    .RenameFile("集計結果 - B17周り");
                //}
            }

        }

    }
}
