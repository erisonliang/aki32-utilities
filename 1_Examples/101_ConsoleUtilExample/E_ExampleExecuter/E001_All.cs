using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.AI;
using Aki32Utilities.ConsoleAppUtilities.Structure;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.PyPlotWrapper;
using Thickness = Aki32Utilities.ConsoleAppUtilities.General.ChainableExtensions.Thickness;

using ClosedXML;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using NumSharp;

using Newtonsoft.Json;
using iTextSharp.text.pdf.codec.wmf;
using MathNet.Numerics.LinearAlgebra;
using ICSharpCode.SharpZipLib.Zip;

using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using DocumentFormat.OpenXml.Bibliography;
using System.Xml.Linq;
using XPlot.Plotly;
using Org.BouncyCastle.Crypto.Macs;
using NumSharp.Utilities;
using DocumentFormat.OpenXml.Office2013.Excel;
using MathNet.Numerics;
using MathNet.Spatial.Euclidean;
using System.Globalization;
using Aki32Utilities.ConsoleAppUtilities.Hobby;
using ClosedXML.Excel;
using System.Xml.XPath;
using MathNet.Numerics.LinearAlgebra.Double;
using Tensorflow;
using static Aki32Utilities.ConsoleAppUtilities.Structure.Material.Steel;

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
            var baseDir_A00 = BASE_DIR.GetChildDirectoryInfo($@"A00_General");

            // A_ChainableExtensions
            {
                var baseDir_A00_A = baseDir_A00.GetChildDirectoryInfo($@"A_ChainableExtensions");

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
                    //var baseDir_A00_A_B001 = baseDir_A00_A.GetChildDirectoryInfo($@"B001_CollectFiles").CreateAndPipe();
                    //baseDir_A00_A_B001.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_B001.GetChildDirectoryInfo($@"input");
                    //input.CollectFiles(null, @"^.*a\.txt$", @"^.*3.*$");
                    //input.CollectFiles(null, @"^.*a\.txt$");
                    //input.CollectFiles(null, @"^.*[0-9]*\\a\.txt$");
                }

                // B002_MakeFilesFromCsv
                {
                    //var baseDir_A00_A_B002 = baseDir_A00_A.GetChildDirectoryInfo($@"B002_MakeFilesFromCsv").CreateAndPipe();
                    //baseDir_A00_A_B002.OpenOnDefaultApp();

                    //var inputList = baseDir_A00_A_B002.GetChildFileInfo($@"list.csv");
                    //var inputTemp = baseDir_A00_A_B002.GetChildFileInfo($@"template.docx");

                    //inputList.MakeFilesFromCsv(null, inputTemp);
                }

                // B003_MoveTo
                {
                    //var baseDir_A00_A_B003 = baseDir_A00_A.GetChildDirectoryInfo($@"B003_MoveTo").CreateAndPipe();
                    //baseDir_A00_A_B003.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_B003.GetChildDirectoryInfo($@"input");
                    //var keep = baseDir_A00_A_B003.GetChildDirectoryInfo($@"keep");
                    //var output = baseDir_A00_A_B003.GetChildDirectoryInfo($@"output");
                    //input.MoveTo(output);
                }

                // B004_CopyTo
                {
                    //var baseDir_A00_A_B004 = baseDir_A00_A.GetChildDirectoryInfo($@"B004_CopyTo").CreateAndPipe();
                    //baseDir_A00_A_B004.OpenOnDefaultApp();

                    //// directory-wise
                    //var inputD = baseDir_A00_A_B004.GetChildDirectoryInfo($@"input");
                    //var outputD = baseDir_A00_A_B004.GetChildDirectoryInfo($@"output");
                    //inputD.CopyTo(outputD);

                    ////// file
                    ////var inputF = baseDir_A00_A_B004.GetChildFileInfo($@"a.txt");
                    ////inputF.CopyTo();
                }

                // B005_RenameFiles
                {
                    //var baseDir_A00_A_B005 = baseDir_A00_A.GetChildDirectoryInfo($@"B005_RenameFiles").CreateAndPipe();
                    //baseDir_A00_A_B005.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_B005.GetChildDirectoryInfo($@"input");
                    //var output = baseDir_A00_A_B005.GetChildDirectoryInfo($@"output");
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
                    //var baseDir_A00_A_B006 = baseDir_A00_A.GetChildDirectoryInfo($@"B006_RenameDirs").CreateAndPipe();
                    //baseDir_A00_A_B006.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_B006.GetChildDirectoryInfo($@"input");
                    //var output = baseDir_A00_A_B006.GetChildDirectoryInfo($@"output");
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
                    //var baseDir_A00_A_B007 = baseDir_A00_A.GetChildDirectoryInfo($@"B007_Compression").CreateAndPipe();
                    //baseDir_A00_A_B007.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_B007.GetChildDirectoryInfo($@"input");
                    //var input2 = baseDir_A00_A_B007.GetChildFileInfo($@"input2.docx");

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
                    //var baseDir_A00_A_B008 = baseDir_A00_A.GetChildDirectoryInfo($@"B008_Crypt").CreateAndPipe();
                    //baseDir_A00_A_B008.OpenOnDefaultApp();

                    //baseDir_A00_A_B008
                    //    .GetChildFileInfo($@"input.txt")
                    //    .Encrypt(null, "aiueo")
                    //    .Decrypt(null, "aiueo")
                    //    ;
                }

                // B009_DrawGraph
                {
                    //var baseDir_A00_A_B009 = baseDir_A00_A.GetChildDirectoryInfo($@"B009_DrawGraph").CreateAndPipe();
                    //baseDir_A00_A_B009.OpenOnDefaultApp();

                    //baseDir_A00_A_B009.GetChildFileInfo($@"input1.csv")
                    //    .DrawGraph_OnPlotly("t", "v", TimeHistory.ChartType.Line);

                    //baseDir_A00_A_B009.GetChildFileInfo($@"input1.csv")
                    //    .DrawGraph_OnPlotly(0, 1, TimeHistory.ChartType.Line);

                    ////baseDir_A00_A_B009.GetChildFileInfo($@"input2.csv")
                    ////    .DrawGraph_OnPlotly(TimeHistory.ChartType.Line);

                }

                // B102_ReadObjectFromLocalXXX, 0101_SaveAsXXX
                {
                    //var baseDir_A00_A_B102 = baseDir_A00_A.GetChildDirectoryInfo($@"B102_ReadObjectToLocal").CreateAndPipe();
                    //baseDir_A00_A_B102.OpenOnDefaultApp();

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
                    //    var outputFile = baseDir_A00_A_B102.GetChildFileInfo($@"output.json");
                    //    testClassToWrite.SaveAsJson(outputFile, true);
                    //    var testClassRead = outputFile.ReadObjectFromLocalJson<UtilTestClass1>();
                    //}

                    //// xml
                    //{
                    //    var outputFile = baseDir_A00_A_B102.GetChildFileInfo($@"output.xml");
                    //    testClassToWrite.SaveAsXml(outputFile);
                    //    var testClassRead = outputFile.ReadObjectFromLocalXml<UtilTestClass1>();
                    //}

                    //// csv
                    //{
                    //    var outputFile = baseDir_A00_A_B102.GetChildFileInfo($@"output.csv");
                    //    var testClassListToWrite = new List<UtilTestClass1> { testClassToWrite };
                    //    testClassListToWrite.SaveAsCsv(outputFile);
                    //    var testClassRead = outputFile.ReadObjectFromLocalCsv<UtilTestClass1>();
                    //}

                }

                // C004_ExtractCsvColumns
                {
                    //var baseDir_A00_A_C004 = baseDir_A00_A.GetChildDirectoryInfo($@"C004_ExtractCsvColumns").CreateAndPipe();
                    //baseDir_A00_A_C004.OpenOnDefaultApp();

                    //baseDir_A00_A_C004
                    //    .GetChildFileInfo($@"input.csv")
                    //    .ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");

                    ////baseDir_A00_A_C004
                    ////    .ExtractCsvColumnsForMany_Loop(null, 0, ("a", new int[] { 0, 3 }, "t,x"), ("b", new int[] { 0, 4 }, "t,y"));
                }

                // C005_CollectCsvColumns
                {
                    //var baseDir_A00_A_C005 = baseDir_A00_A.GetChildDirectoryInfo($@"C005_CollectCsvColumns").CreateAndPipe();
                    //baseDir_A00_A_C005.OpenOnDefaultApp();
                    //baseDir_A00_A_C005
                    //    .GetChildDirectoryInfo($@"input")
                    //    .CollectCsvColumns(null, 3);

                }

                // C006_Csvs2ExcelSheets
                {
                    //var baseDir_A00_A_C006 = baseDir_A00_A.GetChildDirectoryInfo($@"C006_Csvs2ExcelSheets").CreateAndPipe();
                    //baseDir_A00_A_C006.OpenOnDefaultApp();

                    //baseDir_A00_A_C006
                    //    .GetChildDirectoryInfo($@"input")
                    //    .Csvs2ExcelSheets(null);

                    ////baseDir_A00_A_C006
                    ////    .GetChildFileInfo($@"input\data1.csv")
                    ////    .Csv2ExcelSheet(null);

                }

                // C101_ReadCsv, 2201_Transpose2DArray, 2103_SaveCsv
                {
                    //var baseDir_A00_A_C101 = baseDir_A00_A.GetChildDirectoryInfo($@"C101_ReadCsv, SaveCsv").CreateAndPipe();
                    //baseDir_A00_A_C101.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_C101.GetChildFileInfo($@"input.csv");
                    //var output1 = baseDir_A00_A_C101.GetChildFileInfo($@"output1.csv");
                    //var output2 = baseDir_A00_A_C101.GetChildFileInfo($@"output2.csv");
                    //var output3 = baseDir_A00_A_C101.GetChildFileInfo($@"output3.csv");
                    //var output4 = baseDir_A00_A_C101.GetChildFileInfo($@"output4.csv");

                    //input.ReadCsv_Rows().SaveAsCsv_Rows(output1);
                    //input.ReadCsv_Rows().SaveAsCsv_Columns(output2);
                    //input.ReadCsv_Columns().SaveAsCsv_Rows(output3);
                    //input.ReadCsv_Columns().SaveAsCsv_Columns(output4);
                }

                // D001_MergeAllLines
                {
                    //var baseDir_A00_A_D001 = baseDir_A00_A.GetChildDirectoryInfo($@"D001_MergeAllLines").CreateAndPipe();
                    //baseDir_A00_A_D001.OpenOnDefaultApp();

                    //baseDir_A00_A_D001
                    //    .GetChildDirectoryInfo($@"input")
                    //    .MergeAllLines(null);
                }

                // E001_CropImage
                {
                    //var baseDir_A00_A_E001 = baseDir_A00_A.GetChildDirectoryInfo($@"E001_CropImage").CreateAndPipe();
                    //baseDir_A00_A_E001.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_E001.GetChildFileInfo($@"input.bmp");

                    //var crops = new Thickness[]
                    //{
                    //    new Thickness(0.13, 0.13, 0.13, 0.13),
                    //    new Thickness(0, 0, 0.3, 0.3),
                    //    new Thickness(0.3, 0, 0, 0.3),
                    //    new Thickness(0.3, 0.3, 0, 0),
                    //    new Thickness(0, 0.3, 0.3, 0),
                    //};

                    //input.CropImageForMany(null, crops);
                    //input.CropImage(null, new Thickness(0.13, 0.13, 0.13, 0.13));
                }

                // E002_ConvertImageColor
                {
                    //var baseDir_A00_A_E002 = baseDir_A00_A.GetChildDirectoryInfo($@"E002_ConvertImageColor").CreateAndPipe();
                    //baseDir_A00_A_E002.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_E002.GetChildFileInfo($@"input.png");

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
                    //var baseDir_A00_A_E004 = baseDir_A00_A.GetChildDirectoryInfo($@"E004_Images2PDF").CreateAndPipe();
                    //baseDir_A00_A_E004.OpenOnDefaultApp();

                    //baseDir_A00_A_E004
                    //    .Images2PDF(null);
                }

                // E005_Images2Video, L001_Video2Images
                {
                    //var baseDir_A00_A_E004 = baseDir_A00_A.GetChildDirectoryInfo($@"E004_Images2PDF").CreateAndPipe();
                    //baseDir_A00_A_E004.OpenOnDefaultApp();

                    //baseDir_A00_A
                    //    .GetChildDirectoryInfo($@"E005_Images2Video")
                    //    .Images2Video(null, 3)
                    //    .Video2Images(null, 3);

                }

                // E006_ResizeImage
                {
                    //var baseDir_A00_A_E006 = baseDir_A00_A.GetChildDirectoryInfo($@"E006_ResizeImage").CreateAndPipe();
                    //baseDir_A00_A_E006.OpenOnDefaultApp();

                    //baseDir_A00_A_E006
                    //    .ResizeImage_Loop(null, new Size(100, 100));
                }

                // E007_DistortImage
                {
                    //var baseDir_A00_A_E007 = baseDir_A00_A.GetChildDirectoryInfo($@"E007_DistortImage").CreateAndPipe();
                    //baseDir_A00_A_E007.OpenOnDefaultApp();

                    //var input5 = baseDir_A00_A_E007.GetChildFileInfo($@"5.png");
                    //var output510 = baseDir_A00_A_E007.GetChildFileInfo($@"5-1-0.png");
                    //var output520 = baseDir_A00_A_E007.GetChildFileInfo($@"5-2-0.png");
                    //var output521 = baseDir_A00_A_E007.GetChildFileInfo($@"5-2-1.png");
                    //var output530 = baseDir_A00_A_E007.GetChildFileInfo($@"5-3-0.png");
                    //var output531 = baseDir_A00_A_E007.GetChildFileInfo($@"5-3-1.png");
                    //var output540 = baseDir_A00_A_E007.GetChildFileInfo($@"5-4-0.png");
                    //var output541 = baseDir_A00_A_E007.GetChildFileInfo($@"5-4-1.png");
                    //var output542 = baseDir_A00_A_E007.GetChildFileInfo($@"5-4-2.png");

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
                    //var baseDir_A00_A_E008 = baseDir_A00_A.GetChildDirectoryInfo($@"E008_AddTextToImage").CreateAndPipe();
                    //baseDir_A00_A_E008.OpenOnDefaultApp();

                    //baseDir_A00_A_E008
                    //    .AddTextToImageProportionally_Loop(null, "%FN", new PointF(0.9f, 0.8f),
                    //    fontSizeRatio: 0.1, alignRight: true);
                }

                // E010_ConcatenateImage
                {
                    //var baseDir_A00_A_E010 = baseDir_A00_A.GetChildDirectoryInfo($@"E010_ConcatenateImage").CreateAndPipe();
                    //baseDir_A00_A_E010.OpenOnDefaultApp();

                    //var input1 = baseDir_A00_A_E010.GetChildFileInfo($@"input1.png");
                    //var input2 = baseDir_A00_A_E010.GetChildFileInfo($@"input2.png");

                    //var outputH = baseDir_A00_A_E010.GetChildFileInfo($@"outputH.png");
                    //var outputV = baseDir_A00_A_E010.GetChildFileInfo($@"outputV.png");

                    //input1.ConcatenateImage(input2, outputV, isVertical: true);
                    //input1.ConcatenateImage(input2, outputH, isVertical: false);

                }

                // E102_SaveScreenShot
                {
                    //var baseDir_A00_A_E012 = baseDir_A00_A.GetChildDirectoryInfo($@"E102_SaveScreenShot").CreateAndPipe();
                    //baseDir_A00_A_E012.OpenOnDefaultApp();

                    //var output = baseDir_A00_A_E012;
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

                    //output.DrawGraph_OnPlotly("a");

                }

                // G002_PDF2Images
                {
                    //var baseDir_A00_A_G002 = baseDir_A00_A.GetChildDirectoryInfo($@"G002_PDF2Images").CreateAndPipe();
                    //baseDir_A00_A_G002.OpenOnDefaultApp();

                    //baseDir_A00_A_G002
                    //    .GetChildFileInfo($@"input.pdf")
                    //    .PDF2Images(null);

                }

                // G101_PDFPageCount
                {
                    //var baseDir_A00_A_G101 = baseDir_A00_A.GetChildDirectoryInfo($@"G101_PDFPageCount").CreateAndPipe();
                    //baseDir_A00_A_G101.OpenOnDefaultApp();

                    //baseDir_A00_A_G101
                    //    .GetChildDirectoryInfo($@"input")
                    //    .PDFPageCount();

                }

                // H101_DownloadFile
                {
                    //var baseDir_A00_A_H101 = baseDir_A00_A.GetChildDirectoryInfo($@"H101_DownloadFile").CreateAndPipe();
                    //baseDir_A00_A_H101.OpenOnDefaultApp();

                    //var source = new Uri(@"https://raw.githubusercontent.com/aki32/aki32-utilities/main/1%20Console%20App%20Utilities/%23%20TestModel/A%20-%20ChainableExtensions/B%20-%20General/E001%20CropImage/input.bmp");
                    //var output = baseDir_A00_A_H101.GetChildFileInfo($@"output.bmp");
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
                    //var baseDir_A00_A_K001 = baseDir_A00_A.GetChildDirectoryInfo($@"K001_ExcelSheets2Csvs").CreateAndPipe();
                    //baseDir_A00_A_K001.OpenOnDefaultApp();

                    //baseDir_A00_A_K001
                    //    .GetChildFileInfo($@"input.xlsx")
                    //    .ExcelSheets2Csvs(null);

                }

                // L101_PlayVideo
                {
                    //var baseDir_A00_A_L101 = baseDir_A00_A.GetChildDirectoryInfo($@"L101_PlayVideo").CreateAndPipe();
                    //baseDir_A00_A_L101.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_L101.GetChildFileInfo($@"input.mp4");
                    //input.PlayVideo_OnThread();
                    //input.PlayVideo_OnDefaultApp();

                }

                // M101_PlaySound
                {
                    //var baseDir_A00_A_M101 = baseDir_A00_A.GetChildDirectoryInfo($@"M101_PlaySound").CreateAndPipe();
                    //baseDir_A00_A_M101.OpenOnDefaultApp();

                    //var input = baseDir_A00_A_M101.GetChildFileInfo($@"input.mp3");

                    ////input.PlaySound_OnDefaultApp();

                    //input.PlaySound_OnConsole();

                    ////var device = input.PlaySound_OnConsole(true);
                    ////Thread.Sleep(3000);
                    ////device.Stop();
                }

            }

            // B_UsefulClasses
            {
                var baseDir_A00_B = baseDir_A00.GetChildDirectoryInfo($@"B_UsefulClasses");

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
                var baseDir_A00_C = baseDir_A00.GetChildDirectoryInfo($@"C_AwesomeModels");

                // A_TimeHistoryModel
                {
                    //var baseDir_C_A = baseDir_A00_C.GetChildDirectoryInfo($@"A_TimeHistoryModel").CreateAndPipe();
                    //baseDir_C_A.OpenOnDefaultApp();

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

                    }

                }

            }

            // D_ExternalAPIControllers
            {
                var baseDir_A00_D = baseDir_A00.GetChildDirectoryInfo($@"D_ExternalAPIControllers");

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

        }

        // A01_PythonAndNumerics
        {
            var baseDir_A01 = BASE_DIR.GetChildDirectoryInfo($@"A01_PythonAndNumerics");

            // A_ChainableExtensions
            {
                var baseDir_A01_A = baseDir_A01.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // F101_DrawGraph_OnPyPlot
                {
                    //var baseDir_A01_A_F101 = baseDir_A01_A.GetChildDirectoryInfo($@"F101_DrawGraph_OnPyPlot").CreateAndPipe();
                    //baseDir_A01_A_F101.OpenOnDefaultApp();
                    //var output = baseDir_A01_A_F101.GetChildFileInfo("output.png");

                    //PythonController.Initialize();
                    //new TimeHistory()
                    //{
                    //    t = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    //    x = new double[] { 10, 14, 15, 13, 17, 18, 16, 11, 12, 19 },
                    //}
                    //.DrawGraph_OnPyPlot(output, "t", "x")
                    //.OpenOnDefaultApp();

                }

            }

            // C_AwesomeModels
            {
                var baseDir_A01_C = baseDir_A01.GetChildDirectoryInfo($@"C_AwesomeModels");

                // 3001_GaussianProcessRegression
                {
                    //var baseDir_A01_3001 = baseDir_A01_C.GetChildDirectoryInfo($@"3001_GaussianProcessRegression").CreateAndPipe();
                    //baseDir_A01_3001.OpenOnDefaultApp();

                    //PythonController.Initialize();
                    //GaussianProcessRegressionExecuter.RunExampleModel(baseDir_A01_3001.GetChildFileInfo("gpr.png"));

                }

            }

            // D_ExternalAPIControllers
            {
                var baseDir_A01_D = baseDir_A01.GetChildDirectoryInfo($@"D_ExternalAPIControllers");

                // D004_Python
                {
                    //var baseDir_A01_D004 = baseDir_A01_D.GetChildDirectoryInfo($@"D004_Python").CreateAndPipe();
                    //baseDir_A01_D004.OpenOnDefaultApp();

                    //var output = baseDir_A01_D004.GetChildDirectoryInfo($@"output");
                    //PythonController.PythonPath = @"C:\Python310";
                    //PythonController.DllName = @"python310.dll";
                    //PythonController.AdditionalPath = new List<string> { @"C:\Users\XXXX\YourPythonCodeDirectory" };
                    //PythonController.Initialize();

                    //dynamic np = PythonController.Import("numpy");
                    //Console.WriteLine($"np.cos(np.pi/4) = {np.cos(np.pi / 4)}");


                    //PythonController.PythonExample_WithStringInvoke();
                    //PythonController.PythonExample_WithDynamicInvoke();

                    //PythonController.Shutdown();

                }

            }

            // E_Wrappers
            {
                var baseDir_A01_E = baseDir_A01.GetChildDirectoryInfo($@"E_Wrappers");

                // E001_NumpyWrapper
                {
                    //var baseDir_A01_E001 = baseDir_A01_E.GetChildDirectoryInfo($@"E001_NumpyWrapper").CreateAndPipe();
                    //baseDir_A01_E001.OpenOnDefaultApp();

                    //var output = baseDir_A01_E001.GetChildDirectoryInfo($@"output");


                }

                // E002_PyPlotWrapper
                {
                    //var baseDir_A01_E002 = baseDir_A01_E.GetChildDirectoryInfo($@"E002_PyPlotWrapper").CreateAndPipe();
                    //baseDir_A01_E002.OpenOnDefaultApp();
                    //var output = baseDir_A01_E002.GetChildDirectoryInfo($@"output");
                    //PythonController.Initialize();


                    //PyPlotWrapper.LinePlot.RunExampleModel(output.GetChildFileInfo("line.png"));

                    //PyPlotWrapper.Bar3DPlot.RunExampleModel(output.GetChildFileInfo("bar3d.png"));

                    //PyPlotWrapper.SurfacePlot.RunExampleModel(output.GetChildFileInfo("surface.png"));

                    //PyPlotWrapper.GridHeatMapPlot.RunExampleModel(output.GetChildFileInfo("grid heatmap.png"));

                    //PyPlotWrapper.ContinuousHeatMapPlot.RunExampleModel(output.GetChildFileInfo("continuous heatmap.png"));

                    //PyPlotWrapper.QuiverPlot.RunExampleModel(output.GetChildFileInfo("quiver.png"));

                    //PyPlotWrapper.TextPlot.RunExampleModel(output.GetChildFileInfo("text.png"));

                    //PyPlotWrapper.FillBetweenPlot.RunExampleModel(output.GetChildFileInfo("fill between.png"));

                }

                // E701__360ImageCropperWrapper
                {
                    //var baseDir_A01_E701 = baseDir_A01_E.GetChildDirectoryInfo($@"E701__360ImageCropperWrapper").CreateAndPipe();
                    //baseDir_A01_E701.OpenOnDefaultApp();

                    //var inputImage = baseDir_A01_E701.GetChildFileInfo($@"input.jpg");
                    //var outputDir = baseDir_A01_E701.GetChildDirectoryInfo($@"output");
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

        // A02_AI
        {
            var baseDir_A02 = BASE_DIR.GetChildDirectoryInfo($@"A02_AI");

            // A_ChainableExtensions
            {
                var baseDir_A02_A = baseDir_A02.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // BB ImageToImage
                {
                    // BB01 AI_GetHigherResolutionImage
                    {
                        //var baseDir_A02_A_BB01 = baseDir_A02_A.GetChildDirectoryInfo("BB01_GetHigherResolutionImage").CreateAndPipe();
                        //baseDir_A02_A_BB01.OpenOnDefaultApp();

                        //PythonController.Initialize();

                        //// single
                        //baseDir_A02_A_BB01
                        //    .GetChildFileInfo($@"input\00.jpg")
                        //    .AI_GetHigherResolutionImage(null);

                        ////// loop
                        ////baseDir_A02_A_BB01
                        ////    .GetChildDirectoryInfo($@"input")
                        ////    .AI_GetHigherResolutionImage_Loop(null);

                        //PythonController.Shutdown();

                    }
                }
            }

            // B_UsefulClasses
            {
                var baseDir_A02_B = baseDir_A02.GetChildDirectoryInfo($@"B_UsefulClasses");



            }

            // C_AwesomeModels
            {
                var baseDir_A02_C = baseDir_A02.GetChildDirectoryInfo($@"C_AwesomeModels");

                // none
                {


                }

            }

            // D_ExternalAPIControllers
            {
                var baseDir_A02_D = baseDir_A02.GetChildDirectoryInfo($@"D_ExternalAPIControllers");

                // D001_OpenAIController
                {

                    //var baseDir_A02_D_D001 = baseDir_A02_D.GetChildDirectoryInfo($@"D001_OpenAIController").CreateAndPipe();
                    //baseDir_A02_D_D001.OpenOnDefaultApp();
                    //var output = baseDir_A02_D.GetChildDirectoryInfo($@"output");

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
                    ////    var GeneratedImageFile = baseDir_A02_D_D001.GetChildFileInfo($"GeneratedImage{i}.png");
                    ////    result[i].DownloadFile(GeneratedImageFile);
                    ////}

                    ////// ★ whisper
                    ////var input = baseDir_A02_D_D001.GetChildFileInfo("input voice.m4a");
                    ////var result = openAI.CallAudioTranscriptionsAsync(input).Result;
                    ////Console.WriteLine(result);

                    //// ★ chat gpt
                    //Console.Write("ChatGPTに送信：");
                    //var input = Console.ReadLine();
                    //var result = openAI.CallChatAsync(input!).Result;
                    //Console.WriteLine(result[0]);

                }

            }

            // E_Wrappers
            {
                var baseDir_A02_E = baseDir_A02.GetChildDirectoryInfo($@"E_Wrappers");

                // E001_WhisperCppWrapper
                {
                    //var baseDir_A02_E001 = baseDir_A02_E.GetChildDirectoryInfo($@"E001_WhisperCppWrapper");
                    //var input = baseDir_A02_E001.GetChildFileInfo($@"input.m4a");
                    //var output = baseDir_A02_E001.GetChildFileInfo($@"output.txt");

                    //var whisper = new WhisperCppWrapper(
                    //    designatedModelDir: new DirectoryInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\WhisperCpp"),
                    //    designatedMainExecuterFile: new FileInfo(@"..\..\..\..\..\9_Assets\Dlls\whisper.exe"),
                    //    designatedWhisperDllFile: new FileInfo(@"..\..\..\..\..\9_Assets\Dlls\whisper.dll")
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
                var baseDir_A02_X = baseDir_A02.GetChildDirectoryInfo($@"X_CheatSheet");

                // A001_CheatSheet_MLNetExampleSummary
                {
                    //var baseDir_A02_X_A001 = baseDir_A02_X.GetChildDirectoryInfo($@"A001_MLNetExampleSummary");
                    //var baseDir_A02_X_A001_Private = baseDir_A02_X_A001.GetChildDirectoryInfo($@"@Private");

                    //var runner = new MLNetExampleSummary(MLNetExampleScenario.B003_MultiClassClassification_MNIST, baseDir_A02_X_A001_Private);
                    ////runner.ExperimentTime_InSeconds = 60;
                    ////runner.RunPrediction();
                    //runner.RunAll();

                }
            }

        }

        // A10_Structure
        {
            var baseDir_A10 = BASE_DIR.GetChildDirectoryInfo($@"A10_Structure");

            // A_ChainableExtensions
            {
                var baseDir_A10_A = baseDir_A10.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // B001_RainflowCycleCounting
                {
                    //var baseDir_A10_A_B001 = baseDir_A10_A.GetChildDirectoryInfo("B001_RainflowCycleCounting").CreateAndPipe();
                    //baseDir_A10_A_B001.OpenOnDefaultApp();

                    //baseDir_A10_A_B001
                    //    .GetChildFileInfo($@"input3.csv")
                    //    .Rainflow(null, 4, 1 / 3d, false);

                    //baseDir_A10_A_B001
                    //    .Rainflow_Loop(null, 4, 1 / 3d, false);

                }

                // B002_RDTechnique
                {
                    //var baseDir_A10_A_B002 = baseDir_A10_A.GetChildDirectoryInfo("B002_RDTechnique").CreateAndPipe();
                    //baseDir_A10_A_B002.OpenOnDefaultApp();

                    //baseDir_A10_A_B002
                    //    .GetChildFileInfo(@$"input.csv")
                    //    .CalcRandomDecrement(null, 200)
                    //    ;

                }

                // B003_CreateAccFromCsv
                {
                    //var baseDir_A10_A_B003 = baseDir_A10_A.GetChildDirectoryInfo("B003_CreateAccFromCsv").CreateAndPipe();
                    //baseDir_A10_A_B003.OpenOnDefaultApp();

                    //baseDir_A10_A_B003
                    //    .GetChildFileInfo(@$"kobe L1.csv")
                    //    .CreateAccFromCsv_For_DynamicPro(null);

                }

                // F203_FFT
                {
                    //var baseDir_A10_A_F203 = baseDir_A10_A.GetChildDirectoryInfo("F203_FFT").CreateAndPipe();
                    //baseDir_A10_A_F203.OpenOnDefaultApp();
                    //baseDir_A10_A_F203
                    //    .GetChildFileInfo($@"input.csv")
                    //    .GetTimeHistoryFromFile(new string[] { "t", "x" })
                    //    .FFT("x")
                    //    .SaveToCsv();

                }

                // F204_Wavelet
                {
                    //var baseDir_A10_A_F204 = baseDir_A10_A.GetChildDirectoryInfo("F204_Wavelet").CreateAndPipe();
                    //baseDir_A10_A_F204.OpenOnDefaultApp();

                    //baseDir_A10_A_F204
                    //    .GetChildFileInfo($@"input.csv")
                    //    .GetTimeHistoryFromFile(new string[] { "x" })
                    //    .Wavelet("x")
                    //    .SaveToCsv();

                }

            }

            // C_AwesomeModels
            {
                var baseDir_A10_C = baseDir_A10.GetChildDirectoryInfo(@$"C_AwesomeModels");

                // B001_ElastoplasticAnalysis
                {
                    var baseDir_A10_B001 = baseDir_A10_C.GetChildDirectoryInfo(@$"B001_EPAnalysis");

                    // XX_All
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

                        //    var saveDir = baseDir_A10_B001.GetChildDirectoryInfo(@$"output");
                        //    result.SaveToCsv(saveDir);
                        //    result.DrawGraph_OnPlotly("f", "x");
                        //}

                        // combined
                        {
                            //var waveAnalysisModelList = new List<ITimeHistoryAnalysisModel>
                            //{
                            //    new NewmarkBetaModel(0.25),
                            //    //new WilsonTheta(1.4,0.25),
                            //    //new NigamJenningsModel(),
                            //};

                            //var waveCsv = baseDir_A10_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
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

                            //var waveCsv = baseDir_A10_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
                            //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                            ////var waveAnalysisModel = new NewmarkBetaModel(0.25);
                            //var waveAnalysisModel = new NigamJenningsModel();

                            //var resultSet = SDoFModel.CalcResponseSpectrum(TList, hList, wave, waveAnalysisModel, ep);
                            //resultSet.SaveToExcel(waveCsv.Directory);
                        }

                    }

                    // 00_StructureModels
                    {
                        var baseDir_A10_B001_00 = baseDir_A10_B001.GetChildDirectoryInfo($@"00_StructureModels").CreateAndPipe();

                        // 20_SimpleBeamModel
                        {
                            //// Define IO paths
                            //var baseDir_A10_B001_00_20 = baseDir_A10_B001_00.GetChildDirectoryInfo($@"20_SimpleBeamModel").CreateAndPipe();
                            //var inputDir = baseDir_A10_B001_00_20.GetChildDirectoryInfo($@"input");
                            //var outputDir = baseDir_A10_B001_00_20.GetChildDirectoryInfo($@"output");
                            //baseDir_A10_B001_00_20.OpenOnDefaultApp();

                            ////Run Example
                            //SimpleBeamModel.RunExampleModel1(inputDir, outputDir);

                        }

                    }

                    // 01_MaterialModels
                    {
                        var baseDir_A10_B001_01 = baseDir_A10_B001.GetChildDirectoryInfo($@"01_MaterialModels").CreateAndPipe();

                        // 00_Steel
                        {
                            var baseDir_A10_B001_01_00 = baseDir_A10_B001_01.GetChildDirectoryInfo($@"00_Steel").CreateAndPipe();
                            var input = baseDir_A10_B001_01_00.GetChildFileInfo($@"SteelTrue.csv");

                            var EP = TimeHistory.FromCsv(input);
                            var steel = new Material.Steel("Steel_001", EP[0], EP[1], StressType.True);

                            var sign = steel.Steps.Select(s => s.Sig_n).ToArray();
                            var sigt = steel.Steps.Select(s => s.Sig_t).ToArray();
                            var epsn = steel.Steps.Select(s => s.Eps_n).ToArray();
                            var epst = steel.Steps.Select(s => s.Eps_t).ToArray();

                            PythonController.Initialize();
                            LinePlot.DrawSimpleGraph(epsn, sign);
                            LinePlot.DrawSimpleGraph(epst, sigt);

                        }
                    }

                    // 02_ElastoplasticCharacteristic
                    {
                        // newmark beta
                        {
                            //var model = SDoFModel.FromT(1, 0.03);

                            //var waveCsv = baseDir_A10_B001.GetChildFileInfo(@$"Hachinohe-NS.csv");
                            //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                            //var waveAnalysisModel = new NewmarkBetaModel(0.25);
                            //var result = model.Calc(wave, waveAnalysisModel);

                            //result.SaveToCsv();
                            //result.DrawGraph("x", "t");
                        }

                        // nigam jennings
                        {
                            //var model = SDoFModel.FromT(1, 0.03);

                            //var waveCsv = new FileInfo(@$"{baseDir_A10_B001}\Hachinohe-NS.csv");
                            //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                            //var waveAnalysisModel = new NigamJenningsModel();
                            //var result = model.Calc(wave, waveAnalysisModel);

                            //result.SaveToCsv();
                            //result.DrawGraph("x", "t");
                        }

                    }

                }

                // B002_RainflowCycleCounting
                {
                    //var baseDir_A10_C_B002 = baseDir_A10_C.GetChildDirectoryInfo($@"B002_RainflowCycleCounting");
                    //baseDir_A10_C_B002.OpenOnDefaultApp();
                    //var inputCsv = baseDir_A10_C_B002.GetChildFileInfo($@"input2.csv");

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
                    //var baseDir_A10_C_B003 = baseDir_A10_C.GetChildDirectoryInfo($@"B003_RDTechnique");
                    //var input = baseDir_A10_C_B003.GetChildFileInfo($@"input.csv");

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
                    // see A10_Structure > A_ChainableExtensions > F204_Wavelet
                }

                // B005_FFT
                {
                    // see A10_Structure > A_ChainableExtensions > F203_FFT
                }

                // B999_BeamCyclicLoading_Keep
                {
                    //// Define IO paths
                    //var baseDir_A10_C_F002 = baseDir_A10_C.GetChildDirectoryInfo($@"B999_BeamCyclicLoading_Keep").CreateAndPipe();
                    //var inputDir = baseDir_A10_C_F002.GetChildDirectoryInfo($@"input");
                    //var outputDir = baseDir_A10_C_F002.GetChildDirectoryInfo($@"output");
                    //baseDir_A10_C_F002.OpenOnDefaultApp();

                    ////Run Example
                    //BeamCyclicLoading.RunExampleModel1(inputDir, outputDir);

                }

                // F001_MatrixDisplacementMethod
                {
                    //// Define IO paths
                    //var baseDir_A10_C_F001 = baseDir_A10_C.GetChildDirectoryInfo($@"F001_MatrixDisplacementMethod").CreateAndPipe();
                    //var output1 = baseDir_A10_C_F001.GetChildFileInfo($@"output1.png");
                    //var output2 = baseDir_A10_C_F001.GetChildFileInfo($@"output2.png");
                    //baseDir_A10_C_F001.OpenOnDefaultApp();

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
                var baseDir_A10_X = baseDir_A10.GetChildDirectoryInfo(@$"X_SoftwareHelpers");

                // X001_SNAPHelper
                {
                    // KeepClosingExcel
                    {
                        //SNAPHelper.KeepClosingExcel(5000);

                    }

                }

                // X002_DynamicProHelper
                {
                    //var baseDir_A10_X_X002 = baseDir_A10_X.GetChildDirectoryInfo(@$"X002_DynamicProHelper");
                    //baseDir_A10_X_X002.OpenOnDefaultApp();

                    //// CreateAccFromCsv
                    //var input = baseDir_A10_X_X002.GetChildFileInfo($@"kobe L1.csv");
                    //var output = baseDir_A10_X_X002.GetChildFileInfo($@"kobe L1.acc");
                    //DynamicProHelper.CreateAccFromCsv(input, output);

                }

                // X003_KnetHelper
                {
                    //var baseDir_A10_X_X003 = baseDir_A10_X.GetChildDirectoryInfo(@$"X003_KnetHelper");
                    //baseDir_A10_X_X003.OpenOnDefaultApp();

                    ////// KnetAccData
                    //var input = new FileInfo(@"C:\Users\aki32\Dropbox\PC\Desktop\新しいフォルダー\TKY0022203162336\TKY0022203162336.EW");
                    //var aaa = new KNetHelper.KNetAccData(input);

                }
            }

        }

        // A11_Research
        {
            var baseDir_A11 = BASE_DIR.GetChildDirectoryInfo($@"A11_Research");

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
            var baseDir_AXX = BASE_DIR.GetChildDirectoryInfo($@"AXX_Hobby");

            // A_ChainableExtensions
            {
                var baseDir_AXX_A = baseDir_AXX.GetChildDirectoryInfo($@"A_ChainableExtensions");

                // E001_Steganography
                {
                    //var baseDir_AXX_A_E001 = baseDir_AXX_A.GetChildDirectoryInfo($@"E001_Steganography").CreateAndPipe();
                    //baseDir_AXX_A_E001.OpenOnDefaultApp();

                    //var input1_show = baseDir_AXX_A_E001.GetChildFileInfo($@"input1_show.png");

                    //var input2_hide = baseDir_AXX_A_E001.GetChildFileInfo($@"input2_hide.png");
                    //var input2_encrypted = baseDir_AXX_A_E001.GetChildFileInfo($@"input2_encrypted.png");
                    //var input2_decrypted = baseDir_AXX_A_E001.GetChildFileInfo($@"input2_decrypted.png");
                    //input1_show.SteganographyEncryptImage(input2_hide, input2_encrypted);
                    //input2_encrypted.SteganographyDecryptImage(input2_decrypted);

                    //var input3_hide = baseDir_AXX_A_E001.GetChildFileInfo($@"input3_hide.png");
                    //var input3_encrypted = baseDir_AXX_A_E001.GetChildFileInfo($@"input3_encrypted.png");
                    //var input3_decrypted = baseDir_AXX_A_E001.GetChildFileInfo($@"input3_decrypted.png");
                    //input1_show.SteganographyEncryptImage(input3_hide, input3_encrypted);
                    //input3_encrypted.SteganographyDecryptImage(input3_decrypted);

                }
            }
        }

    }
}
