using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// ResizeImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static FileInfo ResizeImage(this FileInfo inputFile, FileInfo? outputFile, Size outputSize,
        ResizeImageMode mode = ResizeImageMode.Stretch,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        imageFormat = imageFormat.DecideImageFormatIfNull(inputFile);
        var extension = imageFormat.GetExtension();
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, $"output{extension}");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var img = ResizeImage(inputImage, outputSize, mode);
        img.Save(outputFile!.FullName, imageFormat);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// ResizeImage Proportionally
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="outputSizeRatio">1 for the same size. More than 0. </param>
    /// <returns></returns>
    public static FileInfo ResizeImageProportionally(this FileInfo inputFile, FileInfo? outputFile, SizeF outputSizeRatio,
        ResizeImageMode mode = ResizeImageMode.Stretch,
        ImageFormat? imageFormat = null)
    {
        // sugar
        using var inputImage = inputFile.GetImageFromFile();
        var outputSize = new Size(
            (int)(inputImage.Width * outputSizeRatio.Width),
            (int)(inputImage.Height * outputSizeRatio.Height));

        return inputFile.ResizeImage(outputFile, outputSize, mode, imageFormat);

    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// ResizeImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static DirectoryInfo ResizeImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Size outputSize,
        ResizeImageMode mode = ResizeImageMode.Stretch,
        ImageFormat? imageFormat = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.ResizeImage(outF, outputSize, mode, imageFormat));

    /// <summary>
    /// ResizeImage Proportionally
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="outputSizeRatio">1 for the same size. More than 0. </param>
    /// <returns></returns>
    public static DirectoryInfo ResizeImageProportionally_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, SizeF outputSizeRatio,
        ResizeImageMode mode = ResizeImageMode.Stretch,
        ImageFormat? imageFormat = null)
           => inputDir.Loop(outputDir, (inF, outF) => inF.ResizeImageProportionally(outF, outputSizeRatio, mode, imageFormat));


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static Image ResizeImage(this Image inputImage, Size outputSize,
        ResizeImageMode resizeMode = ResizeImageMode.Stretch,
        InterpolationMode interpolationMode = InterpolationMode.NearestNeighbor
        )
    {
        switch (resizeMode)
        {
            case ResizeImageMode.Stretch:
                {
                    using var outputBitmap = new Bitmap(outputSize.Width, outputSize.Height);

                    foreach (var prop in inputImage.PropertyItems)
                        outputBitmap.SetPropertyItem(prop);

                    if (outputSize.Width <= inputImage.Width && outputSize.Height <= inputImage.Height)
                    {
                        // when make it smaller, use normal way.
                        // (!) 1セル目を拡大するときに半分のサイズになっておかしい。だから拡大には使わない。
                        {
                            var attributes = new ImageAttributes();
                            attributes.SetWrapMode(WrapMode.TileFlipXY);
                            var destination = new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height);

                            using var g = Graphics.FromImage(outputBitmap);
                            g.InterpolationMode = interpolationMode;
                            g.DrawImage(inputImage, destination, 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                    else
                    {
                        // when make it bigger, use special way
                        using var tempBitmap = new Bitmap(inputImage.Width + 1, inputImage.Height + 1);
                        {
                            var destination = new Rectangle(1, 1, inputImage.Width, inputImage.Height);
                            using var g = Graphics.FromImage(tempBitmap);
                            g.DrawImage(inputImage, destination, 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);
                        }
                        {
                            var offsetX = outputSize.Width / inputImage.Width;
                            var offsetY = outputSize.Height / inputImage.Height;
                            var destination = new Rectangle(-offsetX / 2, -offsetY / 2, outputBitmap.Width + offsetX, outputBitmap.Height + offsetY);

                            using var g = Graphics.FromImage(outputBitmap);
                            g.InterpolationMode = interpolationMode;
                            g.DrawImage(tempBitmap, destination, 0, 0, tempBitmap.Width, tempBitmap.Height, GraphicsUnit.Pixel);
                        }
                    }

                    return (Image)outputBitmap.Clone();
                }
            case ResizeImageMode.Uniform_NotImplemented:
                {
                    throw new NotImplementedException();

                }
            case ResizeImageMode.UniformToFill_NotImplemented:
                {
                    throw new NotImplementedException();

                }
            default:
                {
                    throw new NotImplementedException();
                }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ResizeImageMode
    {
        Stretch,
        Uniform_NotImplemented,
        UniformToFill_NotImplemented,
    }


    // ★★★★★★★★★★★★★★★ 

}
