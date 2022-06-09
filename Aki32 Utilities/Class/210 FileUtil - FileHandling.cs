using ClosedXML.Excel;
using System.Text;

namespace Aki32_Utilities.Class;
internal static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 211 CollectFiles

    /// <summary>
    /// move all matching files to one dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Parent.FullName}/output_CollectFiles</param>
    /// <param name="serchPattern"></param>
    /// <returns></returns>
    internal static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string serchPattern)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** CollectFiles() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.Parent.FullName, "output_CollectFiles"));
        if (!outputDir.Exists) outputDir.Create();

        // main
        var files = inputDir.GetFiles(serchPattern, SearchOption.AllDirectories).Select(f => f.FullName);
        foreach (var file in files)
        {
            var newFileName = file.Replace(inputDir.FullName, "");
            foreach (var item in serchPattern.Split("*", StringSplitOptions.RemoveEmptyEntries))
                newFileName = newFileName.Replace(item, "");
            newFileName = newFileName.Replace(Path.DirectorySeparatorChar, '_').Trim('_');
            var newOutputFilePath = Path.Combine(outputDir.FullName, newFileName + Path.GetExtension(file));

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

        return outputDir;
    }

    // ★★★★★★★★★★★★★★★ 212 MakeFilesFromCsv

    /// <summary>
    /// create many template files named by csv list
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/output_MakeFilesFromCsv</param>
    /// <param name="templateFile"></param>
    /// <returns></returns>
    internal static DirectoryInfo MakeFilesFromCsv(this FileInfo inputFile, DirectoryInfo? outputDir, FileInfo templateFile)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** Csvs2ExcelSheets() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputFile.DirectoryName, "output_MakeFilesFromCsv"));
        if (!outputDir.Exists) outputDir.Create();

        // main
        var tempDataEx = Path.GetExtension(templateFile.Name);
        using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;

            try
            {
                var targetPath = Path.Combine(outputDir.FullName, $"{line}{tempDataEx}");
                templateFile.CopyTo(targetPath, true);
                Console.WriteLine($"O: {targetPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"X: {ex}");
            }
        }

        return outputDir;
    }

    // ★★★★★★★★★★★★★★★ 

}
