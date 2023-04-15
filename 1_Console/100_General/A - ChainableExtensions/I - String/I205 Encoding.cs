using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// ConvertEncoding
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ConvertEncoding(this string input, Encoding inputEncoding, Encoding outputEncoding)
    {
        byte[] src_temp = inputEncoding.GetBytes(input);
        byte[] dest_temp = Encoding.Convert(inputEncoding, outputEncoding, src_temp);
        string ret = outputEncoding.GetString(dest_temp);
        return ret;
    }

    /// <summary>
    /// ConvertEncoding
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ConvertEncoding_FromShiftJIS(this string input, Encoding outputEncoding)
    {
        byte[] src_temp = Encoding.GetEncoding("Shift_JIS").GetBytes(input);
        byte[] dest_temp = Encoding.Convert(Encoding.ASCII, outputEncoding, src_temp);
        string ret = outputEncoding.GetString(dest_temp);
        return ret;
    }

}
