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
    public static DirectoryInfo ExcelSheets2Csvs(this FileInfo inputFile, DirectoryInfo? outputDir, bool includeExcelFileName = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);
        var outputFileSuffix = includeExcelFileName ? $"{Path.GetFileNameWithoutExtension(inputFile.Name)}_" : "";

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


            var outputFile = outputDir!.GetChildFileInfo($"{outputFileSuffix}{sheet.Name}.csv");

            csvCells
                .ConvertToJaggedArray()
                .SaveCsv_Rows(outputFile);

        }


        // post process
        return outputDir!;
    }

    /// <summary>
    /// create csvs from excel sheets
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo ExcelSheets2Csvs_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
        => inputDir.Loop(outputDir, (inF, outF) => inF.ExcelSheets2Csvs(outF.Directory!, includeExcelFileName: true),
            searchRegexen: GetRegexen_XmlTypedExcelFiles(xlsx: true, xlsm: true, xltx: true, xltm: true)
            );

}
