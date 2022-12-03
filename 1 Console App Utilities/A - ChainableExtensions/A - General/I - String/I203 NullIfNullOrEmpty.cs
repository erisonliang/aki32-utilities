﻿using System.IO.Compression;
using System.Text;

namespace Aki32_Utilities.Console_App_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Return null if IsNullOrEmpty() is true
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string? NullIfNullOrEmpty(this string? input)
    {
        return string.IsNullOrEmpty(input) ? null : input;
    }

}
