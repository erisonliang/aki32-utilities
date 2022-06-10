using Aki32_Utilities.Class;
using System.Drawing;

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
            // 200 FileUtil
            {
                var baseDir = @"..\..\..\# TestModel\200 FileUtil";

                // Combination!!!!
                {
                    //var _0 = new DirectoryInfo($@"{baseDir}\★ Combination\0 initial");
                }

                // 211 CollectFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir}\211 CollectFiles\input");
                    //input.CollectFiles(null, "a.txt");
                }

                // 212 MakeFilesFromCsv
                {
                    //var inputList = new FileInfo($@"{baseDir}\212 MakeFilesFromCsv\input\list.csv");
                    //var inputTemp = new FileInfo($@"{baseDir}\212 MakeFilesFromCsv\input\template.docx");
                    //inputList.MakeFilesFromCsv(null, inputTemp);
                }

                // 213 MoveTo
                {
                    //var input = new DirectoryInfo($@"{baseDir}\213 MoveTo\input");
                    //var output = new DirectoryInfo($@"{baseDir}\213 MoveTo\output");
                    //input.MoveDir(output);
                }

                // 214 CopyTo
                {
                    var input = new DirectoryInfo($@"{baseDir}\214 CopyTo\input");
                    var output = new DirectoryInfo($@"{baseDir}\214 CopyTo\output");
                    input.CopyTo(output);
                }

                // 221 ExtractCsvColumns
                {
                    //var input = new FileInfo($@"{baseDir}\221 ExtractCsvColumns\input.csv");
                    //input.ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");
                }

                // 222 CollectCsvColumns
                {
                    //var input = new DirectoryInfo($@"{baseDir}\222 CollectCsvColumns\input");
                    //input.CollectCsvColumns(null, 3);
                }

                // 223 Csvs2ExcelSheets
                {
                    //var input = new DirectoryInfo($@"{baseDir}\223 Csvs2ExcelSheets\input");
                    //input.Csvs2ExcelSheets(null);
                }

                // 224 MergeAllLines
                {
                    //var input = new DirectoryInfo($@"{baseDir}\224 MergeAllLines\input");
                    //input.MergeAllLines(null);
                }
            }

            // 300 ImageUtil
            {
                var baseDir = @"..\..\..\# TestModel\300 ImageUtil";

                // 311 CropImage
                {
                    //var input = new FileInfo($@"{baseDir}\311 CropImage\input.bmp");
                    //input.CropImage(null, new ImageUtil.CropSize(0.13, 0.13, 0.13, 0.13));
                    //input.CropImage(null, new ImageUtil.CropSize(0, 0, 0.3, 0.3));
                    //input.CropImage(null, new ImageUtil.CropSize(0.3, 0, 0, 0.3));
                    //input.CropImage(null, new ImageUtil.CropSize(0.3, 0.3, 0, 0));
                    //input.CropImage(null, new ImageUtil.CropSize(0, 0.3, 0.3, 0));
                }

                // 312 ConvertImageColor
                {
                    //var input = new FileInfo($@"{baseDir}\312 ConvertImageColorAndSave\input.png");

                    //var targetInfos = new (string, Color)[]
                    //{
                    //    ("BBB", Color.Blue),
                    //    ("GGG", Color.Green),
                    //    ("CCC", Color.Cyan),
                    //};
                    //var targetColors = new Color[]
                    //{
                    //    Color.Pink,
                    //    Color.Red,
                    //    Color.Orange,
                    //    Color.Yellow,
                    //};

                    //input.ConvertImageColor(null, Color.White);
                    //input.ConvertImageColor_Loop(null, targetInfos);
                    //input.ConvertImageColor_Loop(null, targetColors);
                }
            }

            // 400 PDFUtil
            {
                var baseDir = @"..\..\..\# TestModel\400 PDFUtil";

                // 411 PDFPageCount
                {
                    //var input = new DirectoryInfo($@"{baseDir}\411 PDFPageCount\input");
                    //input.PDFPageCount();
                }

            }
        }


        // for aki32 private use
        // SNAPのパラメトリックスタディ！
        {
            var baseDirF = @"F:\e-defenseモデル\model";
            var baseDirC = @"C:\Users\aki32\Dropbox\Documents\02 東大関連\0 授業\3 建築学専攻\建築構造・材料演習\# 演習\e-defenseモデル\calc";

            // 最大値
            {
                //var input = new DirectoryInfo($@"{baseDirF}");
                //var output = new DirectoryInfo($@"{baseDirC}\集計処理, max");

                //// 必要csvだけ持ってきて，対象列を1つのcsvに集約
                //input
                //    .CollectFiles(null, @"*.NAP-AVDQRFMList.csv")
                //    .MoveTo(output)
                //    .CollectCsvColumns_Loop(null, ("PSV220", 1));
            }

            // 履歴ファイル rainflow前
            {
                //var input = new DirectoryInfo($@"{baseDirF}");
                //var output = new DirectoryInfo($@"{baseDirC}\集計処理, history");

                //// 必要csvだけ持ってきて，全部11行目だけ取り出す！
                //input
                //    .CollectFiles(null, @"*.NAP-AVDQRFMList.csv")
                //    .MoveTo(output)
                //    .ExtractCsvColumns_Loop(null, new int[] { 0, 11 }, 6, "t, mu");
            }

            // 履歴ファイル rainflow後
            {
                //var input = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\0 授業\3 建築学専攻\建築構造・材料演習\# 演習\e-defenseモデル\calc\集計処理, history\output_ExtractCsvColumns\rainflow");

                //// 最後に集計。
                //input.CollectCsvColumns_Loop(null, ("totalDamage", 3));
            }
        }


        Console.WriteLine();
        Console.WriteLine($"★ Process Finished!");
        Console.WriteLine();

        Console.ReadLine();
    }
}