using Aki32_Utilities.General.ChainableExtensions;

namespace Aki32_Utilities.OwesomeModels.ChainableExtensions;
public static partial class TimeHistoryExensions
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


    // ★★★★★★★★★★★★★★★

}
