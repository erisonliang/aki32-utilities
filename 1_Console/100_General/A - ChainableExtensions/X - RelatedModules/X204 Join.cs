using System.Runtime.InteropServices;
using System.Security;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// return appended string.
    /// </summary>
    /// <param name="strings"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable<string> strings, string separater)
    {
        return string.Join(separater, strings);
    }

    /// <summary>
    /// return appended string.
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string Join(this IEnumerable<char> chars, string separater)
    {
        return string.Join(separater, chars);
    }


    // ★★★★★★★★★★★★★★★ 

}
