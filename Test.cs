using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32_Utilities;
public partial class Program
{
    static void Test()
    {
        {
            // A Extensions (methods)
            {
                var baseDir = @"..\..\..\# TestModel\A Extensions";

                // B001 CollectFiles
                {
                    //var input = new DirectoryInfo($@"{baseDir}\B001 CollectFiles\input");
                    //input.CollectFiles(null, @"^.*a\.txt$");
                    ////input.CollectFiles(null, @"^[0-9]*\\a\.txt$");
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
                    //var input = new DirectoryInfo($@"{baseDir}\B004 CopyTo\input");
                    //var output = new DirectoryInfo($@"{baseDir}\B004 CopyTo\output");
                    //input.CopyTo(output);
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

                // C001 ReadCsv, C002 SaveCsv, F001 Transpose
                {
                    //var innerBaseDir = $@"{baseDir}\C001 ReadCsv, SaveCsv";
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

                // C004 ExtractCsvColumns
                {
                    //var input = new FileInfo($@"{baseDir}\C004 ExtractCsvColumns\input.csv");
                    //input.ExtractCsvColumns(null, new int[] { 0, 3 }, 0, "t,x");
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

                // D001 MergeAllLines
                {
                    //var input = new DirectoryInfo($@"{baseDir}\D001 MergeAllLines\input");
                    //input.MergeAllLines(null);
                }

                // E001 CropImage
                {
                    //var input = new FileInfo($@"{baseDir}\E001 CropImage\input.bmp");
                    //var crops = new OwesomeExtensions.CropSize[]
                    //{
                    //    new OwesomeExtensions.CropSize(0.13, 0.13, 0.13, 0.13),
                    //    new OwesomeExtensions.CropSize(0, 0, 0.3, 0.3),
                    //    new OwesomeExtensions.CropSize(0.3, 0, 0, 0.3),
                    //    new OwesomeExtensions.CropSize(0.3, 0.3, 0, 0),
                    //    new OwesomeExtensions.CropSize(0, 0.3, 0.3, 0),
                    //};

                    //input.CropImage(null, crops);
                    ////input.CropImage(null, new OwesomeExtensions.CropSize(0.13, 0.13, 0.13, 0.13));
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

                // F002 To2DArray, ToJaggedArray
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

                // F003 Encrypt, Decrypt
                {
                    //var input = new FileInfo($@"{baseDir}\F003 Crypt\input.txt");
                    //input
                    //    .Encrypt(null, "aiueo")
                    //    .Decrypt(null, "aiueo")
                    //    ;
                }

                // G001 PDFPageCount
                {
                    //var input = new DirectoryInfo($@"{baseDir}\G001 PDFPageCount\input");
                    //input.PDFPageCount();
                }

            }

            // B Extensions (class)
            {
                // 001 ConsoleExtension
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

                // 002 IODeviceExtension
                {
                    //// Move Mouse Cursor 
                    //IODeviceExtension.MouseCursorMoveTo(4600, 700);

                    //// Mouse Left Click
                    //IODeviceExtension.MouseClick();

                    //// Mouse Wheel Click
                    //IODeviceExtension.MouseClick(IODeviceExtension.IODeviceButton.MouseMiddle);

                    //// Wheel Scroll
                    //IODeviceExtension.MouseWheelScroll(true);

                    ////Keyboard Input
                    //IODeviceExtension.SendKeys("abc ABC"); // "abc ABC"
                    //IODeviceExtension.SendKey(65); // "a"
                    //IODeviceExtension.SendKey(ConsoleKey.V, withCtrl: true); // Ctrl+V
                    //IODeviceExtension.SendKey(ConsoleKey.V, withCtrl: true, withShift:true); // Ctrl+Shift+V
                    //IODeviceExtension.SendKey(ConsoleKey.E, withAlt: true); // Alt+E
                    //IODeviceExtension.SendKey(ConsoleKey.Spacebar, withShift: true, withAlt: true); // Alt Shift Space


                    //// Get Mouse Cursor Position 
                    //Console.CursorVisible = false;
                    //while (!Console.KeyAvailable)
                    //{
                    //    var p = IODeviceExtension.GetMouseCursorPosition();
                    //    ConsoleExtension.ClearCurrentConsoleLine();
                    //    Console.Write(p);
                    //    Thread.Sleep(10);
                    //}
                    //Console.CursorVisible = true;

                }

            }

            // C OwesomeModels
            {
                // A TimeHistoryModel
                {
                    var baseDir = @"..\..\..\# TestModel\C OwesomeModels\A TimeHistoryModel";

                    // TimeHistory
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

                    }

                    // FFT
                    {
                        //var input = new FileInfo($@"{baseDir}\FFT\input.csv");
                        //TimeHistory
                        //    .FromCsv(input, new string[] { "t", "x" })
                        //    .FFT("x").Result
                        //    .SaveToCsv();
                    }

                }
            }

            // D ExternalAPIControllers
            {
                // 001 LINEController
                {
                    //var accessToken = ""; // LINE Notify
                    //var line = new LINEController(accessToken);
                    //line.SendMessageAsync(@"Hello LINE from C#. 日本語");
                }

                // 002 GitController
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

        }

    }

}
