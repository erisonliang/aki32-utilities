using OpenCvSharp;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.ExtendedProperties;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using OpenCvSharp.XImgProc;
using System.Diagnostics;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// open selected file on default application
    /// </summary>
    /// <param name="inputFile"></param>
    public static void OpenOnDefaultApp(this FileInfo inputFile, bool waitForExit = true, string? arguments = null)
    {
        // main
        using var p = Process.Start(new ProcessStartInfo
        {
            FileName = inputFile.FullName,
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
