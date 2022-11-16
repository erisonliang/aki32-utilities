using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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


        // main
        switch (ps.Length)
        {
            case 1:
                {
                    using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
                    using var g = Graphics.FromImage(outputBitmap);

                    var move = Point.Subtract(ps[0].tagrtPoint, ((Size)ps[0].originalPoint));
                    var ppp = new Point[]
                    {
                        Point.Add(new Point(0,0), (Size)move),
                        Point.Add(new Point(inputImage.Width,0), (Size)move),
                        Point.Add(new Point(0,inputImage.Height), (Size)move),
                    };

                    g.DrawImage(inputImage, ppp);

                    return (Image)outputBitmap.Clone();
                }
            case 2:
                {
                    throw new NotImplementedException();

                    using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
                    using var g = Graphics.FromImage(outputBitmap);

                    //var move = Point.Subtract(ps[0].tagrtPoint, ((Size)ps[0].originalPoint));
                    //var ppp = new Point[]
                    //{
                    //    new Point(0,0),
                    //    new Point(400,0),
                    //    new Point(300,300),
                    //};

                    var ppp = new Point[]
                    {
                        new Point(0,0),
                        new Point(400,0),
                        new Point(300,300),
                      };

                    g.DrawImage(inputImage, ppp);

                    return (Image)outputBitmap.Clone();
                }
            case 3:
                {
                    throw new NotImplementedException();

                    using var outputBitmap = new Bitmap(inputImage.Width, inputImage.Height);
                    using var g = Graphics.FromImage(outputBitmap);

                    //var move = Point.Subtract(ps[0].tagrtPoint, ((Size)ps[0].originalPoint));
                    //var ppp = new Point[]
                    //{
                    //    new Point(0,0),
                    //    new Point(400,0),
                    //    new Point(300,300),
                    //};

                    var ppp = new Point[]
                      {
                                new Point(0,0),
                                new Point(400,0),
                                new Point(300,300),
                      };

                    g.DrawImage(inputImage, ppp);

                    return (Image)outputBitmap.Clone();
                }
            default:
                throw new InvalidDataException("length of ps must be 1 - 3");
        }
    }

    // ★★★★★★★★★★★★★★★

}
