﻿using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
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
        bool alignRight = false,
        ImageFormat? imageFormat = null)
    {
        // preprocess
        imageFormat = imageFormat.DecideImageFormatIfNull(inputFile);
        var extension = imageFormat.GetExtension();
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, $"output{extension}");
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
        img.Save(outputFile!.FullName, imageFormat);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
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
        bool alignRight = false,
        ImageFormat? imageFormat = null)
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
            alignRight: alignRight,
            imageFormat: imageFormat
            );
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
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
        bool alignRight = false,
        ImageFormat? imageFormat = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.AddTextToImage(outF, addingText, textUpperLeftPoint, fontSize, brush, alignRight, imageFormat), maxDegreeOfParallelism: 1);

    /// <summary>
    /// AddTextToImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
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
        bool alignRight = false,
        ImageFormat? imageFormat = null)
        => inputDir.Loop(outputDir, (inF, outF) => inF.AddTextToImageProportionally(outF, addingText, textUpperLeftPointRatio, fontSizeRatio, brush, alignRight, imageFormat), maxDegreeOfParallelism: 1);


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
