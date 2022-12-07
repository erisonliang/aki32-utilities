using ClosedXML.Excel;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// create csvs from excel sheets
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo ExcelSheets2Csvs(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);
        if (!inputFile.Name.EndsWith(".xlsx"))
            throw new Exception("inputFile name must end with .xlsx");


        // main
        using var workbook = new XLWorkbook(inputFile.FullName);

        foreach (var sheet in workbook.Worksheets)
        {
            var maxRow = sheet.LastRowUsed().RowNumber();
            var maxColumn = sheet.LastColumnUsed().ColumnNumber();

            var csvCells = new string[maxRow, maxColumn];

            for (int r = 0; r < maxRow; r++)
                for (int c = 0; c < maxColumn; c++)
                    csvCells[r, c] = sheet.Cell(r + 1, c + 1).Value.ToString() ?? "";

            csvCells
                .ConvertToJaggedArray()
                .SaveCsv_Rows(new FileInfo(Path.Combine(outputDir!.FullName, $"{sheet.Name}.csv")));

        }


        // post process
        return outputDir;
    }

}
