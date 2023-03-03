

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class EnumerableExtension
{
    // ★★★★★★★★★★★★★★★ for use

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<double> Range_WithCount(double from, double to, int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => x * (to - from) / (count - 1) + from)
            ;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<double> Range_WithStep(double from, double to, double step)
    {
        var count = (int)((to - from) / step);
        return Range_WithCount(from, to, count);
    }


    // ★★★★★★★★★★★★★★★ 

}
