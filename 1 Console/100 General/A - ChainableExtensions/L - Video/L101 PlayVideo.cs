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
    /// play video file frame by frame on this console application without any sound included
    /// </summary>
    /// <param name="inputFile"></param>
    public static void PlayVideo_OnThread(this FileInfo inputFile, bool closeAfterFinishingPlaying = false)
    {
        // main
        using var capture = new VideoCapture(inputFile.FullName);
        if (!capture.IsOpened() || capture.FrameCount < 0)
        {
            ConsoleExtension.WriteWithColor("(!) 読み込みに失敗しました。正しい動画ファイルかどうか確認してください。\r\n", ConsoleColor.Red);
            return;
        }
        var interval = (int)(1000 / capture.Fps);

        var windowName = "Play Video";
        var window = new Window(windowName);
        var trackbarClicked = new TrackbarCallback((int i) => capture.PosFrames = i);
        using var trackBar = window.CreateTrackbar("frames", trackbarClicked);
        trackBar.SetMax(capture.FrameCount);

        using var image = new Mat();

        while (true)
        {
            var loopStartTime = DateTime.Now;

            // exit after playing
            if (closeAfterFinishingPlaying && capture.PosFrames >= capture.FrameCount)
                break;

            // press Q to exit
            if (Cv2.WaitKey(1) == 113)
                break;

            // close window to exit
            if (IsCv2WindowClosed(windowName))
                break;

            // show next frame
            try
            {
                capture.Read(image);
                window.ShowImage(image);
                trackBar.Pos = capture.PosFrames;
            }
            catch (Exception e)
            {
                // overflow
            }

            var timeElapsed = (int)(DateTime.Now - loopStartTime).TotalMilliseconds;

            while (timeElapsed > interval)
            {
                capture.PosFrames++;
                timeElapsed -= interval;
            }
            Thread.Sleep(interval - timeElapsed);
        }

        try
        {
            window.Dispose();
        }
        catch (Exception)
        {
        }
    }

    
    // ★★★★★★★★★★★★★★★ 

}
