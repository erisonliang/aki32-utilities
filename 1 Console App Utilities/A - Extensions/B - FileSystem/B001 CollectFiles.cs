
namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// move all matching files to one dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="searchPatterns"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        int maxDegreeOfParallelism = 999,
        bool useMove = false,
        params string[] searchRegexen
        )
        => inputDir.Loop(outputDir, (inF, outF) =>
            {
                var outputFileName = inF.FullName.Replace(inputDir.FullName, "").Replace(Path.DirectorySeparatorChar, '_').Trim('_');
                var outputFilePath = Path.Combine(outF.Directory!.FullName, outputFileName);
                if (useMove)
                    inF.MoveTo(outputFilePath, true);
                else
                    inF.CopyTo(outputFilePath, true);
            },
            targetFilesOption: SearchOption.AllDirectories,
            searchRegexen: searchRegexen,
            maxDegreeOfParallelism: maxDegreeOfParallelism,
            overrideTargetOutputDirCandidate: inputDir.Parent!
            );


    // ★★★★★★★★★★★★★★★ sugar

    public static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params string[] searchRegexen)
      => inputDir.CollectFiles(outputDir, 999, false, searchRegexen);


    // ★★★★★★★★★★★★★★★ 

}
