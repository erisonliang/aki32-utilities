using System.IO.Compression;
using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Try shorten string input. Return input when failed
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Shorten(this string input, Range range)
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
