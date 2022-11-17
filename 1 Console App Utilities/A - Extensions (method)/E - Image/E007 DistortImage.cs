using System;
using System.Collections.Generic;
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
    /// <param name="ps">List of original points and target points. min 1, max 3</param>
    /// <returns></returns>
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Brush? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);
        if (!inputFile!.Name.EndsWith(".png"))
            throw new Exception("inputFile name must end with .png");
        if (!outputFile!.Name.EndsWith(".png"))
            throw new Exception("outputFile name must end with .png");


        // main
        using var inputImage = Image.FromFile(inputFile.FullName);
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
    /// <param name="ps">List of original points and target points. min 1, max 3</param>
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


    // ★★★★★★★★★★★★★★★ Image process



    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="ps">List of relative values of original point and target points. min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, Brush? fill = null, params (PointF originalPoint, PointF tagrtPoint)[] pps)
    {
        var ps = pps.Select(pp => (
            new Point((int)(pp.originalPoint.X * inputImage.Width), (int)(pp.originalPoint.Y * inputImage.Height)),
            new Point((int)(pp.tagrtPoint.X * inputImage.Width), (int)(pp.tagrtPoint.Y * inputImage.Height)))
        ).ToArray();

        return DistortImage(inputImage, ps: ps);
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="ps">List of original points and target points. min length 1, max length 3</param>
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

    // ★★★★★★★★★★★★★★★

}

