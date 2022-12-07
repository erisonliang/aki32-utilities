﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.ExternalAPIControllers;
using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.OwesomeModels;
using Aki32Utilities.ConsoleAppUtilities.OwesomeModels.ChainableExtensions;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

using ClosedXML;

using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;

using Newtonsoft.Json;

using Org.BouncyCastle.Asn1.X509.Qualified;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
public class TestHelper
{

    public static void All()
    {
        // A - ChainableExtensions
        {
            // _ - Base
            {
                var baseDir = $@"..\..\..\# TestModel\A - ChainableExtensions\_ - Base";

            }

            // A - General
            {

                var baseDir = $@"..\..\..\# TestModel\A - ChainableExtensions\A - General";

                // B001 CollectFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir}\B001 CollectFiles\input");
                    //input.CollectFiles(null, @"^.*a\.txt$", @"^.*3.*$");
                    //input.CollectFiles(null, @"^.*a\.txt$");
                    //input.CollectFiles(null, @"^.*[0-9]*\\a\.txt$");
                }

                // B002 MakeFilesFromCsv
                {
                    //var inputList = new FileInfo($@"{baseDir}\B002 MakeFilesFromCsv\list.csv");
                    //var inputTemp = new FileInfo($@"{baseDir}\B002 MakeFilesFromCsv\template.docx");
                    //inputList.MakeFilesFromCsv(null, inputTemp);
                }

                // B003 MoveTo
                {
                    //var input = new DirectoryInfo($@"{baseDir}\B003 MoveTo\input");
                    //var keep = new DirectoryInfo($@"{baseDir}\B003 MoveTo\keep");
                    //var output = new DirectoryInfo($@"{baseDir}\B003 MoveTo\output");
                    //input.MoveTo(output);
                }

                // B004 CopyTo
                {
                    // directry-wise
                    //var input = new DirectoryInfo($@"{baseDir}\B004 CopyTo\input");
                    //var output = new DirectoryInfo($@"{baseDir}\B004 CopyTo\output");
                    //input.CopyTo(output);

                    // file
                    //var input = new FileInfo($@"{baseDir}\B004 CopyTo\a.txt");
                    //input.CopyTo();
                }

                // B005 RenameFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir}\B005 RenameFiles\input");
                    //var output = new DirectoryInfo($@"{baseDir}\B005 RenameFiles\output");
                    //input.CopyTo(output);
                    //// auto
                    //output.RenameFiles();
                    ////// append
                    ////output.RenameFiles_Append("xxx*xxx");
                    ////// replace
                    ////output.RenameFiles_Replace(("ABC", "abc"), ("XYZ", "xyz"));
                }

                // B007 Zip
                {
                    //var input = new DirectoryInfo($@"{baseDir}\B007 Zip\input");
                    //input
                    //    .Zip(null)
                    //    .Unzip(null);
                }

                // B102 ReadObjectFromLocalXXX, J101 SaveAsXXX
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
                    //    var outputFile = new FileInfo($@"{baseDir}\B102 ReadObjectToLocal\output.json");

                    //    testClassToWrite.SaveAsJson(outputFile, true);

                    //    var testClassRead = outputFile.ReadObjectFromLocalJson<UtilTestClass1>();

                    //}

                    //// xml
                    //{
                    //    var outputFile = new FileInfo($@"{baseDir}\B102 ReadObjectToLocal\output.xml");

                    //    testClassToWrite.SaveAsXml(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalXml<UtilTestClass1>();

                    //}

                    //// csv
                    //{
                    //    var outputFile = new FileInfo($@"{baseDir}\B102 ReadObjectToLocal\output.csv");

                    //    var testClassListToWrite = new List<UtilTestClass1> { testClassToWrite };
                    //    testClassListToWrite.SaveAsCsv(outputFile);

                    //    var testClassRead = outputFile.ReadObjectFromLocalCsv<UtilTestClass1>();

                    //}

                }

                // C004 ExtractCsvColumns
                {
                    //new FileInfo($@"{baseDir}\C004 ExtractCsvColumns\input.csv")
                    //    .ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");

                    //new DirectoryInfo($@"{baseDir}\C004 ExtractCsvColumns")
                    //    .ExtractCsvColumnsForMany_Loop(null, 0, ("a", new int[] { 0, 3 }, "t,x"), ("b", new int[] { 0, 4 }, "t,y"));
                }

                // C005 CollectCsvColumns
                {
                    //var input = new DirectoryInfo($@"{baseDir}\C005 CollectCsvColumns\input");
                    //input.CollectCsvColumns(null, 3);
                }

