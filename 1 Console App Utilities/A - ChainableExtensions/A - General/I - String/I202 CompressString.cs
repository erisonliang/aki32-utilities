using System.IO.Compression;
using System.Text;

namespace Aki32_Utilities.Console_App_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Compress String
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string CompressString(this string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);

        using var inputStream = new MemoryStream();
        using var compressedInputStream = new DeflateStream(inputStream, CompressionMode.Compress, true);

        compressedInputStream.Write(inputBytes, 0, inputBytes.Length);
        compressedInputStream.Close();

        var outputBytes = inputStream.ToArray();

        return Convert.ToBase64String(outputBytes, Base64FormattingOptions.InsertLineBreaks);
    }

    /// <summary>
    /// Decompress String
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string DecompressString(this string input)
    {
        var inputBytes = Convert.FromBase64String(input);

        using var inputStream = new MemoryStream(inputBytes);
        using var outputStream = new MemoryStream();
        using var decompressedInputStream = new DeflateStream(inputStream, CompressionMode.Decompress);

        while (true)
        {
            int rb = decompressedInputStream.ReadByte();
            if (rb == -1)
                break;
            outputStream.WriteByte((byte)rb);
        }

        string result = Encoding.UTF8.GetString(outputStream.ToArray());

        return result;
    }

}
