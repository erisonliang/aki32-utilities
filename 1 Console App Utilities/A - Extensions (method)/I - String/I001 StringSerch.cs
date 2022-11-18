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

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// Return if string match any of search regexen
    /// </summary>
    /// <param name="inputString">target string</param>
    /// <param name="searchRegexen">match patterns</param>
    /// <returns></returns>
    public static bool IsMatchAny(this string inputString, params string[] searchRegexen)
    {
        foreach (var searchRegex in searchRegexen)
            if (Regex.IsMatch(inputString, searchRegex))
                return true;
        return false;
    }

    // ★★★★★★★★★★★★★★★ sub

    public static string[] GetImageFileRegexen(bool jpg = false, bool png = false)
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

        return regexen.ToArray();
    }


    // ★★★★★★★★★★★★★★★

}
