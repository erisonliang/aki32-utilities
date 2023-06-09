﻿

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo ExtractCsvColumns(this FileInfo inputFile, FileInfo? outputFile, int[] extractingColumns, int skipRowCount = 0, string? header = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        var inputCsv = inputFile.ReadCsv_Rows(0, skipRowCount);

        var resultList = new List<string[]>();

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

        resultList.ToArray().SaveAsCsv_Rows(outputFile!, header);
        GC.Collect();

        // post process
        return outputFile!;
    }

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="skipRowCount"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static FileInfo ExtractCsvColumnsForMany(this FileInfo inputFile, FileInfo? outputFile, int skipRowCount = 0, params (string name, int[] extractingColumns, string? header)[] targets)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        foreach (var (name, extractingColumns, header) in targets)
        {
            var oldname = Path.GetFileNameWithoutExtension(outputFile!.Name);
            var newOutputFile = new FileInfo(Path.Combine(outputFile!.Directory!.FullName, $"{oldname}_{name}{inputFile.Extension}"));
            inputFile.ExtractCsvColumns(newOutputFile, extractingColumns, skipRowCount, header);
        }


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo ExtractCsvColumns_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int[] extractingColumns, int skipRowCount = 0, string? header = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.ExtractCsvColumns(outF, extractingColumns, skipRowCount, header));

    /// <summary>
    /// extranct designated columns from csv to new csv
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="skipRowCount"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static DirectoryInfo ExtractCsvColumnsForMany_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipRowCount = 0, params (string name, int[] extractingColumns, string? header)[] targets)
        => inputDir.Loop(outputDir, (inF, outF) => inF.ExtractCsvColumnsForMany(outF, skipRowCount, targets));


    // ★★★★★★★★★★★★★★★ 

}
