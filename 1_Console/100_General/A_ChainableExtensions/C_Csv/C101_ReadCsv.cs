using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// read csv as a list of rows
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount">negative to get from bottom. eg: -1 for only the last row</param>
    /// <param name="ignoreEmptyLine"></param>
    /// <returns></returns>
    public static string[][] ReadCsv_Rows(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false, char separater = ',', string encodeName = "SHIFT_JIS")
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main            
        var rows = new List<string[]>();

        using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding(encodeName));

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine()!;
            if (string.IsNullOrEmpty(line.Replace(",", "")))
            {
                if (!ignoreEmptyLine && skipRowCount <= 0)
                    rows.Add(Array.Empty<string>());

                // even when ignoring empty line, think as skipped 
                if (skipRowCount > 0)
                    skipRowCount--;
            }
            else
            {
                var escapedFlag = false;

                // While " count is odd number, merge next line
                while (line.Count(c => c == '"') % 2 == 1)
                {
                    line += "\r\n";
                    line += sr.ReadLine()!;
                }

                if (skipRowCount > 0)
                {
                    skipRowCount--;
                    continue;
                }

                if (line.Contains('"'))
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
                        rows.Add(Array.Empty<string>());
                }
                else
                {
                    rows.Add(lineItems[skipColumnCount..]);
                }
            }
        }

        // if negative
        if (skipRowCount < 0)
            rows.RemoveRange(0, rows.Count + skipRowCount);

        // post process
        return rows.ToArray();
    }

    /// <summary>
    /// read csv as a list of columns
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount">negative to get from bottom. eg: -1 for only the last row</param>
    /// <param name="ignoreEmptyLine"></param>
    /// <returns></returns>
    public static string[][] ReadCsv_Columns(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false)
    {
        // main
        var rows = ReadCsv_Rows(inputFile, skipColumnCount, skipRowCount, ignoreEmptyLine);
        var columns = rows.Transpose();
        return columns!;
    }

}
