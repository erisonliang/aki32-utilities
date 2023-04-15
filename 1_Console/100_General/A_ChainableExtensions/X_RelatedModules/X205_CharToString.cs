

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// return appended chars string.
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string ToString_Extension(this IEnumerable<char> chars)
    {
        return new string(chars.ToArray());
    }


    // ★★★★★★★★★★★★★★★ 

}
