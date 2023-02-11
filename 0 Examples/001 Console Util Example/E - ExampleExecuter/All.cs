using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.AI;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;

using ClosedXML;

using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using Newtonsoft.Json;
using Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
using iTextSharp.text.pdf.codec.wmf;
using MathNet.Numerics.LinearAlgebra;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
public static partial class ExampleExecuter
{
    const string BASE_DIR = $@"..\..\..\# ExampleModel\";

    public static void All()
    {
        // TEST
        {




        }

        // A - ChainableExtensions
        {
            var baseDir_A = $@"{BASE_DIR}\A - ChainableExtensions";

            // 100 - General
            {
                var baseDir_A_100 = $@"{baseDir_A}\100 - General";

                // 2202 To2DArray, ToJaggedArray
                {
                    //var x1 = new int[][]
                    //{
                    //    new int[]{1,2,3,4,5,6,7},
                    //    new int[]{1,2},
                    //    new int[]{1,2,3},
                    //    new int[]{1,2,3,4,5},
                    //};
                    //Console.WriteLine("x1:");
                    //x1.WriteToConsole();

                    //var x2 = x1.ConvertTo2DArray(999);
                    //Console.WriteLine("x2:");
                    //x2.WriteToConsole();

                    //var x3 = x2.ConvertToJaggedArray();
                    //Console.WriteLine("x3:");
                    //x3.WriteToConsole();

                }

                // 3201 ReShape, 3202 GetRangeSlice, J201 DrawHeatmap, E103 ShowImage
                {
                    //var i2d = Enumerable.Range(0, 36).Select(x => (double)x).ToArray();
                    //Console.WriteLine("i2d:");
                    //i2d.WriteToConsole();
                    //Console.WriteLine();
                    //i2d.DrawHeatmapToConsole();
                    //Console.WriteLine();

                    //var i2d_Reshaped = i2d.ReShape(6, 6);
                    //Console.WriteLine("i2d_Reshaped:");
                    //i2d_Reshaped.WriteToConsole();
                    //Console.WriteLine();
                    //i2d_Reshaped.DrawHeatmapToConsole();
                    //i2d_Reshaped
                    //    .DrawHeatmapAsImage()
                    //    .ShowImage_OnThread();
                    //Console.WriteLine();

                    //var i2d_Reshaped_Reshaped = i2d_Reshaped.ReShape();
                    //Console.WriteLine("i2d_Reshaped_Reshaped:");
                    //i2d_Reshaped_Reshaped.WriteToConsole();
                    //Console.WriteLine();

                    //var i2d_Reshaped_Sliced = i2d_Reshaped.GetRangeSlice(1..^1, 1..^1);
                    //Console.WriteLine("i2d_Reshaped_Sliced:");
                    //i2d_Reshaped_Sliced.WriteToConsole();
                    //Console.WriteLine();

                    //var i2d_Reshaped_Shrunk = i2d_Reshaped.GetRangeSlice(1, 1..^1);
                    //Console.WriteLine("i2d_Reshaped_Shrunk:");
                    //i2d_Reshaped_Shrunk.WriteToConsole();
                    //Console.WriteLine();

                    //Console.WriteLine();
                    //Console.WriteLine();
                    //Console.WriteLine();
                    //Console.WriteLine();

                    //var i3d = Enumerable.Range(0, 27).ToArray();
                    //Console.WriteLine("i3d:");
                    //i3d.WriteToConsole();
                    //Console.WriteLine();

                    //var i3d_Reshaped = i3d.ReShape(3, 3, 3);
                    //Console.WriteLine("i3d_Reshaped:");
                    //i3d_Reshaped.WriteToConsole();
                    //Console.WriteLine();

                    //var i3d_Reshaped_Reshaped = i3d_Reshaped.ReShape();
                    //Console.WriteLine("i3d_Reshaped_Reshaped:");
                    //i3d_Reshaped_Reshaped.WriteToConsole();
                    //Console.WriteLine();

                    //var i3d_Reshaped_Sliced = i3d_Reshaped.GetRangeSlice(.., 0..^1, 1..^1);
                    //Console.WriteLine("i3d_Reshaped_Sliced:");
                    //i3d_Reshaped_Sliced.WriteToConsole();
                    //Console.WriteLine();

                }

                // B001 CollectFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\B001 CollectFiles\input");
                    //input.CollectFiles(null, @"^.*a\.txt$", @"^.*3.*$");
                    //input.CollectFiles(null, @"^.*a\.txt$");
                    //input.CollectFiles(null, @"^.*[0-9]*\\a\.txt$");
                }

                // B002 MakeFilesFromCsv
                {
                    //var inputList = new FileInfo($@"{baseDir_A_100}\B002 MakeFilesFromCsv\list.csv");
                    //var inputTemp = new FileInfo($@"{baseDir_A_100}\B002 MakeFilesFromCsv\template.docx");
                    //inputList.MakeFilesFromCsv(null, inputTemp);
                }

                // B003 MoveTo
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\B003 MoveTo\input");
                    //var keep = new DirectoryInfo($@"{baseDir_A_100}\B003 MoveTo\keep");
                    //var output = new DirectoryInfo($@"{baseDir_A_100}\B003 MoveTo\output");
                    //input.MoveTo(output);
                }

                // B004 CopyTo
                {
                    // directory-wise
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\B004 CopyTo\input");
                    //var output = new DirectoryInfo($@"{baseDir_A_100}\B004 CopyTo\output");
                    //input.CopyTo(output);

                    // file
                    //var input = new FileInfo($@"{baseDir_A_100}\B004 CopyTo\a.txt");
                    //input.CopyTo();
                }

                // B005 RenameFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\B005 RenameFiles\input");
                    //var output = new DirectoryInfo($@"{baseDir_A_100}\B005 RenameFiles\output");
                    //input.CopyTo(output);
                    //// auto
                    //output.RenameFiles();
                    ////// append
                    ////output.RenameFiles_Append("xxx*xxx");
                    ////// replace
                    ////output.RenameFiles_Replace(("ABC", "abc"), ("XYZ", "xyz"));
                }

