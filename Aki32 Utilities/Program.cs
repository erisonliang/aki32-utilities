using Aki32_Utilities.Class;

namespace Aki32_Utilities;
internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine($"★ Process Started!");
        Console.WriteLine();

        // tests
        {
            // FileOrganizer
            {
                var baseDir = @"..\..\..\# TestModel\FileUtil";

                // Combination!!!!
                {
                    //var _0 = new DirectoryInfo($@"{baseDir}\★ Combination\0 initial");
                }

                // CollectFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir}\CollectFiles\input");
                    //var output = new DirectoryInfo($@"{baseDir}\CollectFiles\output");
                    //input.CollectFiles(output, "a.txt");
                }

                // MakeFilesFromCsv
                {
                    //var inputList = new FileInfo($@"{baseDir}\MakeFilesFromCsv\list.csv");
                    //var inputTemplate = new FileInfo($@"{baseDir}\MakeFilesFromCsv\template.docx");
                    //var output = new DirectoryInfo($@"{baseDir}\MakeFilesFromCsv\output");
                    //inputList.MakeFilesFromCsv(inputTemplate, output);
                }

                // CollectCsvColumns
                {
                    //var input = new DirectoryInfo($@"{baseDir}\CollectCsvColumns\input");
                    //var output = new FileInfo($@"{baseDir}\CollectCsvColumns\collected D columns.csv");
                    //input.CollectCsvColumns(output, 3);
                }

                // ExtractCsvColumns
                {
                    //var input = new FileInfo($@"{baseDir}\ExtractCsvColumns\input.csv");
                    //var output = new FileInfo($@"{baseDir}\ExtractCsvColumns\output.csv");
                    //input.ExtractCsvColumns(output, new int[] { 0, 3 }, 0, "t,x");
                }

                // Csvs2ExcelSheets
                {
                    //var input = new DirectoryInfo($@"{baseDir}\Csvs2ExcelSheets\input");
                    //var output = new FileInfo($@"{baseDir}\Csvs2ExcelSheets\output.xlsx");
                    //input.Csvs2ExcelSheets(output);
                }

                // MergeAllLines
                {
                    //var input = new DirectoryInfo($@"{baseDir}\MergeAllLines\input");
                    //var output = new FileInfo($@"{baseDir}\MergeAllLines\merged.txt");
                    //input.MergeAllLines(output);
                }
            }
        }


        // for aki32 private use
        // SNAPのパラメトリックスタディ！
        {
            var baseDir = @"C:\Users\aki32\Dropbox\Documents\02 東大関連\0 授業\3 建築学専攻\建築構造・材料演習\# 演習\e-defenseモデル\calc";
            // 最大値と最小値
            {
                //var _0D = new DirectoryInfo(@"F:\e-defenseモデル\model");
                //var _1D = new DirectoryInfo($@"{baseDir}\集計処理, max, 0 collected");
                //var _2D = new DirectoryInfo($@"{baseDir}\集計処理, max, 1 collected");

                //// 集約して，Excelに変換
                //_0D.CollectFiles(_1D, @"*.NAP-AVDQRFMList.csv");
                //_1D.ForEach_CollectCsvColumns(_2D, ("PSV220", 1));
            }

            // 履歴ファイル rainflow前
            {
                //var _0D = new DirectoryInfo(@"F:\e-defenseモデル\model");
                //var _1D = new DirectoryInfo($@"{baseDir}\集計処理, history, 0 collected");
                //var _2D = new DirectoryInfo($@"{baseDir}\集計処理, history, 1 extracted");

                //// 集約して，全部11行目だけ取り出す！
                //_0D.CollectFiles(_1D, @"*.NAP-AVDQRFMList.csv");
                //_1D.ForEach_ExtractCsvColumns(_2D, new int[] { 0, 11 }, 6, "t, mu");
            }

            // 履歴ファイル rainflow後
            {
                //var _3D = new DirectoryInfo($@"{baseDir}\集計処理, history, 2 rainflow");
                //var _4D = new DirectoryInfo($@"{baseDir}\集計処理, history, 3 collected");

                //// 最後に集計。
                //_3D.ForEach_CollectCsvColumns(_4D, ("totalDamage", 3));
            }
        }


        Console.WriteLine();
        Console.WriteLine($"★ Process Finished!");
        Console.WriteLine();

        Console.ReadLine();
    }
}