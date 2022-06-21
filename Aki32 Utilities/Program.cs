using Aki32_Utilities.OverridingUtils;
using System.Drawing;

namespace Aki32_Utilities;
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine($"★ Process Started!");
        Console.WriteLine();

        // tests
        {
            // 01 OverridingUtils
            {
                // 200 FileUtil
                {
                    var baseDir = @"..\..\..\# TestModel\200 FileUtil";

                    // Combination!!!!
                    {
                    }

                    // 211 CollectFiles
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\211 CollectFiles\input");
                        //input.CollectFiles(null, "a.txt");
                    }

                    // 212 MakeFilesFromCsv
                    {
                        //var inputList = new FileInfo($@"{baseDir}\212 MakeFilesFromCsv\list.csv");
                        //var inputTemp = new FileInfo($@"{baseDir}\212 MakeFilesFromCsv\template.docx");
                        //inputList.MakeFilesFromCsv(null, inputTemp);
                    }

                    // 213 MoveTo
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\213 MoveTo\input");
                        //var output = new DirectoryInfo($@"{baseDir}\213 MoveTo\output");
                        //input.MoveTo(output);
                    }

                    // 214 CopyTo
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\214 CopyTo\input");
                        //var output = new DirectoryInfo($@"{baseDir}\214 CopyTo\output");
                        //input.CopyTo(output);
                    }

                    // 221 ReadCsv, 222 SaveCsv, 511 Transpose
                    {
                        //var innerBaseDir = $@"{baseDir}\221 ReadCsv, SaveCsv";
                        //var input = new FileInfo($@"{innerBaseDir}\input.csv");
                        //var output1 = new FileInfo($@"{innerBaseDir}\output1.csv");
                        //var output2 = new FileInfo($@"{innerBaseDir}\output2.csv");
                        //var output3 = new FileInfo($@"{innerBaseDir}\output3.csv");
                        //var output4 = new FileInfo($@"{innerBaseDir}\output4.csv");

                        //input.ReadCsv_Rows().SaveCsv_Rows(output1);
                        //input.ReadCsv_Rows().SaveCsv_Columns(output2);
                        //input.ReadCsv_Columns().SaveCsv_Rows(output3);
                        //input.ReadCsv_Columns().SaveCsv_Columns(output4);
                    }

                    // 223 ExtractCsvColumns
                    {
                        var input = new FileInfo($@"{baseDir}\223 ExtractCsvColumns\input.csv");
                        //input.ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");
                    }

                    // 224 CollectCsvColumns
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\224 CollectCsvColumns\input");
                        //input.CollectCsvColumns(null, 3);
                    }

                    // 225 Csvs2ExcelSheets
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\225 Csvs2ExcelSheets\input");
                        //input.Csvs2ExcelSheets(null);
                    }

                    // 231 MergeAllLines
                    {
                        //var input = new DirectoryInfo($@"{baseDir}\231 MergeAllLines\input");
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

                // 500 DataUtil
                {
                    // 511 To2DArray, ToJaggedArray
                    {
                        //var inputData = new int[][]
                        //{
                        //    new int[]{1,2,3,4,5,6,7},
                        //    new int[]{1,2},
                        //    new int[]{1,2,3},
                        //    new int[]{1,2,3,4,5},
                        //};
                        //Console.WriteLine("inputData:");
                        //foreach (var line in inputData)
                        //    Console.WriteLine(String.Join(", ", line.Select(x => $"{x,3}")));

                        //var outputData1 = inputData.To2DArray(999);
                        //Console.WriteLine("outputData1:");
                        //for (int i = 0; i < outputData1.GetLength(0); i++)
                        //{
                        //    for (int j = 0; j < outputData1.GetLength(1); j++)
                        //        Console.Write($"{outputData1[i, j],3}, ");
                        //    Console.WriteLine();
                        //}

                        //var outputData2 = outputData1.ToJaggedArray();
                        //Console.WriteLine("outputData2:");
                        //foreach (var line in outputData2)
                        //    Console.WriteLine(String.Join(", ", line.Select(x => $"{x,3}")));
                    }
                }
            }

            // 02 OwesomeModels
            {

            }
        }

        Console.WriteLine();
        Console.WriteLine($"★ Process Finished!");
        Console.WriteLine();

        Console.ReadLine();
    }
}