                // C006 Csvs2ExcelSheets
                {
                    //var input = new DirectoryInfo($@"{baseDir}\C006 Csvs2ExcelSheets\input");
                    //input.Csvs2ExcelSheets(null);
                }

                // C101 ReadCsv, F201 Transpose2DArray, C103 SaveCsv
                {
                    //var innerBaseDir = $@"{baseDir}\C101 ReadCsv, SaveCsv";
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

                // D001 MergeAllLines
                {
                    //var input = new DirectoryInfo($@"{baseDir}\D001 MergeAllLines\input");
                    //input.MergeAllLines(null);
                }

                // E001 CropImage
                {
                    //var input = new FileInfo($@"{baseDir}\E001 CropImage\input.bmp");
                    //var crops = new OwesomeExtensions.Thickness[]
                    //{
                    //    new OwesomeExtensions.Thickness(0.13, 0.13, 0.13, 0.13),
                    //    new OwesomeExtensions.Thickness(0, 0, 0.3, 0.3),
                    //    new OwesomeExtensions.Thickness(0.3, 0, 0, 0.3),
                    //    new OwesomeExtensions.Thickness(0.3, 0.3, 0, 0),
                    //    new OwesomeExtensions.Thickness(0, 0.3, 0.3, 0),
                    //};

                    //input.CropImageForMany(null, crops);
                    ////input.CropImage(null, new OwesomeExtensions.Thickness(0.13, 0.13, 0.13, 0.13));
                }

                // E002 ConvertImageColor
                {
                    //var input = new FileInfo($@"{baseDir}\E002 ConvertImageColor\input.png");

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
                    //var input = new DirectoryInfo($@"{baseDir}\E004 Images2PDF");
                    //input.Images2PDF(null);
                }

                // E005 Images2Video
                {
                    //var input = new DirectoryInfo($@"{baseDir}\E005 Images2Video");
                    //input.Images2Video(null, 3);
                }

                // E006 ResizeImage
                {
                    //var input = new DirectoryInfo($@"{baseDir}\E006 ResizeImage");
                    //input.ResizeImage_Loop(null, new Size(100, 100));
                }

                // E007 DistortImage
                {
                    //var input5 = new FileInfo($@"{baseDir}\E007 DistortImage\5.png");
                    //var output510 = new FileInfo($@"{baseDir}\E007 DistortImage\5-1-0.png");
                    //var output520 = new FileInfo($@"{baseDir}\E007 DistortImage\5-2-0.png");
                    //var output521 = new FileInfo($@"{baseDir}\E007 DistortImage\5-2-1.png");
                    //var output530 = new FileInfo($@"{baseDir}\E007 DistortImage\5-3-0.png");
                    //var output531 = new FileInfo($@"{baseDir}\E007 DistortImage\5-3-1.png");
                    //var output540 = new FileInfo($@"{baseDir}\E007 DistortImage\5-4-0.png");
                    //var output541 = new FileInfo($@"{baseDir}\E007 DistortImage\5-4-1.png");
                    //var output542 = new FileInfo($@"{baseDir}\E007 DistortImage\5-4-2.png");

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
                    //var input = new DirectoryInfo($@"{baseDir}\E008 AddTextToImage");
                    //input.AddTextToImageProportionally_Loop(null, "%FN", new PointF(0.9f, 0.8f),
                    //    fontSizeRatio: 0.1, alignRight: true);
                }

                // E102 SaveScreenShot
                {
                    //var output = new DirectoryInfo($@"{baseDir}\E102 SaveScreenShot");
                    //OwesomeExtensions.SaveScreenShot(output, new Point(0, 0), new Point(1000, 1000));
                }

                // F003 Encrypt, Decrypt
                {
                    //var input = new FileInfo($@"{baseDir}\F003 Crypt\input.txt");
                    //input
                    //    .Encrypt(null, "aiueo")
                    //    .Decrypt(null, "aiueo")
                    //    ;
                }

                // F202 To2DArray, ToJaggedArray
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
                    //    Console.WriteLine(string.Join(", ", line.Select(x => $"{x,3}")));

                    //var outputData1 = inputData.ConvertTo2DArray(999);
                    //Console.WriteLine("outputData1:");
                    //for (int i = 0; i < outputData1.GetLength(0); i++)
                    //{
                    //    for (int j = 0; j < outputData1.GetLength(1); j++)
                    //        Console.Write($"{outputData1[i, j],3}, ");
                    //    Console.WriteLine();
                    //}

                    //var outputData2 = outputData1.ConvertToJaggedArray();
                    //Console.WriteLine("outputData2:");
                    //foreach (var line in outputData2)
                    //    Console.WriteLine(string.Join(", ", line.Select(x => $"{x,3}")));
                }

