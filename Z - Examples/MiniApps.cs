using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aki32_Utilities.Extensions;

namespace Aki32_Utilities;
public class MiniApps
{
    /// <summary>
    /// keep taking screenshot of some dataset
    /// </summary>
    public static void Books2PDF()
    {
        var lastPosition = new PointF(0, 0);

        Console.WriteLine("★★★★★★★★★★★★★★★ 定義");

        Console.WriteLine("※未実装");


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 座標入力\r\n");

        var reasons = new string[] {
            "撮影対象の左上",
            "撮影対象の右上",
            "先送りボタン",
        };
        var ps = new PointF[reasons.Length];

        for (int i = 0; i < reasons.Length; i++)
        {
            Console.WriteLine($"\r\n{reasons[i]} の位置にカーソルを合わせて、Enterを押してください。");

            Console.CursorVisible = false;
            while (!Console.KeyAvailable)
            {
                lastPosition = IODeviceExtension.GetMouseCursorPosition();
                ConsoleExtension.ClearCurrentConsoleLine();
                Console.Write(lastPosition);
                Thread.Sleep(10);
            }
            Console.CursorVisible = true;
            Console.ReadLine();
            ps[i] = lastPosition;
        }

        Console.WriteLine();
        for (int i = 0; i < reasons.Length; i++)
            Console.WriteLine($"{reasons[i]}:{ps[i]}");


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 撮影\r\n");

        Console.WriteLine("※未実装");


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ PDF化\r\n");

        Console.WriteLine("※未実装");


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

    }
}
