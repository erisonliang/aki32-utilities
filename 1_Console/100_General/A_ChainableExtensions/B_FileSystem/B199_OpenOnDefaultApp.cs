using System.Diagnostics;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// open selected file (or directory) on default application
    /// </summary>
    /// <param name="inputFileOrDir"></param>
    public static void OpenOnDefaultApp(this FileSystemInfo inputFileOrDir, bool waitForExit = true, string? arguments = null)
    {
        // main
        using var p = Process.Start(new ProcessStartInfo
        {
            FileName = inputFileOrDir.FullName,
            Arguments = arguments,
            UseShellExecute = true,
        });

        // TODO: p will be null; so this does not work well 
        if (p != null && waitForExit)
        {
            p.WaitForExit();
        }

    }


    // ★★★★★★★★★★★★★★★ 

}
