using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Class;
internal static class UnlabeledUtil
{
    // still coding...




    /// <summary>
    /// MacOS が生成するゴミを削除
    /// </summary>
    /// <param name="inputDir"></param>
    internal static void OrganizeMacOsJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** OrganizeMacOsJuncFiles() Called");

        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            if (fi.Name == "_DS_Store" || fi.Name.StartsWith("._"))
            {
                fi.Delete();
                Console.WriteLine($"削除：{fi}");
            }
        }
    }

    /// <summary>
    /// 選んだフォルダ内のデータのPhotoとVideoの接頭辞を削除
    /// </summary>
    /// <param name="inputDir"></param>
    internal static void OrganizeDropBoxJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** OrganizeDropBoxJuncFiles() Called");

        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            string oldName = fi.Name;
            string newName = fi.Name;

            newName = newName
                .Replace("スクリーンショット ", "")
                .Replace("Photo ", "")
                .Replace("Video ", "");

            if (oldName != newName)
            {
                var oldFullName = fi.FullName;
                var newFullName = oldFullName.Replace(oldName, newName);

                try
                {
                    fi.MoveTo(newFullName);
                    Console.WriteLine($"変更");
                    Console.WriteLine($"　前：{oldFullName}");
                    Console.WriteLine($"　後：{newFullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー：{newFullName}");
                }
            }
        }
    }



    // ↓完成。テストする！

    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="inputDir"></param>
    internal static int PDFPageCount(this DirectoryInfo inputDir)
    {
        // main
        var totalCount = 0;

        foreach (FileInfo f in inputDir.GetFiles("*.pdf", SearchOption.AllDirectories))
        {
            var page = f.PDFPageCount();
            if (page > 0)
                totalCount += page;
        }

        Console.WriteLine($"Total: {totalCount}");
        return totalCount;
    }
    /// <summary>
    /// PDF page count
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="rootDirPath"></param>
    static int PDFPageCount(this FileInfo inputFile)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためには必要。
        if (UtilConfig.ConsoleOutput)
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
                    Console.WriteLine($"{pages} pages : {inputFile.FullName}");
                    return pages;
                }
            }
        }
        catch (Exception e)
        {
        }
        Console.WriteLine($"X fail : {inputFile.FullName}");
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

}
