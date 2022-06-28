using ClosedXML.Excel;
using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// create an excel file that have sheets from csvs
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Csvs2ExcelSheets/output.xlsx</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static FileInfo Csvs2ExcelSheets(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** Csvs2ExcelSheets() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputDir.FullName, UtilConfig.GetNewOutputDirName("Csvs2ExcelSheets"), "output.xlsx"));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (!outputFile.Name.EndsWith(".xlsx"))
            outputFile = new FileInfo(Path.GetFileNameWithoutExtension(outputFile.FullName) + ".xlsx");
        if (outputFile.Exists) outputFile.Delete();


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
