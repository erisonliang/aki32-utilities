using System.Diagnostics;
using System.Drawing;

using Aki32_Utilities.UsefulClasses;

using HomographySharp;

using MathNet.Numerics.LinearAlgebra;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, Color? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);
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
    /// DistortImageProportionally
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static FileInfo DistortImageProportionally(this FileInfo inputFile, FileInfo? outputFile,
        Color? fill = null,
        params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps
        )
    {
        // sugar
        using var inputImage = inputFile.GetImageFromFile();
        var ps = inputImage.GetPointsFromPointRatios_For_DistortImage(pps);
        return inputFile.DistortImage(outputFile, fill, ps);
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        Color? fill = null,
        int maxDegreeOfParallelism = 5,
        params (Point originalPoint, Point tagrtPoint)[] ps
        )
        => inputDir.Loop(outputDir, (inF, outF) => DistortImage(inF, outF, fill, ps),
            maxDegreeOfParallelism: maxDegreeOfParallelism
            );

    /// <summary>
    /// DistortImageProportionally_Loop
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static DirectoryInfo DistortImageProportionally_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        Color? fill = null,
        int maxDegreeOfParallelism = 5,
        params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir);


        // main
        var firstImageFile = inputDir.GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetRegexen_ImageFiles()).FirstOrDefault();
        if (firstImageFile == null)
            return outputDir;

        using var inputImage = firstImageFile.GetImageFromFile();
        var ps = inputImage.GetPointsFromPointRatios_For_DistortImage(pps);


        // post process
        return inputDir.DistortImage_Loop(outputDir, fill,
            maxDegreeOfParallelism: maxDegreeOfParallelism,
            ps: ps
            );
    }


    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process (applied)

    /// <summary>
    /// DistortImage.
    /// Return Task of DistortImage List after picking points.
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="presetTargetPointRatios">List of target point ratios. Length 3</param>
    /// <returns></returns>
    public static Func<DirectoryInfo> DistortImageConversationally_Loop_Func(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        Color? fill = null,
        PointF[] presetTargetPointRatios = null,
        Point[] presetFramePoints = null,
        int pickingPointCount = 4
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir.Parent!);


        // main
        var inputImageFiles = inputDir
            .GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetRegexen_ImageFiles())
            .Sort();

        if (!inputImageFiles.Any())
            return () => outputDir!;


        Console.WriteLine($"\r\n★★★★★ {inputImageFiles.First().FullName} started\r\n");

        if (presetTargetPointRatios == null)
        {
            Console.WriteLine("\r\n★★★★★ Target Point Coordinate Input (Press escape key to go back to previous parameter)\r\n");
            presetTargetPointRatios = GetTargetPointRatiosConversationally_For_DistortImage(
                presetFramePoints: presetFramePoints,
                pickingPointCount: pickingPointCount
                );
        }

        var process = Process.Start(new ProcessStartInfo()
        {
            FileName = inputImageFiles.First().FullName,
            UseShellExecute = true,
        });


        Console.WriteLine("\r\n★★★★★ Original Point Coodinates Input (Press escape key to go back to previous parameter)\r\n");

        var ratiosO = GetTargetPointRatiosConversationally_For_DistortImage(
                presetFramePoints: presetFramePoints,
                pickingPointCount: pickingPointCount
                );
        process?.Kill();


        Console.WriteLine("\r\n★★★★★ Started to process distortion\r\n");

        var pps = new List<(PointF, PointF)>();
        for (int i = 0; i < pickingPointCount; i++)
            pps.Add((ratiosO[i], presetTargetPointRatios[i]));


        //post process
        return
            () => inputDir!.DistortImageProportionally_Loop(outputDir,
            fill: fill,
            pps: pps.ToArray()
            );
    }


    // ★★★★★★★★★★★★★★★ helper

    public static Point[] GetFramePointsConversationally_For_DistortImage()
    {
        var pointNames = new string[]
        {
            "Upper Left of the Image（┏  ）",
            "Bottom Right of the Image（┛  ）",
        };

        var ps = IODeviceExtension.GetMouseCursorPositionConversationallyForMany(pointNames, ConsoleKey.Escape);

        return ps;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="presetFramePoints"></param>
    /// <param name="pickingPointCount"></param>
    /// <returns></returns>
    public static PointF[] GetTargetPointRatiosConversationally_For_DistortImage(Point[] presetFramePoints = null, int pickingPointCount = 4)
    {
        // preprocess
        if (pickingPointCount is < 1 or > 4)
            throw new InvalidDataException("pickingPointCount length must be in range 1 - 4");

        var framePoints = presetFramePoints ?? GetFramePointsConversationally_For_DistortImage();

        var pointNames = Enumerable.Range(1, pickingPointCount).Select(s => $"Point {s}").ToArray();
        var tagrtPoints = IODeviceExtension.GetMouseCursorPositionConversationallyForMany(pointNames, ConsoleKey.Escape);

        var Fs = framePoints;
        var Os = tagrtPoints;

        float Fw = Fs[1].X - Fs[0].X;
        float Fh = Fs[1].Y - Fs[0].Y;

        return Os.Select(o => new PointF((o.X - Fs[0].X) / Fw, (o.Y - Fs[0].Y) / Fh)).ToArray();
    }

    /// <summary>
    /// For DistortImage. Convert from PointFs to Points
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    private static (Point originalPoint, Point tagrtPoint)[] GetPointsFromPointRatios_For_DistortImage(this Image inputImage, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        return pps.Select(pp => (
           new Point((int)(pp.originalPointRatio.X * inputImage.Width), (int)(pp.originalPointRatio.Y * inputImage.Height)),
           new Point((int)(pp.tagrtPointRatio.X * inputImage.Width), (int)(pp.tagrtPointRatio.Y * inputImage.Height)))
           ).ToArray();
    }


    // ★★★★★★★★★★★★★★★ image process

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="ps">List of original points and target points. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, Color? fill = null, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        fill ??= Color.Transparent;
        ps = IncreaseDimTo4_ForDistortImage(ps);


        // main
        var oriPs = new List<Point2<double>>();
        var tarPs = new List<Point2<double>>();

        for (int i = 0; i < ps.Length; i++)
        {
            oriPs.Add(new Point2<double>(ps[i].originalPoint.X, ps[i].originalPoint.Y));
            tarPs.Add(new Point2<double>(ps[i].tagrtPoint.X, ps[i].tagrtPoint.Y));
        }

        // Homography (https://blog.neno.dev/entry/2019/01/12/homographysharp/)
        var H = Homography.Find(oriPs, tarPs).ToMathNetMatrix();
        var HI = H.PseudoInverse();

        var outputImage = DistortImageFromInverseMatrix(inputImage, HI, fill);


        // output
        GC.Collect();
        return outputImage;
    }

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image DistortImageProportionally(this Image inputImage, Color? fill = null, params (PointF originalPointRatio, PointF tagrtPointRatio)[] pps)
    {
        // sugar
        var ps = inputImage.GetPointsFromPointRatios_For_DistortImage(pps);
        return DistortImage(inputImage, fill, ps);
    }

    /// <summary>
    /// DistortImage Main
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    private static Image DistortImageFromInverseMatrix(Image inputImage, Matrix<double> invMatrix, Color? fill = null, bool useLinear = false)
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
                var tarF = CreateMatrix.Dense<double>(3, 1);

                tarF[0, 0] = w;
                tarF[1, 0] = h;
                tarF[2, 0] = 1;

                var oriF = invMatrix * tarF;

                double oriX = oriF[0, 0] / oriF[2, 0];
                double oriY = oriF[1, 0] / oriF[2, 0];

                if (useLinear)
                {
                    // 色を直線補完
                    int oriXint = (int)oriX;
                    int oriYint = (int)oriY;
                    double oXW = oriX - oriXint;
                    double oYW = oriY - oriYint;

                    if (0 < oriX && oriX < inputImage.Width - 1 && 0 < oriY && oriY < inputImage.Height - 1)
                    {
                        var c00 = inFBmp.GetPixel(oriXint + 0, oriYint + 0);
                        var c10 = inFBmp.GetPixel(oriXint + 1, oriYint + 0);
                        var c01 = inFBmp.GetPixel(oriXint + 0, oriYint + 1);
                        var c11 = inFBmp.GetPixel(oriXint + 1, oriYint + 1);

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
                }
                else
                {
                    // その点をそのまま返す
                    int oriXint = (int)oriX;
                    int oriYint = (int)oriY;

                    if (0 < oriX && oriX < inputImage.Width && 0 < oriY && oriY < inputImage.Height)
                    {
                        pixCol = inFBmp.GetPixel(oriXint, oriYint);
                    }
                    else
                    {
                        pixCol = fill.Value;
                    }
                }


                outFBmp.SetPixel(w, h, pixCol);
            }
        }

        outFBmp.EndAccess();
        inFBmp.EndAccess();

        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★ helper

    private static (Point originalPoint, Point tagrtPoint)[] IncreaseDimTo4_ForDistortImage((Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        if (ps.Length is < 1 or > 4)
            throw new InvalidDataException("ps length must be in range 1 - 4");


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


        // post process
        return ps;
    }


    // ★★★★★★★★★★★★★★★

}