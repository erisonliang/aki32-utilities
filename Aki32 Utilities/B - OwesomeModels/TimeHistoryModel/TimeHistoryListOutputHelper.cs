using Aki32_Utilities.OverridingUtils;

namespace Aki32_Utilities.OwesomeModels;

public static class TimeHistoryListOutputHelper
{

    /// <summary>
    /// Output TimeHistory List to csv
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo OutputToCsv(this IEnumerable<TimeHistory> timeHistoryList, FileInfo outputFile)
    {
        var tempDir = new DirectoryInfo(Path.Combine(outputFile.DirectoryName, ".__temp"));
        if (!tempDir.Exists)
            tempDir.Create();

        foreach (var timeHistory in timeHistoryList)
        {
            var timeHistoryName = string.IsNullOrEmpty(timeHistory.Name) ? timeHistoryList.ToList().IndexOf(timeHistory).ToString() : timeHistory.Name;
            var outputCsvPath = Path.Combine(tempDir.FullName, $"{timeHistoryName}.csv");
            var outputCsv = new FileInfo(outputCsvPath);
            timeHistory.OutputToCsv(outputCsv);
        }

        tempDir.Csvs2ExcelSheets(outputFile);
        tempDir.Delete(true);

        return outputFile;
    }
  
    /// <summary>
    /// Output TimeHistory List to csv
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo OutputToCsv(this IEnumerable<TimeHistory> timeHistoryList, DirectoryInfo outputDir)
    {
        var outputFile = new FileInfo(Path.Combine(outputDir.FullName, "output.xlsx"));
        return timeHistoryList.OutputToCsv(outputFile);
    }

    /// <summary>
    /// Output TimeHistory List to console
    /// </summary>
    public static void OutputToConsole(this IEnumerable<TimeHistory> timeHistoryList)
    {
        foreach (var item in timeHistoryList)
        {
            Console.WriteLine(item.Name);
            item.OutputToConsole();
        }
    }

}
