

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// save TimeHistory List to excel sheets
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo SaveToExcel(this IEnumerable<TimeHistory> timeHistoryList, FileInfo outputFile)
    {
        // preprocess
        if (outputFile != null && !outputFile!.Name.EndsWith(".xlsx"))
            throw new Exception("outputFile name must end with .xlsx");

        var tempDir = outputFile!.Directory!.GetChildDirectoryInfo(".__temp");
        if (tempDir.Exists)
            tempDir.Delete(true);

        try
        {
            tempDir.Create();

            foreach (var timeHistory in timeHistoryList)
            {
                var timeHistoryName = string.IsNullOrEmpty(timeHistory.Name) ? timeHistoryList.ToList().IndexOf(timeHistory).ToString() : timeHistory.Name;
                var outputCsvPath = Path.Combine(tempDir.FullName, $"{timeHistoryName}.csv");
                var outputCsv = new FileInfo(outputCsvPath);
                timeHistory.SaveToCsv(outputCsv);
            }

            tempDir.Csvs2ExcelSheets(outputFile);
        }
        finally
        {
            tempDir.Delete(true);
        }

        return outputFile!;
    }

    /// <summary>
    /// save TimeHistory List to excel sheets
    /// </summary>
    /// <param name="outputFilePath"></param>
    public static FileInfo SaveToExcel(this IEnumerable<TimeHistory> timeHistoryList, DirectoryInfo outputDir)
    {
        var outputFile = outputDir.GetChildFileInfo("output.xlsx");
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


}
