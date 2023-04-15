using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo SaveScreenShot(this FileInfo outputFile, Point upperLeftCoordinate, Point bottomRightCoordinate,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        imageFormat ??= ImageFormat.Png;
        var extension = imageFormat.GetExtension();
        UtilPreprocessors.PreprocessOutFile(ref outputFile!, null!, $"output{extension}");


        // main
        var outputImage = TakeScreenShot(upperLeftCoordinate, bottomRightCoordinate);
        outputImage.Save(outputFile!.FullName, imageFormat);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo SaveScreenShot(this DirectoryInfo outputDir, Point upperLeftCoordinate, Point bottomRightCoordinate,
        ImageFormat? imageFormat = null)
    {
        // pre process
        imageFormat ??= ImageFormat.Png;
        var extension = imageFormat.GetExtension();
        var outputFile = new FileInfo($@"{outputDir.FullName}\{DateTime.Now.ToString("s").Replace(":", "-")}{extension}");


        // main
        outputFile.SaveScreenShot(upperLeftCoordinate, bottomRightCoordinate);


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static Image TakeScreenShot(Point upperLeftCoordinate, Point bottomRightCoordinate)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main
        var ul = upperLeftCoordinate;
        var br = bottomRightCoordinate;

        if (br.X - ul.X < 0)
            (br.X, ul.X) = (ul.X, br.X);
        if (br.Y - ul.Y < 0)
            (br.Y, ul.Y) = (ul.Y, br.Y);
        using var outputBitmap = new Bitmap(br.X - ul.X, br.Y - ul.Y);
        using var g = Graphics.FromImage(outputBitmap);
        g.CopyFromScreen(ul, new Point(0, 0), outputBitmap.Size);

        // post process
        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★ 

}
