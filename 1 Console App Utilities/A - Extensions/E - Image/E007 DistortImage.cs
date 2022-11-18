using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using MathNet.Numerics.LinearAlgebra;

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
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Color? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
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
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Color? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
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
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Color? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        var firstImageFile = inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen(png: true, jpg: true, bmp: true)).FirstOrDefault();
        if (firstImageFile == null)
            return outputDir;

        using var inputImage = firstImageFile.GetImageFromFile();
        var ps = inputImage.__For_DistortImage_ConvertFromPointFsToPoints(pps);

        return inputDir.DistortImage_Loop(outputDir, fill, ps: ps);
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_DistortImage</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Color? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
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

            GC.Collect();
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
    public static Action DistortImage_Loop_Conversationally(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Color? fill = null, PointF[] targetPoints = null, Point[] framePoints = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir.Parent!);


        // main
        var ImageFiles = inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen(png: true, jpg: true, bmp: true));

        if (!ImageFiles.Any())
            return new Action(() => { });


        Console.WriteLine($"\r\n★★★★★ {ImageFiles.First().FullName} started\r\n");

        if (targetPoints == null)
        {
            Console.WriteLine("\r\n★★★★★ Target Point Coordinate Input (Press escape key to go back to previous parameter)\r\n");
            targetPoints = __For_DistortImage_GetTargetPointRatios(framePoints);
        }

        var process = Process.Start(new ProcessStartInfo()
        {
            FileName = ImageFiles.First().FullName,
            UseShellExecute = true,
        });

        Console.WriteLine("\r\n★★★★★ Original Point Coodinates Input (Press escape key to go back to previous parameter)\r\n");

        var ratiosO = __For_DistortImage_GetTargetPointRatios(framePoints);
        process?.Kill();


        Console.WriteLine("\r\n★★★★★ Started to process distortion\r\n");

        //post process
        return new Action(() =>
        {
            inputDir!.DistortImage_Loop(outputDir, fill,
                (ratiosO[0], targetPoints[0]),
                (ratiosO[1], targetPoints[1]),
                (ratiosO[2], targetPoints[2]));
        });
    }


    // ★★★★★★★★★★★★★★★ Helper

    public static Point[] __For_DistortImage_GetFramePoints()
    {
        var reasons = new string[] {
                "Upper Left of the Image（┏  ）",
                "Bottom Right of the Image（┛  ）",
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

        return ps;
    }

    public static PointF[] __For_DistortImage_GetTargetPointRatios(Point[] framePoints = null)
    {
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
                if (i < 2 && framePoints != null)
                {
                    ps[i] = framePoints[i];
                    continue;
                }

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

        return ratiosO;
    }


    // ★★★★★★★★★★★★★★★ Image process

    // process 4 points!
    // https://www.productiverage.com/approximately-correcting-perspective-with-c-sharp-fixing-a-blurry-presentation-video-part-two


    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, Color? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
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
    public static Image DistortImage(this Image inputImage, Color? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        if (ps.Length is < 1 or > 4)
            throw new InvalidDataException("ps length must be in range 1 - 4");
        fill ??= Color.Transparent;


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
        if (ps.Length == 3)
        {
            // increase dim
            var oriP0 = ps[0].originalPoint;
            var oriP1 = ps[1].originalPoint;
            var oriP2 = ps[2].originalPoint;
            var oriP3X = oriP1.X + oriP2.X - oriP0.X;
            var oriP3Y = oriP1.Y + oriP2.Y - oriP0.Y;
            var oriP3 = new Point(oriP3X, oriP3Y);

            var tarP0 = ps[0].tagrtPoint;
            var tarP1 = ps[1].tagrtPoint;
            var tarP2 = ps[2].tagrtPoint;
            var tarP3X = tarP1.X + tarP2.X - tarP0.X;
            var tarP3Y = tarP1.Y + tarP2.Y - tarP0.Y;
            var tarP3 = new Point(tarP3X, tarP3Y);

            ps = ps.Append((oriP3, tarP3)).ToArray();
        }

        var oriO = CreateMatrix.Dense<float>(3, 4);
        var tarO = CreateMatrix.Dense<float>(3, 4);

        for (int i = 0; i < 4; i++)
        {
            oriO[0, i] = ps[i].originalPoint.X;
            oriO[1, i] = ps[i].originalPoint.Y;
            oriO[2, i] = 1;

            tarO[0, i] = ps[i].tagrtPoint.X;
            tarO[1, i] = ps[i].tagrtPoint.Y;
            tarO[2, i] = 1;
        }

        var m1 = tarO;
        var m2 = oriO;
        var m1I = m1.PseudoInverse();
        var invMatrix = m2 * m1I;

        var outputImage = __DistortImage_FromInverseMatrix(inputImage, invMatrix, fill);

        return outputImage;

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

    /// <summary>
    /// DistortImage Main
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    private static Image __DistortImage_FromInverseMatrix(Image inputImage, Matrix<float> invMatrix, Color? fill = null)
    {
        fill ??= Color.Transparent;

        var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);

        var inFBmp = new FastBitmap((Bitmap)inputImage);
        var outFBmp = new FastBitmap(outputBitmap);


        inFBmp.BeginAccess();
        outFBmp.BeginAccess();

        Color pixCol;
        for (int w = 0; w < outputBitmap.Width; w++)
        {
            for (int h = 0; h < outputBitmap.Height; h++)
            {
                // from target frame to original frame
                var tarF = CreateMatrix.Dense<float>(3, 1);

                tarF[0, 0] = w;
                tarF[1, 0] = h;
                tarF[2, 0] = 1;

                var oriF = invMatrix * tarF;
                int oriX = (int)oriF[0, 0];
                int oriY = (int)oriF[1, 0];
                float oXW = oriF[0, 0] - oriX;
                float oYW = oriF[1, 0] - oriY;

                if (0 < oriX && oriX < inputImage.Width - 1 && 0 < oriY && oriY < inputImage.Height - 1)
                {
                    var c00 = inFBmp.GetPixel(oriX + 0, oriY + 0);
                    var c10 = inFBmp.GetPixel(oriX + 1, oriY + 0);
                    var c01 = inFBmp.GetPixel(oriX + 0, oriY + 1);
                    var c11 = inFBmp.GetPixel(oriX + 1, oriY + 1);

                    var w00 = (1 - oXW) + (1 - oYW);
                    var w10 = (0 + oXW) + (1 - oYW);
                    var w01 = (1 - oXW) + (0 + oYW);
                    var w11 = (0 + oXW) + (0 + oYW);

                    var A = (int)((c00.A * w00 + c10.A * w10 + c01.A * w01 + c11.A * w11) / 4);
                    var R = (int)((c00.R * w00 + c10.R * w10 + c01.R * w01 + c11.R * w11) / 4);
                    var G = (int)((c00.G * w00 + c10.G * w10 + c01.G * w01 + c11.G * w11) / 4);
                    var B = (int)((c00.B * w00 + c10.B * w10 + c01.B * w01 + c11.B * w11) / 4);

                    pixCol = Color.FromArgb(A, R, G, B);
                }
                else
                {
                    pixCol = fill.Value;
                }

                outFBmp.SetPixel(w, h, pixCol);
            }
        }

        outFBmp.EndAccess();
        inFBmp.EndAccess();

        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★

}