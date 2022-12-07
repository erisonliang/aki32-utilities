using ClosedXML.Excel;

using DocumentFormat.OpenXml.Spreadsheet;

namespace Aki32Utilities.ConsoleAppUtilities.General;
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
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir!, "output.xlsx");
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
            csv.Csv2Excel(workbook);

        workbook.SaveAs(outputFile.FullName, true);


        // post process
        return outputFile;
    }

    /// <summary>
    /// create an excel file from csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static FileInfo Csv2Excel(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, "output.xlsx");
        if (!outputFile!.Name.EndsWith(".xlsx"))
            throw new Exception("outputFile name must end with .xlsx");


        // main
        using var workbook = new XLWorkbook();
        inputFile.Csv2Excel(workbook);
        workbook.SaveAs(outputFile.FullName, true);


        // post process
        return outputFile;
    }

    /// <summary>
    /// map csv values to an excel sheet 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="excelWorkBook"></param>
    static void Csv2Excel(this FileInfo inputFile, XLWorkbook excelWorkBook)
    {
        var sheetName = Path.GetFileNameWithoutExtension(inputFile.Name);
        var worksheet = excelWorkBook.AddWorksheet(sheetName);

        var inputCsv = inputFile.ReadCsv_Rows();
        for (int i = 0; i < inputCsv.Length; i++)
        {
            var line = inputCsv[i];

            for (int j = 0; j < line.Length; j++)
                worksheet.Cell(i + 1, j + 1).Value = line[j];
        }
    }

}
