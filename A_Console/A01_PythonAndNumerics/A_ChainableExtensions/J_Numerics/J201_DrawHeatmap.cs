using System.Drawing;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Numerics;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable (to console)

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
    public static void DrawHeatmapToConsole(this IEnumerable<float> inputData, float? minValue = null, float? maxValue = null, int repeat = 2, string StringSet = FadeStringSet5)
        => inputData.Select(v => (double)v).DrawHeatmapToConsole(minValue, maxValue, repeat, StringSet);

    /// <summary>
    /// Write 1D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this IEnumerable<int> inputData, int? minValue = null, int? maxValue = null, int repeat = 2, string StringSet = FadeStringSet5)
        => inputData.Select(v => (double)v).DrawHeatmapToConsole(minValue, maxValue, repeat, StringSet);

    /// <summary>
    /// Write 2D array heatmap to console
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static void DrawHeatmapToConsole(this int[,] inputData, int? minValue = null, int? maxValue = null, int repeat = 2, string StringSet = FadeStringSet5)
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
    public static void DrawHeatmapToConsole(this float[,] inputData, float? minValue = null, float? maxValue = null, int repeat = 2, string StringSet = FadeStringSet5)
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
    public static void DrawHeatmapToConsole(this double[,] inputData, double? minValue = null, double? maxValue = null, int repeat = 2, string StringSet = FadeStringSet5)
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
    public static FileInfo DrawHeatmapAsSimpleImage(this double[,] inputData,
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

        using var bmp = new Bitmap(dim0, dim1);
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
                fbmp.SetPixel(d1, d0, c);
            }
        }

        fbmp.EndAccess();


        // resize image
        var zoomFactor = targetImageWidth.HasValue
            ? targetImageWidth.Value / dim0
            : targetCellSize
            ?? 300 / dim0;

        using var bmp2 = bmp.ResizeImage(new Size(dim0 * zoomFactor, dim1 * zoomFactor));
        bmp2.Save(outputFile!.FullName);

        return outputFile!;
    }

    /// <summary>
    /// Write 2D array heatmap as an image
    /// </summary>
    /// <param name="inputData"></param>
    /// <param name="outputFile">when null, automatically set to temporary file</param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="outputExtension">when outputFile is null, you can designate output file extension here. Must include "."</param>
    /// <param name="preview"></param>
    /// <returns></returns>
    public static FileInfo DrawHeatmapAsPyPlotImage(this double[,] inputData,
        FileInfo? outputFile = null,
        double? minValue = null,
        double? maxValue = null,
        string outputExtension = ".png",
        bool preview = false
        )
    {
        // preprocess
        outputFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(outputExtension));
        UtilPreprocessors.PreprocessOutFile(ref outputFile!, null!, $"heatmap{outputExtension}");

        var reshaped = inputData.ReShape();
        minValue ??= reshaped.Min();
        maxValue ??= reshaped.Max();

        var dim0 = inputData.GetLength(0);
        var dim1 = inputData.GetLength(1);

        var XX = EnumerableExtension.Range_WithStep(0, dim0 - 1, 1).ToArray();
        var YY = EnumerableExtension.Range_WithStep(0, dim1 - 1, 1).ToArray();

        // main
        new PyPlotWrapper.Figure()
        {
            IsTightLayout = true,
            SubPlot = new PyPlotWrapper.SubPlot()
            {
                XLabel = "dim = 0",
                YLabel = "dim = 1",
                HasGrid = false,
                Plot = new PyPlotWrapper.ContinuousHeatMapPlot(XX, YY, inputData)
                {
                    ColorMap = "gray",
                    ColorLim = (minValue, maxValue),
                    OverwriteXAxisTickTop = true,
                    OverwriteYAxisInvert = true,
                },
            }
        }.Run(outputFile, preview);

        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ sub

    internal const string FadeStringSet1 = " ░▒▓█";
    internal const string FadeStringSet2 = " ▁▂▃▄▅▆▇█";
    internal const string FadeStringSet3 = " .'`^\",:;Il!i><~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
    internal const string FadeStringSet4 = " .:-=+*#%@";
    internal const string FadeStringSet5 = " .,-~:;=!*#$@";


    // ★★★★★★★★★★★★★★★ 

}
