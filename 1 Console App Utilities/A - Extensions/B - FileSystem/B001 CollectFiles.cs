using System.Text.RegularExpressions;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// move all matching files to one dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="searchPatterns"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        int maxDegreeOfParallelism = 999,
        params string[] searchRegexen)
        => inputDir.Loop(outputDir, (inF, outF) =>
            {
                var outputFileName = inF.FullName.Replace(inputDir.FullName, "").Replace(Path.DirectorySeparatorChar, '_').Trim('_');
                var outputFilePath = Path.Combine(outF.Directory!.FullName, outputFileName);
                File.Copy(inF.FullName, outputFilePath, true);
            },
            targetFilesOption: SearchOption.AllDirectories,
            searchRegexen: searchRegexen,
            maxDegreeOfParallelism: maxDegreeOfParallelism,
            overrideTargetOutputDirCandidate: inputDir.Parent!
            );


    // ★★★★★★★★★★★★★★★ 

}
