using OpenCvSharp;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// Save video frames as many images.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="capturingFrameRate">Capture only entered number of images per second.</param>
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
        var capturingFrameRate_i = capture.Fps / capturingFrameRate;

        using var progress = new ProgressManager(capture.FrameCount);
        progress.StartAutoWrite(100);

        for (int i = 0; i < capture.FrameCount; i++)
        {
            capture.Read(image);

            if (capturingFrameRate_i != null && (i % capturingFrameRate_i) != 0)
                continue;

            var imgNumber = i.ToString().PadLeft(8, '0');

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
