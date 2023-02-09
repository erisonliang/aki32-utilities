

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

    /// <summary>
    /// Write 1D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this IEnumerable<int> inputData, int? minValue = null, int? maxValue = null, string separator = "", string StringSet = FadeStringSet4)
    {
        // main
        minValue ??= inputData.Min();
        maxValue ??= inputData.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        var lineItems = inputData.Select(v =>
        {
            var c = StringSet.Last();
            for (int i = 0; i < StringSet.Length; i++)
                if (v < minValue + (i + 1) * strand)
                {
                    c = StringSet[i];
                    break;
                }
            return $"{c}{c}";
        });

        Console.WriteLine(string.Join(separator, lineItems));

    }

    /// <summary>
    /// Write 1D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this IEnumerable<double> inputData, double? minValue = null, double? maxValue = null, string separator = "", string StringSet = FadeStringSet4)
    {
        // main
        minValue ??= inputData.Min();
        maxValue ??= inputData.Max();
        var range = maxValue - minValue;
        var strand = range / StringSet.Length;

        var lineItems = inputData.Select(v =>
        {
            var c = StringSet.Last();
            for (int i = 0; i < StringSet.Length; i++)
                if (v < minValue + (i + 1) * strand)
                {
                    c = StringSet[i];
                    break;
                }
            return $"{c}{c}";
        });

        Console.WriteLine(string.Join(separator, lineItems));

    }

    /// <summary>
    /// Write 2D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this int[,] inputData, int? minValue = null, int? maxValue = null, string separator = "", string StringSet = FadeStringSet4)
    {
        // main
        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
        {
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
            {
                var v = inputData[d0, d1];
                var c = StringSet.Last();

                for (int i = 0; i < StringSet.Length; i++)
                    if (v < minValue + (i + 1) * strand)
                    {
                        c = StringSet[i];
                        break;
                    }

                var item = $"{c}{c}";
                Console.Write($"{item}{separator}");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Write 2D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this double[,] inputData, double? minValue = null, double? maxValue = null, string separator = "", string StringSet = FadeStringSet4)
    {
        // main
        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
        {
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
            {
                var v = inputData[d0, d1];
                var c = StringSet.Last();

                for (int i = 0; i < StringSet.Length; i++)
                    if (v < minValue + (i + 1) * strand)
                    {
                        c = StringSet[i];
                        break;
                    }

                var item = $"{c}{c}";
                Console.Write($"{item}{separator}");
            }
            Console.WriteLine();
        }
    }


    // ★★★★★★★★★★★★★★★ sub

    internal const string FadeStringSet1 = " ░▒▓█";
    internal const string FadeStringSet2 = " ▁▂▃▄▅▆▇█";
    internal const string FadeStringSet3 = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
    internal const string FadeStringSet4 = " .:-=+*#%@";

    // ★★★★★★★★★★★★★★★ 

}
