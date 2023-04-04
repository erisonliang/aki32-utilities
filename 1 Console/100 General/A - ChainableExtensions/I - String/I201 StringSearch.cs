using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json.Serialization;

using OpenCvSharp.Flann;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// Syntax sugar for Regex.IsMatch method.
    /// Return true if string matches to search regex.
    /// </summary>
    /// <param name="inputString">target string</param>
    /// <param name="searchRegex">match pattern</param>
    /// <returns>True if string matches to search regex</returns>
    public static bool IsMatch(this string inputString, string searchRegex)
    {
        return Regex.IsMatch(inputString, searchRegex);
    }

    /// <summary>
    /// Return true if string matches to any of search regexen.
    /// </summary>
    /// <param name="inputString">target string</param>
    /// <param name="searchRegexen">match patterns</param>
    /// <returns>True if string matches to any of search regexen</returns>
    public static bool IsMatchAny(this string inputString, params string[] searchRegexen)
    {
        foreach (var searchRegex in searchRegexen)
            if (inputString.IsMatch(searchRegex))
                return true;
        return false;
    }

    public static bool ContainsJapanese(this string text)
    {
        return text.IsMatch(@"[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+");
    }


    // ★★★★★★★★★★★★★★★ sub

    public static string[] GetRegexen_ImageFiles(
            bool jpg = false,
            bool png = false,
            bool bmp = false,
            bool heic = false)
    {
        var regexen = new List<string>();

        if (jpg)
        {
            regexen.Add(@"^.*\.jpg$");
            regexen.Add(@"^.*\.JPG$");
            regexen.Add(@"^.*\.jpeg$");
            regexen.Add(@"^.*\.JPEG$");
        }
        if (png)
        {
            regexen.Add(@"^.*\.png$");
            regexen.Add(@"^.*\.PNG$");
        }
        if (bmp)
        {
            regexen.Add(@"^.*\.bmp$");
            regexen.Add(@"^.*\.BMP$");
        }
        if (heic)
        {
            regexen.Add(@"^.*\.heic");
            regexen.Add(@"^.*\.HEIC$");
        }

        return regexen.ToArray();
    }

    public static string[] GetRegexen_SoundFiles(
        bool wav = false,
        bool mp3 = false,
        bool m4a = false)
    {
        var regexen = new List<string>();

        if (wav)
        {
            regexen.Add(@"^.*\.wav$");
            regexen.Add(@"^.*\.WAV$");
        }
        if (mp3)
        {
            regexen.Add(@"^.*\.mp3$");
            regexen.Add(@"^.*\.MP3$");
        }
        if (m4a)
        {
            regexen.Add(@"^.*\.m4a$");
            regexen.Add(@"^.*\.M4A$");
        }

        return regexen.ToArray();
    }

    public static string[] GetRegexen_VideoFiles(
        bool mp4 = false,
        bool avi = false)
    {
        var regexen = new List<string>();

        if (mp4)
        {
            regexen.Add(@"^.*\.mp4$");
            regexen.Add(@"^.*\.MP4$");
        }
        if (avi)
        {
            regexen.Add(@"^.*\.avi$");
            regexen.Add(@"^.*\.AVI$");
        }

        return regexen.ToArray();
    }

    public static string[] GetRegexen_CsvFiles(
        bool csv = false)
    {
        var regexen = new List<string>();

        if (csv)
        {
            regexen.Add(@"^.*\.csv$");
            regexen.Add(@"^.*\.CSV$");
        }

        return regexen.ToArray();
    }

    public static string[] GetRegexen_XmlTypedExcelFiles(
        bool xlsx = false,
        bool xlsm = false,
        bool xltx = false,
        bool xltm = false)
    {
        var regexen = new List<string>();

        if (xlsx)
        {
            regexen.Add(@"^.*\.xlsx$");
            regexen.Add(@"^.*\.XLSX$");
        }

        if (xlsm)
        {
            regexen.Add(@"^.*\.xlsm$");
            regexen.Add(@"^.*\.XLSM$");
        }

        if (xltx)
        {
            regexen.Add(@"^.*\.xltx$");
            regexen.Add(@"^.*\.XLTX$");
        }

        if (xltm)
        {
            regexen.Add(@"^.*\.xltm$");
            regexen.Add(@"^.*\.XLTM$");
        }

        return regexen.ToArray();
    }


    // ★★★★★★★★★★★★★★★

}
