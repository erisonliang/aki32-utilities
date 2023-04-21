using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.Numerics;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

    /// <summary>
    /// Compare Points forcibly
    /// </summary>
    /// <param name="target0"></param>
    /// <param name="target1"></param>
    /// <returns></returns>
    public static int Compare(this Point2D target0, Point2D target1)
    {
        if (target0.X == target1.X)
        {
            if (target0.Y == target1.Y)
                return 0;

            if (target0.Y > target1.Y)
                return 1;
            return -1;
        }

        if (target0.X > target1.X)
            return 1;
        return -1;
    }


    // ★★★★★★★★★★★★★★★ 

}
