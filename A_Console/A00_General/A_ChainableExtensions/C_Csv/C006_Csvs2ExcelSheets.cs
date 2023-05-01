using ClosedXML.Excel;

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
            return outputFile!;
        }

        foreach (var csv in csvs)
            csv.Csv2ExcelSheet(workbook);

        workbook.SaveAs(outputFile.FullName, true);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// create an excel file from csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static FileInfo Csv2ExcelSheet(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, "output.xlsx");
        if (!outputFile!.Name.EndsWith(".xlsx"))
            throw new Exception("outputFile name must end with .xlsx");


        // main
        using var workbook = new XLWorkbook();
        inputFile.Csv2ExcelSheet(workbook);
        workbook.SaveAs(outputFile.FullName, true);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// map csv values to an excel sheet 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="excelWorkBook"></param>
    private static void Csv2ExcelSheet(this FileInfo inputFile, XLWorkbook excelWorkBook)
    {
        var sheetName = Path.GetFileNameWithoutExtension(inputFile.Name);
        excelWorkBook.SetExcelSheet(sheetName, inputFile.ReadCsv_Rows());
    }

    /// <summary>
    /// set matrix data to excel sheet
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="targetSheetName"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static XLWorkbook SetExcelSheet(this XLWorkbook targetExcelWorkBook, string addingSheetName, string[][] inputData, int? position = null)
    {
        var worksheet = position.HasValue
            ? targetExcelWorkBook.AddWorksheet(addingSheetName, position.Value)
            : targetExcelWorkBook.AddWorksheet(addingSheetName);

        for (int i = 0; i < inputData.Length; i++)
        {
            var line = inputData[i];
            for (int j = 0; j < line.Length; j++)
                worksheet.Cell(i + 1, j + 1).Value = line[j];
        }

        return targetExcelWorkBook;
    }

}
