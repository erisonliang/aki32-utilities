

namespace Aki32Utilities.ConsoleAppUtilities.General;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
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
