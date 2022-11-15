using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
    /// Imgs2Video (currently, png to avi only)
    /// </summary>
    /// <remarks>
    /// To use this methods, you need to put openh264-*.dll to executable folder!!!
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Imgs2Video/output.avi</param>
    /// <returns></returns>
    [STAThread]
    public static FileInfo Imgs2Video(this DirectoryInfo inputDir, FileInfo? outputFile,
        int videoWidth,
        int videoHeight,
        int imgFrameRate,
        int videoFrameRate = 60
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.avi", takesTimeFlag: true);
        if (!outputFile!.Name.EndsWith(".avi"))
            throw new Exception("outputFile name must end with .avi");


        // force install openh264-*.dll (To use Imgs2Video method, you need to put openh264-*.dll to executable folder!!!)
        if (!File.Exists("openh264-1.8.0-win64.dll"))
            DownloadFileSync(
                new FileInfo("openh264-1.8.0-win64.dll"),
                new Uri("https://github.com/aki32/aki32-utilities/raw/main/Properties/openh264-1.8.0-win64.dll"));


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

                if (!(image.Width == videoWidth && image.Height == videoHeight))
                    throw new InvalidDataException("Sorry, all images' size must match to video's. Please use ResizeImage() first.");

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
