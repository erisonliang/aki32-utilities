using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.AI;
using Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
using Aki32Utilities.ConsoleAppUtilities.Research;

using ClosedXML;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using NumSharp;

using Newtonsoft.Json;
using Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
using iTextSharp.text.pdf.codec.wmf;
using MathNet.Numerics.LinearAlgebra;
using ICSharpCode.SharpZipLib.Zip;

using Thickness = Aki32Utilities.ConsoleAppUtilities.General.ChainableExtensions.Thickness;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using DocumentFormat.OpenXml.Bibliography;
using System.Xml.Linq;
using XPlot.Plotly;
using DocumentFormat.OpenXml.Drawing;
using Org.BouncyCastle.Crypto.Macs;
using NumSharp.Utilities;
using DocumentFormat.OpenXml.Office2013.Excel;
using MathNet.Numerics;
using MathNet.Spatial.Euclidean;
using System.Globalization;
using Aki32Utilities.ConsoleAppUtilities.Hobby;
using ClosedXML.Excel;
using System.Xml.XPath;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
public static partial class ExampleExecuter
{
    static readonly DirectoryInfo BASE_DIR = new($@"..\..\..\# SampleData\");

    public static void All()
    {
        // TEST
        {


        }

        // A00_General
        {
            var baseDir_100 = BASE_DIR.GetChildDirectoryInfo($@"A00_General");

            // A_ChainableExtensions
            {
                var baseDir_100_A = baseDir_100.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // 2202_To2DArray, ToJaggedArray
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

                // 3201_ReShape, 3202_GetRangeSlice, J201_DrawHeatmap, E103_ShowImage
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
                    //i2d_Reshaped.DrawHeatmapAsSimpleImage().ShowImage_OnThread(false);
                    //PythonController.Initialize();
                    //i2d_Reshaped.DrawHeatmapAsPyPlotImage(preview: true , outputExtension:".svg");
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

                // B001_CollectFiles
                {
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B001_CollectFiles\input");
                    //input.CollectFiles(null, @"^.*a\.txt$", @"^.*3.*$");
                    //input.CollectFiles(null, @"^.*a\.txt$");
                    //input.CollectFiles(null, @"^.*[0-9]*\\a\.txt$");
                }

                // B002_MakeFilesFromCsv
                {
                    //var inputList = baseDir_100_A_A.GetChildFileInfo($@"B002_MakeFilesFromCsv\list.csv");
                    //var inputTemp = baseDir_100_A_A.GetChildFileInfo($@"B002_MakeFilesFromCsv\template.docx");
                    //inputList.MakeFilesFromCsv(null, inputTemp);
                }

                // B003_MoveTo
                {
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B003_MoveTo\input");
                    //var keep = baseDir_100_A_A.GetChildDirectoryInfo($@"B003_MoveTo\keep");
                    //var output = baseDir_100_A_A.GetChildDirectoryInfo($@"B003_MoveTo\output");
                    //input.MoveTo(output);
                }

                // B004_CopyTo
                {
                    // directory-wise
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B004_CopyTo\input");
                    //var output = baseDir_100_A_A.GetChildDirectoryInfo($@"B004_CopyTo\output");
                    //input.CopyTo(output);

                    // file
                    //var input = baseDir_100_A_A.GetChildFileInfo($@"B004_CopyTo\a.txt");
                    //input.CopyTo();
                }

                // B005_RenameFiles
                {
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B005_RenameFiles\input");
                    //var output = baseDir_100_A_A.GetChildDirectoryInfo($@"B005_RenameFiles\output");
                    //if (output.Exists)
                    //    output.Delete(true);
                    //input.CopyTo(output);

                    //// auto
                    //output.RenameFiles();
                    ////// append
                    ////output.RenameFiles_Append("xxx*xxx");
                    ////// replace
                    ////output.RenameFiles_Replace(("ABC", "abc"), ("XYZ", "xyz"));
                }

                // B006_RenameDirs
                {
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B006_RenameDirs\input");
                    //var output = baseDir_100_A_A.GetChildDirectoryInfo($@"B006_RenameDirs\output");
                    //if (output.Exists)
                    //    output.Delete(true);
                    //input.CopyTo(output);

                    //// auto
                    //output.RenameDirs("*", Array.Empty<(string from, string to)>(), true);
                    ////output.RenameDirs("*", new (string from, string to)[] { ("ABC", "") }, true);
                    ////// append
                    ////output.RenameFiles_Append("xxx*xxx");
                    ////// replace
                    ////output.RenameFiles_Replace(("ABC", "abc"), ("XYZ", "xyz"));
                }

                // B007_Compression
                {
                    //var input = baseDir_100_A_A.GetChildDirectoryInfo($@"B007_Compression\input");
                    //var input2 = baseDir_100_A_A.GetChildFileInfo($@"B007_Compression\input2.docx");

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

                    ////input3
                    ////    .Decompress_Rar(null)
                    ////    ;

                }

                // B008_Encrypt, Decrypt
                {
                    //baseDir_100_A
                    //    .GetChildFileInfo($@"B008_Crypt\input.txt")
                    //    .Encrypt(null, "aiueo")
                    //    .Decrypt(null, "aiueo")
                    //    ;
                }

                // B009_DrawGraph
                {
                    //baseDir_100_A.GetChildFileInfo($@"B009_DrawGraph\input1.csv")
                    //    .DrawGraph_OnPlotly("t", "v", TimeHistory.ChartType.Line);


                    //baseDir_100_A.GetChildFileInfo($@"B009_DrawGraph\input1.csv")
                    //    .DrawGraph_OnPlotly(0, 1, TimeHistory.ChartType.Line);


                    //baseDir_100_A.GetChildFileInfo($@"B009_DrawGraph\input2.csv")
                    //    .DrawGraph_OnPlotly(TimeHistory.ChartType.Line);

                }

                // B102_ReadObjectFromLocalXXX, 0101_SaveAsXXX
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
                    //    var outputFile = baseDir_100_A.GetChildFileInfo($@"B102_ReadObjectToLocal\output.json");

                    //    testClassToWrite.SaveAsJson(outputFile, true);

                    //    var testClassRead = outputFile.ReadObjectFromLocalJson<UtilTestClass1>();

                    //}

                    //// xml
                    //{
                    //    var outputFile = baseDir_100_A.GetChildFileInfo($@"B102_ReadObjectToLocal\output.xml");

                    //    testClassToWrite.SaveAsXml(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalXml<UtilTestClass1>();

                    //}

                    //// csv
                    //{
                    //    var outputFile = baseDir_100_A.GetChildFileInfo($@"B102_ReadObjectToLocal\output.csv");

                    //    var testClassListToWrite = new List<UtilTestClass1> { testClassToWrite };
                    //    testClassListToWrite.SaveAsCsv(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalCsv<UtilTestClass1>();

                    //}

                }

                // C004_ExtractCsvColumns
                {
                    //baseDir_100_A
                    //    .GetChildFileInfo($@"C004_ExtractCsvColumns\input.csv")
                    //    .ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");

                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"C004_ExtractCsvColumns")
                    //    .ExtractCsvColumnsForMany_Loop(null, 0, ("a", new int[] { 0, 3 }, "t,x"), ("b", new int[] { 0, 4 }, "t,y"));
                }

                // C005_CollectCsvColumns
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"C005_CollectCsvColumns\input")
                    //    .CollectCsvColumns(null, 3);

                }

