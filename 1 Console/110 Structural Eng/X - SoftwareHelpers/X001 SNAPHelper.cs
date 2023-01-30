using System.Diagnostics;
using System.Reflection;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

using DocumentFormat.OpenXml.Wordprocessing;

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


    // ★★★★★★★★★★★★★★★ CreateWaveFile

    /// <summary>
    /// with cm/s
    /// </summary>
    /// <param name="outputFile"></param>
    /// <param name="accs"></param>
    /// <param name="dt"></param>
    /// <param name="name"></param>
    /// <param name="amax"></param>
    /// <param name="vmax"></param>
    /// <returns></returns>
    public static FileInfo CreateWaveFile(FileInfo outputFile, double[] accs, double dt, string name, double amax, double vmax)
    {
        // pre process
        var now = DateTime.Now;
        if (amax == 0)
            amax = accs.Max(x => Math.Abs(x));


        // main
        using var sr = new StreamWriter(outputFile.OpenWrite());
        sr.WriteLine(@$"VERSION=""{now:yyyy.MM.dd.0}""");
        sr.WriteLine(@$"FILENAME=""{name}""");
        sr.WriteLine(@$"HPTYPE=""0""");
        sr.WriteLine(@$"DIRECTION=""0""");
        sr.WriteLine(@$"DT=""{dt}""");
        sr.WriteLine(@$"UNITID=""0""");
        sr.WriteLine(@$"AMAX=""{amax}""");
        sr.WriteLine(@$"VMAX=""{vmax}""");
        sr.WriteLine(@$"TIME=""{dt * (accs.Length - 1)}""");
        sr.WriteLine(@$"DATA");
        foreach (var acc in accs)
            sr.WriteLine(acc);


        // post process
        return outputFile;

    }
    public static FileInfo CreateWaveFile(FileInfo outputFile, TimeHistory inputWave, string name, double amax, double vmax)
    {
        return CreateWaveFile(outputFile, inputWave.a, inputWave.TimeStep, name, amax, vmax);
    }
    public static FileInfo CreateWaveFile(DirectoryInfo outputDir, double[] accs, double dt, string name, double amax, double vmax)
    {
        Thread.Sleep(50);
        //var outputFile = new FileInfo(Path.Combine(outputDir.FullName, @$"{DateTime.Now:yyyyMMddHHmmssff}.wv"));
        var outputFile = new FileInfo(Path.Combine(outputDir.FullName, @$"{name}.wv"));
        return CreateWaveFile(outputFile, accs, dt, name, amax, vmax);
    }
    public static FileInfo CreateWaveFile(DirectoryInfo outputDir, TimeHistory inputWave, string name, double amax, double vmax)
    {
        return CreateWaveFile(outputDir, inputWave.a, inputWave.TimeStep, name, amax, vmax);
    }


    // ★★★★★★★★★★★★★★★ 

}
