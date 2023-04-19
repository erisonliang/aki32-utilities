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
    public static FileInfo CropImage(this FileInfo inputFile, FileInfo? outputFile, Thickness cropSize,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        imageFormat = imageFormat.DecideImageFormatIfNull(inputFile);
        var extension = imageFormat.GetExtension();
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {cropSize.ToString()}{extension}");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = CropImage(inputImage, cropSize);
        outputImage.Save(outputFile!.FullName, imageFormat);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="cropSizes"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImageForMany(this FileInfo inputFile, DirectoryInfo? outputDir, Thickness[] cropSizes,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();


        // main
        foreach (var crop in cropSizes)
        {
            var outputFile = outputDir!.GetChildFileInfo($"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png");
            inputFile.CropImage(outputFile, crop, imageFormat);
        }


        // post process
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="cropSize"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Thickness cropSize,
        ImageFormat? imageFormat = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.CropImage(outF, cropSize, imageFormat));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set </param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImageForMany_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Thickness[] crorpSizes,
        ImageFormat? imageFormat = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.CropImageForMany(outF.Directory, crorpSizes, imageFormat));


    // ★★★★★★★★★★★★★★★ image process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="cropSize"></param>
    /// <returns></returns>
    public static Image CropImage(this Image inputImage, Thickness cropSize)
    {
        var outputBitmap = ((Bitmap)inputImage).Clone(cropSize.GetImageRectangle(inputImage), inputImage.PixelFormat);
        return (Image)outputBitmap.Clone();
    }

    // ★★★★★★★★★★★★★★★

}
