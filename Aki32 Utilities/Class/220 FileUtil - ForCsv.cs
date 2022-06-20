using ClosedXML.Excel;
using System.Text;

namespace Aki32_Utilities.Class;
internal static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 221 ReadCsv

    /// <summary>
    /// read csv as list of lines
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="ignoreEmptyLine"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static string[][] ReadCsv_Rows(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS


        // main            
        var rows = new List<string[]>();

        using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

        for (int i = 0; i < skipRowCount; i++)
            sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrEmpty(line.Replace(",", "")))
            {
                if (!ignoreEmptyLine)
                    rows.Add(new string[0]);
            }
            else
            {
                var escapedFlag = false;
                if (line.Contains("\""))
                {
                    escapedFlag = true;
                    line = line.Replace($"\"\"", "{ignoringDQ}");
                    var splitedLine = line.Split("\"", StringSplitOptions.None);
                    for (int i = 0; i < splitedLine.Length; i++)
                        if (i % 2 == 1)
                            splitedLine[i] = splitedLine[i].Replace(",", "{ignoringComma}");
                    line = string.Join("", splitedLine);
                    line = line.Replace("{ignoringDQ}", $"\"");
                }

                var lineItems = line.Split(",", StringSplitOptions.None);

                if (escapedFlag)
                    lineItems = lineItems.Select(x => x.Replace("{ignoringComma}", ",")).ToArray();

                if (lineItems.Length < skipColumnCount)
                {
                    if (!ignoreEmptyLine)
                        rows.Add(new string[0]);
                }
                else
                {
                    rows.Add(lineItems[skipColumnCount..]);
                }
            }
        }

        return rows.ToArray();
    }
    internal static string[][] ReadCsv_Columns(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false)
    {
        // main
        var rows = ReadCsv_Rows(inputFile, skipColumnCount, skipRowCount, ignoreEmptyLine);
        var columns = rows.Transpose();
        return columns;
    }

    // ★★★★★★★★★★★★★★★ 222 SaveCsv

    /// <summary>
    /// save csv from list of lines
    /// </summary>
    /// <param name="inputFile_Lines"></param>
    /// <param name="outputFile"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static FileInfo SaveCsv_Rows(this string[][] inputFile_Rows, FileInfo outputFile)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS


        // main
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));

        foreach (var row in inputFile_Rows)
        {
            var correctedLine = row
                .Select(x => x ?? "")
                .Select(x => x.Replace("\"", "\"\""))
                .Select(x => (x.Contains(',') || x.Contains('\"')) ? $"\"{x}\"" : x);
            sw.WriteLine(string.Join(',', correctedLine));
        }

        return outputFile;
    }
    internal static FileInfo SaveCsv_Columns(this string[][] inputFile_Columns, FileInfo outputFile)
    {
        // main
        var inputFile_Rows = inputFile_Columns.Transpose();
        return inputFile_Rows.SaveCsv_Rows(outputFile);
    }

    // ★★★★★★★★★★★★★★★ 223 ExtractCsvColumns

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_ExtractCsvColumns/{inputFile.Name}</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    internal static FileInfo ExtractCsvColumns(this FileInfo inputFile, FileInfo? outputFile, int[] extractingColumns, int skipRowCount = 0, string header = null)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, "output_ExtractCsvColumns", inputFile.Name));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        var inputCsv = inputFile.ReadCsv_Rows(0, skipRowCount);

        var resultList = new List<string[]>();

        if (header is not null)
            resultList.Add(header.Split(","));

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

        resultList.ToArray().SaveCsv_Rows(outputFile);

        return outputFile;
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
    internal static DirectoryInfo ExtractCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int[] extractingColumns, int skipRowCount = 0, string header = null)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** ExtractCsvColumns() Called (This takes time...)");
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, "output_ExtractCsvColumns"));
        if (!outputDir.Exists) outputDir.Create();

        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir.FullName, file.Name);
            try
            {
                file.ExtractCsvColumns(new FileInfo(newFilePath), extractingColumns, skipRowCount, header);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {newFilePath}, {ex.Message}");
            }
        }

        return outputDir;
    }

    // ★★★★★★★★★★★★★★★ 224 CollectCsvColumns

    /// <summary>
    /// move all csvs' target column to one csv (.xlsx is also acceptable)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns/output.csv</param>
    /// <param name="targetColumn"></param>
    /// <param name="initialColumn"></param>
    /// <returns></returns>
    internal static FileInfo CollectCsvColumns(this DirectoryInfo inputDir, FileInfo? outputFile, int targetColumn, int initialColumn = 0, int skipRowCount = 0)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** CollectCsvColumns() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputDir.FullName, "output_CollectCsvColumns", "output.csv"));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        var outputDirPath = outputFile.DirectoryName;
        var itemName = Path.GetFileNameWithoutExtension(outputFile.Name);
        var outputExtension = Path.GetExtension(outputFile.Name);

        var csvs = inputDir
            .GetFiles("*.csv", SearchOption.TopDirectoryOnly)
            .Where(x => x.FullName != outputFile.FullName)
            .ToArray();
        if (csvs.Length == 0)
        {
            Console.WriteLine($"※ No csv file found in {inputDir.FullName}");
            return outputFile;
        }

        if (outputExtension == ".csv")
        {
            // collect all data to this column list and save eventually
            var resultColumnList = new string[csvs.Length + 1][];

            // copy one column from inputCsv to outputCsv method
            void AddColumnToResultColumnList(FileInfo csv, int targetInputCsvColumn, int targetOutputCsvColumn, string header = "")
            {
                var csvData = csv.ReadCsv_Rows(skipRowCount);
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
            AddColumnToResultColumnList(csvs[0], 0, initialColumn);

            // copy all of the rest
            for (int i = 0; i < csvs.Length; i++)
            {
                var csvPath = csvs[i].FullName;
                try
                {
                    var header = Path.GetFileNameWithoutExtension(csvPath);
                    AddColumnToResultColumnList(csvs[i], targetColumn, i + 1, header);

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
            CopyCsvToExcelColumn(csvs[0], initialColumn, 0);

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

        return outputFile;
    }
    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, (string name, int targetColumn, int initialColumn)[] targets, int skipRowCount = 0)
    {
        // preprocess
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, "output_CollectCsvColumns"));
        if (inputDir.FullName == outputDir.FullName)
            throw new InvalidOperationException("※ inputDir and outputDir must be different");
        if (!outputDir.Exists) outputDir.Create();

        // main
        foreach (var item in targets)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, item.name + ".csv"));
            inputDir.CollectCsvColumns(outputFile, item.targetColumn, item.initialColumn, skipRowCount);
        }

        return outputDir;
    }
    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns</param>
    /// <param name="target"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, (string name, int targetColumn, int initialColumn) target, int skipRowCount = 0)
    {
        return CollectCsvColumns_Loop(inputDir, outputDir, new (string name, int targetColumn, int initialColumn)[] { target }, skipRowCount);
    }
    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, (string name, int targetColumn)[] targets, int skipRowCount = 0)
    {
        return inputDir.CollectCsvColumns_Loop(outputDir, targets.Select(x => (x.name, x.targetColumn, 0)).ToArray(),skipRowCount);
    }
    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns</param>
    /// <param name="target"></param>
    /// <returns></returns>
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, (string name, int targetColumn) target, int skipRowCount = 0)
    {
        return inputDir.CollectCsvColumns_Loop(outputDir, new (string name, int targetColumn)[] { target }, skipRowCount);
    }

    // ★★★★★★★★★★★★★★★ 225 Csvs2ExcelSheets

    /// <summary>
    /// create an excel file that have sheets from csvs
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Csvs2ExcelSheets/output.xlsx</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static FileInfo Csvs2ExcelSheets(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** Csvs2ExcelSheets() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputDir.FullName, "output_Csvs2ExcelSheets", "output.xlsx"));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        using var workbook = new XLWorkbook();

        var csvs = inputDir
                    .GetFiles("*.csv", SearchOption.TopDirectoryOnly)
                    .Where(x => x.FullName != outputFile.FullName)
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

        return outputFile;
    }

    // ★★★★★★★★★★★★★★★ 

}
