

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// turn byte[] into char list
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
    /// turn byte[] into short list
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
    /// turn byte[] into int list
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
    /// turn byte[] into float list
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
    /// turn byte[] into double list
    /// </summary>
    /// <returns></returns>
    public static double[] ToDoubleArray(this byte[] byteData, int skipBytes = 0)
    {
        var convertedList = new List<double>();
        for (int i = skipBytes; i < byteData.Length; i += 8)
            convertedList.Add(BitConverter.ToDouble(byteData, i));
        return convertedList.ToArray();
    }


    // ★★★★★★★★★★★★★★★ 

}
