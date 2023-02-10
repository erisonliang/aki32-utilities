using OpenCvSharp;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.ExtendedProperties;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using OpenCvSharp.XImgProc;

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
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="capturingFrameRate">Capture only entered number of images per second.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public static void ShowImage_OnThread(this FileInfo inputFile, bool waitForExit = false)
    {
        // main
        var windowName = inputFile.Name; //"ShowImage";
        using var window = new Window(windowName);
        using var image = new Mat(inputFile.FullName);

        if (waitForExit)
            while (!IsCv2WindowClosed(windowName))
                Task.Delay(200).Wait();

    }


    // ★★★★★★★★★★★★★★★ 

}
