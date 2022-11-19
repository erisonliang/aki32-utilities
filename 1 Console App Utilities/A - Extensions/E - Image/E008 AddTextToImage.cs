using System.Drawing;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_AddTextToImage/output.png</param>
    /// <param name="outputFile"></param>
    /// <param name="textUpperLeftPoint"></param>
    /// <param name="addingText"></param>
    /// <param name="isRightToLeft"></param>
    /// <returns></returns>
    public static FileInfo AddTextToImage(this FileInfo inputFile, FileInfo? outputFile, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        Brush? brush = null,
        bool isRightToLeft = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, $"output.png");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var img = AddTextToImage(inputImage, addingText, textUpperLeftPoint,
            fontSize: fontSize,
            brush: brush,
            isRightToLeft: isRightToLeft
            );
        img.Save(outputFile!.FullName);


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
    public static DirectoryInfo AddTextToImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        Brush? brush = null,
        bool isRightToLeft = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir!);


        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            try
            {
                var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
                inputFile.AddTextToImage(outputFile, addingText, textUpperLeftPoint,
                    fontSize: fontSize,
                    brush: brush,
                    isRightToLeft: isRightToLeft
                    );

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {inputFile.FullName}");
            }
            catch (Exception e)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {inputFile.FullName}, {e.Message}");
            }
        }


        // post process
        return outputDir!;
    }

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_AddTextToImage/output.png</param>
    /// <param name="outputFile"></param>
    /// <param name="textUpperLeftPoint"></param>
    /// <param name="addingText"></param>
    /// <param name="isRightToLeft"></param>
    /// <returns></returns>
    public static FileInfo AddTextToImageProportionally(this FileInfo inputFile, FileInfo? outputFile, string addingText, PointF textUpperLeftPointRatio,
        double fontSizeRatio = 0.2,
        Brush? brush = null,
        bool isRightToLeft = false)
    {
        // sugar
        using var inputImage = inputFile.GetImageFromFile();
        var textUpperLeftPoint = new Point(
            (int)(inputImage.Width * textUpperLeftPointRatio.X),
            (int)(inputImage.Height * textUpperLeftPointRatio.Y));
        var fontSize = (int)(inputImage.Height * fontSizeRatio);

        return inputFile.AddTextToImage(outputFile, addingText, textUpperLeftPoint,
            fontSize: fontSize,
            brush: brush,
            isRightToLeft: isRightToLeft
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="outputSize"></param>
    /// <returns></returns>
    public static DirectoryInfo AddTextToImageProportionally_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string addingText, PointF textUpperLeftPointRatio,
        double fontSizeRatio = 0.2,
        Brush? brush = null,
        bool isRightToLeft = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir!);


        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            try
            {
                var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
                inputFile.AddTextToImageProportionally(outputFile, addingText, textUpperLeftPointRatio,
                    fontSizeRatio: fontSizeRatio,
                    brush: brush,
                    isRightToLeft: isRightToLeft
                    );

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {inputFile.FullName}");
            }
            catch (Exception e)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {inputFile.FullName}, {e.Message}");
            }
        }


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <returns></returns>
    public static Image AddTextToImage(this Image inputImage, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        string fontName = "MS UI Gothic",
        Brush? brush = null,
        bool isRightToLeft = false)
    {
        // preprocess for 

        using var font = new Font(fontName, fontSize);
        brush ??= Brushes.Black;
        var format = isRightToLeft ? new StringFormat(StringFormatFlags.DirectionRightToLeft) : new StringFormat();
        using var outputBitmap = new Bitmap(inputImage);
        using var g = Graphics.FromImage(outputBitmap);

        g.DrawString(addingText, font, brush, (PointF)textUpperLeftPoint, format);

        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★ 

}
