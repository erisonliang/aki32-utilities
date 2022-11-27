using System.Text;

namespace Aki32_Utilities.ChainableExtensions.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// save csv from a list of rows
    /// </summary>
    /// <param name="inputFile_Rows"></param>
    /// <param name="outputFile"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo SaveCsv_Rows(this string[][] inputFile_Rows, FileInfo outputFile, string? header = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));

        if (!string.IsNullOrEmpty(header))
            sw.WriteLine(header);

        foreach (var row in inputFile_Rows)
        {
            var correctedLine = row
                .Select(x => x ?? "")
                .Select(x => x.Replace("\"", "\"\""))
                .Select(x => (x.Contains(',') || x.Contains('\"')) ? $"\"{x}\"" : x);
            sw.WriteLine(string.Join(',', correctedLine));
        }


        // post process
        return outputFile;
    }

    /// <summary>
    /// save csv from a list of columns
    /// </summary>
    /// <param name="inputFile_Columns"></param>
    /// <param name="outputFile"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo SaveCsv_Columns(this string[][] inputFile_Columns, FileInfo outputFile, string? header = null)
    {
        // main
        var inputFile_Rows = inputFile_Columns.Transpose();
        return inputFile_Rows!.SaveCsv_Rows(outputFile, header);
    }

}
