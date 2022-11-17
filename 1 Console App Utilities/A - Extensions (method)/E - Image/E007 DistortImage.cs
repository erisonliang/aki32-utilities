using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Math;

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
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);
        if (!inputFile!.Name.EndsWith(".png"))
            throw new Exception("inputFile name must end with .png");
        if (!outputFile!.Name.EndsWith(".png"))
            throw new Exception("outputFile name must end with .png");


        // main
        using var inputImage = Image.FromFile(inputFile.FullName);
        var outputImage = DistortImage(inputImage, ps);
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
    public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir!.FullName, file.Name);
            try
            {
                file.DistortImage(new FileInfo(newFilePath), ps);
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
    /// <param name="ps">List of original points and target points. min 1, max 3</param>
    /// <returns></returns>
    public static Image DistortImage(this Image inputImage, params (Point originalPoint, Point tagrtPoint)[] ps)
    {
        // preprocess
        var dim = ps.Length;
        if (dim is < 1 or > 3)
            throw new InvalidDataException("ps length must be in range 1 - 3");

        // main
        switch (dim)
        {
            case 1:
                {
                    // 移動のみ
                    throw new NotImplementedException();
                }
            case 2:
                {
                    // 回転・移動のみ
                    throw new NotImplementedException();
                }
            case 3:
                {
                    // 回転・移動・せん断

                    using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
                    using var g = Graphics.FromImage(outputBitmap);
                    g.FillRectangle(Brushes.Orange, new Rectangle(new Point(0, 0), outputBitmap.Size));


                    var oriO = CreateMatrix.Dense<float>(3, 2);
                    var tarO = CreateMatrix.Dense<float>(3, 2);

                    for (int i = 0; i < dim; i++)
                    {
                        oriO[i, 0] = ps[i].originalPoint.X;
                        oriO[i, 1] = ps[i].originalPoint.Y;
                        tarO[i, 0] = ps[i].tagrtPoint.X;
                        tarO[i, 1] = ps[i].tagrtPoint.Y;
                    }


                    var oriO0 = CreateMatrix.Dense<float>(3, 2);
                    var tarO0 = CreateMatrix.Dense<float>(3, 2);

                    for (int i = 0; i < 3; i++)
                    {
                        oriO0[i, 0] = oriO[0, 0];
                        oriO0[i, 1] = oriO[0, 1];
                        tarO0[i, 0] = tarO[0, 0];
                        tarO0[i, 1] = tarO[0, 1];
                    }


                    var oriF = CreateMatrix.Dense<float>(3, 2);
                    var tarF = CreateMatrix.Dense<float>(3, 2);

                    oriF[1, 0] = inputImage.Width;
                    oriF[2, 1] = inputImage.Height;


                    //tarP -= tarP0;
                    //oriP -= tarP0;
                    ////oriP -= oriP0;
                    ////var oriEXSize = oriE[1, 0] - oriE[0, 0];
                    ////var oriEYSize = oriE[1, 1] - oriE[0, 1];

                    //var X = tarP * oriP.PseudoInverse();
                    //tarE = X * oriE;

                    //tarE += tarP0;
                    //tarE -= oriP0;


                    var m1 = tarO - tarO0;
                    var m2 = (oriO - oriO0).PseudoInverse();
                    var m3 = oriF - oriO0;

                    tarF = m1 * m2 * m3 + tarO0;


                    var points = new PointF[]
                    {
                        new PointF(tarF[0, 0], tarF[0, 1]),
                        new PointF(tarF[1, 0], tarF[1, 1]),
                        new PointF(tarF[2, 0], tarF[2, 1]),
                    };

                    g.DrawImage(inputImage, points);

                    return (Image)outputBitmap.Clone();
                }


            //case 1:
            //    {
            //        using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
            //        using var g = Graphics.FromImage(outputBitmap);

            //        var desCenter = ps[0].tagrtPoint;
            //        var oriCenter = ps[0].originalPoint;

            //        var move1 = Point.Subtract(desCenter, ((Size)oriCenter));
            //        var points = new Point[]
            //        {
            //            Point.Add(new Point(0,0), (Size)move1),
            //            Point.Add(new Point(inputImage.Width,0), (Size)move1),
            //            Point.Add(new Point(0,inputImage.Height), (Size)move1),
            //        };

            //        g.DrawImage(inputImage, points);

            //        return (Image)outputBitmap.Clone();
            //    }
            //case 2:
            //    {
            //        throw new NotImplementedException();

            //        using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
            //        using var g = Graphics.FromImage(outputBitmap);

            //        // 移動元の重心を原点に持ってきて，それを移動先の重心に移動。

            //        //var oriCenter = ps.Average(p => p.tagrtPoint);
            //        //var desCenter = ps[0].originalPoint;

            //        //var move1 = Point.Subtract(oriCenter, ((Size)desCenter));


            //        var move1 = Point.Subtract(ps[0].tagrtPoint, ((Size)ps[0].originalPoint));
            //        var points = new Point[]
            //        {
            //            Point.Add(new Point(0,0), (Size)move1),
            //            Point.Add(new Point(inputImage.Width,0), (Size)move1),
            //            Point.Add(new Point(0,inputImage.Height), (Size)move1),
            //        };

            //        g.DrawImage(inputImage, points);

            //        return (Image)outputBitmap.Clone();
            //    }
            //case 3:
            //    {
            //        throw new NotImplementedException();

            //        using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
            //        using var g = Graphics.FromImage(outputBitmap);

            //        var points = new Point[]
            //          {
            //                    new Point(0,0),
            //                    new Point(400,0),
            //                    new Point(300,300),
            //          };

            //        g.DrawImage(inputImage, points);

            //        return (Image)outputBitmap.Clone();
            //    }

            default:
                throw new InvalidDataException("length of ps must be 1 - 3");
        }

        // ★★★★★★★★★★★★★★★

    }
}

