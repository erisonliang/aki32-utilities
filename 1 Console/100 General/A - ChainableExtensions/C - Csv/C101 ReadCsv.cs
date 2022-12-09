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
    public static string[][] ReadCsv_Rows(this FileInfo inputFile, int skipColumnCount = 0, int skipRowCount = 0, bool ignoreEmptyLine = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main            
        var rows = new List<string[]>();

        rows = LoadCsv(inputFile.FullName);

        //using var sr = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

        //for (int i = 0; i < skipRowCount; i++)
        //    sr.ReadLine();

        //while (!sr.EndOfStream)
        //{
        //    var line = sr.ReadLine()!;
        //    if (string.IsNullOrEmpty(line.Replace(",", "")))
        //    {
        //        if (!ignoreEmptyLine)
        //            rows.Add(Array.Empty<string>());
        //    }
        //    else
        //    {
        //        var escapedFlag = false;
        //        if (line.Contains('"'))
        //        {
        //            escapedFlag = true;
        //            line = line.Replace($"\"\"", "{ignoringDQ}");
        //            var splitedLine = line.Split("\"", StringSplitOptions.None);
        //            for (int i = 0; i < splitedLine.Length; i++)
        //                if (i % 2 == 1)
        //                    splitedLine[i] = splitedLine[i].Replace(",", "{ignoringComma}");
        //            line = string.Join("", splitedLine);
        //            line = line.Replace("{ignoringDQ}", $"\"");
        //        }

        //        var lineItems = line.Split(",", StringSplitOptions.None);

        //        if (escapedFlag)
        //            lineItems = lineItems.Select(x => x.Replace("{ignoringComma}", ",")).ToArray();

        //        if (lineItems.Length < skipColumnCount)
        //        {
        //            if (!ignoreEmptyLine)
        //                rows.Add(new string[0]);
        //        }
        //        else
        //        {
        //            rows.Add(lineItems[skipColumnCount..]);
        //        }
        //    }
        //}

        //// if negative
        //if (skipRowCount < 0)
        //    rows.RemoveRange(0, rows.Count + skipRowCount);

        // post process
        return rows.ToArray();
    }

    private static List<string[]> LoadCsv(string fileName, char delimiter = ',', string encodeName = "shift-jis")
    {
        //結果を格納するリスト
        List<string[]> result = new List<string[]>();

        //カンマで分割した1行分を格納するリスト
        List<string> line = new List<string>();

        //１カラム分の値を格納する変数
        StringBuilder value = new StringBuilder();

        //ダブルクォーテーションの中であることを現わすフラグ
        bool dq_flg = false;

        //ファイルをオープンする
        using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(encodeName)))
        {
            //ファイルの最後になるまでループする
            while (!sr.EndOfStream)
            {
                //1文字読み込む
                var ch = (char)sr.Read();

                //ダブルクオーテーションが見つかるとフラグを反転する
                dq_flg = (ch == '\"') ? !dq_flg : dq_flg;

                //ダブルクォーテーション中ではないキャリッジリターンは破棄する
                if (ch == '\r' && dq_flg == false)
                {
                    continue;
                }

                //ダブルクォーテーション中ではない時にカンマが見つかったら、
                //それまでに読み取った文字列を１つのかたまりとしてline に追加する
                if (ch == delimiter && dq_flg == false)
                {
                    line.Add(to_str(value));
                    value.Clear();
                    continue;
                }

                //ダブルクォーテーション中ではない時にラインフィードが見つかったら
                //line（1行分） を result に追加する
                if (ch == '\n' && dq_flg == false)
                {
                    line.Add(to_str(value));
                    result.Add(line.ToArray());
                    line.Clear();
                    value.Clear();
                    continue;
                }
                value.Append(ch);
            }
        }

        //ファイル末尾が改行コードでない場合、ループを抜けてしまうので、
        //未処理の項目がある場合は、ここでline に追加
        if (value.Length > 0)
        {
            line.Add(to_str(value));
            result.Add(line.ToArray());
        }

        return result;

        //前後のダブルクォーテーションを削除し、2個連続するダブルクォーテーションを1個に置換する
        string to_str(StringBuilder p_str)
        {
            string l_val = p_str.ToString().Replace("\"\"", "\"");
            int l_start = (l_val.StartsWith("\"")) ? 1 : 0;
            int l_end = l_val.EndsWith("\"") ? 1 : 0;
            return l_val.Substring(l_start, l_val.Length - l_start - l_end);
        }
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
