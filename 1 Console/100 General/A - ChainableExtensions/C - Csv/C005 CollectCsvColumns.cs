using Aki32Utilities.ConsoleAppUtilities.General;

using ClosedXML.Excel;

using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
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
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir!, "output.csv");
        if (!outputFile!.Name.EndsWith(".csv"))
            throw new NotImplementedException("outputFile need to be .csv or null.");


        // main
        var outputDirPath = outputFile!.DirectoryName;
        var itemName = Path.GetFileNameWithoutExtension(outputFile.Name);

        var csvs = inputDir
            .GetFiles("*.csv", SearchOption.TopDirectoryOnly)
            .Where(x => x.FullName != outputFile.FullName)
            .Sort()
            .ToArray();

        if (csvs.Length == 0)
        {
            ConsoleExtension.WriteLineWithColor($"※ No csv file found in {inputDir.FullName}", ConsoleColor.Red);
            return outputFile!;
        }

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
            var header = Path.GetFileNameWithoutExtension(csvPath);
            AddColumnToResultColumnList(csvs[i], targetColumn, i + UseInitColumnThen1, header);
        }

        // save
        resultColumnList.SaveCsv_Columns(outputFile);


        // post process
        return outputFile!;
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
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir, takesTimeFlag: true);
        if (inputDir.FullName == outputDir!.FullName)
            throw new InvalidOperationException("※ inputDir and outputDir must be different");
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();


        // main
        foreach (var (name, initialColumn, targetColumn) in targets)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, $"{name}.csv"));
            inputDir.CollectCsvColumns(outputFile, targetColumn, initialColumn, skipRowCount);
        }


        // post process
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return outputDir!;
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
