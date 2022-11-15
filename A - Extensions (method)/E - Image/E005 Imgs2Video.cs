using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


using OpenCvSharp;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{
    /// <summary>
    /// Imgs2Video
    ///  - Currently png only
    /// </summary>
    /// <remarks>
    /// To use this methods, you need to put openh264-*.dll to executable folder!!!
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Imgs2PDF/output.mp4</param>
    /// <returns></returns>
    //[STAThread]
    public static FileInfo Imgs2Video(this DirectoryInfo inputDir, FileInfo? outputFile,
             int frameRate = 15,
             int videoWidth = 100,
             int videoHeight = 100
            )
    {
        // preprocess
        if (UtilConfig.ConsoleOutput) Console.WriteLine("\r\n(This takes time...)");
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.avi");
        if (!outputFile!.Name.EndsWith(".avi"))
            throw new Exception("outputFile name must end with .avi");

        Console.WriteLine("To use Imgs2Video method, you need to put openh264-*.dll to executable folder!!!");

        // main
        var pngFIs = inputDir
            .GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .Sort()
            .ToArray();

        // 指定サイズの画像を作ってからビデオ処理！！

        using var Writer = new VideoWriter();
        Writer.Open(outputFile.FullName, FourCC.H264, frameRate, new OpenCvSharp.Size(videoWidth, videoHeight));

        foreach (var png in pngFIs)
        {
            var image = Mat.FromStream(png.OpenRead(), ImreadModes.Color);
            Writer.Write(image);
        }


        // post process
        return outputFile!;
    }

}
