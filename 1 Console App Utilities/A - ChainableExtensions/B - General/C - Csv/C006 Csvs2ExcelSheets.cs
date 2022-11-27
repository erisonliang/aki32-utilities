using ClosedXML.Excel;

namespace Aki32_Utilities.General.ChainableExtensions;
public static partial class ChainableExtensions
{

    /// <summary>
    /// create an excel file that have sheets from csvs
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static FileInfo Csvs2ExcelSheets(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.xlsx");
        if (!outputFile!.Name.EndsWith(".xlsx"))
            throw new Exception("outputFile name must end with .xlsx");


        // main
        using var workbook = new XLWorkbook();

        var csvs = inputDir
            .GetFiles("*.csv", SearchOption.TopDirectoryOnly)
            .Where(x => x.FullName != outputFile.FullName)
            .Sort()
            .ToArray();
        if (csvs.Length == 0)
        {
            Console.WriteLine($"※ No csv file found in {inputDir.FullName}");
            return outputFile;
        }

        foreach (var csv in csvs)
        {
            var csvPath = csv.FullName;
            try
            {
                var sheetName = Path.GetFileNameWithoutExtension(csv.Name);
                var worksheet = workbook.AddWorksheet(sheetName);

                var inputCsv = csv.ReadCsv_Rows();
                for (int i = 0; i < inputCsv.Length; i++)
                {
                    var line = inputCsv[i];

                    for (int j = 0; j < line.Length; j++)
                        worksheet.Cell(i + 1, j + 1).Value = line[j];
                }

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {csvPath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {csvPath}, {ex.Message}");
            }
        }

        workbook.SaveAs(outputFile.FullName, true);


        // post process
        return outputFile;
    }

}
