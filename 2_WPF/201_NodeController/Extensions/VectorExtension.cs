using System.Windows;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Extensions;
public static class VectorExtension
{
    public static double DotProduct(this Vector a, Vector b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static Vector GetNormalized(this Vector v)
    {
        var temp = v;
        temp.Normalize();

        return temp;
    }

    public static double GetTheta(this Vector v)
    {
        return Math.Atan2(v.Y, v.X);
    }

}
