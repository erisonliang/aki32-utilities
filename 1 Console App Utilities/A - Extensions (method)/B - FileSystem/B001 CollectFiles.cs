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
        var files = new List<string>();

        var allFiles = inputDir.GetFiles("*", SearchOption.AllDirectories).Select(f => f.FullName);

        foreach (var targetFile in allFiles)
        {
            var ComparingString = targetFile.Replace(inputDir.FullName, "").TrimStart('\\');
            foreach (var searchRegex in searchRegexen)
            {
                if (Regex.IsMatch(ComparingString, searchRegex))
                {
                    files.Add(targetFile);
                    break;
                }
            }
        }

        foreach (var file in files)
        {
            var newFileName = file.Replace(inputDir.FullName, "");
            newFileName = newFileName.Replace(Path.DirectorySeparatorChar, '_').Trim('_');
            var newOutputFilePath = Path.Combine(outputDir!.FullName, newFileName);

            try
            {
                File.Copy(file, newOutputFilePath, true);
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
