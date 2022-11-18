using Aki32_Utilities.Extensions;

using MathNet.Numerics.IntegralTransforms;

using System.Numerics;

namespace Aki32_Utilities.OwesomeModels;
public static class TimeHistoryExension
{

    // ★★★★★★★★★★★★★★★ IEnumerable<TimeHistory> save helper

    /// <summary>
    /// save TimeHistory List to excel sheets
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo SaveToExcel(this IEnumerable<TimeHistory> timeHistoryList, FileInfo outputFile)
    {
        var tempDir = new DirectoryInfo(Path.Combine(outputFile.DirectoryName!, ".__temp"));
        if (!tempDir.Exists)
            tempDir.Create();

        foreach (var timeHistory in timeHistoryList)
        {
            var timeHistoryName = string.IsNullOrEmpty(timeHistory.Name) ? timeHistoryList.ToList().IndexOf(timeHistory).ToString() : timeHistory.Name;
            var outputCsvPath = Path.Combine(tempDir.FullName, $"{timeHistoryName}.csv");
            var outputCsv = new FileInfo(outputCsvPath);
            timeHistory.SaveToCsv(outputCsv);
        }

        tempDir.Csvs2ExcelSheets(outputFile);
        tempDir.Delete(true);

        return outputFile;
    }

    /// <summary>
    /// save TimeHistory List to excel sheets
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo SaveToExcel(this IEnumerable<TimeHistory> timeHistoryList, DirectoryInfo outputDir)
    {
        var outputFile = new FileInfo(Path.Combine(outputDir.FullName, "output.xlsx"));
        return timeHistoryList.SaveToExcel(outputFile);
    }

    /// <summary>
    /// output TimeHistory List to console
    /// </summary>
    public static void OutputToConsole(this IEnumerable<TimeHistory> timeHistoryList)
    {
        foreach (var item in timeHistoryList)
        {
            Console.WriteLine(item.Name);
            item.WriteToConsole();
        }
    }


    // ★★★★★★★★★★★★★★★ initialize helper

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static TimeHistory GetTimeHistoryFromFile(this FileInfo inputFile, string[]? overwriteHeaders = null)
    {
        return TimeHistory.FromCsv(inputFile, overwriteHeaders);
    }


    // ★★★★★★★★★★★★★★★ data analysis

    public static async Task<TimeHistory> FFT(this TimeHistory inputHistory, string targetIndex)
    {
        var inputHistoryClone = inputHistory.Clone();
        var resultHistory = inputHistory.Clone();
        resultHistory.Name += "_FFT";
        resultHistory.data = new Dictionary<string, double[]>();

        if (inputHistoryClone.TimeStep == 0)
            throw new InvalidDataException("\"t\" index is required in inputHistory");
        var sampleRate = 1d / inputHistoryClone.TimeStep;

        await Task.Run(() =>
        {
            // 2の累乗にカットして複素数データに変換
            double len = inputHistoryClone.DataRowCount;
            int Np = 0;
            while (len > 1)
            {
                Np++;
                len /= 2;
            }
            Np--;

            if (Np == 0) return;
            int N = (int)Math.Pow(2, Np); // サンプル数

            var complexVoice = inputHistoryClone[targetIndex]
                .Take(N)
                .Select(x => new Complex(x, 0))
                .ToArray();

            // FFTを実行
            Fourier.Forward(complexVoice, FourierOptions.Default);

            // グラフのデータを変換。
            var freqStep = sampleRate / N;

            for (int i = 0; i < N / 2; i++)
            {
                var step = new TimeHistoryStep();
                var freq = freqStep * i;
                step["Freq"] = freq;
                step["T"] = freq == 0 ? double.PositiveInfinity : 1d / freq;
                step["Amp"] = Math.Pow(complexVoice[i].Magnitude, 2);
                resultHistory.AppendStep(step);
            }
        });

        return resultHistory;
    }


    // ★★★★★★★★★★★★★★★

}
