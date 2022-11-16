using System.Drawing;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// ResizeImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_ResizeImage/{inputFile.Name} - {crop.ToString()}.png</param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static FileInfo ResizeImage(this FileInfo inputFile, FileInfo? outputFile, Size outputSize,
        ResizeImageMode mode = ResizeImageMode.Stretch)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, $"output.png");

        // main
        try
        {
            using var inputImage = Image.FromFile(inputFile.FullName);
            var img = ResizeImage(inputImage, outputSize, mode);
            img.Save(outputFile!.FullName);

            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"O: {inputFile.FullName}");
        }
        catch (Exception e)
        {
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"X: {inputFile.FullName}, {e.Message}");
        }


        // post process
        return outputFile!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static DirectoryInfo ResizeImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Size outputSize,
        ResizeImageMode mode = ResizeImageMode.Stretch)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir!);


        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
            inputFile.ResizeImage(outputFile, outputSize, mode);
        }


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static Image ResizeImage(this Image inputImage, Size outputSize,
        ResizeImageMode mode = ResizeImageMode.Stretch)
    {
        switch (mode)
        {
            case ResizeImageMode.Stretch:
                {
                    using var outputBitmap = new Bitmap(inputImage, outputSize);
                    return (Image)outputBitmap.Clone();

                }
            case ResizeImageMode.Uniform_NotImpremented:
                {
                    throw new NotImplementedException();

                }
            case ResizeImageMode.UniformToFill_NotImpremented:
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
        Uniform_NotImpremented,
        UniformToFill_NotImpremented,
    }


    // ★★★★★★★★★★★★★★★ 

}
