using OpenCvSharp;

using Size = System.Drawing.Size;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{
    /// <summary>
    /// Images2Video (currently, png to avi only)
    /// </summary>
    /// <remarks>
    /// To use this methods, you need to put openh264-*.dll to executable folder!!!
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Images2Video/output.avi</param>
    /// <param name="imgFrameRate"></param>
    /// <param name="videoFrameRate"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="InvalidDataException"></exception>
    [STAThread]
    public static FileInfo Images2Video(this DirectoryInfo inputDir, FileInfo? outputFile,
        int imgFrameRate,
        int videoFrameRate = 60
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.avi", takesTimeFlag: true);
        if (!outputFile!.Name.EndsWith(".avi"))
            throw new Exception("outputFile name must end with .avi");


        // force download required dll
        var requiredDllName = "openh264-1.8.0-win64.dll";
        if (!File.Exists(requiredDllName))
            DownloadFileSync(new FileInfo(requiredDllName), new Uri($@"https://github.com/aki32/aki32-utilities/raw/main/Properties/Assets/{requiredDllName}"));


        // main
        var pngFiles = inputDir
            .GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .Sort()
            .ToArray();

        // find video size
        using var img0 = Mat.FromStream(pngFiles[0].OpenRead(), ImreadModes.Color);
        var videoSize = new OpenCvSharp.Size(img0.Width, img0.Height);

        using var Writer = new VideoWriter();
        Writer.Open(outputFile.FullName, FourCC.H264, videoFrameRate, videoSize);

        for (int i = 0; i < pngFiles.Length * videoFrameRate / imgFrameRate; i++)
        {
            try
            {
                var pngFile = pngFiles[i * imgFrameRate / videoFrameRate];
                using var image = Mat.FromStream(pngFile.OpenRead(), ImreadModes.Color);

                if (!(videoSize.Equals(new OpenCvSharp.Size(image.Width, image.Height))))
                    throw new InvalidDataException("All images' size must be the same. Please use ResizeImage() first.");

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
