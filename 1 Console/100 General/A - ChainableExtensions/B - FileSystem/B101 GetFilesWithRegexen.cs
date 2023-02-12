

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

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


    // ★★★★★★★★★★★★★★★ sugar

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFiles_Images(this DirectoryInfo inputDir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        => inputDir.GetFilesWithRegexen(searchOption, GetRegexen_ImageFiles());

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFiles_Videos(this DirectoryInfo inputDir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        => inputDir.GetFilesWithRegexen(searchOption, GetRegexen_VideoFiles());

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFiles_Csvs(this DirectoryInfo inputDir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        => inputDir.GetFilesWithRegexen(searchOption, GetRegexen_CsvFiles());

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static IEnumerable<FileInfo> GetFiles_XmlTypedExcels(this DirectoryInfo inputDir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        => inputDir.GetFilesWithRegexen(searchOption, GetRegexen_XmlTypedExcelFiles());


    // ★★★★★★★★★★★★★★★ 

}
