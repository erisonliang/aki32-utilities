using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

    /// <summary>
    /// Check if point is in circle 
    /// 円の中に点が入ってるかどうか。
    /// <param name="targetPoint"></param>
    /// <param name="circle"></param>
    /// <returns></returns>
    public static bool IsPointInCircle(this Circle2D circle, Point2D targetPoint)
    {
        var distance = targetPoint.DistanceTo(circle.Center);
        return distance < circle.Radius;
    }


    // ★★★★★★★★★★★★★★★ 

}
