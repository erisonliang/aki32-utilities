using System.Drawing;

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
    public static FileInfo CropImage(this FileInfo inputFile, FileInfo? outputFile, Thickness cropSize)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {cropSize.ToString()}.png");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = CropImage(inputImage, cropSize);
        outputImage.Save(outputFile!.FullName);


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
    public static DirectoryInfo CropImageForMany(this FileInfo inputFile, DirectoryInfo? outputDir, Thickness[] cropSizes)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);
        (var init_ConsoleOutput_Preprocess, UtilConfig.ConsoleOutput_Preprocess) = (UtilConfig.ConsoleOutput_Preprocess, false);


        // main
        foreach (var crop in cropSizes)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png"));
            inputFile.CropImage(outputFile, crop);
        }


        // post process
        UtilConfig.ConsoleOutput_Preprocess = init_ConsoleOutput_Preprocess;
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
    public static DirectoryInfo CropImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Thickness cropSize)
        => inputDir.Loop(outputDir, (inF, outF) => CropImage(inF, outF, cropSize));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set </param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImageForMany_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Thickness[] crorpSizes)
        => inputDir.Loop(outputDir, (inF, outF) => CropImageForMany(inF, outputDir, crorpSizes));


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
