using OpenCvSharp;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.ExtendedProperties;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using OpenCvSharp.XImgProc;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ helper

    /// <summary>
    /// get if cv2 window is closed or not
    /// </summary>
    /// <param name="windowName"></param>
    /// <returns></returns>
    private static bool IsCv2WindowClosed(string windowName)
    {
        try
        {
            if (Cv2.GetWindowProperty(windowName, WindowPropertyFlags.Fullscreen) < 0)
                return true;
        }
        catch
        {
            return true;
        }
        return false;
    }


    // ★★★★★★★★★★★★★★★ 

}