                // B007 Compression
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\B007 Compression\input");
                    //var input2 = new FileInfo($@"{baseDir_A_100}\B007 Compression\input2.docx");

                    //input
                    //    .Compress_Zip(null)
                    //    .Decompress_Zip(null)
                    //    ;

                    //input
                    //    .Compress_Tar(null)
                    //    .Decompress_Tar(null)
                    //    ;

                    //input2
                    //    .Compress_Gzip(null)
                    //    .Decompress_Gzip(null)
                    //    ;

                }

                // B008 Encrypt, Decrypt
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\B008 Crypt\input.txt");
                    //input
                    //    .Encrypt(null, "aiueo")
                    //    .Decrypt(null, "aiueo")
                    //    ;
                }

                // B102 ReadObjectFromLocalXXX, 0101 SaveAsXXX
                {

                    //// test class
                    //var testClassToWrite = new UtilTestClass1()
                    //{
                    //    StringProp1 = "aaa",
                    //    StringProp2 = "あああ",
                    //    StringProp3 = "阿阿阿",
                    //    IntProp = 11111,

                    //    StringArrayProp = new string[] { "iii", "いいい", "胃胃胃" },
                    //    IntArrayProp = new int[] { 1, 2, 3 },

                    //    StringListProp = new List<string> { "uuu", "ううう", "宇宇宇" },
                    //    IntListProp = new List<int> { 11, 22, 33 },
                    //};

                    //// json
                    //{
                    //    var outputFile = new FileInfo($@"{baseDir_A_100}\B102 ReadObjectToLocal\output.json");

                    //    testClassToWrite.SaveAsJson(outputFile, true);

                    //    var testClassRead = outputFile.ReadObjectFromLocalJson<UtilTestClass1>();

                    //}

                    //// xml
                    //{
                    //    var outputFile = new FileInfo($@"{baseDir_A_100}\B102 ReadObjectToLocal\output.xml");

                    //    testClassToWrite.SaveAsXml(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalXml<UtilTestClass1>();

                    //}

                    //// csv
                    //{
                    //    var outputFile = new FileInfo($@"{baseDir_A_100}\B102 ReadObjectToLocal\output.csv");

                    //    var testClassListToWrite = new List<UtilTestClass1> { testClassToWrite };
                    //    testClassListToWrite.SaveAsCsv(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalCsv<UtilTestClass1>();

                    //}

                }

                // C004 ExtractCsvColumns
                {
                    //new FileInfo($@"{baseDir_A_100}\C004 ExtractCsvColumns\input.csv")
                    //    .ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");

                    //new DirectoryInfo($@"{baseDir_A_100}\C004 ExtractCsvColumns")
                    //    .ExtractCsvColumnsForMany_Loop(null, 0, ("a", new int[] { 0, 3 }, "t,x"), ("b", new int[] { 0, 4 }, "t,y"));
                }

                // C005 CollectCsvColumns
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\C005 CollectCsvColumns\input");
                    //input.CollectCsvColumns(null, 3);
                }

                // C006 Csvs2ExcelSheets
                {
                    //new DirectoryInfo($@"{baseDir_A_100}\C006 Csvs2ExcelSheets\input")
                    //    .Csvs2ExcelSheets(null);

                    //new FileInfo($@"{baseDir_A_100}\C006 Csvs2ExcelSheets\input\data1.csv")
                    //    .Csv2ExcelSheet(null);

                }

                // C101 ReadCsv, 2201 Transpose2DArray, 2103 SaveCsv
                {
                    //var innerbaseDir_A_100 = $@"{baseDir_A_100}\C101 ReadCsv, SaveCsv";
                    //var input = new FileInfo($@"{innerbaseDir_A_100}\input.csv");
                    //var output1 = new FileInfo($@"{innerbaseDir_A_100}\output1.csv");
                    //var output2 = new FileInfo($@"{innerbaseDir_A_100}\output2.csv");
                    //var output3 = new FileInfo($@"{innerbaseDir_A_100}\output3.csv");
                    //var output4 = new FileInfo($@"{innerbaseDir_A_100}\output4.csv");

                    //input.ReadCsv_Rows().SaveCsv_Rows(output1);
                    //input.ReadCsv_Rows().SaveCsv_Columns(output2);
                    //input.ReadCsv_Columns().SaveCsv_Rows(output3);
                    //input.ReadCsv_Columns().SaveCsv_Columns(output4);
                }

                // D001 MergeAllLines
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\D001 MergeAllLines\input");
                    //input.MergeAllLines(null);
                }

                // E001 CropImage
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\E001 CropImage\input.bmp");
                    //var crops = new AwesomeExtensions.Thickness[]
                    //{
                    //    new AwesomeExtensions.Thickness(0.13, 0.13, 0.13, 0.13),
                    //    new AwesomeExtensions.Thickness(0, 0, 0.3, 0.3),
                    //    new AwesomeExtensions.Thickness(0.3, 0, 0, 0.3),
                    //    new AwesomeExtensions.Thickness(0.3, 0.3, 0, 0),
                    //    new AwesomeExtensions.Thickness(0, 0.3, 0.3, 0),
                    //};

                    //input.CropImageForMany(null, crops);
                    ////input.CropImage(null, new AwesomeExtensions.Thickness(0.13, 0.13, 0.13, 0.13));
                }

                // E002 ConvertImageColor
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\E002 ConvertImageColor\input.png");

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

                // E004 Images2PDF
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\E004 Images2PDF");
                    //input.Images2PDF(null);
                }

                // E005 Images2Video, L001 Video2Images
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\E005 Images2Video");
                    //input
                    //    .Images2Video(null, 3)
                    //    .Video2Images(null, 3);

                }

                // E006 ResizeImage
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\E006 ResizeImage");
                    //input.ResizeImage_Loop(null, new Size(100, 100));
                }

                // E007 DistortImage
                {
                    //var input5 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5.png");
                    //var output510 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-1-0.png");
                    //var output520 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-2-0.png");
                    //var output521 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-2-1.png");
                    //var output530 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-3-0.png");
                    //var output531 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-3-1.png");
                    //var output540 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-4-0.png");
                    //var output541 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-4-1.png");
                    //var output542 = new FileInfo($@"{baseDir_A_100}\E007 DistortImage\5-4-2.png");

                    ////★ 1 point → move
                    //input5.DistortImage(output510, Color.Orange,
                    //    (new Point(96, 80), new Point(0, 0)));

                    //// ★ 2 points → move, resize, rotate
                    //input5.DistortImage(output520, Color.Orange,
                    //    (new Point(96, 80), new Point(0, 0)),
                    //    (new Point(373, 120), new Point(400, 0)));

                    //input5.DistortImage(output521, Color.Orange,
                    //    (new Point(96, 80), new Point(0, 0)),
                    //    (new Point(116, 138), new Point(0, 200)));

                    //// ★ 3 points →  move, resize, rotate, shear
                    //input5.DistortImage(output530, Color.Orange,
                    //    (new Point(96, 80), new Point(0, 0)),
                    //    (new Point(373, 120), new Point(400, 0)),
                    //    (new Point(116, 138), new Point(0, 200)));

                    //input5.DistortImage(output531, Color.Orange,
                    //    (new Point(96, 80), new Point(50, 50)),
                    //    (new Point(373, 120), new Point(350, 50)),
                    //    (new Point(116, 138), new Point(50, 150)));

                    //// ★ 4 points →  move, resize, rotate, shear, perspective
                    //input5.DistortImage(output540, Color.Orange,
                    //  (new Point(96, 80), new Point(50, 50)),
                    //  (new Point(373, 120), new Point(350, 50)),
                    //  (new Point(116, 138), new Point(50, 150)),
                    //  (new Point(394, 178), new Point(350, 150)));

                    //input5.DistortImage(output541, Color.Orange,
                    //   (new Point(96, 80), new Point(50, 50)),
                    //   (new Point(373, 120), new Point(350, 50)),
                    //   (new Point(116, 138), new Point(50, 150)),
                    //   (new Point(394, 178), new Point(400, 200)));

                }

                // E008 AddTextToImage
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\E008 AddTextToImage");
                    //input.AddTextToImageProportionally_Loop(null, "%FN", new PointF(0.9f, 0.8f),
                    //    fontSizeRatio: 0.1, alignRight: true);
                }

                // E102 SaveScreenShot
                {
                    //var output = new DirectoryInfo($@"{baseDir_A_100}\E102 SaveScreenShot");
                    // AwesomeExtensions.SaveScreenShot(output, new Point(0, 0), new Point(1000, 1000));
                }

                // F201 DivideTimeStep
                {
                    //var input = new TimeHistory()
                    //{
                    //    t = new double[] { 0, 1, 2, 3, 4, 5, 6 },
                    //    a = new double[] { 1, 2, 0, 3, -1, 4, -2 },
                    //};
                    //input.WriteToConsole();

                    //var output = input.GetTimeDividedHistory(10, "a");
                    //output.WriteToConsole();

                    //output.DrawGraph("a");
                }

                // G002 PDF2Images
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\G002 PDF2Images\input.pdf");
                    //input.PDF2Images(null);
                }

                // G101 PDFPageCount
                {
                    //var input = new DirectoryInfo($@"{baseDir_A_100}\G101 PDFPageCount\input");
                    //input.PDFPageCount();
                }

                // H101 DownloadFile
                {
                    //var source = new Uri(@"https://raw.githubusercontent.com/aki32/aki32-utilities/main/1%20Console%20App%20Utilities/%23%20TestModel/A%20-%20ChainableExtensions/B%20-%20General/E001%20CropImage/input.bmp");
                    //var output = new FileInfo($@"{baseDir_A_100}\H101 DownloadFile\output.bmp");
                    //source.DownloadFileAsync(output).Wait();

                }

                // H201 CallAPI
                {
                    //var source = new Uri(@"https://weather.tsukumijima.net/api/forecast/city/400040");
                    //var result = source.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

                    //Console.WriteLine(result);

                }

                // K001 ExcelSheets2Csvs
                {
                    //new FileInfo($@"{baseDir_A_100}\K001 ExcelSheets2Csvs\input.xlsx")
                    //    .ExcelSheets2Csvs(null);

                }

                // L101 PlayVideo 
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\L101 PlayVideo\input.mp4");
                    //input.PlayVideo_OnThread();
                    //input.PlayVideo_OnDefaultApp();

                }

                // M101 PlaySound 
                {
                    //var input = new FileInfo($@"{baseDir_A_100}\M101 PlaySound\input.mp3");

                    ////input.PlaySound_OnDefaultApp();

                    //input.PlaySound_OnConsole();

                    ////var device = input.PlaySound_OnConsole(true);
                    ////Thread.Sleep(3000);
                    ////device.Stop();
                }

            }

            // 100C - AwesomeModels
            {
                var baseDir_A_100C = $@"{baseDir_A}\100C - AwesomeModels";

                // A - TimeHistoryModel
                {
                    var baseDir_A_100C_A = $@"{baseDir_A_100C}\A - TimeHistoryModel";

                    // A002 DrawGraph
                    {
                        //new FileInfo($@"{baseDir_A_100C_A}\A002 DrawGraph\input1.csv")
                        //    .DrawGraph("t", "v", TimeHistory.ChartType.Line);


                        //new FileInfo($@"{baseDir_A_100C_A}\A002 DrawGraph\input1.csv")
                        //    .DrawGraph(0, 1, TimeHistory.ChartType.Line);


                        //new FileInfo($@"{baseDir_A_100C_A}\A002 DrawGraph\input2.csv")
                        //    .DrawGraph_ForAll(TimeHistory.ChartType.Line);

                    }

                    // B001 FFT
                    {
                        //new FileInfo($@"{baseDir_A_100C_A}\B001 FFT\input.csv")
                        //    .GetTimeHistoryFromFile(new string[] { "t", "x" })
                        //    .FFT("x").Result
                        //    .SaveToCsv();

                    }


                }

            }

            // 101 - AI
            {
                var baseDir_A_101 = $@"{baseDir_A}\101 - AI";

                // BB ImageToImage ML_GetHigherResolutionImage_Loop
                {

                    //PythonController.Initialize();

                    //// single
                    //var input = new FileInfo($@"{baseDir_A_101}\BB01 GetHigherResolutionImage\input\00.jpg");
                    //input.AI_GetHigherResolutionImage(null);

                    //// loop
                    ////var input = new DirectoryInfo($@"{baseDir_A_101}\BB01 GetHigherResolutionImage\input");
                    ////input.ML_GetHigherResolutionImage_Loop(null);

                    //PythonController.Shutdown();

                }

            }

            // 110 - StructuralEngineering
            {
                var baseDir_A_110 = $@"{baseDir_A}\110 - StructuralEngineering";

                // B001 RainflowCycleCounting
                {
                    //new FileInfo(Path.Combine(baseDir_A_110, "B001 RainflowCycleCounting", @"input3.csv"))
                    //    .Rainflow(null, 4, 1 / 3d, false);

                    //new DirectoryInfo(Path.Combine(baseDir_A_110, "B001 RainflowCycleCounting"))
                    //    .Rainflow_Loop(null, 4, 1 / 3d, false);

                }

                // B002 RDTechnique
                {
                    //new FileInfo(Path.Combine(baseDir_A_110, "B002 RDTechnique", @"input.csv"))
                    //    .CalcRD(null, 200)
                    //    ;

                }

                // B003 CreateAccFromCsv
                {
                    //new FileInfo(Path.Combine(baseDir_A_110, "B003 CreateAccFromCsv", @"kobe L1.csv"))
                    //    .CreateAccFromCsv_For_DynamicPro(null);

                }

            }

        }

        // B - UsefulClasses
        {
            var baseDir_B = $@"{BASE_DIR}\B - UsefulClasses";

            // B004 ConsoleExtension
            {
                //var timer = new System.Timers.Timer(500);

                //var counter = 0;
                //var countUntil = 10;
                //timer.Elapsed += delegate
                //{
                //    ConsoleExtension.ClearCurrentConsoleLine();
                //    Console.Write(++counter);
                //    if (counter >= countUntil)
                //        timer.Stop();
                //};

                //timer.Start();
                //while (timer.Enabled) { }
                //Console.WriteLine();
            }

            // B005 IODeviceExtension
            {
                //// ★ Move Mouse Cursor 
                //IODeviceExtension.MouseCursorMoveTo(4600, 700);

                //// ★ Mouse Left Click
                //IODeviceExtension.MouseClick();

                //// ★ Mouse Wheel Click
                //IODeviceExtension.MouseClick(IODeviceExtension.IODeviceButton.MouseMiddle);

                //// ★ Wheel Scroll
                //IODeviceExtension.MouseWheelScroll(true);

                //// ★ Keyboard Input
                //IODeviceExtension.SendKeys("abc ABC"); // "abc ABC"
                //IODeviceExtension.SendKey(65); // "a"
                //IODeviceExtension.SendKey(ConsoleKey.V, withCtrl: true); // Ctrl+V
                //IODeviceExtension.SendKey(ConsoleKey.V, withCtrl: true, withShift:true); // Ctrl+Shift+V
                //IODeviceExtension.SendKey(ConsoleKey.E, withAlt: true); // Alt+E
                //IODeviceExtension.SendKey(ConsoleKey.Spacebar, withShift: true, withAlt: true); // Alt Shift Space

                //// ★ Get Mouse Cursor Position 
                //Console.WriteLine($"Move mouse cursor and press enter to decide");
                //Console.CursorVisible = false;
                //while (!Console.KeyAvailable)
                //{
                //    var p = IODeviceExtension.GetMouseCursorPosition();
                //    ConsoleExtension.ClearCurrentConsoleLine();
                //    Console.Write(p);
                //    Thread.Sleep(10);
                //}
                //Console.CursorVisible = true;
                //Console.ReadLine();

            }

        }

        // C - AwesomeModels
        {
            var baseDir_C = $@"{BASE_DIR}\C - AwesomeModels";

            // A - TimeHistoryModel
            {
                var baseDir_C_A = $@"{baseDir_C}\A - TimeHistoryModel";

                // ★ TimeHistory
                {

                    //var th = new TimeHistory();
                    //th["A"] = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    //th["B"] = new double[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    //th["C"] = new double[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
                    //th["D"] = new double[] { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 };

                    //th.WriteToConsole();
                    //th.DrawLineGraph("A", "C");
                    //th.DropColumn("C");
                    //th.WriteToConsole();
                    //th.DropStep(4);
                    //th.WriteToConsole();
                    //var ths = th.GetStep(5);
                    //ths.WriteToConsole();

                    //var ts = new TimeHistoryStep();
                    //ts["E"] = 3;
                    //ts.WriteToConsole();

                    ////TimeHistory
                    ////    .FromCsv(input, "t", "v")
                    ////    .RenameColumn("v", "x")
                    ////    .WriteToConsole(5)
                    ////    .DrawLineGraph("x")
                    ////    ;

                }

            }

        }

        // D - ExternalAPIControllers
        {
            var baseDir_D = $@"{BASE_DIR}\D - ExternalAPIControllers";

            // D001 LINEController
            {
                //var accessToken = ""; // LINE Notify
                //var line = new LINEController(accessToken);
                //var result = line.SendMessageAsync(@"Hello LINE from C#. 日本語").Result;

            }

            // D002 GitController
            {
                //var remotePath = $@"https://github.com/aki32/test"; // remote url
                //var localPath = $@"C:\Users\aki32\Dropbox\PC\Desktop\test"; // local path
                //var signatureName = $@"aki32"; // your name
                //var signatureEmail = $@"aki32@aaa"; // your email

                //var gc = new GitController(remotePath, localPath, signatureName, signatureEmail)
                //{
                //    Credentials = new LibGit2Sharp.UsernamePasswordCredentials
                //    {
                //        // your credential info (obtainable from https://github.com/settings/tokens)
                //        Username = "",
                //        Password = "",
                //    },
                //};

                //gc.Sync();
            }

            // D004 Python
            {

                //PythonController.Initialize(
                //    pythonPath: @"C:\Python310",
                //    dllName: @"python310.dll",
                //    additionalPath: new List<string>
                //    {
                //        @"C:\Users\aki32\Dropbox\Codes\# Projects\研究\修士論文研究\2 Python Jupyter"
                //    }
                //    );


                //dynamic np = PythonController.Import("numpy");
                //Console.WriteLine($"np.cos(np.pi/4) = {np.cos(np.pi / 4)}");


                //PythonController.PythonExample_WithStringInvoke();
                ////PythonController.PythonExample_WithDynamicInvoke();
                ////PythonController.PythonExample_WithOwnLibraryInvoke();

                //PythonController.Shutdown();

            }

            // D005 CommandPrompt
            {

                // objective
                {
                    //using var prompt = new CommandPromptController()
                    //{
                    //    RealTimeConsoleWriteLineOutput = true,
                    //    OmitCurrentDirectoryDisplay = false,
                    //};

                    //prompt.WriteLine(@"");
                    //prompt.WriteLine(@"cd ..\..\..\# TestModel\A - Extensions\E007 DistortImage");
                    //prompt.WriteLine(@"");
                    //prompt.WriteLine(@"lnn");
                    //prompt.WriteLine(@"");
                    //prompt.WriteLine(@"ls");
                    //prompt.WriteLine(@"");

                    //var output = prompt.ResponseList.ToArray();

                }

                // static
                {

                    //var commands = new string[] {
                    //   @"",
                    //   @"cd ..\..\..\# TestModel\A - Extensions\E007 DistortImage",
                    //   @"",
                    //   @"lnn",
                    //   @"",
                    //   @"ls",
                    //   @""};

                    //var output = CommandPromptController.Execute(
                    //    realTimeConsoleWriteLineOutput: true,
                    //    omitCurrentDirectoryDisplay: true,
                    //    outputReceivedAction: null,
                    //    commands: commands);

                }

            }

        }

        // 101 - AI
        {
            var baseDir_101 = $@"{BASE_DIR}\101 - AI";

            // XA001 - CheatSheet - MLNetExampleSummary
            {
                var baseDir_101_XA001 = $@"{baseDir_101}\XA001 MLNetExampleSummary";

                var runner = new MLNetExampleSummary(MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST, new DirectoryInfo(baseDir_101_XA001));
                //runner.ExperimentTime_InSeconds = 60;
                //runner.RunPrediction();
                runner.RunAll();


            }

        }

        // 110 - StructuralEngineering
        {
            var baseDir_110 = $@"{BASE_DIR}\110 - StructuralEngineering";

            // B001 ElastoplasticAnalysis
            {
                var baseDir_110_B001 = @$"{baseDir_110}\B001 EPAnalysis";

                // newmark beta
                {
                    //var model = SDoFModel.FromT(1, 0.03);

                    //var waveCsv = new FileInfo(@$"{baseDir_110_B001}\Hachinohe-NS.csv");
                    //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                    //var waveAnalysisModel = new NewmarkBetaModel(0.25);
                    //var result = model.Calc(wave, waveAnalysisModel);

                    //result.SaveToCsv();
                    //result.DrawLineGraph("x", "t");
                }

                // nigam jennings
                {
                    //var model = SDoFModel.FromT(1, 0.03);

                    //var waveCsv = new FileInfo(@$"{baseDir_110_B001}\Hachinohe-NS.csv");
                    //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                    //var waveAnalysisModel = new NigamJenningsModel();
                    //var result = model.Calc(wave, waveAnalysisModel);

                    //result.SaveToCsv();
                    //result.DrawLineGraph("x", "t");
                }

                // ep test
                {
                    //var epList = new List<ElastoplasticCharacteristicBase>
                    //{
                    //    //new ElasticModel(2),

                    //    //new ElasticBilinearModel(2, 0.1, 80),
                    //    //new BilinearModel(2, 0.1, 80),
                    //    //new CloughModel(2, 0.1, 80),
                    //    //new DegradingCloughModel(2, 0.1, 80, 0.4),
                    //    //new PerfectElastoPlasticModel(2,80),

                    //    //new ElasticTrilinearModel(2, 0.5, 80, 0.1, 100),
                    //    new TrilinearModel(2, 0.5, 80, 0.1, 100),
                    //    //new TrilinearModel_Obsolete(2, 0.5, 80, 0.1, 100),

                    //    //new ElasticTetralinearModel(2, 0.5, 80, 0.25, 90, 0.1, 110),
                    //};

                    //foreach (var ep in epList)
                    //{
                    //    var tester = new EPTester(ep);
                    //    var result = tester.Calc(EPTester.TestWave.TestWave1);

                    //    var saveDir = new DirectoryInfo(@$"{baseDir_110_B001}\output");
                    //    result.SaveToCsv(saveDir);
                    //    result.DrawLineGraph("f", "x");
                    //}

                    // combined
                    {
                        //var waveAnalysisModelList = new List<ITimeHistoryAnalysisModel>
                        //{
                        //    new NewmarkBetaModel(0.25),
                        //    //new WilsonTheta(1.4,0.25),
                        //    //new NigamJenningsModel(),
                        //};

                        //var waveCsv = new FileInfo(@$"{baseDir_110_B001}\Hachinohe-NS.csv");
                        //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                        //foreach (var waveAnalysisModel in waveAnalysisModelList)
                        //{
                        //    var epList = new List<ElastoplasticCharacteristicBase>
                        //    {
                        //        //new ElasticModel(2),
                        //        //new BilinearModel(2, 0.1, 8),
                        //        new TrilinearModel(2, 0.5, 5, 0.1, 7),
                        //        //new CloughModel(2, 0.1, 8),
                        //        //new DegradingCloughModel(2, 0.1, 8, 0.4),
                        //    };

                        //    foreach (var ep in epList)
                        //    {
                        //        try
                        //        {
                        //            var model = SDoFModel.FromT(1, 0.03, ep);
                        //            var result = model.Calc(wave, waveAnalysisModel);
                        //            result.DrawLineGraph("x", "t");
                        //            result.DrawLineGraph("f", "x");
                        //            result.SaveToCsv();
                        //            Console.WriteLine("[O]");
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine($"[X]: {ex.Message}");
                        //            throw;
                        //        }
                        //    }
                        //}
                    }

                    // spectrum analysis
                    {
                        //var ep = new ElasticModel(2);
                        ////var ep = new DegradingCloughModel(2, 0.1, 8, 0.4);

                        //var TList = Enumerable.Range(100, 400).Select(x => x / 100d).ToArray();
                        //var hList = new double[] { 0.00, 0.03, 0.05, 0.10 };

                        //var waveCsv = new FileInfo(@$"{baseDir_110_B001}\Hachinohe-NS.csv");
                        //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                        ////var waveAnalysisModel = new NewmarkBetaModel(0.25);
                        //var waveAnalysisModel = new NigamJenningsModel();

                        //var resultSet = SDoFModel.CalcResponseSpectrum(TList, hList, wave, waveAnalysisModel, ep);
                        //resultSet.SaveToExcel(waveCsv.Directory);
                    }

                }

            }

            // B002 RainflowCycleCounting
            {
                //var inputCsv = new FileInfo($@"{baseDir_110}\B002 RainflowCycleCounting\input3.csv");
                ////var inputCsv = new FileInfo(@"C:\Users\aki32\Desktop\anaAll_beam.csv");

                //var rainflow = RainflowCalculator.FromCsv(inputCsv);
                //rainflow.CalcRainflow(4, 1 / 3d, false);
                //rainflow.SaveResultHistoryToCsv();
                //rainflow.SaveRainBranchesToCsv();
            }

            // B003 RDTechnique
            {
                //// Define IO paths
                //var input = new FileInfo($@"{baseDir_110}\B003 RDTechnique\input.csv");

                //// Read input csv
                //var rd = RDTechniqueCalculator.FromCsv(input);

                //// Calc and show
                //rd.Calc(200);
                //rd.InputHistory.DrawLineGraph("v");
                //rd.ResultHistory.DrawLineGraph("v");

                //// Calc AttenuationConstant and show
                //var att = rd.CalcAttenuationConstant(4, true);
                //Console.WriteLine();
                //Console.WriteLine($"result h = {att}");

            }

            // X001 SNAPHelper
            {
                // KeepClosingExcel
                {
                    //SNAPHelper.KeepClosingExcel(5000);

                }

            }

            // X002 DynamicProHelper
            {
                // CreateAccFromCsv
                {
                    //var input = new FileInfo($@"{baseDir_110}\X002 DynamicProHelper\kobe L1.csv");
                    //var output = new FileInfo($@"{baseDir_110}\X002 DynamicProHelper\kobe L1.acc");
                    //DynamicProHelper.CreateAccFromCsv(input, output);
                }

            }

            // X003 KnetHelper
            {
                // KnetAccData
                {
                    //var input = new FileInfo(@"C:\Users\aki32\Dropbox\PC\Desktop\新しいフォルダー\TKY0022203162336\TKY0022203162336.EW");
                    //var aaa = new KNetHelper.KNetAccData(input);








                }

            }
        }

        // 111 - Research
        {
            var baseDir_101 = $@"{BASE_DIR}\111 - Research";


            // A - ResearchManager
            {
                //var localDir = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");

                //var research = new ResearchArticlesManager(localDir);
                //research.OpenDatabase();


                ////articles from j - stage
                {
                    //var accessor = new JStageArticleAPIAccessor()
                    //{
                    //    PublishedFrom = 2022,
                    //    ISSN = ISSN.Architecture_Structure,
                    //    RecordCount = 3,
                    //    //Start = 1,
                    //};
                    //research.PullArticleInfo(accessor);

                }

                ////articles from cinii
                {
                    //var accessor = new CiNiiArticleAPIAccessor()
                    //{
                    //    RecordCount = 5,
                    //    ISSN = ISSN.Architecture_Structure,
                    //    SearchFreeWord = "小振幅"
                    //};
                    //research.PullArticleInfo(accessor);

                }

                ////article from crossref
                {
                    //var accessor = new CrossRefAPIAccessor()
                    //{
                    //    DOI = "10.3130/aijs.87.822"
                    //};
                    //research.PullArticleInfo(accessor);

                }

                // articles from ndl search
                {
                    //var accessor = new NDLSearchAPIAccessor()
                    //{
                    //    RecordCount = 5,
                    //    SearchFreeWord = "空間情報を表現するグラフ構造",
                    //};
                    //research.PullArticleInfo(accessor);

                }


                // display
                {
                    //research.SaveDatabase(true, true);

                    //Console.WriteLine();
                    //Console.WriteLine();
                    //Console.WriteLine($"★ {research.ArticleDatabase.Count} found in total");
                    //Console.WriteLine();
                    ////foreach (var article in research.ArticleDatabase)
                    ////    Console.WriteLine($" + {article.Title_Japanese}");
                    ////Console.WriteLine();
                    //Console.WriteLine();

                    ////research.ArticleDatabase.First(x => x.DOI == "10.3130/aijs.87.822").TryOpenPDF(research.PDFsDirectory);

                }

            }

        }

        // M - MiniApps
        {
            var baseDir_M = $@"{BASE_DIR}\M - MiniApps";

            // M001 Books2PDF
            {
                //var outputDir = new DirectoryInfo($@"{baseDir_M}\M001 Books2PDF");
                //MiniApps.Books2PDF(outputDir, 10);

            }

            //
            {

            }

            //
            {

            }

        }

    }

}
