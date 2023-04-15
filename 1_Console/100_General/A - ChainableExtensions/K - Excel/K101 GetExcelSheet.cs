using ClosedXML.Excel;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    /// <summary>
    /// create csvs from excel sheets
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="includeExcelFileName">include input excel file name as an output file name suffix</param>
    /// <returns></returns>
    public static string[,] GetExcelSheet(this FileInfo inputFile, string targetSheetName)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic();


        // main
        using var workbook = new XLWorkbook(inputFile.FullName);

        var sheet = workbook.Worksheets.First(x => x.Name == targetSheetName);
        var maxRow = sheet.LastRowUsed().RowNumber();
        var maxColumn = sheet.LastColumnUsed().ColumnNumber();

        var csvCells = new string[maxRow, maxColumn];

        for (int r = 0; r < maxRow; r++)
            for (int c = 0; c < maxColumn; c++)
                csvCells[r, c] = sheet.Cell(r + 1, c + 1).Value.ToString() ?? "";


        // post process
        return csvCells;
    }

}
