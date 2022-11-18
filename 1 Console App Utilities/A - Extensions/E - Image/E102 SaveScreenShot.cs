using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output.pdf</param>
    /// <returns></returns>
    public static FileInfo SaveScreenShot(this FileInfo outputFile, Point upperLeftCoordinate, Point bottomRightCoordinate)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, null, "output.png");

        // main
        var outputImage = TakeScreenShot(upperLeftCoordinate, bottomRightCoordinate);
        outputImage.Save(outputFile.FullName, ImageFormat.Png);


        // post process
        return outputFile;
    }

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output.pdf</param>
    /// <returns></returns>
    public static FileInfo SaveScreenShot(this DirectoryInfo outputDir, Point upperLeftCoordinate, Point bottomRightCoordinate)
    {
        // pre process
        var outputFile = new FileInfo($@"{outputDir.FullName}\{DateTime.Now.ToString("s").Replace(":", "-")}.png");


        // main
        SaveScreenShot(outputFile, upperLeftCoordinate, bottomRightCoordinate);


        // post process
        return outputFile;
    }


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// SaveScreenShot
    /// </summary>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output.pdf</param>
    /// <returns></returns>
    public static Image TakeScreenShot(Point upperLeftCoordinate, Point bottomRightCoordinate)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main
        var ul = upperLeftCoordinate;
        var br = bottomRightCoordinate;

        if (br.X - ul.X < 0) (br.X, ul.X) = (ul.X, br.X);
        if (br.Y - ul.Y < 0) (br.Y, ul.Y) = (ul.Y, br.Y);

        using var outputBitmap = new Bitmap(br.X - ul.X, br.Y - ul.Y);
        using var g = Graphics.FromImage(outputBitmap);
        g.CopyFromScreen(ul, new Point(0, 0), outputBitmap.Size);

        // post process
        return (Image)outputBitmap.Clone();
    }


    // ★★★★★★★★★★★★★★★ 

}
