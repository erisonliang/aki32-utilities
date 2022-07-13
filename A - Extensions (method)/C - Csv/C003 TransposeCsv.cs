using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Transpose csv and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_TransposeCsv/{inputFile.Name}</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo TransposeCsv(this FileInfo inputFile, FileInfo? outputFile, int skipColumnCount = 0, int skipRowCount = 0, string header = null)
    {
        // preprocess
        if (outputFile is null)
            throw new ArgumentNullException(nameof(outputFile));
        UtilPreprocessors.PreprocessOutFile(ref outputFile, "TransposeCsv", false, inputFile.Directory!, inputFile.Name);


        // main
        return inputFile.ReadCsv_Rows(skipColumnCount, skipRowCount).SaveCsv_Columns(outputFile, header);
    }
  
    /// <summary>
    /// Transpose csv and save
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TransposeCsv</param>
    /// <param name="skipColumnCount"></param>s
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo TransposeCsv_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipColumnCount = 0, int skipRowCount = 0, string header = null)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));
        if (!outputDir.Exists) outputDir.Create();
        UtilPreprocessors.PreprocessOutDir(ref outputDir, "TransposeCsv", true, inputDir);


        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir.FullName, file.Name);
            try
            {
                file.TransposeCsv(new FileInfo(newFilePath), skipColumnCount, skipRowCount, header);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {newFilePath}, {ex.Message}");
            }
        }


        // post process
        return outputDir;
    }

}
