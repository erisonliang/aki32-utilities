

using System.Drawing;

using NAudio.Wave;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable (to console)

    /// <summary>
    /// Write 1D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this IEnumerable<int> inputData, int? minValue = null, int? maxValue = null, int repeat = 2, string StringSet = FadeStringSet4)
    {
        // preprocess
        minValue ??= inputData.Min();
        maxValue ??= inputData.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        // main
        var lineItems = inputData.Select(v =>
        {
            var c = StringSet.Last();
            for (int i = 0; i < StringSet.Length; i++)
                if (v < minValue + (i + 1) * strand)
                {
                    c = StringSet[i];
                    break;
                }
            return Enumerable.Repeat(c, repeat).ToString_Extension();
        });

        Console.WriteLine(string.Join("", lineItems));

    }

    /// <summary>
    /// Write 1D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this IEnumerable<double> inputData, double? minValue = null, double? maxValue = null, int repeat = 2, string StringSet = FadeStringSet4)
    {
        // preprocess
        minValue ??= inputData.Min();
        maxValue ??= inputData.Max();
        var range = maxValue - minValue;
        var strand = range / StringSet.Length;

        // main
        var lineItems = inputData.Select(v =>
        {
            var c = StringSet.Last();
            for (int i = 0; i < StringSet.Length; i++)
                if (v < minValue + (i + 1) * strand)
                {
                    c = StringSet[i];
                    break;
                }
            return Enumerable.Repeat(c, repeat).ToString_Extension();
        });

        Console.WriteLine(string.Join("", lineItems));

    }

    /// <summary>
    /// Write 2D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this int[,] inputData, int? minValue = null, int? maxValue = null, int repeat = 2, string StringSet = FadeStringSet4)
    {
        // preprocess
        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        var dim0 = inputData.GetLength(0);
        var dim1 = inputData.GetLength(1);

        // main
        for (int d0 = 0; d0 < dim0; d0++)
        {
            for (int d1 = 0; d1 < dim1; d1++)
            {
                var v = inputData[d0, d1];
                var c = StringSet.Last();

                for (int i = 0; i < StringSet.Length; i++)
                    if (v < minValue + (i + 1) * strand)
                    {
                        c = StringSet[i];
                        break;
                    }

                var item = Enumerable.Repeat(c, repeat).ToString_Extension();
                Console.Write(item);
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Write 2D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this double[,] inputData, double? minValue = null, double? maxValue = null, int repeat = 2, string StringSet = FadeStringSet4)
    {
        // preprocess
        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();
        var range = maxValue - minValue;
        var strand = (double)range / StringSet.Length;

        var dim0 = inputData.GetLength(0);
        var dim1 = inputData.GetLength(1);

        // main
        for (int d0 = 0; d0 < dim0; d0++)
        {
            for (int d1 = 0; d1 < dim1; d1++)
            {
                var v = inputData[d0, d1];
                var c = StringSet.Last();

                for (int i = 0; i < StringSet.Length; i++)
                    if (v < minValue + (i + 1) * strand)
                    {
                        c = StringSet[i];
                        break;
                    }

                var item = Enumerable.Repeat(c, repeat).ToString_Extension();
                Console.Write(item);
            }
            Console.WriteLine();
        }
    }


    // ★★★★★★★★★★★★★★★ chainable (as image)

    /// <summary>
    /// Write 2D array heatmap as an image
    /// </summary>
    /// <param name="inputData"></param>
    /// <param name="outputFile">when null, automatically set to temporary file</param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="targetImageWidth">set desired image width to this. 300 by default</param>
    /// <param name="targetCellSize">number of cell pixels for one data</param>
    /// <returns></returns>
    public static FileInfo DrawHeatmapAsImage(this double[,] inputData,
        FileInfo? outputFile = null,
        double? minValue = null,
        double? maxValue = null,
        int? targetImageWidth = null,
        int? targetCellSize = null,
        bool blackToWhite = true
        )
    {
        // preprocess
        outputFile ??= new FileInfo(Path.GetTempFileName());
        UtilPreprocessors.PreprocessOutFile(ref outputFile!, null!, "heatmap.png");

        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();
        var range = maxValue - minValue;

        var dim0 = inputData.GetLength(0);
        var dim1 = inputData.GetLength(1);
        var zoomFactor = targetImageWidth.HasValue
            ? targetImageWidth.Value / dim0
            : targetCellSize
            ?? 300 / dim0;

        using var bmp = new Bitmap(zoomFactor * dim0, zoomFactor * dim1);
        var fbmp = new FastBitmap(bmp);

        fbmp.BeginAccess();

        using var g = Graphics.FromImage(bmp);

        // main
        for (int d0 = 0; d0 < dim0; d0++)
        {
            for (int d1 = 0; d1 < dim1; d1++)
            {
                var v = inputData[d0, d1];
                var weight = (int)(255 * (v - minValue) / (double)range);
                if (!blackToWhite)
                    weight = 255 - weight;
                weight = MathExtension.Between(0, weight, 255);
                var c = Color.FromArgb(weight, weight, weight);
                for (int in0 = 0; in0 < zoomFactor; in0++)
                    for (int in1 = 0; in1 < zoomFactor; in1++)
                        fbmp.SetPixel(d0 * zoomFactor + in0, d1 * zoomFactor + in1, c);

            }
        }

        fbmp.EndAccess();

        bmp.Save(outputFile!.FullName);

        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ sub

    internal const string FadeStringSet1 = " ░▒▓█";
    internal const string FadeStringSet2 = " ▁▂▃▄▅▆▇█";
    internal const string FadeStringSet3 = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
    internal const string FadeStringSet4 = " .:-=+*#%@";

    // ★★★★★★★★★★★★★★★ 

}
