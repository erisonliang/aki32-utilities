using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_ConvertImageColor/{targetColor.Name}.png</param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static FileInfo ConvertImageColor(this FileInfo inputFile, FileInfo? outputFile, Color targetColor)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = ConvertImageColor(inputImage, targetColor);
        outputImage.Save(outputFile!.FullName, ImageFormat.Png);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/output_ConvertImageColor/{fileName}.png</param>
    /// <param name="targetInfos"></param>
    /// <returns></returns>
    public static DirectoryInfo ConvertImageColorForMany(this FileInfo inputFile, DirectoryInfo? outputDir,
        int maxDegreeOfParallelism = 999,
        params (string fileName, Color targetColor)[] targetInfos)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputFile.Directory!);


        // main
        var option = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
        Parallel.ForEach(targetInfos, option, targetInfos =>
        {
            var (targetName, targetColor) = targetInfos;
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, $"{targetName.Replace(".png", "")}.png"));
            inputFile.ConvertImageColor(outputFile, targetColor);
        });


        // post process
        return outputDir;
    }

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/ConvertImageColorOutput/{targetColor.Name}.png</param>
    /// <param name="targetColors"></param>
    /// <returns></returns>
    public static DirectoryInfo ConvertImageColorForMany(this FileInfo inputFile, DirectoryInfo? outputDir, params Color[] targetColors)
        => inputFile.ConvertImageColorForMany(
            outputDir,
            targetInfos: targetColors.Select(c => (c.IsNamedColor ? c.Name : c.ToArgb().ToString(), c)).ToArray()
            );


    // ★★★★★★★★★★★★★★★ image process

    /// <summary>
    /// Convert image color to targetColor
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static Image ConvertImageColor(Image inputImage, Color targetColor)
    {
        var outputBitmap = new Bitmap(inputImage);

        var bmpP = new FastBitmap(outputBitmap);

        bmpP.BeginAccess();

        for (int i = 0; i < outputBitmap.Width; i++)
        {
            for (int j = 0; j < outputBitmap.Height; j++)
            {
                var bmpCol = bmpP.GetPixel(i, j);
                bmpP.SetPixel(i, j, Color.FromArgb(bmpCol.A, targetColor));
            }
        }

        bmpP.EndAccess();


        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★

}
