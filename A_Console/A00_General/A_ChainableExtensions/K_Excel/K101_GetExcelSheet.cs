using ClosedXML.Excel;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    /// <summary>
    /// get matrix data from excel sheet
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="targetSheetName"></param>
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

    /// <summary>
    /// set matrix data to excel sheet
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="targetSheetName"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static void SetExcelSheet(this FileInfo targetExcelFile, string targetSheetName, string[][] inputData)
    {
        // main
        if (targetExcelFile.Exists)
        {
            using var workbook = new XLWorkbook(targetExcelFile.FullName);
            workbook.SetExcelSheet(targetSheetName, inputData);
            workbook.Save(true);
        }
        else
        {
            using var workbook = new XLWorkbook();
            workbook.SetExcelSheet(targetSheetName, inputData);
            workbook.SaveAs(targetExcelFile.FullName, true);
        }
    }

}
