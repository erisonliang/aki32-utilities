using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Class;
internal static class PDFUtil
{

    // ★★★★★★★★★★★★★★★ 411 PDFPageCount

    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="inputDir"></param>
    internal static int PDFPageCount(this DirectoryInfo inputDir, bool topDirectoryOnly = false)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** PDFPageCount() Called");

        // main
        var totalCount = 0;

        foreach (FileInfo f in inputDir.GetFiles("*.pdf", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories))
        {
            var page = f.PDFPageCount(false);
            if (page > 0) totalCount += page;
        }

        Console.WriteLine($"--------------------------");
        Console.WriteLine($"{totalCount,5} pages in total (only for success counts)");
        Console.WriteLine($"--------------------------");
        return totalCount;
    }
    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="rootDirPath"></param>
    internal static int PDFPageCount(this FileInfo inputFile, bool initialConsoleOutput = true)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (UtilConfig.ConsoleOutput && initialConsoleOutput)
            Console.WriteLine("\r\n** PDFPageCount() Called");

        // main
        try
        {
            Regex rgx = new Regex(@"/Count ", RegexOptions.IgnoreCase);
            using var sw = new StreamReader(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS"));

            while (!sw.EndOfStream)
            {
                var line = sw.ReadLine();
                if (rgx.Match(line).Success)
                {
                    var pages = GetPageCount(line);
                    Console.WriteLine($"{pages,5} pages in {inputFile.FullName}");
                    return pages;
                }
            }
        }
        catch (Exception e)
        {
        }
        Console.WriteLine($"---ERROR--- in {inputFile.FullName}");
        return -1;

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
            return int.Parse(rtLine);
        }
    }

    // ★★★★★★★★★★★★★★★ 

}
