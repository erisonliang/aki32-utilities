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
        ResizeImageMode mode = ResizeImageMode.Stretch)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, $"output.png");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var img = ResizeImage(inputImage, outputSize, mode);
        img.Save(outputFile!.FullName);


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
    public static FileInfo ResizeImagePropotionally(this FileInfo inputFile, FileInfo? outputFile, SizeF outputSizeRatio,
        ResizeImageMode mode = ResizeImageMode.Stretch)
    {
        // sugar
        using var inputImage = inputFile.GetImageFromFile();
        var outputSize = new Size(
            (int)(inputImage.Width * outputSizeRatio.Width),
            (int)(inputImage.Height * outputSizeRatio.Height));

        return inputFile.ResizeImage(outputFile, outputSize, mode);

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
        ResizeImageMode mode = ResizeImageMode.Stretch)
        => inputDir.Loop(outputDir, (inF, outF) => inF.ResizeImage(outF, outputSize, mode));

    /// <summary>
    /// ResizeImage Proportionally
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="outputSizeRatio">1 for the same size. More than 0. </param>
    /// <returns></returns>
    public static DirectoryInfo ResizeImagePropotionally_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, SizeF outputSizeRatio,
        ResizeImageMode mode = ResizeImageMode.Stretch)
           => inputDir.Loop(outputDir, (inF, outF) => inF.ResizeImagePropotionally(outF, outputSizeRatio, mode));


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

                    {
                        var attributes = new ImageAttributes();
                        attributes.SetWrapMode(WrapMode.TileFlipXY);
                        var destination = new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height);

                        using var g = Graphics.FromImage(outputBitmap);
                        g.InterpolationMode = interpolationMode;
                        g.DrawImage(inputImage, destination, 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, attributes);
                    }

                    return (Image)outputBitmap.Clone();
                }
            case ResizeImageMode.Uniform_Notimplemented:
                {
                    throw new NotImplementedException();

                }
            case ResizeImageMode.UniformToFill_Notimplemented:
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
        Uniform_Notimplemented,
        UniformToFill_Notimplemented,
    }


    // ★★★★★★★★★★★★★★★ 

}
