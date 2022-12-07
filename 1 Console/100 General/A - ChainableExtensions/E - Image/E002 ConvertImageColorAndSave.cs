using System.Drawing;
using System.Drawing.Imaging;

using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static FileInfo ConvertImageColor(this FileInfo inputFile, FileInfo? outputFile, Color targetColor)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


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
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="targetInfos"></param>
    /// <returns></returns>
    public static DirectoryInfo ConvertImageColorForMany(this FileInfo inputFile, DirectoryInfo? outputDir,
        params (string fileName, Color targetColor)[] targetInfos)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();


        // main
        foreach (var (targetName, targetColor) in targetInfos)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, $"{targetName.Replace(".png", "")}.png"));
            inputFile.ConvertImageColor(outputFile, targetColor);
        }


        // post process
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return outputDir!;
    }

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
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
