using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.FSharp.Core;

using NAudio.Wave;

using OpenCvSharp.Dnn;

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


    // ★★★★★★★★★★★★★★★ SNAPWaveData

    public class SNAPWaveData
    {

        // ★★★★★★★★★★★★★★★ props

        // ★★★★★ from data

        [CsvIgnore]
        public TimeHistory Accs { get; set; }

        public string? VERSION { get; set; } = null;
        public string FILENAME { get; set; } = "";
        public int HPTYPE { get; set; } = 0;
        public int DIRECTION { get; set; } = 0;
        public int UNITID { get; set; } = 0;


        // ★★★★★ generated / calculated

        public double DT => Accs.TimeStep;
        public int TIME => (int)Accs.t.Last();

        private double __maxAcc = 0;
        public double MaxAcc
        {
            get
            {
                if (__maxAcc == 0)
                {
                    __maxAcc = Accs.ytt.Max(x => Math.Abs(x));
                }
                return __maxAcc;
            }
            set
            {
                __maxAcc = value;
            }
        }
        private double __maxVal = 0;
        public double MaxVel
        {
            get
            {
                if (__maxVal == 0)
                {
                    Accs.CalcIntegral_Simple("ytt", "yt");
                    __maxVal = Accs.yt.Max(v => Math.Abs(v));
                }
                return __maxVal;
            }
            set
            {
                __maxVal = value;
            }
        }


        // ★★★★★★★★★★★★★★★ inits

        public SNAPWaveData()
        {
            Accs = new TimeHistory();
        }
        public SNAPWaveData(TimeHistory accs)
        {
            // accs
            Accs = accs;
        }
        public SNAPWaveData(double[] accs, double dt)
        {
            // accs
            Accs = new TimeHistory(FILENAME)
            {
                t = Enumerable.Range(0, accs.Length).Select(t => t * dt).ToArray(),
                ytt = accs.ToArray()
            };
        }
        public SNAPWaveData(FileInfo originalWaveFile)
        {
            using var sr = new StreamReader(originalWaveFile.FullName);

            var meta = new List<string>();
            for (int i = 0; i < 10; i++)
                meta.Add(sr.ReadLine()!);

            // metas
            VERSION = meta[0][9..^1];
            FILENAME = meta[1][10..^1];
            HPTYPE = int.Parse(meta[2][8..^1]);
            DIRECTION = int.Parse(meta[3][11..^1]);
            var dt = double.Parse(meta[4][4..^1]);
            UNITID = int.Parse(meta[5][8..^1]);
            MaxAcc = double.Parse(meta[6][6..^1]);
            MaxVel = double.Parse(meta[7][6..^1]);


            // accs
            var accs = new List<double>();
            while (!sr.EndOfStream)
                accs.Add(double.Parse(sr.ReadLine()!));

            Accs = new TimeHistory(FILENAME)
            {
                t = Enumerable.Range(0, accs.Count).Select(t => t * dt).ToArray(),
                ytt = accs.ToArray()
            };
        }


        // ★★★★★★★★★★★★★★★ dynamic methods

        /// <summary>
        /// with cm/s
        /// </summary>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public FileInfo CreateSNAPWaveFile(FileInfo outputFile, bool useCurrentDateTimeAsVersionString = true)
        {
            // main
            using var sr = new StreamWriter(outputFile.OpenWrite());
            if (useCurrentDateTimeAsVersionString)
                sr.WriteLine(@$"VERSION=""{DateTime.Now:yyyy.MM.dd.0}""");
            else
                sr.WriteLine(@$"VERSION=""{VERSION}""");
            sr.WriteLine(@$"FILENAME=""{FILENAME}""");
            sr.WriteLine(@$"HPTYPE=""{HPTYPE}""");
            sr.WriteLine(@$"DIRECTION=""0""");
            sr.WriteLine(@$"DT=""{DT:F6}""");
            sr.WriteLine(@$"UNITID=""{UNITID}""");
            sr.WriteLine(@$"AMAX=""{MaxAcc:F3}""");
            sr.WriteLine(@$"VMAX=""{MaxVel:F2}""");
            sr.WriteLine(@$"TIME=""{TIME:F6}""");
            sr.WriteLine(@$"DATA");
            foreach (var acc in Accs.ytt)
                sr.WriteLine($"{acc:F6}");


            // post process
            return outputFile;

        }


        // ★★★★★★★★★★★★★★★ static methods

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
        public static FileInfo CreateSNAPWaveFile(FileInfo outputFile, TimeHistory inputWave, string name, double amax, double vmax)
        {
            // main
            new SNAPWaveData(inputWave)
            {
                FILENAME = name,
                MaxAcc = amax,
                MaxVel = vmax,
            }
            .CreateSNAPWaveFile(outputFile);


            // post process
            return outputFile;
        }
        public static FileInfo CreateSNAPWaveFile(DirectoryInfo outputDir, TimeHistory inputWave, string name, double amax, double vmax)
          => CreateSNAPWaveFile(outputDir.GetChildFileInfo(@$"{name}.wv"), inputWave, name, amax, vmax);

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
        public static FileInfo CreateSNAPWaveFile(FileInfo outputFile, double[] accs, double dt, string name, double amax, double vmax)
        {
            // main
            new SNAPWaveData(accs, dt)
            {
                FILENAME = name,
                MaxAcc = amax,
                MaxVel = vmax,
            }
            .CreateSNAPWaveFile(outputFile);


            // post process
            return outputFile;
        }
        public static FileInfo CreateSNAPWaveFile(DirectoryInfo outputDir, double[] accs, double dt, string name, double amax, double vmax)
            => CreateSNAPWaveFile(outputDir.GetChildFileInfo(@$"{name}.wv"), accs, dt, name, amax, vmax);


        // ★★★★★★★★★★★★★★★ 

    }


    // ★★★★★★★★★★★★★★★ 

}
