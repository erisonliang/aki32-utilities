﻿using System.Drawing;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="crop"></param>a
    /// <returns></returns>
    public static FileInfo CropImage(this FileInfo inputFile, FileInfo? outputFile, CropSize crop)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = CropImage(inputImage, crop);
        outputImage.Save(outputFile!.FullName);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="crops"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImageForMany(this FileInfo inputFile, DirectoryInfo? outputDir, CropSize[] crops)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, false, inputFile.Directory!);


        // main
        foreach (var crop in crops)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png"));
            inputFile.CropImage(outputFile, crop);
        }


        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, CropSize crop)
        => inputDir.Loop(outputDir, (inF, outF) => CropImage(inF, outF, crop));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set </param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImageForMany_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, CropSize[] crops)
        => inputDir.Loop(outputDir, (inF, outF) => CropImageForMany(inF, outputDir, crops));


    // ★★★★★★★★★★★★★★★ image process

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static Image CropImage(this Image inputImage, CropSize crop)
    {
        var outputBitmap = ((Bitmap)inputImage).Clone(crop.GetImageCropRect(inputImage), inputImage.PixelFormat);
        return (Image)outputBitmap.Clone();
    }

    /// <summary>
    /// 
    /// </summary>
    public class CropSize
    {
        public double L { get; set; }
        public double T { get; set; }
        public double R { get; set; }
        public double B { get; set; }

        public CropSize(double l, double t, double r, double b)
        {
            if (l < 0 && 1 < l)
                throw new InvalidDataException("L, T, R, B must be in range [0.0, 1.0]");

            L = l;
            T = t;
            R = r;
            B = b;
        }

        public Rectangle GetImageCropRect(Image img)
        {
            var cx = (int)(L * img.Width);
            var cy = (int)(T * img.Height);
            var cw = (int)((1 - L - R) * img.Width);
            var ch = (int)((1 - T - B) * img.Height);

            return new Rectangle(cx, cy, cw, ch);
        }

        public new string ToString() => $"{L:F2},{T:F2},{R:F2},{B:F2}";

    }

    // ★★★★★★★★★★★★★★★

}
