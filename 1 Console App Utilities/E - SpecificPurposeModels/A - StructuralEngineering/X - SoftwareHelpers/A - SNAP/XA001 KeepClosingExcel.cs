using System.Diagnostics;

namespace Aki32_Utilities.StructuralEngineering;
public static class KeepClosingExcel
{
    private const int WAIT_FOR_EXIT_TIME = 5000;

    public static int TotalKillCount = 0;

    public static bool CheckAndKillOnce()
    {
        try
        {
            var ps = Process.GetProcessesByName("EXCEL");

            if (ps.Length == 0)
                return false;

            Console.WriteLine($"プロセス発見！ 個数 = {ps.Length}");
            foreach (var p in ps)
            {
                p.CloseMainWindow();
                p.WaitForExit(WAIT_FOR_EXIT_TIME);
                if (p.HasExited)
                    Console.WriteLine($"プロセス終了！ ID = {p.Id}, 累計キル回数 = {++TotalKillCount}");
            }

            return true;
        }
        catch (Exception)
        {
            Console.WriteLine("一時エラー");
            return false;
        }
    }

    public static void CheckAndKillEndless(int interval = 5000)
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
            CheckAndKillOnce();
        }

    }

}
