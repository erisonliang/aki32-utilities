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
    public static FileInfo ExtractCsvColumns(this FileInfo inputFile, FileInfo? outputFile, int[] extractingColumns, int skipRowCount = 0, string? header = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


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

        resultList.ToArray().SaveCsv_Rows(outputFile!, header);


        // post process
        return outputFile!;
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
    public static DirectoryInfo ExtractCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int[] extractingColumns, int skipRowCount = 0, string? header = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir, takesTimeFlag: true);


        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            try
            {
                var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
                inputFile.ExtractCsvColumns(outputFile, extractingColumns, skipRowCount, header);

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
        return outputDir!;
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
    public static DirectoryInfo ExtractCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipRowCount = 0, params (string name, int[] extractingColumns, string? header)[] targets)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir, takesTimeFlag: true);


        // main
        foreach (var target in targets)
        {
            foreach (var inputFile in inputDir.GetFiles())
            {
                try
                {
                    var inputFileNameWithoutEx = Path.GetFileNameWithoutExtension(inputFile.Name);
                    var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, $"{inputFileNameWithoutEx}_{target.name}{inputFile.Extension}"));
                    inputFile.ExtractCsvColumns(outputFile, target.extractingColumns, skipRowCount, target.header);

                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"O: {inputFile.FullName}");
                }
                catch (Exception ex)
                {
                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"X: {inputFile.FullName}, {ex.Message}");
                }
            }
        }


        // post process
        return outputDir!;
    }

}
