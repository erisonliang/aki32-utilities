using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// merge all file's lines in one file
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_MergeAllLines/output.txt</param>
    /// <param name="skipRowCount"></param>
    /// <returns></returns>
    public static FileInfo MergeAllLines(this DirectoryInfo inputDir, FileInfo? outputFile, int skipRowCount = 0)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, $"output.txt");


        // main
        var inputFiles = inputDir
            .GetFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x => !x.Name.Contains(outputFile!.DirectoryName!))
            .Sort()
            .ToArray();
  
        using var sw = new StreamWriter(outputFile!.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));
        foreach (var inputFile in inputFiles)
        {
            try
            {
                var inputTexts = File.ReadLines(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS")).ToArray();
                for (int i = skipRowCount; i < inputTexts.Length; i++)
                    sw.WriteLine(inputTexts[i]);

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {inputFile.FullName}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {inputFile.FullName}, {ex.Message}");
            }
        }


        // post process
        return outputFile;
    }

}