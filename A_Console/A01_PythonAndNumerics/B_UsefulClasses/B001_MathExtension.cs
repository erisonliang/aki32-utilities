using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Numerics;
public class MathExtension
{

    // ★★★★★★★★★★★★★★★ for use

    public static T Between<T>(T min, T target, T max) where T : IComparable<T>
    {
        var topComparer = new T[] { target, max };
        var bottomComparer = new T[] { min, topComparer.Min()! };
        return bottomComparer.Max()!;
    }


    // ★★★★★★★★★★★★★★★ 

}