                // G002 PDF2Images
                {
                    //var input = new FileInfo($@"{baseDir}\G002 PDF2Images\input.pdf");
                    //input.PDF2Images(null);
                }

                // G101 PDFPageCount
                {
                    //var input = new DirectoryInfo($@"{baseDir}\G101 PDFPageCount\input");
                    //input.PDFPageCount();
                }

                // H101 DownloadFile
                {
                    //var source = new Uri("https://raw.githubusercontent.com/aki32/aki32-utilities/main/1%20Console%20App%20Utilities/%23%20TestModel/A%20-%20ChainableExtensions/B%20-%20General/E001%20CropImage/input.bmp");
                    //var output = new FileInfo($@"{baseDir}\H101 DownloadFile\output.bmp");
                    //source.DownloadFileAsync(output).Wait();

                }

                // H201 CallAPI
                {
                    //var source = new Uri("https://weather.tsukumijima.net/api/forecast/city/400040");
                    //var result = source.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

                    //Console.WriteLine(result);

                }


            }

            // B - UsefulClasses
            {
                var baseDir = $@"..\..\..\# TestModel\A - ChainableExtensions\B- UsefulClasses";

                // A001 XXXXXX
                {






                }

                // A002 XXXXXX
                {




                }

            }

            // C - OwesomeModels
            {
                var baseDir = $@"..\..\..\# TestModel\A - ChainableExtensions\C - OwesomeModels";

                // A - TimeHistoryModel
                {
                    var baseDir_A = $@"{baseDir}\A - TimeHistoryModel";

                    // B001 FFT
                    {
                        //new FileInfo($@"{baseDir_A}\B001 FFT\input.csv")
                        //    .GetTimeHistoryFromFile(new string[] { "t", "x" })
                        //    .FFT("x").Result
                        //    .SaveToCsv();

                    }

                    // B002 DrawGraph
                    {
                        //new FileInfo($@"{baseDir_A}\B002 DrawGraph\input1.csv")
                        //    .DrawGraph("t", "v", TimeHistory.ChartType.Line);


                        //new FileInfo($@"{baseDir_A}\B002 DrawGraph\input1.csv")
                        //    .DrawGraph(0, 1, TimeHistory.ChartType.Line);


                        //new FileInfo($@"{baseDir_A}\B002 DrawGraph\input2.csv")
                        //    .DrawGraph_ForAll(TimeHistory.ChartType.Line);

                    }

                }

            }

            // E - SpecificPurposeModels
            {
                var baseDir = $@"..\..\..\# TestModel\A - ChainableExtensions\E - SpecificPurposeModels";

                // A - StructuralEngineering
                {
                    var baseDir_A = $@"{baseDir}\A - StructuralEngineering";

                    // A001 RainflowCycleCounting
                    {
                        //new FileInfo(Path.Combine(baseDir_A, "A001 RainflowCycleCounting", @"input3.csv"))
                        //    .Rainflow(null, 4, 1 / 3d, false);

                        //new DirectoryInfo(Path.Combine(baseDir_A, "A001 RainflowCycleCounting"))
                        //    .Rainflow_Loop(null, 4, 1 / 3d, false);

                    }

                    // A002 RDTechnique
                    {
                        //new FileInfo(Path.Combine(baseDir_A, "A002 RDTechnique", @"input.csv"))
                        //    .CalcRD(null, 200)
                        //    ;

                    }

                    // A003 CreateAccFromCsv
                    {
                        //new FileInfo(Path.Combine(baseDir_A, "A003 CreateAccFromCsv", @"kobe L1.csv"))
                        //    .CreateAccFromCsv_For_DynamicPro(null);

                    }

                }

                // B - MachineLearning
                {
                    var baseDir_B = $@"{baseDir}\B - MachineLearning";



                }
            }

        }

        // B - UsefulClasses
        {

            // 003 CommandPrompt
            {

                // objective
                {
                    //using var prompt = new CommandPrompt()
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

                    //var outupt = prompt.ResponseList.ToArray();

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

                    //var outupt = CommandPrompt.Execute(
                    //    realTimeConsoleWriteLineOutput: true,
                    //    omitCurrentDirectoryDisplay: true,
                    //    outputReceivedAction: null,
                    //    commands: commands);

                }

            }

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

        // C - OwesomeModels
        {
            var baseDir = $@"..\..\..\# TestModel\C - OwesomeModels";

            // A - TimeHistoryModel
            {
                var baseDir_A = $@"{baseDir}\A - TimeHistoryModel";

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
            var baseDir = $@"..\..\..\# TestModel\D - ExternalAPIControllers";

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
                //        // your credidential info (obtainable from https://github.com/settings/tokens)
                //        Username = "",
                //        Password = "",
                //    },
                //};

                //gc.Sync();
            }

        }

        // E - SpecificPurposeModels
        {
            var baseDir = $@"..\..\..\# TestModel\E - SpecificPurposeModels";

            // A - StructuralEngineering
            {
                var baseDir_A = $@"{baseDir}\A - StructuralEngineering";


                // B001 ElastoplasticAnalysis
                {
                    var baseDir_B001 = @$"{baseDir_A}\B001 EPAnalysis";

                    // newmark beta
                    {
                        //var model = SDoFModel.FromT(1, 0.03);

                        //var waveCsv = new FileInfo(@$"{baseDir_B001}\Hachinohe-NS.csv");
                        //var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

                        //var waveAnalysisModel = new NewmarkBetaModel(0.25);
                        //var result = model.Calc(wave, waveAnalysisModel);

                        //result.SaveToCsv();
                        //result.DrawLineGraph("x", "t");
                    }

                    // nigam jennings
                    {
                        //var model = SDoFModel.FromT(1, 0.03);

                        //var waveCsv = new FileInfo(@$"{baseDir_B001}\Hachinohe-NS.csv");
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

                        //    var saveDir = new DirectoryInfo(@$"{baseDir_B001}\output");
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

                            //var waveCsv = new FileInfo(@$"{baseDir_B001}\Hachinohe-NS.csv");
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

                            //var waveCsv = new FileInfo(@$"{baseDir_B001}\Hachinohe-NS.csv");
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
                    //var inputCsv = new FileInfo($@"{baseDir_A}\B002 RainflowCycleCounting\input3.csv");
                    ////var inputCsv = new FileInfo(@"C:\Users\aki32\Desktop\anaAll_beam.csv");

                    //var rainflow = RainflowCalculator.FromCsv(inputCsv);
                    //rainflow.CalcRainflow(4, 1 / 3d, false);
                    //rainflow.SaveResultHistoryToCsv();
                    //rainflow.SaveRainBranchesToCsv();
                }

                // B003 RDTechnique
                {
                    //// Define IO paths
                    //var input = new FileInfo($@"{baseDir_A}\B003 RDTechnique\input.csv");

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
                        //var input = new FileInfo($@"{baseDir_A}\X002 DynamicProHelper\kobe L1.csv");
                        //var output = new FileInfo($@"{baseDir_A}\X002 DynamicProHelper\kobe L1.acc");
                        //DynamicProHelper.CreateAccFromCsv(input, output);
                    }

                }

            }

            // B - MachineLearning
            {
                var baseDir_B = $@"{baseDir}\B - MachineLearning";



            }

            // C - Research
            {
                var baseDir_C = new DirectoryInfo($@"{baseDir}\C - Research");


                // A - ResearchManager
                {
                    //UtilConfig.ConsoleOutput_Contents = false;

                    //var localDir = new DirectoryInfo($@"{baseDir_C}\A - ResearchManager\@Private");
                    var localDir = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");

                    var research = new ResearchArticlesManager(localDir);
                    research.OpenDatabase();

                    // articles from j-stage
                    {
                        //var builder = new JStageArticleUriBuilder()
                        //{
                        //    Pubyearfrom = 2022,
                        //    Issn = ISSN.Architecture_Structure,
                        //    Count = 1000,
                        //    //Start = 1,
                        //};
                        //research.PullArticleInfo(builder);

                    }

                    // articles from cinii
                    {
                        //var builder = new CiNiiArticleUriBuilder()
                        //{
                        //    Count = 5,
                        //    ISSN = ISSN.Architecture_Structure,
                        //    FreeWord = "小振幅"
                        //};
                        //research.PullArticleInfo(builder);

                    }

                    // article from crossref
                    {
                        //var builder = new CrossRefArticleUriBuilder()
                        //{
                        //    DOI = "10.3130/aijs.87.822"
                        //};
                        //research.PullArticleInfo(builder);

                    }

                    // display
                    {
                        research.SaveDatabase(true);

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"★ {research.ArticleDatabase.Count} found in total");
                        Console.WriteLine();
                        //foreach (var article in research.ArticleDatabase)
                        //    Console.WriteLine($" + {article.Title_Japanese}");
                        //Console.WriteLine();
                        Console.WriteLine();

                        //research.ArticleDatabase.First(x => x.DOI == "10.3130/aijs.87.822").TryOpenPDF(research.PDFsDirectory);

                    }

                }
            }

        }

        // Z - MiniApps
        {
            var baseDir = $@"..\..\..\# TestModel\Z - UsageExample";

            // A001 Books2PDF
            {
                //var outputDir = new DirectoryInfo($@"{baseDir}\A001 Books2PDF");
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