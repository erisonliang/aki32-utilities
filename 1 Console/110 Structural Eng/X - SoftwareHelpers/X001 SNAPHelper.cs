using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static class SNAPHelper
{

    // ★★★★★★★★★★★★★★★ KeepClosingExcel

    private const int MAX_WAITING_TIME_CloseMainWindow = 5000;
    private const int MAX_WAITING_TIME_Killl = 5000;
    public static int TotalKillCount { get; set; } = 0;
    public static void KeepClosingExcel(int interval = 5000)
    {
        Console.WriteLine();
        Console.WriteLine("全てのエクセルウィンドウを閉じ続けます。");
        Console.WriteLine("このウィンドウ自体を閉じることで停止できます。");
        Console.WriteLine();
        Console.Write("エンターを押して開始：");
        Console.ReadLine();
        Console.WriteLine("開始しました！");
        Console.WriteLine();

        while (true)
        {
            Thread.Sleep(interval);
            Console.WriteLine("監視中…");
            CheckAndCloseExcelOnce();
        }

    }
    public static bool CheckAndCloseExcelOnce()
    {
        try
        {
            var ps = Process.GetProcessesByName("EXCEL");

            if (ps.Length == 0)
                return false;

            ConsoleExtension.WriteLineWithColor($"プロセス発見！ 個数 = {ps.Length}", ConsoleColor.Blue);
            foreach (var p in ps)
            {
                p.CloseMainWindow();
                p.WaitForExit(MAX_WAITING_TIME_CloseMainWindow);
                if (!p.HasExited)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(MAX_WAITING_TIME_Killl);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (p.HasExited)
                    ConsoleExtension.WriteLineWithColor($"プロセス終了！ ID = {p.Id}, 累計キル回数 = {++TotalKillCount}", ConsoleColor.Blue);
            }

            return true;
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"一時エラー：{ex}", ConsoleColor.Red);
        }

        return false;
    }

    // ★★★★★★★★★★★★★★★ 

}
