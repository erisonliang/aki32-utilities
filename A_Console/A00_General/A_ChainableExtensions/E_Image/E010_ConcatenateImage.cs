using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="cropSize"></param>a
    /// <returns></returns>
    public static FileInfo ConcatenateImage(this FileInfo inputFile0, FileInfo inputFile1, FileInfo? outputFile,
        bool isVertical = true,
        Color? backgroundColor = null,
        int tweakDown = 0,
        int tweakRight = 0,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        imageFormat = imageFormat.DecideImageFormatIfNull(inputFile1);
        var extension = imageFormat.GetExtension();
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile1.Directory!, $"{Path.GetFileNameWithoutExtension(inputFile1.Name)}{extension}");


        // main
        using var inputImage0 = inputFile0.GetImageFromFile();
        using var inputImage1 = inputFile1.GetImageFromFile();
        var outputImage = ConcatenateImage(inputImage0!, inputImage1!, isVertical, backgroundColor, tweakDown, tweakRight);
        outputImage.Save(outputFile!.FullName, imageFormat);


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ image process

    /// <summary>
    /// Join 2 images horizontally or vertically
    /// </summary>
    /// <param name="inputImage0"></param>
    /// <param name="inputImage1"></param>
    /// <param name="isVertical"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="tweakDown"></param>
    /// <param name="tweakRight"></param>
    /// <returns></returns>
    public static Image ConcatenateImage(Image inputImage0, Image inputImage1,
        bool isVertical = true,
        Color? backgroundColor = null,
        int tweakDown = 0,
        int tweakRight = 0
        )
    {
        Image result;
        backgroundColor ??= Color.White;

        if (isVertical)
        {
            result = new Bitmap(Math.Max(inputImage0.Width, inputImage1.Width), inputImage0.Height + inputImage1.Height);
            using var g = Graphics.FromImage(result);
            g.FillRectangle(new SolidBrush(backgroundColor.Value), g.VisibleClipBounds);
            g.DrawImage(inputImage0, 0, 0, inputImage0.Width, inputImage0.Height);
            g.DrawImage(inputImage1, 0 + tweakRight, inputImage0.Height + tweakDown, inputImage1.Width, inputImage1.Height);
        }
        else
        {
            result = new Bitmap(inputImage0.Width + inputImage1.Width, Math.Max(inputImage0.Height, inputImage1.Height));
            using var g = Graphics.FromImage(result);
            g.FillRectangle(new SolidBrush(backgroundColor.Value), g.VisibleClipBounds);
            g.DrawImage(inputImage0, 0, 0, inputImage0.Width, inputImage0.Height);
            g.DrawImage(inputImage1, inputImage0.Width + tweakRight, 0 + tweakDown, inputImage1.Width, inputImage1.Height);
        }

        return (Image)result.Clone();
    }

    // ★★★★★★★★★★★★★★★

}
