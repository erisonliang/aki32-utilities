using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// read csv as a list of rows
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="ignoreEmptyLine"></param>
    /// <returns></returns>
    public static string[][] ReadCsv_Rows(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic("ReadCsv", false) ;


        // main            
        var rows = new List<string[]>();

        using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

        for (int i = 0; i < skipRowCount; i++)
            sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine()!;
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


        // post process
        return rows.ToArray();
    }

    /// <summary>
    /// read csv as a list of columns
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
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
