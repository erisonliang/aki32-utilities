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
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, UtilConfig.GetNewOutputDirName("TransposeCsv"), inputFile.Name));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();


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
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** TransposeCsv_Loop() Called");
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, UtilConfig.GetNewOutputDirName("TransposeCsv")));
        if (!outputDir.Exists) outputDir.Create();


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
