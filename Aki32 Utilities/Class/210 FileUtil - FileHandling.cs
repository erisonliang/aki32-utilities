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
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
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
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
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

    // ★★★★★★★★★★★★★★★ 213 MoveTo

    /// <summary>
    /// move entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    internal static DirectoryInfo MoveTo(this DirectoryInfo inputDir, DirectoryInfo outputDir)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** MoveDir() Called");
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));

        // main
        if (inputDir.FullName[0..3] == outputDir.FullName[0..3])
        {
            // use default MoveTo().
            inputDir.MoveTo(outputDir.FullName);
        }
        else
        {
            // We can't use MoveTo() for different drive.
            inputDir.CopyTo(outputDir, false);
            inputDir.Delete(true);
        }

        // postprocess
        return outputDir;
    }

    // ★★★★★★★★★★★★★★★ 214 CopyTo

    /// <summary>
    /// copy entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    internal static DirectoryInfo CopyTo(this DirectoryInfo inputDir, DirectoryInfo outputDir, bool consoleOutput = true)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput && consoleOutput)
            Console.WriteLine("\r\n** CopyDir() Called");
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));

        // main
        // create new directory with the same attribtues
        if (!outputDir.Exists)
        {
            outputDir.Create();
            outputDir.Attributes = inputDir.Attributes;
        }

        // copy files (with overriding existing files)
        foreach (FileInfo fileInfo in inputDir.GetFiles())
            fileInfo.CopyTo(outputDir.FullName + @"\" + fileInfo.Name, true);

        // copy directories
        foreach (var inner_inputDir in inputDir.GetDirectories())
            inner_inputDir.CopyTo(new DirectoryInfo(Path.Combine(outputDir.FullName, inner_inputDir.Name)), false);

        // postprocess
        return outputDir;
    }

    // ★★★★★★★★★★★★★★★ 

}
