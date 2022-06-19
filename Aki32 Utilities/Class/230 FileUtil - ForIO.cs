using System.Text;

namespace Aki32_Utilities.Class;

internal static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 231 ReadCsv

    /// <summary>
    /// read csv as list of lines
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="ignoreEmptyLine"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static string[][] ReadCsv_Rows(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false, char escapeChar = '\"', bool initialConsoleOutput = true)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (UtilConfig.ConsoleOutput && initialConsoleOutput)
            Console.WriteLine("\r\n** ReadCsv_Rows() Called");


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
                // TODO: need to implement escapeChar
                var lineItems = line.Split(",", StringSplitOptions.None);
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
    internal static string[][] ReadCsv_Columns(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false, char escapeChar = '\"')
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** ReadCsv_Columns() Called");

        // main
        var rows = ReadCsv_Rows(inputFile, skipColumnCount, skipRowCount, ignoreEmptyLine, escapeChar, false);
        var columns = rows.Transpose2DArray();
        return columns;
    }

    // ★★★★★★★★★★★★★★★ 232 SaveCsv

    /// <summary>
    /// save csv from list of lines
    /// </summary>
    /// <param name="inputFile_Lines"></param>
    /// <param name="outputFile"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static FileInfo SaveCsv_Rows(this string[][] inputFile_Rows, FileInfo outputFile, char escapeChar = '\"', bool initialConsoleOutput = true)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (UtilConfig.ConsoleOutput && initialConsoleOutput)
            Console.WriteLine("\r\n** SaveCsv_Rows() Called");


        // main
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));

        foreach (var row in inputFile_Rows)
        {
            var correctedLine = row.Select(x => x ?? "").Select(x => x.Contains(',') ? $"{escapeChar}{x}{escapeChar}" : x);
            sw.WriteLine(string.Join(',', correctedLine));
        }

        return outputFile;
    }
    internal static FileInfo SaveCsv_Colums(this string[][] inputFile_Columns, FileInfo outputFile, char escapeChar = '\"')
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** SaveCsv_Colums() Called");


        // main
        var inputFile_Rows = inputFile_Columns.Transpose2DArray();
        return inputFile_Rows.SaveCsv_Rows(outputFile, escapeChar, false);
    }

    // ★★★★★★★★★★★★★★★ 
}