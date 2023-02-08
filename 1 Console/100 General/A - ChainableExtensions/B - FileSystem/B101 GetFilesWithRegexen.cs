

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    /// <summary>
    /// Return if string match any of search regexen in current directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFilesWithRegexen(this DirectoryInfo inputDir, params string[] searchRegexen)
    {
        // main
        return inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, searchRegexen);
    }

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFilesWithRegexen(this DirectoryInfo inputDir, SearchOption searchOption, params string[] searchRegexen)
    {
        // main
        return inputDir
            .GetFiles("*", searchOption)
            .Where(f => f.FullName
                .Replace(inputDir.FullName, "")
                .Trim(Path.PathSeparator)
                .IsMatchAny(searchRegexen)
                )
            ;
    }
}
