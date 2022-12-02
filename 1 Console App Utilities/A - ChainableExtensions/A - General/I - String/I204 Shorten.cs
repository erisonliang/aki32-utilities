using System.IO.Compression;
using System.Text;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Try shorten string input. Return input when failed
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string Shorten(this string input, Range range)
    {
        try
        {
            return input[range];
        }
        catch (Exception)
        {
            return input;
        }
    }

}
