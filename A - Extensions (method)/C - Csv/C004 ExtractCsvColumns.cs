using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_ExtractCsvColumns/{inputFile.Name}</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo ExtractCsvColumns(this FileInfo inputFile, FileInfo? outputFile, int[] extractingColumns, int skipRowCount = 0, string header = null)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, UtilConfig.GetNewOutputDirName("ExtractCsvColumns"), inputFile.Name));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();


        // main
        var inputCsv = inputFile.ReadCsv_Rows(0, skipRowCount);

        var resultList = new List<string[]>();

        for (int i = 0; i < inputCsv.Length; i++)
        {
            var addingLine = new List<string>();
            foreach (var ec in extractingColumns)
            {
                try
                {
                    addingLine.Add(inputCsv[i][ec]);
                }
                catch (Exception)
                {
                    addingLine.Add("");
                }
            }
            resultList.Add(addingLine.ToArray());
        }

        resultList.ToArray().SaveCsv_Rows(outputFile, header);


        // post process
        return outputFile;
    }
  
    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_ExtractCsvColumns</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo ExtractCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int[] extractingColumns, int skipRowCount = 0, string header = null)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** ExtractCsvColumns_Loop() Called (This takes time...)");
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, UtilConfig.GetNewOutputDirName("ExtractCsvColumns")));
        if (!outputDir.Exists) outputDir.Create();


        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir.FullName, file.Name);
            try
            {
                file.ExtractCsvColumns(new FileInfo(newFilePath), extractingColumns, skipRowCount, header);
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
