using System.IO.Compression;
using System.Text;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// return Null if IsNullOrEmpty is true
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string? NullIfNullOrEmpty(this string? input)
    {
        return string.IsNullOrEmpty(input) ? null : input;
    }

}
