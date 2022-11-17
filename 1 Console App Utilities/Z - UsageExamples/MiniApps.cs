using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aki32_Utilities.Extensions;

using Org.BouncyCastle.Asn1.UA;

namespace Aki32_Utilities;
public class MiniApps
{
    /// <summary>
    /// keep taking screenshot of some dataset and save as PDF
    /// </summary>
    public static void Books2PDF(DirectoryInfo targetDirectory, int PageCount, int TimePerPageMilliSeconds = 2000)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref targetDirectory!, true, null!);


        // main
        Console.WriteLine("\r\n★★★★★ 座標入力（ひとつ前に戻るには Escape キーを押す。）\r\n");

        var reasons = new string[] {
            "撮影対象の左上（┏  ）",
            "撮影対象の右下（┛  ）",
            "先送りボタン（→）",
        };
        var ps = new Point[reasons.Length];

        for (int i = 0; i < reasons.Length; i++)
        {
            try
            {
                ps[i] = IODeviceExtension.GetMouseCursorPositionConversationally(ConsoleKey.Escape, reasons[i]);
            }
            catch (OperationCanceledException)
            {
                i -= 2;
                i = Math.Max(-1, i);
                continue;
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        for (int i = 0; i < reasons.Length; i++)
            Console.WriteLine($"{reasons[i]}:{ps[i]}");
        var UL = ps[0];
        var BR = ps[1];
        var ProceedButton = ps[2];


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 撮影\r\n");

        for (int i = 0; i < PageCount; i++)
        {
            OwesomeExtensions.SaveScreenShot(targetDirectory, UL, BR);
            IODeviceExtension.MouseCursorMoveAndClick(ProceedButton);
            Console.WriteLine($"{i + 1}/{PageCount} 枚完了");
            Thread.Sleep(TimePerPageMilliSeconds);
        }


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ PDF化\r\n");

        targetDirectory.Images2PDF(null);


        // post process
        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

    }
}