﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        var lastPosition = new Point(0, 0);

        Console.WriteLine("★★★★★★★★★★★★★★★ 定義");

        Console.WriteLine("※未実装");

        var PageCount = 1000;
        var TimePerPageMilliSeconds = 1000;




        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 座標入力\r\n");

        var reasons = new string[] {
            "撮影対象の左上",
            "撮影対象の右上",
            "先送りボタン",
        };
        var ps = new Point[reasons.Length];

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

        var UL = ps[0];
        var BR = ps[1];
        var ProceedButton = ps[1];


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 撮影\r\n");

        Console.WriteLine("※未実装");
        Console.WriteLine("Altキー長押しで処理を中断します。");
        //OwesomeExtensions.SaveScreenShot(output, new Point(0, 0), new Point(1000, 1000));


        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ PDF化\r\n");

        Console.WriteLine("※未実装");




        Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

    }















    //private async Task ProcessAll(int proceedCount, Point ul, Point br, Point pb, string saveDirPath)
    //{
    //    Inform("★★★★★ 処理開始");

    //    if (!Directory.Exists(saveDirPath))
    //    {
    //        Directory.CreateDirectory(saveDirPath);
    //        Inform($"作成成功：{saveDirPath}");
    //    }

    //    await MoveCursorOnProceedButton(pb, true);

    //    for (int i = 0; i < proceedCount; i++)
    //    {
    //        try
    //        {
    //            var saveFileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".png";
    //            var saveFilePath = Path.Combine(saveDirPath, saveFileName);
    //            GetAndSaveScreenshot(ul, br, saveFilePath);

    //            Inform($"保存成功：{i + 1,4}枚目／{saveFileName}");
    //        }
    //        catch (Exception ex)
    //        {
    //            Inform($"保存失敗：{i + 1,4}枚目／{ex.Message}");
    //        }

    //        await PressProceedButton(pb);

    //        await Task.Delay(TimePerPage);

    //        if ((ModifierKeys & Keys.Alt) == Keys.Alt)
    //        {
    //            Inform("★★★★★ 処理終了（Altキーの長押しを検知）");
    //            return;
    //        }
    //    }

    //    Inform("★★★★★ 処理終了（指定ページ数に到達）");
    //}




}