                // C006_Csvs2ExcelSheets
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"C006_Csvs2ExcelSheets\input")
                    //    .Csvs2ExcelSheets(null);

                    //baseDir_100_A
                    //    .GetChildFileInfo($@"C006_Csvs2ExcelSheets\input\data1.csv")
                    //    .Csv2ExcelSheet(null);

                }

                // C101_ReadCsv, 2201_Transpose2DArray, 2103_SaveCsv
                {
                    //var innerbaseDir_100_A_A = baseDir_100_A_A.GetChildDirectoryInfo($@"C101_ReadCsv, SaveCsv");
                    //var input = innerbaseDir_100_A_A.GetChildFileInfo($@"input.csv");
                    //var output1 = innerbaseDir_100_A_A.GetChildFileInfo($@"output1.csv");
                    //var output2 = innerbaseDir_100_A_A.GetChildFileInfo($@"output2.csv");
                    //var output3 = innerbaseDir_100_A_A.GetChildFileInfo($@"output3.csv");
                    //var output4 = innerbaseDir_100_A_A.GetChildFileInfo($@"output4.csv");

                    //input.ReadCsv_Rows().SaveCsv_Rows(output1);
                    //input.ReadCsv_Rows().SaveCsv_Columns(output2);
                    //input.ReadCsv_Columns().SaveCsv_Rows(output3);
                    //input.ReadCsv_Columns().SaveCsv_Columns(output4);
                }

                // D001_MergeAllLines
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"D001_MergeAllLines\input")
                    //    .MergeAllLines(null);
                }

                // E001_CropImage
                {
                    //var input = baseDir_100_A.GetChildFileInfo($@"E001_CropImage\input.bmp");

                    //var crops = new Thickness[]
                    //{
                    //    new Thickness(0.13, 0.13, 0.13, 0.13),
                    //    new Thickness(0, 0, 0.3, 0.3),
                    //    new Thickness(0.3, 0, 0, 0.3),
                    //    new Thickness(0.3, 0.3, 0, 0),
                    //    new Thickness(0, 0.3, 0.3, 0),
                    //};

                    //input.CropImageForMany(null, crops);
                    //input.CropImage(null, new AwesomeExtensions.Thickness(0.13, 0.13, 0.13, 0.13));
                }

                // E002_ConvertImageColor
                {
                    //var input = baseDir_100_A.GetChildFileInfo($@"E002_ConvertImageColor\input.png");

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
                    //input.ConvertImageColorForMany(null, targetInfos);
                    //input.ConvertImageColorForMany(null, targetColors);
                }

                // E004_Images2PDF
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"E004_Images2PDF")
                    //    .Images2PDF(null);
                }

                // E005_Images2Video, L001_Video2Images
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"E005_Images2Video")
                    //    .Images2Video(null, 3)
                    //    .Video2Images(null, 3);

                }

                // E006_ResizeImage
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"E006_ResizeImage")
                    //    .ResizeImage_Loop(null, new Size(100, 100));
                }

                // E007_DistortImage
                {
                    //var input5 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5.png");
                    //var output510 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-1-0.png");
                    //var output520 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-2-0.png");
                    //var output521 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-2-1.png");
                    //var output530 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-3-0.png");
                    //var output531 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-3-1.png");
                    //var output540 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-4-0.png");
                    //var output541 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-4-1.png");
                    //var output542 = baseDir_100_A.GetChildFileInfo($@"E007_DistortImage\5-4-2.png");

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

                // E008_AddTextToImage
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"E008_AddTextToImage")
                    //    .AddTextToImageProportionally_Loop(null, "%FN", new PointF(0.9f, 0.8f),
                    //    fontSizeRatio: 0.1, alignRight: true);
                }

                // E010_ConcatenateImage
                {
                    //var baseDir_100_A_E010 = baseDir_100_A.GetChildDirectoryInfo($@"E010_ConcatenateImage").CreateAndPipe();

                    //var input1 = baseDir_100_A_E010.GetChildFileInfo($@"input1.png");
                    //var input2 = baseDir_100_A_E010.GetChildFileInfo($@"input2.png");

                    //var outputH = baseDir_100_A_E010.GetChildFileInfo($@"outputH.png");
                    //var outputV = baseDir_100_A_E010.GetChildFileInfo($@"outputV.png");

                    //input1.ConcatenateImage(input2, outputV, isVertical: true);
                    //input1.ConcatenateImage(input2, outputH, isVertical: false);

                    //baseDir_100_A_E010.OpenOnDefaultApp();

                }

                // E102_SaveScreenShot
                {
                    //var output = baseDir_100_A.GetChildDirectoryInfo($@"E102_SaveScreenShot");
                    //Aki32Utilities.ConsoleAppUtilities.General.ChainableExtensions.SaveScreenShot(output, new Point(0, 0), new Point(1000, 1000));
                }

                // F201_DivideTimeStep
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

                // G002_PDF2Images
                {
                    //baseDir_100_A
                    //    .GetChildFileInfo($@"G002_PDF2Images\input.pdf")
                    //    .PDF2Images(null);
                }

                // G101_PDFPageCount
                {
                    //baseDir_100_A
                    //    .GetChildDirectoryInfo($@"G101_PDFPageCount\input")
                    //    .PDFPageCount();
                }

                // H101_DownloadFile
                {
                    //var source = new Uri(@"https://raw.githubusercontent.com/aki32/aki32-utilities/main/1%20Console%20App%20Utilities/%23%20TestModel/A%20-%20ChainableExtensions/B%20-%20General/E001%20CropImage/input.bmp");
                    //var output = baseDir_100_A.GetChildFileInfo($@"H101_DownloadFile\output.bmp");
                    //source.DownloadFileAsync(output).Wait();

                }

                // H201_CallAPI
                {
                    //var source = new Uri(@"https://weather.tsukumijima.net/api/forecast/city/400040");
                    //var result = source.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

                    //Console.WriteLine(result);

                }

                // K001_ExcelSheets2Csvs
                {
                    //baseDir_100_A
                    //    .GetChildFileInfo($@"K001_ExcelSheets2Csvs\input.xlsx")
                    //    .ExcelSheets2Csvs(null);

                }

                // L101_PlayVideo 
                {
                    //var input = baseDir_100_A.GetChildFileInfo($@"L101_PlayVideo\input.mp4");
                    //input.PlayVideo_OnThread();
                    //input.PlayVideo_OnDefaultApp();

                }

                // M101_PlaySound 
                {
                    //var input = baseDir_100_A.GetChildFileInfo($@"M101_PlaySound\input.mp3");

                    ////input.PlaySound_OnDefaultApp();

                    //input.PlaySound_OnConsole();

                    ////var device = input.PlaySound_OnConsole(true);
                    ////Thread.Sleep(3000);
                    ////device.Stop();
                }

            }

            // B_UsefulClasses
            {
                var baseDir_100_B = baseDir_100.GetChildDirectoryInfo($@"B_UsefulClasses");

                // B004_ConsoleExtension
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

                // B005_IODeviceExtension
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

                    //// ★ Intermittent Keyboard Input
                    //IODeviceExtension.SenKeyIntermittently(1000, "XXXX");

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

            // C_AwesomeModels
            {
                var baseDir_100_C = baseDir_100.GetChildDirectoryInfo($@"C_AwesomeModels");

                // A_TimeHistoryModel
                {
                    var baseDir_C_A = baseDir_100_C.GetChildDirectoryInfo($@"A_TimeHistoryModel");

                    // ★ TimeHistory
                    {

                        //var th = new TimeHistory();

                        //th["A"] = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                        //th["B"] = new double[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                        //th["C"] = new double[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
                        //th["D"] = new double[] { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 };

                        //th.WriteToConsole();
                        //th.DrawGraph_OnPlotly("A", "C");
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

            // D_ExternalAPIControllers
            {
                var baseDir_100_D = baseDir_100.GetChildDirectoryInfo($@"D_ExternalAPIControllers");

                // D001_LINEController
                {
                    ////var accessToken = ""; // LINE Notify
                    //var accessToken = Environment.GetEnvironmentVariable("LINE_AccessToken__General")!; // LINE Notify
                    //var line = new LINEController(accessToken);
                    //var result = line.SendMessageAsync(@"Hello LINE from C#. 日本語").Result;

                }

                // D002_GitController
                {
                    //var remotePath = $@"https://github.com/aki32/test"; // remote url
                    //var localPath = $@"C:\Users\aki32\Dropbox\PC\Desktop\test"; // local path
                    //var signatureName = $@"aki32"; // your name
                    //var signatureEmail = $@"aki32@aaa"; // your email

                    //var gc = new GitController(remotePath, localPath, signatureName, signatureEmail)
                    //{
                    //    Credentials = new LibGit2Sharp.UsernamePasswordCredentials
                    //    {
                    //        //// your credential info (obtainable from https://github.com/settings/tokens)
                    //        //Username = "",
                    //        //Password = "",

                    //        Username = Environment.GetEnvironmentVariable("GetHub_Username")!,
                    //        Password = Environment.GetEnvironmentVariable("GetHub_Password")!,
                    //    },
                    //};

                    //gc.Sync();
                }

                // D004_Python
                {
                    var baseDir_100_D004 = baseDir_100_D.GetChildDirectoryInfo($@"D004_Python");
                    var output = baseDir_100_D004.GetChildDirectoryInfo($@"output");

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

                // D005_CommandPrompt
                {

                    // objective
                    {
                        //using var prompt = new CommandPromptController()
                        //{
                        //    RealTimeConsoleWriteLineOutput = true,
                        //    OmitCurrentDirectoryDisplay = false,
                        //};

                        //prompt.WriteLine(@"");
                        //prompt.WriteLine(@"cd ..\..\..\# TestModel\A_Extensions\E007_DistortImage");
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
                        //   @"cd ..\..\..\# TestModel\A_Extensions\E007_DistortImage",
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

            // E_Wrappers
            {
                var baseDir_100_E = baseDir_100.GetChildDirectoryInfo($@"E_Wrappers");


                // E001_NumpyWrapper
                {
                    var baseDir_100_E001 = baseDir_100_E.GetChildDirectoryInfo($@"E001_NumpyWrapper");
                    var output = baseDir_100_E001.GetChildDirectoryInfo($@"output");


                }

                // E002_PyPlotWrapper
                {
                    var baseDir_100_E002 = baseDir_100_E.GetChildDirectoryInfo($@"E002_PyPlotWrapper");
                    var output = baseDir_100_E002.GetChildDirectoryInfo($@"output");

                    PythonController.Initialize();
                    var pi = Math.PI;


                    // line
                    {
                        //var n = 256;

                        //var z = EnumerableExtension.Range_WithCount(-3 * pi, 3 * pi, n).ToArray();
                        //var x = z.Select(z => Math.Cos(z)).ToArray();
                        //var y = z.Select(z => Math.Sin(z)).ToArray();

                        //new PyPlotWrapper.Figure
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot()
                        //    {
                        //        XLabel = "x",
                        //        YLabel = "y",
                        //        ZLabel = "z",
                        //        Title = "helix",
                        //        Plot = new PyPlotWrapper.LinePlot(x, y, z) { Alpha = 0.6 },
                        //    }

                        //}.Run(output.GetChildFileInfo("helix.png"), true);

                    }

                    // 3d bar
                    {
                        //var n = 5;

                        //// ★★★★★
                        //var XX = EnumerableExtension.Range_WithStep(0, n-1, 1).ToArray();
                        //var YY = EnumerableExtension.Range_WithStep(0, n-1, 1).ToArray();
                        //var ZZ = Enumerable.SelectMany(XX, x => YY, (x, y) => (x + 1) * y).ToArray().ReShape(n, n);

                        //// ★★★★★ bar3d
                        //new PyPlotWrapper.Figure(true)
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot(true)
                        //    {
                        //        XLabel = "X",
                        //        YLabel = "Y",
                        //        ZLabel = "Z",
                        //        Title = "bar3d",
                        //        Plot = new PyPlotWrapper.Bar3DPlot(XX, YY, 0.8, 0.8, ZZ, true)
                        //        {
                        //            Alpha = 0.5,
                        //        }
                        //    }
                        //}.Run(output.GetChildFileInfo("bar3d.png"), true);

                    }

                    // surface
                    {
                        //var n = 50;

                        //// ★★★★★ 1
                        ////var XX = EnumerableExtension.Range_WithCount(-3 * pi, 3 * pi, n).ToArray();
                        ////var YY = EnumerableExtension.Range_WithCount(-3 * pi, 3 * pi, n).ToArray();
                        ////var ZZ = Enumerable
                        ////    .SelectMany(XX, x => YY, (x, y) => (double)(np.sin(x / pi) * np.cos(y / pi)))
                        ////    .ToArray().ReShape(n, n);

                        //// ★★★★★ 2
                        //var XX = EnumerableExtension.Range_WithCount(-4, 4, n).ToArray();
                        //var YY = EnumerableExtension.Range_WithCount(-4, 4, n).ToArray();
                        //var ZZ = Enumerable
                        //    .SelectMany(XX, x => YY, (x, y) =>
                        //    {
                        //        var Z1 = Math.Exp(-Math.Pow(x, 2)-Math.Pow(y, 2));
                        //        var Z2 = Math.Exp(-Math.Pow(x-1.2, 2)-Math.Pow(y-0.7, 2));
                        //        var Z3 = Math.Exp(-Math.Pow(x + 0.5, 2)-Math.Pow(y + 1.4, 2));
                        //        return (Z1-Z2-Z3) * 2;
                        //    })
                        //    .ReShape(n, n);

                        //// ★★★★★ surface
                        //new PyPlotWrapper.Figure(true)
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot(true)
                        //    {
                        //        //ZLim=(-1,1),
                        //        XLabel = "X",
                        //        YLabel = "Y",
                        //        ZLabel = "Z",
                        //        Title = "surface",
                        //        Plots = new List<PyPlotWrapper.IPlot>
                        //        {
                        //            new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=XX.Min(), TargetHeightDirection="x", LineWidth=4},
                        //            new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=YY.Max(), TargetHeightDirection="y", LineWidth=4},
                        //            new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=ZZ.Min(), TargetHeightDirection="z", LineWidth=4},
                        //            //new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,Colors="green", ZOffset=ZZ.Min(), ContourLabelFontSize__2D=20},

                        //            //new PyPlotWrapper.WireFramePlot(XX,YY,ZZ){Color="black", LineWidth=3},
                        //            new PyPlotWrapper.SurfacePlot(XX,YY,ZZ){ ColorMap="cividis", Alpha=0.5},

                        //            //new PyPlotWrapper.ScatterPlot(XX,YY,ZZ){ ColorMap="green", MarkerSize=100},
                        //        }
                        //    }
                        //}.Run(output.GetChildFileInfo("surface.png"), true);

                    }

                    // grid heatmap
                    {
                        //// ★★★★★
                        //var n = 10;
                        //var XX = EnumerableExtension.Range_WithStep(0, n-1, 1).ToArray();
                        //var YY = EnumerableExtension.Range_WithStep(0, n-1, 1).ToArray();
                        //var ZZ = Enumerable
                        //    .SelectMany(XX, x => YY, (x, y) =>
                        //    {
                        //        return (x * y);
                        //    })
                        //    .ToArray().ReShape(n, n);

                        //// ★★★★★ 
                        //new PyPlotWrapper.Figure()
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot()
                        //    {
                        //        XLabel = "X",
                        //        YLabel = "Y",
                        //        Title = "grid heatmap",
                        //        HasGrid = false,
                        //        Plot = new PyPlotWrapper.GridHeatMapPlot(XX, YY, ZZ)
                        //        {
                        //            ColorMap = "cividis",
                        //        },
                        //    }
                        //}.Run(output.GetChildFileInfo("grid heatmap.png"), true);

                    }

                    // precise heatmap
                    {
                        //// ★★★★★
                        //var n = 333;
                        //var XX = EnumerableExtension.Range_WithCount(0, 5, n).ToArray();
                        //var YY = EnumerableExtension.Range_WithCount(0, 5, n).ToArray();
                        //var ZZ = Enumerable
                        //    .SelectMany(XX, x => YY, (x, y) =>
                        //    {
                        //        return (x * y);
                        //    })
                        //    .ToArray().ReShape(n, n);

                        //// ★★★★★ 
                        //new PyPlotWrapper.Figure()
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot()
                        //    {
                        //        XLabel = "X",
                        //        YLabel = "Y",
                        //        Title = "continuous heatmap",
                        //        HasGrid = false,
                        //        Plot = new PyPlotWrapper.ContinuousHeatMapPlot(XX, YY, ZZ)
                        //        {
                        //            ColorMap = "cividis",
                        //        },
                        //    }
                        //}.Run(output.GetChildFileInfo("continuous heatmap.png"), true);

                    }

                    // quiver 
                    {
                        //var n = 10;

                        //var X = EnumerableExtension.Range_WithCount(-5, 5, n).ToArray();
                        //var Y = EnumerableExtension.Range_WithCount(-5, 5, n).ToArray();

                        //var XYGrid = Enumerable.SelectMany(X, x => Y, (x, y) => (x, y));
                        //var XGrid = XYGrid.Select(xy => xy.x).ToArray().ReShape(n, n);
                        //var YGrid = XYGrid.Select(xy => xy.y).ToArray().ReShape(n, n);

                        //var U = X.Select(x => 2 * x);
                        //var V = Y.Select(y => 3 * y);

                        //var UVGrid = Enumerable.SelectMany(U, u => V, (u, v) => (u, v));
                        //var UGrid = UVGrid.Select(uv => uv.u).ToArray().ReShape(n, n);
                        //var VGrid = UVGrid.Select(uv => uv.v).ToArray().ReShape(n, n);

                        //new PyPlotWrapper.Figure
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot()
                        //    {
                        //        XLim = (-8, 8),
                        //        YLim = (-8, 8),
                        //        HasGrid = true,
                        //        XLabel = "x",
                        //        YLabel = "y",
                        //        ZLabel = "z",
                        //        Title = "quiver",
                        //        Plot = new PyPlotWrapper.QuiverPlot(XGrid, YGrid, UGrid, VGrid)
                        //        {
                        //            Scale = 10,
                        //        },
                        //    }

                        //}.Run(output.GetChildFileInfo("quiver.png"), true);

                    }

                    // Text
                    {

                        //new PyPlotWrapper.Figure
                        //{
                        //    IsTightLayout = true,
                        //    SubPlot = new PyPlotWrapper.SubPlot()
                        //    {
                        //        HasGrid = true,
                        //        XLabel = "x",
                        //        YLabel = "y",
                        //        ZLabel = "z",
                        //        Plot = new PyPlotWrapper.TextPlot(0.5, 0.5, "aiueoあいうえお")
                        //        {
                        //        },
                        //    }

                        //}.Run(output.GetChildFileInfo("text.png"), true);

                    }

                }

                // E701__360ImageCropperWrapper
                {
                    //var baseDir_100_E701_= baseDir_100_E.GetChildDirectoryInfo($@"E701__360ImageCropperWrapper");

                    //var inputImage = baseDir_100_E701.GetChildFileInfo($@"input.jpg");
                    //var outputDir = baseDir_100_E701.GetChildDirectoryInfo($@"output");
                    //var outputImage = outputDir.GetChildFileInfo($@"output.jpg");
                    //var outputJson = outputDir.GetChildFileInfo($@"output.json");

                    //var ep = new _360ImageCropperWrapper();
                    //ep.CropAndSave(inputImage, outputImage,
                    //    800, 2500, 2000,
                    //    -Math.PI / 4, 0, 0,
                    //    outputJson
                    //    );

                    ////ep.AutoCropAndSave(inputImage, outputDir, 1000, 2000, 1500, false, 10);

                }

            }

        }

        // A01_AI
        {
            var baseDir_101 = BASE_DIR.GetChildDirectoryInfo($@"A01_AI");

            // A_ChainableExtensions
            {
                var baseDir_101_A = baseDir_101.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // BB ImageToImage
                {
                    // BB01 AI_GetHigherResolutionImage
                    {
                        //PythonController.Initialize();

                        //// single
                        //baseDir_101_A
                        //    .GetChildFileInfo($@"BB01 GetHigherResolutionImage\input\00.jpg")
                        //    .AI_GetHigherResolutionImage(null);

                        ////// loop
                        ////baseDir_101_A
                        ////    .GetChildDirectoryInfo($@"BB01 GetHigherResolutionImage\input")
                        ////    .AI_GetHigherResolutionImage_Loop(null);

                        //PythonController.Shutdown();

                    }
                }
            }

            // B_UsefulClasses
            {
                var baseDir_101_B = baseDir_101.GetChildDirectoryInfo($@"B_UsefulClasses");



            }

            // C_AwesomeModels
            {
                var baseDir_101_C = baseDir_101.GetChildDirectoryInfo($@"C_AwesomeModels");

                //
                {
                }

            }

            // D_ExternalAPIControllers
            {
                var baseDir_101_D = baseDir_101.GetChildDirectoryInfo($@"D_ExternalAPIControllers");

                // D001_OpenAIController
                {

                    //var baseDir_101_D_D001_= baseDir_101_D.GetChildDirectoryInfo($@"D001_OpenAIController");
                    //var output = baseDir_101_D.GetChildDirectoryInfo($@"output");

                    ////var apiSecretKey = ""; // API SecretKey
                    //var apiSecretKey = Environment.GetEnvironmentVariable("OpenAI_SecretKey")!; // API SecretKey
                    //var openAI = new OpenAIController(apiSecretKey);

                    ////// ★ get models
                    ////var models = openAI.CallGetModelsAsync().Result;
                    ////Console.WriteLine(models);

                    ////// ★ edit text
                    ////var result = openAI.CallEditTextAsync("Fix the spelling mistakes", "What day of the wek is it?").Result;
                    ////foreach (var item in result)
                    ////    Console.WriteLine(item);

                    ////// ★ generate image
                    ////var result = openAI.CallGenerateImageAsync("beautiful cat with blue eye and white ear", n: 2).Result;
                    ////for (int i = 0; i < result!.Length; i++)
                    ////{
                    ////    var GeneratedImageFile = baseDir_101_D_D001.GetChildFileInfo($"GeneratedImage{i}.png");
                    ////    result[i].DownloadFile(GeneratedImageFile);
                    ////}


                    //// ★ whisper
                    //var input = baseDir_101_D_D001.GetChildFileInfo("input voice.m4a");
                    //var result = openAI.CallAudioTranscriptionsAsync(input).Result;
                    //Console.WriteLine(result);


                    ////// ★ chat gpt
                    ////Console.Write("ChatGPTに送信：");s
                    ////var input = Console.ReadLine();
                    ////var result = openAI.CallChatAsync(input!).Result;
                    ////Console.WriteLine(result[0]);

                }

            }

            // E_Wrappers
            {
                var baseDir_101_E = baseDir_101.GetChildDirectoryInfo($@"E_Wrappers");

                // E001_WhisperCppWrapper
                {
                    //var baseDir_101_E001_= baseDir_101_E.GetChildDirectoryInfo($@"E001_WhisperCppWrapper");
                    //var input = baseDir_101_E001.GetChildFileInfo($@"input.m4a");
                    //var output = baseDir_101_E001.GetChildFileInfo($@"output.txt");

                    //var whisper = new WhisperCppWrapper(
                    //    designatedModelDir: new DirectoryInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\WhisperCpp"),
                    //    designatedMainExecuterFile: new FileInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\WhisperCpp\main.exe"),
                    //    designatedWhisperDllFile: new FileInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\WhisperCpp\whisper.dll")
                    //    );

                    //whisper.ExecuteWhisper(input, output,
                    //    usingModel: WhisperCppWrapper.ModelType.Small,
                    //    outputFormat: WhisperCppWrapper.OutputFormat.srt,
                    //    usingThreadsCount: 16
                    //    );

                }
            }

            // X_CheatSheet
            {
                var baseDir_101_X = baseDir_101.GetChildDirectoryInfo($@"X_CheatSheet");

                // A001_CheatSheet_MLNetExampleSummary
                {
                    //var baseDir_101_X_A001 = baseDir_101_X.GetChildDirectoryInfo($@"A001_MLNetExampleSummary");
                    //var baseDir_101_X_A001_Private = baseDir_101_X_A001.GetChildDirectoryInfo($@"@Private");

                    //var runner = new MLNetExampleSummary(MLNetExampleScenario.B003_MultiClassClassification_MNIST, baseDir_101_X_A001_Private);
                    ////runner.ExperimentTime_InSeconds = 60;
                    ////runner.RunPrediction();
                    //runner.RunAll();

                }
            }

        }

        // A10_StructuralEngineering
        {
            var baseDir_110 = BASE_DIR.GetChildDirectoryInfo($@"A10_StructuralEngineering");

            // A_ChainableExtensions
            {
                var baseDir_110_A = baseDir_110.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // B001_RainflowCycleCounting
                {
                    //baseDir_110_A
                    //    .GetChildFileInfo($@"B001_RainflowCycleCounting\input3.csv")
                    //    .Rainflow(null, 4, 1 / 3d, false);

                    //baseDir_110_A
                    //    .GetChildDirectoryInfo("B001_RainflowCycleCounting")
                    //    .Rainflow_Loop(null, 4, 1 / 3d, false);

                }

                // B002_RDTechnique
                {
                    //baseDir_110_A
                    //    .GetChildFileInfo(@$"B002_RDTechnique\input.csv")
                    //    .CalcRandomDecrement(null, 200)
                    //    ;

                }

                // B003_CreateAccFromCsv
                {
                    //baseDir_110_A
                    //    .GetChildFileInfo(@$"B003_CreateAccFromCsv\kobe L1.csv")
                    //    .CreateAccFromCsv_For_DynamicPro(null);

                }

                // F203_FFT
                {
                    //baseDir_110_A.OpenOnDefaultApp();

                    //baseDir_110_A
                    //    .GetChildFileInfo($@"F203_FFT\input.csv")
                    //    .GetTimeHistoryFromFile(new string[] { "t", "x" })
                    //    .FFT("x")
                    //    .SaveToCsv();

                }

                // F204_Wavelet
                {
                    //baseDir_110_A.OpenOnDefaultApp();
                    //baseDir_110_A
                    //    .GetChildFileInfo($@"F204_Wavelet\input.csv")
                    //    .GetTimeHistoryFromFile(new string[] { "x" })
                    //    .Wavelet("x")
                    //    .SaveToCsv();

                }

            }

            // C_AwesomeModels
            {
                var baseDir_110_C = baseDir_110.GetChildDirectoryInfo(@$"C_AwesomeModels");

                // B001_ElastoplasticAnalysis
                {
                    var baseDir_110_B001 = baseDir_110_C.GetChildDirectoryInfo(@$"B001_EPAnalysis");

                    // newmark beta
                    {
                        //var model = SDoFModel.FromT(1, 0.03);

                        //var waveCsv = baseDir_110_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
                        //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                        //var waveAnalysisModel = new NewmarkBetaModel(0.25);
                        //var result = model.Calc(wave, waveAnalysisModel);

                        //result.SaveToCsv();
                        //result.DrawGraph("x", "t");
                    }

                    // nigam jennings
                    {
                        //var model = SDoFModel.FromT(1, 0.03);

                        //var waveCsv = new FileInfo(@$"{baseDir_110_B001}\Hachinohe-NS.csv");
                        //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                        //var waveAnalysisModel = new NigamJenningsModel();
                        //var result = model.Calc(wave, waveAnalysisModel);

                        //result.SaveToCsv();
                        //result.DrawGraph("x", "t");
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

                        //    var saveDir = baseDir_110_B001.GetChildDirectoryInfo(@$"output");
                        //    result.SaveToCsv(saveDir);
                        //    result.DrawGraph("f", "x");
                        //}

                        // combined
                        {
                            //var waveAnalysisModelList = new List<ITimeHistoryAnalysisModel>
                            //{
                            //    new NewmarkBetaModel(0.25),
                            //    //new WilsonTheta(1.4,0.25),
                            //    //new NigamJenningsModel(),
                            //};

                            //var waveCsv = baseDir_110_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
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

                            //var waveCsv = baseDir_110_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
                            //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                            ////var waveAnalysisModel = new NewmarkBetaModel(0.25);
                            //var waveAnalysisModel = new NigamJenningsModel();

                            //var resultSet = SDoFModel.CalcResponseSpectrum(TList, hList, wave, waveAnalysisModel, ep);
                            //resultSet.SaveToExcel(waveCsv.Directory);
                        }

                    }

                }

                // B002_RainflowCycleCounting
                {
                    //var baseDir_110_C_B002 = baseDir_110_C.GetChildDirectoryInfo($@"B002_RainflowCycleCounting");
                    //baseDir_110_C_B002.OpenOnDefaultApp();
                    //var inputCsv = baseDir_110_C_B002.GetChildFileInfo($@"input2.csv");

                    //var rainflow = new RainflowCalculator(inputCsv);
                    //rainflow.CalcRainflow(4, 1 / 3d, false);
                    //rainflow.SaveResultHistoryToCsv();
                    //rainflow.SaveRainBranchesToCsv();

                    ////var rainflow = RainflowCalculator.ExampleModel1;
                    ////rainflow.InputHistory.WriteToConsole();
                    ////Console.WriteLine("============================");
                    ////rainflow.CalcRainflow(4, 1 / 3d, false);
                    ////rainflow.ResultHistory.WriteToConsole();

                }

                // B003_RDTechnique
                {
                    //// Define IO paths
                    //var baseDir_110_C_B003 = baseDir_110_C.GetChildDirectoryInfo($@"B003_RDTechnique");
                    //var input = baseDir_110_C_B003.GetChildFileInfo($@"input.csv");

                    //// Read input csv
                    //var rd = RDTechniqueCalculator.FromCsv(input);

                    //// Calc and show
                    //rd.Calc(200);
                    //rd.InputHistory.DrawGraph("v");
                    //rd.ResultHistory.DrawGraph("v");

                    //// Calc AttenuationConstant and show
                    //var att = rd.CalcAttenuationConstant(4, true);
                    //Console.WriteLine();
                    //Console.WriteLine($"result h = {att}");

                }

                // B004_Wavelet
                {
                    // see A10_StructuralEngineering > A_ChainableExtensions > F204_Wavelet
                }

                // B005_FFT
                {
                    // see A10_StructuralEngineering > A_ChainableExtensions > F203_FFT
                }

                // F001_MatrixDisplacementMethod
                {
                    //// Define IO paths
                    //var baseDir_110_C_F001 = baseDir_110_C.GetChildDirectoryInfo($@"F001_MatrixDisplacementMethod").CreateAndPipe();
                    //var output1 = baseDir_110_C_F001.GetChildFileInfo($@"input1.png");
                    //var output2 = baseDir_110_C_F001.GetChildFileInfo($@"input2.png");
                    //baseDir_110_C_F001.OpenOnDefaultApp();

                    //// build
                    //var structure = MatrixDisplacementMethod.ExampleModel1;
                    //structure.Draw_Frame().Save(output1.FullName);

                    //// calc
                    //structure.CalculateAll();
                    //structure.Draw_M().Save(output2.FullName);

                }

                // X001_DelaunayTriangulation
                {
                    //UtilPreprocessors.PreprocessBasic();

                    //var delaunay = new DelaunayTriangulationExecuter();

                    //var points = DelaunayTriangulationExecuter.ExampleModel1;
                    //foreach (var point in points)
                    //    Console.WriteLine(point.ToString().ReplaceUnwantedCharacters());

                    //Console.WriteLine("=======================");
                    //var tris = delaunay.Execute(points);
                    //foreach (var tri in tris)
                    //    Console.WriteLine(tri.ToString().ReplaceUnwantedCharacters());

                    //Console.WriteLine("=======================");
                    //var lines = tris.GetDistinctedLines();
                    //foreach (var line in lines)
                    //    Console.WriteLine(line.ToString().ReplaceUnwantedCharacters());

                }

            }

            // X_SoftwareHelpers
            {
                var baseDir_110_X = baseDir_110.GetChildDirectoryInfo(@$"X_SoftwareHelpers");

                // X001_SNAPHelper
                {
                    // KeepClosingExcel
                    {
                        //SNAPHelper.KeepClosingExcel(5000);

                    }

                }

                // X002_DynamicProHelper
                {
                    // CreateAccFromCsv
                    {
                        //var input = baseDir_110_X.GetChildFileInfo($@"X002_DynamicProHelper\kobe L1.csv");
                        //var output = baseDir_110_X.GetChildFileInfo($@"X002_DynamicProHelper\kobe L1.acc");
                        //DynamicProHelper.CreateAccFromCsv(input, output);
                    }

                }

                // X003_KnetHelper
                {
                    // KnetAccData
                    {
                        //var input = new FileInfo(@"C:\Users\aki32\Dropbox\PC\Desktop\新しいフォルダー\TKY0022203162336\TKY0022203162336.EW");
                        //var aaa = new KNetHelper.KNetAccData(input);








                    }

                }
            }

        }

        // A11_Research
        {
            var baseDir_101 = BASE_DIR.GetChildDirectoryInfo($@"A11_Research");

            // A_ResearchManager
            {

                //var databaseDir = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");

                //var research = new ResearchArticlesManager(databaseDir);
                //research.OpenDatabase();

                ////// ★ articles from j-stage
                ////var accessor = new JStage_Main_ArticleAPIAccessor()
                ////{
                ////    PublishedFrom = 2022,
                ////    ISSN = ISSN.Architecture_Structure,
                ////    RecordCount = 3,
                ////    //Start = 1,
                ////};
                ////research.PullArticleInfo(accessor).Wait();

                //// ★ articles from j-stage DOI
                //var accessor = new JStage_DOI_ArticleAPIAccessor()
                //{
                //    DOI = "10.3130/aijs.80.703"
                //};
                //research.PullArticleInfo(accessor).Wait();

                ////// ★ articles from cinii
                ////var accessor = new CiNii_Main_ArticleAPIAccessor()
                ////{
                ////    RecordCount = 5,
                ////    ISSN = ISSN.Architecture_Structure,
                ////    FreeWord = "小振幅"
                ////};
                ////research.PullArticleInfo(accessor).Wait();

                ////// ★ articles from ndl search
                ////var accessor = new NDLSearch_Main_APIAccessor()
                ////{
                ////    RecordCount = 5,
                ////    FreeWord = "空間情報を表現するグラフ構造",
                ////};
                ////research.PullArticleInfo(accessor).Wait();

                ////// ★ article from crossref
                ////var accessor = new CrossRef_DOI_APIAccessor()
                ////{
                ////    DOI = "10.3130/aijs.87.822"
                ////};
                ////research.PullArticleInfo(accessor).Wait();


                ////// display
                ////{
                ////    research.SaveDatabase(true, true);

                ////    Console.WriteLine();
                ////    Console.WriteLine();
                ////    Console.WriteLine($"★ {research.ArticleDatabase.Count} found in total");
                ////    Console.WriteLine();
                ////    //foreach (var article in research.ArticleDatabase)
                ////    //    Console.WriteLine($" + {article.Title_Japanese}");
                ////    //Console.WriteLine();
                ////    Console.WriteLine();

                ////    //research.ArticleDatabase.First(x => x.DOI == "10.3130/aijs.87.822").TryOpenPDF(research.PDFsDirectory);

                ////}

            }

        }

        // AXX_Hobby
        {
            var baseDir_199 = BASE_DIR.GetChildDirectoryInfo($@"AXX_Hobby");

            // A_ChainableExtensions
            {
                var baseDir_199_A = baseDir_199.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // E001_Steganography
                {
                    //var baseDir_199_A_E001 = baseDir_199.GetChildDirectoryInfo($@"E001_Steganography").CreateAndPipe();

                    //var input1_show = baseDir_199_A_E001.GetChildFileInfo($@"input1_show.png");

                    //var input2_hide = baseDir_199_A_E001.GetChildFileInfo($@"input2_hide.png");
                    //var input2_encrypted = baseDir_199_A_E001.GetChildFileInfo($@"input2_encrypted.png");
                    //var input2_decrypted = baseDir_199_A_E001.GetChildFileInfo($@"input2_decrypted.png");
                    //input1_show.SteganographyEncryptImage(input2_hide, input2_encrypted);
                    //input2_encrypted.SteganographyDecryptImage(input2_decrypted);

                    //var input3_hide = baseDir_199_A_E001.GetChildFileInfo($@"input3_hide.png");
                    //var input3_encrypted = baseDir_199_A_E001.GetChildFileInfo($@"input3_encrypted.png");
                    //var input3_decrypted = baseDir_199_A_E001.GetChildFileInfo($@"input3_decrypted.png");
                    //input1_show.SteganographyEncryptImage(input3_hide, input3_encrypted);
                    //input3_encrypted.SteganographyDecryptImage(input3_decrypted);

                    //baseDir_199_A_E001.OpenOnDefaultApp();

                }
            }
        }

    }
}
