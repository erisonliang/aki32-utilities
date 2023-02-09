using NAudio.Wave;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// sugar
    /// play sound file on your default application
    /// </summary>
    /// <param name="inputFile"></param>
    public static void PlaySound_OnDefaultApp(this FileInfo inputFile, bool waitForExit = true)
    {
        // main
        inputFile.OpenOnDefaultApp(waitForExit);
    }

    /// <summary>
    /// play sound file on this console application
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="playBackground">play sound background; do not stick to this method after start playing.</param>
    /// <param name="showSequenceBar">show sequence bar when playing in this methods</param>
    /// <returns>WaveOutEvent where you can stop playing from, when you choose playBackground</returns>
    public static WaveOutEvent? PlaySound_OnConsole(this FileInfo inputFile, bool playBackground = false, bool showSequenceBar = true)
    {
        // preprocess
        var outputDevice = new WaveOutEvent();
        using var afr = new AudioFileReader(inputFile.FullName);


        // main
        outputDevice.Init(afr);
        outputDevice.Play();


        // playing background
        if (playBackground)
        {
            ConsoleExtension.WriteWithColor($"Playing {inputFile.Name}.\r\n");
            return outputDevice;
        }


        // playing on console
        ProgressManager? progress = null;
        if (showSequenceBar)
        {
            progress = new ProgressManager((int)afr.Length)
            {
                WritePercentage = false,
                WriteSteps = false,
                WriteProgressBar = true,
                WriteElapsedTime = false,
                WriteEstimateTime = false,
                WriteErrorCount = false,
                ProgressBarBoxCount = 50,
            };

            var additionalAction = new Action(() => progress.CurrentStep = (int)afr.Position);
            progress.StartAutoWrite(interval: 20, additionalAction: additionalAction);
        }

        var bytesPerSeconds = (int)(afr.Length / afr.TotalTime.TotalSeconds);

        ConsoleExtension.WriteWithColor($"Playing {inputFile.Name}.");
        ConsoleExtension.WriteWithColor($"Press 'R' to restart, '←' or '→' to jump 5s, or any others to stop playing.\r\n");
        Console.WriteLine();

        do
        {
            var inputKey = Console.ReadKey(true);

            if (inputKey.Key == ConsoleKey.R)
            {
                afr.Position = 0;
                outputDevice.Play();
                continue;
            }
            else if (inputKey.Key == ConsoleKey.LeftArrow)
            {
                afr.Position = Math.Max(afr.Position - 5 * bytesPerSeconds, 0);
                outputDevice.Play();
                continue;
            }
            else if (inputKey.Key == ConsoleKey.RightArrow)
            {
                afr.Position = Math.Min(afr.Position + 5 * bytesPerSeconds, afr.Length);
                outputDevice.Play();
                continue;
            }

            outputDevice.Stop();

            break;
        } while (true);

        if (progress != null)
        {
            progress.WriteDone();
            progress.Dispose();
        }

        outputDevice.Dispose();
        return null;
    }


    // ★★★★★★★★★★★★★★★ 

}
