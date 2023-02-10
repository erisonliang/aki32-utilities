using OpenCvSharp;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// sugar
    /// play video file on your default application
    /// </summary>
    /// <param name="inputFile"></param>
    public static void ShowImage_OnDefaultApp(this FileInfo inputFile, bool waitForExit = true)
    {
        // main
        inputFile.OpenOnDefaultApp(waitForExit);
    }

    /// <summary>
    /// Show image on OpenCV2 window.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="waitForExit">wait until close window. "true" recommended</param>
    /// <returns>return window object when waitForExit is false</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public static Window ShowImage_OnThread(this FileInfo inputFile, bool waitForExit = true)
    {
        // main
        var windowName = inputFile.Name; //"ShowImage";
        var window = new Window(windowName);
        var image = new Mat(inputFile.FullName);
        window.ShowImage(image);

        if (!waitForExit)
        {
            Cv2.WaitKey(1);
            return window;
        }

        while (!IsCv2WindowClosed(windowName))
        {
            if (Cv2.WaitKey(1) == 113)
                break;
            Thread.Sleep(100);
        }
        try
        {
            window.Dispose();
            image.Dispose();
        }
        catch (Exception)
        {
        }
        return null;
    }


    // ★★★★★★★★★★★★★★★ 

}
