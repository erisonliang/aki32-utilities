using System.Runtime.CompilerServices;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Turn file-utility-methods into loop method
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Action<FileInfo, FileInfo> targetAction,
        string[] searchRegexen = null,
        int maxDegreeOfParallelism = 999,
        SearchOption targetFilesOption = SearchOption.TopDirectoryOnly,
        DirectoryInfo overrideTargetOutputDirCandidate = null,
        int maxRetryCount = 5,
        [CallerMemberName] string methodName = ""
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, overrideTargetOutputDirCandidate ?? inputDir, true, methodName: methodName);
        searchRegexen ??= new string[] { ".*" };


        // main
        var inputFiles = inputDir.GetFilesWithRegexen(targetFilesOption, searchRegexen).Sort();
        var option = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

        using var progress = new ProgressManager(inputFiles.Count());
        progress.StartAutoWrite(100);

        Parallel.ForEach(inputFiles, option, inputFile =>
        {
            try
            {
                var retryCount = 0;
                while (true)
                {
                    try
                    {
                        var outputFile = outputDir!.GetChildFileInfo(inputFile.Name);
                        targetAction(inputFile, outputFile);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (++retryCount < maxRetryCount)
                        {
                            Thread.Sleep(100);
                            GC.Collect();
                            continue;
                        }
                        progress.AddErrorMessage($"{inputFile.FullName}, {ex.Message}");
                        break;
                    }
                }
            }
            finally
            {
                progress.CurrentStep++;
            }
        });

        progress.WriteDone();


        // post process
        return outputDir!;
    }


}
