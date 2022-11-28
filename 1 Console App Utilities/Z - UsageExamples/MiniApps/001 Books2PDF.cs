using Aki32_Utilities.UsefulClasses;
using Aki32_Utilities.General;

using Org.BouncyCastle.Asn1.UA;

namespace Aki32_Utilities.UsageExamples;
public partial class MiniApps
{

    /// <summary>
    /// keep taking screenshot of some dataset and save as PDF
    /// </summary>
    public static void Books2PDF(DirectoryInfo targetDirectory, int PageCount, int TimePerPageMilliSeconds = 2000)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref targetDirectory!, true, null!);


        // main
        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ 座標入力\r\n");

        var pointNames = new string[] {
            "撮影対象の左上（┏  ）",
            "撮影対象の右下（┛  ）",
            "先送りボタン（→）",
        };

        var ps = IODeviceExtension.GetMouseCursorPositionConversationallyForMany(pointNames, ConsoleKey.Escape, true);

        var UL = ps[0];
        var BR = ps[1];
        var ProceedButton = ps[2];


        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ 撮影\r\n");

        Console.WriteLine("マウスカーソルを動かさないでください。");

        var progress = new ProgressManager(PageCount);
        for (int i = 0; i < PageCount; i++)
        {
            ChainableExtensions.SaveScreenShot(targetDirectory, UL, BR);
            IODeviceExtension.MouseCursorMoveAndClick(ProceedButton);
            progress.WriteCurrentState(i);
            Thread.Sleep(TimePerPageMilliSeconds);
        }
        progress.WriteDone();


        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ PDF化\r\n");

        targetDirectory.Images2PDF(null);


        // post process
        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

    }

}