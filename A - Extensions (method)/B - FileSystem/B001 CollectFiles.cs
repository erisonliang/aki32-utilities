using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// move all matching files to one dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Parent.FullName}/output_CollectFiles</param>
    /// <param name="serchPattern"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectFiles(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params string[] serchPatterns)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** CollectFiles() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.Parent.FullName, UtilConfig.GetNewOutputDirName("CollectFiles")));
        if (!outputDir.Exists)
            outputDir.Create();


        // main
        var files = new List<string>();
        foreach (var serchPattern in serchPatterns)
            files.AddRange(inputDir.GetFiles(serchPattern, SearchOption.AllDirectories).Select(f => f.FullName));
        files = files.Distinct().ToList();

        foreach (var file in files)
        {
            var newFileName = file.Replace(inputDir.FullName, "");
            newFileName = newFileName.Replace(Path.DirectorySeparatorChar, '_').Trim('_');
            var newOutputFilePath = Path.Combine(outputDir.FullName, newFileName);

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


        // postprocess
        return outputDir;
    }

}
