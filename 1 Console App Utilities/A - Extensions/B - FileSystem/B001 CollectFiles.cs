using System.Text.RegularExpressions;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// move all matching files to one dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Parent.FullName}/output_CollectFiles</param>
    /// <param name="searchPatterns"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params string[] searchRegexen)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir.Parent!);


        // main
        //var targetFileNames = inputDir
        //    .GetFiles("*", SearchOption.AllDirectories)
        //    .Select(f => f.FullName)
        //    .Where(f => f.Replace(inputDir.FullName, "").TrimStart('\\').IsMatchAny(searchRegexen));

        var targetFiles = inputDir.GetFilesWithRegexen(SearchOption.AllDirectories, searchRegexen);

        foreach (var targetFile in targetFiles)
        {
            var targetFileName = targetFile.FullName;
            var newFileName = targetFileName.Replace(inputDir.FullName, "");
            newFileName = newFileName.Replace(Path.DirectorySeparatorChar, '_').Trim('_');
            var newOutputFilePath = Path.Combine(outputDir!.FullName, newFileName);

            try
            {
                File.Copy(targetFileName, newOutputFilePath, true);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newOutputFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {newOutputFilePath}, {ex.Message}");
            }
        }


        return outputDir!;
    }

}
