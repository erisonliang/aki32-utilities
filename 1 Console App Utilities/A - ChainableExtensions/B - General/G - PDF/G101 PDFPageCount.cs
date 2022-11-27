using System.Text;
using System.Text.RegularExpressions;

namespace Aki32_Utilities.General.ChainableExtensions;
public static partial class ChainableExtensions
{

    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="initialConsoleOutput"></param>
    /// <param name="resultConsoleOutput"></param>
    /// <returns> -1 if error </returns>
    public static int? PDFPageCount(this FileInfo inputFile, bool initialConsoleOutput = true, bool resultConsoleOutput = true)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main
        try
        {
            var rgx = new Regex(@"/Count ", RegexOptions.IgnoreCase);
            using var sw = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

            while (!sw.EndOfStream)
            {
                var line = sw.ReadLine()!;
                if (rgx.Match(line).Success)
                {
                    var pages = GetPageCount(line);
                    if (resultConsoleOutput)
                        Console.WriteLine($"{pages,5} pages in {inputFile.FullName}");
                    return pages;
                }
            }
        }
        catch (Exception)
        {
        }
        if (resultConsoleOutput)
            Console.WriteLine($"---ERROR--- in {inputFile.FullName}");
        return null;

        static int GetPageCount(string line)
        {
            string rtLine = "";
            string searchWord = "/Count ";
            int i0 = line.IndexOf("/Count ");
            if (i0 >= 0)
            {
                int ii0 = line.IndexOf("/", i0 + searchWord.Length);
                if (ii0 < 0) { ii0 = line.IndexOf(" ", i0 + searchWord.Length); }
                if (ii0 < 0) { ii0 = line.IndexOf(">>", i0 + searchWord.Length); }
                if (ii0 >= 0)
                {
                    line = line.Substring(i0, ii0 - i0);
                }
                else
                {
                    line = line.Substring(i0);
                }
                line = line.Replace("/Count ", "");
                rtLine = line;
            }
            return Math.Abs(int.Parse(rtLine));
        }
    }

    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="topDirectoryOnly"></param>
    /// <param name="ConsoleOutput"></param>
    /// <returns> -1 if error </returns>
    public static int[] PDFPageCount(this DirectoryInfo inputDir, bool topDirectoryOnly = false, bool ConsoleOutput = true)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);


        // main
        var pageList = new List<int>();
        var totalCount = 0;
        var errorCount = 0;

        var targets = inputDir.GetFiles("*.pdf", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories).ToArray();
        Parallel.ForEach(targets, t =>
        {
            var page = t.PDFPageCount(false);
            if (page == null)
            {
                errorCount++;
            }
            else
            {
                pageList.Add(page.Value);
                totalCount += page.Value;
            }
        });

        // post process
        if (ConsoleOutput)
        {
            Console.WriteLine($"--------------------------");
            Console.WriteLine($"{totalCount,5} pages in total {(errorCount > 0 ? $"(※ exept for {errorCount} errors)" : "")}");
            Console.WriteLine($"--------------------------");
        }
        return pageList.ToArray();
    }

}
