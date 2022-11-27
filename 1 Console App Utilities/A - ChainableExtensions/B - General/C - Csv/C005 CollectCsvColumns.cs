using ClosedXML.Excel;

using System.Text;

namespace Aki32_Utilities.ChainableExtensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// move all csvs' target column to one csv (.xlsx is also acceptable)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="targetColumn"></param>
    /// <param name="initialColumn"></param>
    /// <returns></returns>
    public static FileInfo CollectCsvColumns(this DirectoryInfo inputDir, FileInfo? outputFile, int targetColumn, int? initialColumn = 0, int skipRowCount = 0)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.csv");


        // main
        var outputDirPath = outputFile!.DirectoryName;
        var itemName = Path.GetFileNameWithoutExtension(outputFile.Name);
        var outputExtension = Path.GetExtension(outputFile.Name);

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

        if (outputExtension == ".csv")
        {
            var UseInitColumnThen1 = initialColumn.HasValue ? 1 : 0;

            // collect all data to this column list and save eventually
            var resultColumnList = new string[csvs.Length + UseInitColumnThen1][];

            // copy one column from inputCsv to outputCsv method
            void AddColumnToResultColumnList(FileInfo csv, int targetInputCsvColumn, int targetOutputCsvColumn, string header = "")
            {
                var csvData = csv.ReadCsv_Rows(0, skipRowCount);
                var tempCsvColumn = new List<string> { header };
                for (int i = 0; i < csvData.Length; i++)
                {
                    try
                    {
                        tempCsvColumn.Add(csvData[i][targetInputCsvColumn]);
                    }
                    catch (Exception)
                    {
                        tempCsvColumn.Add("");
                    }
                }
                resultColumnList[targetOutputCsvColumn] = tempCsvColumn.ToArray();
            }

            // copy initialColumn 
            if (initialColumn.HasValue)
                AddColumnToResultColumnList(csvs[0], initialColumn.Value, 0);

            // copy all of the rest
            for (int i = 0; i < csvs.Length; i++)
            {
                var csvPath = csvs[i].FullName;
                try
                {
                    var header = Path.GetFileNameWithoutExtension(csvPath);
                    AddColumnToResultColumnList(csvs[i], targetColumn, i + UseInitColumnThen1, header);

                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"O: {csvPath}");
                }
                catch (Exception ex)
                {
                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"X: {csvPath}, {ex.Message}");
                }
            }

            // save
            resultColumnList.SaveCsv_Columns(outputFile);
        }
        else if (outputExtension == ".xlsx")
        {
            // Excel (※[1,1] refers A1 cell)
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("All");

            // copy one column from csv to excel method
            void CopyCsvToExcelColumn(FileInfo csv, int targetCsvColumn, int targetExcelColumn)
            {
                var sr = new StreamReader(csv.FullName, Encoding.GetEncoding("SHIFT_JIS"));
                var all = sr.ReadToEnd().Split("\r\n").Select(x => { try { return x.Split(',')[targetCsvColumn]; } catch { return ""; } }).ToArray();

                for (int i = 0; i < all.Count(); i++)
                    worksheet.Cell(i + 2, targetExcelColumn + 1).Value = all[i];
            }

            // copy initialColumn 
            if (initialColumn.HasValue)
                CopyCsvToExcelColumn(csvs[0], initialColumn.Value, 0);

            // copy all of the rest
            foreach (var csv in csvs)
            {
                var csvPath = csv.FullName;
                try
                {
                    int lastColumn = worksheet.LastColumnUsed().ColumnNumber();
                    CopyCsvToExcelColumn(csv, targetColumn, lastColumn);
                    worksheet.Cell(1, lastColumn + 1).Value = Path.GetFileNameWithoutExtension(csvPath);

                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"O: {csvPath}");
                }
                catch (Exception ex)
                {
                    if (UtilConfig.ConsoleOutput)
                        Console.WriteLine($"X: {csvPath}, {ex.Message}");
                }
            }

            // save
            workbook.SaveAs(outputFile.FullName, true);
        }
        else
        {
            throw new NotImplementedException("outputFile need to be .csv or .xlsx file null.");
        }


        // post process
        return outputFile;
    }

    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static DirectoryInfo CollectCsvColumnsForMany(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipRowCount, params (string name, int? initialColumn, int targetColumn)[] targets)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, false, inputDir, takesTimeFlag: true);
        if (inputDir.FullName == outputDir!.FullName)
            throw new InvalidOperationException("※ inputDir and outputDir must be different");


        // main
        foreach (var target in targets)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, target.name + ".csv"));
            inputDir.CollectCsvColumns(outputFile, target.targetColumn, target.initialColumn, skipRowCount);
        }


        // post process
        return outputDir;
    }


    // ★★★★★★★★★★★★★★★ sugar

    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static DirectoryInfo CollectCsvColumnsForMany(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params (string name, int? initialColumn, int targetColumn)[] targets)
        => inputDir.CollectCsvColumnsForMany(outputDir, 0, targets);

    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectCsvColumnsForMany(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int? initialColumn, int skipRowCount, params (string name, int targetColumn)[] targets)
        => inputDir.CollectCsvColumnsForMany(outputDir, skipRowCount, targets.Select(x => (x.name, initialColumn, x.targetColumn)).ToArray());

    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static DirectoryInfo CollectCsvColumnsForMany(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params (string name, int targetColumn)[] targets)
        => inputDir.CollectCsvColumnsForMany(outputDir, 0, 0, targets);


    // ★★★★★★★★★★★★★★★

}
