using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.OwesomeModels.ChainableExtensions;
public static partial class TimeHistoryExensions
{

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static TimeHistory GetTimeHistoryFromFile(this FileInfo inputFile, string[]? overwriteHeaders = null)
    {
        return TimeHistory.FromCsv(inputFile, overwriteHeaders);
    }

}
