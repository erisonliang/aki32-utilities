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
    /// <param name="addingText">
    /// Text to add in image.
    /// (%FN → FileName)
    /// (%CT → CreationTime)
    /// (%WT → LastWriteTime)
    /// </param>
    /// <returns></returns>
    public static FileInfo AddTextToImage(this FileInfo inputFile, FileInfo? outputFile, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        Brush? brush = null,
        bool alignRight = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, $"output.png");
        addingText = addingText
            .Replace("%FN", inputFile.Name)
            .Replace("%CT", inputFile.CreationTime.ToString("s"))
            .Replace("%WT", inputFile.LastWriteTime.ToString("s"))
            ;


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var img = AddTextToImage(inputImage, addingText, textUpperLeftPoint,
            fontSize: fontSize,
            brush: brush,
            alignRight: alignRight
            );
        img.Save(outputFile!.FullName);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_AddTextToImage/output.png</param>
    /// <param name="outputFile"></param>  
    /// <param name="addingText">
    /// Text to add in image.
    /// (%FN → FileName)
    /// (%CT → CreationTime)
    /// (%WT → LastWriteTime)
    /// </param>
    /// <param name="isRightToLeft"></param>
    /// <returns></returns>
    public static FileInfo AddTextToImageProportionally(this FileInfo inputFile, FileInfo? outputFile, string addingText, PointF textUpperLeftPointRatio,
        double fontSizeRatio = 0.1,
        Brush? brush = null,
        bool alignRight = false)
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
            alignRight: alignRight
            );
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="addingText">
    /// Text to add in image.
    /// (%FN → FileName)
    /// (%CT → CreationTime)
    /// (%WT → LastWriteTime)
    /// </param>
    /// <returns></returns>
    public static DirectoryInfo AddTextToImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        Brush? brush = null,
        bool alignRight = false)
        => inputDir.Loop(outputDir, (inF, outF) => AddTextToImage(inF, outF, addingText, textUpperLeftPoint,
            fontSize: fontSize,
            brush: brush,
            alignRight: alignRight
            ));

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="addingText">
    /// Text to add in image.
    /// (%FN → FileName)
    /// (%CT → CreationTime)
    /// (%WT → LastWriteTime)
    /// </param>
    /// <returns></returns>
    public static DirectoryInfo AddTextToImageProportionally_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string addingText, PointF textUpperLeftPointRatio,
        double fontSizeRatio = 0.1,
        Brush? brush = null,
        bool alignRight = false)
        => inputDir.Loop(outputDir, (inF, outF) => AddTextToImageProportionally(inF, outF, addingText, textUpperLeftPointRatio,
            fontSizeRatio: fontSizeRatio,
            brush: brush,
            alignRight: alignRight
            ));


    // ★★★★★★★★★★★★★★★ Image process


    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="addingText"></param>
    /// <param name="textUpperLeftPoint"></param>
    /// <param name="fontSize"></param>
    /// <param name="fontName"></param>
    /// <param name="brush"></param>
    /// <param name="alignRight"></param>
    /// <returns></returns>
    public static Image AddTextToImage(this Image inputImage, string addingText, Point textUpperLeftPoint,
        int fontSize = 20,
        string fontName = "MS UI Gothic",
        Brush? brush = null,
        bool alignRight = false)
    {
        using var font = new Font(fontName, fontSize);
        brush ??= Brushes.Black;
        var format = new StringFormat
        {
            Alignment = alignRight ? StringAlignment.Far : StringAlignment.Near
        };

        using var outputBitmap = new Bitmap(inputImage);
        using var g = Graphics.FromImage(outputBitmap);
        g.DrawString(addingText, font, brush, (PointF)textUpperLeftPoint, format);

        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★ 

}
