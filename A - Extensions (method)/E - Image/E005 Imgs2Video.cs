using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
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
    [STAThread]
    public static FileInfo Imgs2Video(this DirectoryInfo inputDir, FileInfo? outputFile,
        int videoWidth,
        int videoHeight,
        int imgFrameRate,
        int videoFrameRate = 30
        )
    {
        // preprocess
        if (UtilConfig.ConsoleOutput) Console.WriteLine("\r\n(This takes time...)");
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.avi");
        if (!outputFile!.Name.EndsWith(".avi"))
            throw new Exception("outputFile name must end with .avi");

        Console.WriteLine("To use Imgs2Video method, you need to put openh264-*.dll to executable folder!!!");

        // main
        var pngFiles = inputDir
            .GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .Sort()
            .ToArray();


        using var Writer = new VideoWriter();
        Writer.Open(outputFile.FullName, FourCC.H264, videoFrameRate, new OpenCvSharp.Size(videoWidth, videoHeight));

        for (int i = 0; i < pngFiles.Length * videoFrameRate / imgFrameRate; i++)
        {
            try
            {
                var pngFile = pngFiles[i * imgFrameRate / videoFrameRate];
                using var image = Mat.FromStream(pngFile.OpenRead(), ImreadModes.Color);
                Writer.Write(image);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }


        // post process
        return outputFile!;
    }

}
