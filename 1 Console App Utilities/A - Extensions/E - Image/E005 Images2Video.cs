using OpenCvSharp;

using Size = System.Drawing.Size;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// Merge all images in a folder to a video.
    /// Ccurrently, (jpg, .png) to (.avi, .mp4) is only supported.
    /// </summary>
    /// <remarks>
    /// To use this methods, you need to put openh264-*.dll to executable folder!!!
    /// It will be automatically downloaded!
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="imgFrameRate"></param>
    /// <param name="videoFrameRate"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public static FileInfo Images2Video(this DirectoryInfo inputDir, FileInfo? outputFile,
        int imgFrameRate,
        int videoFrameRate = 60
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.mp4", takesTimeFlag: true);
        if (!outputFile!.Name.IsMatchAny(GetVideoFilesRegexen()))
            throw new Exception("outputFile name must end with .mp4 or .avi");


        // force download required dll
        var requiredDllName = "openh264-1.8.0-win64.dll";
        if (!File.Exists(requiredDllName))
            DownloadFileSync(new FileInfo(requiredDllName), new Uri($@"https://github.com/aki32/aki32-utilities/raw/main/1%20Console%20App%20Utilities/Properties/Assets/{requiredDllName}"));


        // main
        var imageFiles = inputDir
            .GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen())
            .Sort()
            .ToArray();

        // find video size
        using var img0 = Mat.FromStream(imageFiles[0].OpenRead(), ImreadModes.Color);
        var videoSize = new OpenCvSharp.Size(img0.Width, img0.Height);

        using var Writer = new VideoWriter();
        Writer.Open(outputFile.FullName, FourCC.H264, videoFrameRate, videoSize);

        var maxCount = imageFiles.Length * videoFrameRate / imgFrameRate;
        var progress = new ProgressManager(maxCount);

        for (int i = 0; i < maxCount; i++)
        {
            try
            {
                if (i % 10 == 0) progress.WriteCurrentState(i);

                var pngFile = imageFiles[i * imgFrameRate / videoFrameRate];
                using var image = Mat.FromStream(pngFile.OpenRead(), ImreadModes.Color);

                if (!videoSize.Equals(new OpenCvSharp.Size(image.Width, image.Height)))
                    throw new InvalidDataException("All images' size must be the same. Please use ResizeImage() first.");

                Writer.Write(image);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        progress.WriteDone();

        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ 

}
