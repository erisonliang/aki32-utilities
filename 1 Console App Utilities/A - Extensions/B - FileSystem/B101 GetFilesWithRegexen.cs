using System.IO.Compression;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_Zip/{inputFile.Name}</param>
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
