using System.Text.RegularExpressions;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

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
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir.Parent!);


        // main
        var inputFiles = inputDir.GetFilesWithRegexen(SearchOption.AllDirectories, searchRegexen);
        Parallel.ForEach(inputFiles, new ParallelOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism }, (FileInfo inputFile) =>
        {
            try
            {
                var inputFileName = inputFile.FullName;
                var outputFileName = inputFileName.Replace(inputDir.FullName, "");
                outputFileName = outputFileName.Replace(Path.DirectorySeparatorChar, '_').Trim('_');
                var outputFilePath = Path.Combine(outputDir!.FullName, outputFileName);

                File.Copy(inputFileName, outputFilePath, true);

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {inputFile.FullName}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {inputFile.FullName}, {ex.Message}");
            }
        });


        return outputDir!;
    }

}
