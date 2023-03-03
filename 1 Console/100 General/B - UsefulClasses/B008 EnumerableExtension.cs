using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OpenCvSharp.Internal;

namespace Aki32Utilities.ConsoleAppUtilities.General;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
public class EnumerableExtension
{
    // ★★★★★★★★★★★★★★★ for use

    /// <summary>
    /// 
    /// </code>
    public static IEnumerable<double> Range(double from, double to, int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => x * (to - from) / (count-1) + from)
            ;
    }


    // ★★★★★★★★★★★★★★★ 

}
