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

namespace Aki32_Utilities.ChainableExtensions;
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


    // ★★★★★★★★★★★★★★★ sub

    public static string[] GetImageFilesRegexen(
        bool jpg = true,
        bool png = true,
        bool bmp = true,
        bool heic = true)
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

    public static string[] GetVideoFilesRegexen(
        bool mp4 = true,
        bool avi = true)
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


    // ★★★★★★★★★★★★★★★

}
