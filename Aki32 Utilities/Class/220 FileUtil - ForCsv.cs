using ClosedXML.Excel;
using System.Text;

namespace Aki32_Utilities.Class;
internal static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 221 ExtractCsvColumns

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
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, "output_ExtractCsvColumns", inputFile.Name));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        var inputCsv = inputFile.ReadCsv_Lines();

        var resultList = new List<string>();

        if (header is not null)
            resultList.Add(string.Join(",", header));

        for (int i = skipRowCount; i < inputCsv.Length; i++) //TODO make this faster
        {
            var addingLine = "";
            foreach (var ec in extractingColumns)
            {
                try
                {
                    addingLine += inputCsv[i][ec];
                }
                catch (Exception)
                {
                }
                addingLine += ",";
            }
            resultList.Add(addingLine);
        }

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));
        foreach (var item in resultList)
            sw.WriteLine(item);

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

    // ★★★★★★★★★★★★★★★ 222 CollectCsvColumns

    /// <summary>
    /// move all csvs' target column to one csv (.xlsx is also acceptable)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns/output.csv</param>
    /// <param name="targetColumn"></param>
    /// <param name="initialColumn"></param>
    /// <returns></returns>
    internal static FileInfo CollectCsvColumns(this DirectoryInfo inputDir, FileInfo? outputFile, int targetColumn, int initialColumn = 0)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** CollectCsvColumns() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
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
            // 全てのをテーブルにまとめて，最後に整形して保存する流れ。
            var resultColumnList = new string[csvs.Length + 1][];

            // CsvからExcelにコピーさせる関数
            void AddColumnToResultColumnList(FileInfo csv, int targetInputCsvColumn, int targetOutputCsvColumn, string header = "")
            {
                var csvData = csv.ReadCsv_Lines();
                var tempCsvColumn = new List<string>();
                tempCsvColumn.Add(header);
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

            // 最初の1列を持ってくる。
            AddColumnToResultColumnList(csvs[0], 0, initialColumn);

            // 他の全てを持ってくる
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

            // 行列を入れ替えて，保存。
            var columnCount = resultColumnList.Length;
            var rowCount = resultColumnList.Max(x => x.Length);
            var resultCsv = new string[rowCount][];
            for (int i = 0; i < rowCount; i++)
            {
                var line = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    try
                    {
                        line[j] = resultColumnList[j][i];
                    }
                    catch (Exception)
                    {
                        line[j] = "";
                    }
                }
                resultCsv[i] = line;
            }

            resultCsv.SaveCsv_Lines(outputFile);
        }
        else if (outputExtension == ".xlsx")
        {
            // Excel (※A1セルの座標が(1,1))
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("All");

            // CsvからExcelにコピーさせる関数
            void CopyCsvToExcelColumn(FileInfo csv, int targetCsvColumn, int targetExcelColumn)
            {
                var sr = new StreamReader(csv.FullName, Encoding.GetEncoding("SHIFT_JIS"));
                var all = sr.ReadToEnd().Split("\r\n").Select(x => { try { return x.Split(',')[targetCsvColumn]; } catch { return ""; } }).ToArray();

                for (int i = 0; i < all.Count(); i++)
                    worksheet.Cell(i + 2, targetExcelColumn + 1).Value = all[i];
            }

            // 最初の1列を持ってくる。
            CopyCsvToExcelColumn(csvs[0], initialColumn, 0);

            // 他の全てを持ってくる
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
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params (string name, int targetColumn, int initialColumn)[] targets)
    {
        // preprocess
        if (inputDir.FullName == outputDir.FullName)
            throw new InvalidOperationException("※ inputDir and outputDir must be different");
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, "output_CollectCsvColumns"));
        if (!outputDir.Exists) outputDir.Create();

        // main
        foreach (var item in targets)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, item.name + ".csv"));
            inputDir.CollectCsvColumns(outputFile, item.targetColumn, item.initialColumn);
        }

        return outputDir;
    }
    /// <summary>
    /// move all csvs' target column to one csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_CollectCsvColumns</param>
    /// <param name="targets"></param>
    /// <returns></returns>
    internal static DirectoryInfo CollectCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params (string name, int targetColumn)[] targets)
    {
        return inputDir.CollectCsvColumns_Loop(outputDir, targets.Select(x => (x.name, x.targetColumn, 0)).ToArray());
    }

    // ★★★★★★★★★★★★★★★ 223 Csvs2ExcelSheets

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
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
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

                var inputCsv = csv.ReadCsv_Lines();
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

    // ★★★★★★★★★★★★★★★ 224 MergeAllLines

    /// <summary>
    /// merge all file's lines in one file
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_MergeAllLines/output.txt</param>
    /// <param name="skipRowCount"></param>
    /// <returns></returns>
    internal static FileInfo MergeAllLines(this DirectoryInfo inputDir, FileInfo outputFile, int skipRowCount = 0)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** MergeAllLines() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputDir.FullName, "output_MergeAllLines", $"output.txt"));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        var files = inputDir.GetFiles("*", SearchOption.TopDirectoryOnly);
        files = files.Where(x => !x.Name.Contains(outputFile.DirectoryName)).ToArray();

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));
        foreach (var f in files)
        {
            try
            {
                var input = File.ReadLines(f.FullName, Encoding.GetEncoding("SHIFT_JIS")).ToArray();
                for (int i = skipRowCount; i < input.Length; i++)
                    sw.WriteLine(input[i]);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {f.FullName}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {f.FullName}, {ex.Message}");
            }
        }

        return outputFile;
    }

    // ★★★★★★★★★★★★★★★ 

}
