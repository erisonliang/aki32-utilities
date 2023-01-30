using OpenCvSharp;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

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
    public static DirectoryInfo Video2Images(this FileInfo inputFile, DirectoryInfo? outputDir, int? capturingFrameRate = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!, takesTimeFlag: true);


        // main
        using var capture = new VideoCapture(inputFile.FullName);
        using var window = new Window("Video2Images Progress");
        using var image = new Mat();

        using var progress = new ProgressManager(capture.FrameCount);
        progress.StartAutoWrite(100);

        var i = 0;
        var capturingFrameRate_i = capture.Fps / capturingFrameRate;
        while (capture.IsOpened())
        {
            capture.Read(image);
            if (image.Empty())
                break;

            i++;
            var imgNumber = i.ToString().PadLeft(8, '0');

            if (capturingFrameRate_i != null && i % capturingFrameRate_i != 0)
                break;

            var frameImageFileName = $@"{outputDir!.FullName}\image{imgNumber}.png";
            Cv2.ImWrite(frameImageFileName, image);

            window.ShowImage(image);

            if (Cv2.WaitKey(1) == 113) // Q
                break;

            progress.CurrentStep = i;
        }

        progress.WriteDone();


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ 

}
