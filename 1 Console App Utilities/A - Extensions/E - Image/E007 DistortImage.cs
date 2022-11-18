using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;

using LibGit2Sharp;

using MathNet.Numerics.LinearAlgebra;

using Org.BouncyCastle.Asn1.Crmf;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Brush? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        using var inputImage = inputFile.GetImageFromFile();
        var ps = inputImage.__For_DistortImage_ConvertFromPointFsToPoints(pps);
        return inputFile.DistortImage(outputFile, fill, ps);
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Brush? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);
        //if (!inputFile!.Name.IsMatchAny(GetImageFilesRegexen(png: true, jpg: true,bmp:true)))
        //    throw new Exception("inputFile name must end with .png, .jpg or .bmp");
        //if (!outputFile!.Name.IsMatchAny(GetImageFilesRegexen(png: true, jpg: true, bmp: true)))
        //    throw new Exception("outputFile name must end with .png, .jpg or .bmp");


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = DistortImage(inputImage, fill, ps);
        outputImage.Save(outputFile!.FullName);


        // post process
        return outputFile!;
    }


    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_DistortImage</param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Brush? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        var firstImageFile = inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen(png: true, jpg: true, bmp: true)).FirstOrDefault();
        if (firstImageFile == null)
            return outputDir;

        using var inputImage = firstImageFile.GetImageFromFile();
        var ps = inputImage.__For_DistortImage_ConvertFromPointFsToPoints(pps);

        return inputDir.DistortImage_Loop(outputDir, fill, ps);
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_DistortImage</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Brush? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir!.FullName, file.Name);
            try
            {
                file.DistortImage(new FileInfo(newFilePath), fill, ps);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {newFilePath}, {ex.Message}");
            }
        }


        // post process
        return outputDir!;
    }

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process (applied)

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_DistortImage</param>
    /// <param name="targetPoints">List of target points. Length 3</param>
    /// <returns></returns>
    public static Task<DirectoryInfo> DistortImage_Loop_Conversationally(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Brush? fill = null, params PointF[] targetPoints)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir.Parent!);


        // main
        var ImageFiles = inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen(png: true, jpg: true, bmp: true));

        if (ImageFiles.Any())
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = ImageFiles.First().FullName,
                UseShellExecute = true,
            });
        }
        else
        {
            return Task.Run(() => outputDir!);
        }

        Console.WriteLine("\r\n★★★★★ Coodinate Input (Press escape key to go back to previous parameter)\r\n");

        // 画像の点を拾う。
        var reasons = new string[] {
                "Upper Left of the Image（┏  ）",
                "Bottom Right of the Image（┛  ）",
                "Upper Left of the Object（┏  ）",
                "Upper Right of the Object（┓  ）",
                "Bottom Left of the Object（┗  ）",
            };
        var ps = new Point[reasons.Length];

        for (int i = 0; i < reasons.Length; i++)
        {
            try
            {
                ps[i] = IODeviceExtension.GetMouseCursorPositionConversationally(ConsoleKey.Escape, reasons[i]);
            }
            catch (OperationCanceledException)
            {
                i -= 2;
                i = Math.Max(-1, i);
                continue;
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        for (int i = 0; i < reasons.Length; i++)
            Console.WriteLine($"{reasons[i]}:{ps[i]}");

        var ulF = ps[0];
        var brF = ps[1];
        var ulO = ps[2];
        var urO = ps[3];
        var blO = ps[4];

        float widthF = brF.X - ulF.X;
        float heightF = brF.Y - ulF.Y;

        var ratiosO = new PointF[3]
        {
                new PointF((ulO.X - ulF.X) / widthF, (ulO.Y - ulF.Y) / heightF),
                new PointF((urO.X - ulF.X) / widthF, (urO.Y - ulF.Y) / heightF),
                new PointF((blO.X - ulF.X) / widthF, (blO.Y - ulF.Y) / heightF),
        };


        Console.WriteLine("\r\n★★★★★ Started to process distortion\r\n");

        // post process
        return Task.Run(() =>
        {
            return inputDir!.DistortImage_Loop(outputDir, fill,
                (ratiosO[0], targetPoints[0]),
                (ratiosO[1], targetPoints[1]),
                (ratiosO[2], targetPoints[2]));
        });

    }


    // ★★★★★★★★★★★★★★★ Image process

    // process 4 points!
    // https://www.productiverage.com/approximately-correcting-perspective-with-c-sharp-fixing-a-blurry-presentation-video-part-two


    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, Brush? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        var ps = inputImage.__For_DistortImage_ConvertFromPointFsToPoints(pps);
        return DistortImage(inputImage, fill, ps);
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, Brush? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        if (ps.Length is < 1 or > 3)
            throw new InvalidDataException("ps length must be in range 1 - 3");
        fill ??= Brushes.Transparent;


        // main

        if (ps.Length == 1)
        {
            // increase dim
            ps = ps.Append((Point.Add(ps[0].originalPoint, new Size(100, 0)), Point.Add(ps[0].tagrtPoint, new Size(100, 0)))).ToArray();
            ps = ps.Append((Point.Add(ps[0].originalPoint, new Size(0, 100)), Point.Add(ps[0].tagrtPoint, new Size(0, 100)))).ToArray();
        }
        if (ps.Length == 2)
        {
            // increase dim
            var oriP0 = ps[0].originalPoint;
            var oriP1 = ps[1].originalPoint;
            var oriV1 = Point.Subtract(oriP1, (Size)oriP0);
            var oriV2 = Point.Add(new Point(-oriV1.Y, oriV1.X), (Size)oriP0);

            var tarP0 = ps[0].tagrtPoint;
            var tarP1 = ps[1].tagrtPoint;
            var tarV1 = Point.Subtract(tarP1, (Size)tarP0);
            var tarV2 = Point.Add(new Point(-tarV1.Y, tarV1.X), (Size)tarP0);

            ps = ps.Append((oriV2, tarV2)).ToArray();
        }

        using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
        using var g = Graphics.FromImage(outputBitmap);
        g.FillRectangle(fill!, new Rectangle(new Point(0, 0), outputBitmap.Size));

        var oriO = CreateMatrix.Dense<float>(4, 3);
        var tarO = CreateMatrix.Dense<float>(4, 3);

        for (int i = 0; i < 3; i++)
        {
            oriO[0, i] = ps[i].originalPoint.X;
            oriO[1, i] = ps[i].originalPoint.Y;
            oriO[2, i] = 1;
            oriO[3, i] = 1;

            tarO[0, i] = ps[i].tagrtPoint.X;
            tarO[1, i] = ps[i].tagrtPoint.Y;
            tarO[2, i] = 1;
            tarO[3, i] = 1;
        }

        var oriF = CreateMatrix.Dense<float>(4, 3);

        oriF[0, 1] = inputImage.Width;
        oriF[1, 2] = inputImage.Height;
        oriF[2, 0] = 1;
        oriF[2, 1] = 1;
        oriF[2, 2] = 1;
        oriF[3, 0] = 1;
        oriF[3, 1] = 1;
        oriF[3, 2] = 1;

        var m1 = tarO;
        var m2 = oriO;
        var m3 = oriF;
        var m2I = m2.PseudoInverse();
        var tarF = m1 * m2I * m3;

        var points = new PointF[]
        {
            new PointF(tarF[0,0], tarF[1,0]),
            new PointF(tarF[0,1], tarF[1,1]),
            new PointF(tarF[0,2], tarF[1,2]),
        };

        g.DrawImage(inputImage, points);

        return (Image)outputBitmap.Clone();
    }

    /// <summary>
    /// For DistortImage. Convert from PointFs to Points
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    private static (Point originalPoint, Point tagrtPoint)[] __For_DistortImage_ConvertFromPointFsToPoints(this Image inputImage, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        return pps.Select(pp => (
             new Point((int)(pp.originalPointRatio.X * inputImage.Width), (int)(pp.originalPointRatio.Y * inputImage.Height)),
             new Point((int)(pp.tagrtPointRatio.X * inputImage.Width), (int)(pp.tagrtPointRatio.Y * inputImage.Height)))
             ).ToArray();
    }


    // ★★★★★★★★★★★★★★★

}