

using System.Text;

using DocumentFormat.OpenXml.Bibliography;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ From Bytes

    /// <summary>
    /// turn byte array into char array
    /// </summary>
    /// <returns></returns>
    public static char[] ToCharArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<char>();
        for (int i = skipBytes; i < byteData.Length; i += 1)
            convertedList.Add(BitConverter.ToChar(byteData, i));
        return convertedList.ToArray();
    }

    /// <summary>
    /// turn byte array into short array
    /// </summary>
    /// <returns></returns>
    public static short[] ToShortArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<short>();
        for (int i = skipBytes; i < byteData.Length; i += 2)
            convertedList.Add(BitConverter.ToInt16(byteData, i));
        return convertedList.ToArray();
    }

    /// <summary>
    /// turn byte array into int array
    /// </summary>
    /// <returns></returns>
    public static int[] ToIntArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<int>();
        for (int i = skipBytes; i < byteData.Length; i += 4)
            convertedList.Add(BitConverter.ToInt32(byteData, i));
        return convertedList.ToArray();
    }

    /// <summary>
    /// turn byte array into float array
    /// </summary>
    /// <returns></returns>
    public static float[] ToFloatArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<float>();
        for (int i = skipBytes; i < byteData.Length; i += 4)
            convertedList.Add(BitConverter.ToSingle(byteData, i));
        return convertedList.ToArray();
    }

    /// <summary>
    /// turn byte array into double array
    /// </summary>
    /// <returns></returns>
    public static double[] ToDoubleArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<double>();
        for (int i = skipBytes; i < byteData.Length; i += 8)
            convertedList.Add(BitConverter.ToDouble(byteData, i));
        return convertedList.ToArray();
    }


    // ★★★★★★★★★★★★★★★ To Bytes (sugar)

    public static byte[] ToBytes(this short input) => BitConverter.GetBytes(input);
    public static byte[] ToBytes(this int input) => BitConverter.GetBytes(input);
    public static byte[] ToBytes(this long input) => BitConverter.GetBytes(input);

    public static byte[] ToBytes(this ushort input) => BitConverter.GetBytes(input);
    public static byte[] ToBytes(this uint input) => BitConverter.GetBytes(input);
    public static byte[] ToBytes(this ulong input) => BitConverter.GetBytes(input);

    public static byte[] ToBytes(this float input) => BitConverter.GetBytes(input);
    public static byte[] ToBytes(this double input) => BitConverter.GetBytes(input);

    public static byte[] ToBytes(this string input) => Encoding.ASCII.GetBytes(input);
    public static byte[] ToBytes(this IEnumerable<char> input) => Encoding.ASCII.GetBytes(input.ToArray());


    // ★★★★★★★★★★★★★★★ 

}
