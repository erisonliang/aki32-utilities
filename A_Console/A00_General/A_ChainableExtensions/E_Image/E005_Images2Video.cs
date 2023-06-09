﻿using OpenCvSharp;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
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
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir!, "output.mp4", takesTimeFlag: true);
        if (!outputFile!.Name.IsMatchAny(GetRegexen_VideoFiles(mp4: true, avi: true)))
            throw new Exception("outputFile name must end with .mp4 or .avi");

        // force download required dll
        var requiredDllName = "openh264-1.8.0-win64.dll";
        if (!File.Exists(requiredDllName))
            new Uri($@"https://github.com/aki32/aki32-utilities/raw/main/9_Assets/Dlls/{requiredDllName}")
                .DownloadFileAsync(new FileInfo(requiredDllName))
                .Wait();

        // main
        var imageFiles = inputDir
            .GetFiles_Images()
            .Sort()
            .ToArray();

        // find video size
        using var img0 = Mat.FromStream(imageFiles[0].OpenRead(), ImreadModes.Color);
        var videoSize = new Size(img0.Width, img0.Height);

        using var Writer = new VideoWriter();
        Writer.Open(outputFile.FullName, FourCC.H264, videoFrameRate, videoSize);

        var maxCount = imageFiles.Length * videoFrameRate / imgFrameRate;
        using var progress = new ProgressManager(maxCount);
        progress.StartAutoWrite(100);

        for (int i = 0; i < maxCount; i++)
        {
            try
            {
                var pngFile = imageFiles[i * imgFrameRate / videoFrameRate];
                using var image = Mat.FromStream(pngFile.OpenRead(), ImreadModes.Color);

                if (!videoSize.Equals(new Size(image.Width, image.Height)))
                    throw new InvalidDataException("All images' size must be the same. Please use ResizeImage() first.");

                Writer.Write(image);

                progress.CurrentStep = i;
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
