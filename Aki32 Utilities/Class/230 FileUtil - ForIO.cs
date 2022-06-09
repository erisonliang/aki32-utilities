using System.Text;

namespace Aki32_Utilities.Class;

internal static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 231 ReadCsv_Lines

    /// <summary>
    /// read csv as list of lines
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="ignoreEmptyLine"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static string[][] ReadCsv_Lines(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false, char escapeChar = '\"')
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。


        // main            
        var lines = new List<string[]>();

        using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

        for (int i = 0; i < skipRowCount; i++)
            sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrEmpty(line.Replace(",", "")))
            {
                if (!ignoreEmptyLine)
                    lines.Add(new string[0]);
            }
            else
            {
                // TODO process escapeChar
                var lineItems = line.Split(",", StringSplitOptions.None);
                if (lineItems.Length < skipColumnCount)
                {
                    if (!ignoreEmptyLine)
                        lines.Add(new string[0]);
                }
                else
                {
                    lines.Add(lineItems[skipColumnCount..]);
                }
            }
        }

        return lines.ToArray();
    }

    // ★★★★★★★★★★★★★★★ 233 SaveCsv_Lines

    /// <summary>
    /// save csv from list of lines
    /// </summary>
    /// <param name="inputFile_Lines"></param>
    /// <param name="outputFile"></param>
    /// <param name="escapeChar"></param>
    /// <returns></returns>
    internal static FileInfo SaveCsv_Lines(this string[][] inputFile_Lines, FileInfo outputFile, char escapeChar = '\"')
    {
        // 初期処理
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));

        foreach (var line in inputFile_Lines)
        {
            var correctedLine = line.Select(x => x.Contains(',') ? $"{escapeChar}{x}{escapeChar}" : x);
            sw.WriteLine(string.Join(',', correctedLine));
        }

        return outputFile;
    }

    // ★★★★★★★★★★★★★★★ 

}