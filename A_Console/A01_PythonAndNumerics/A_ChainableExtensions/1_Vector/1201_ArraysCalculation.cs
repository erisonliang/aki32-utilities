

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable (generic, slower)

    public static T[] AddForEach<T>(this T[] array1, T[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => (T)((dynamic)a1! + (dynamic)a2!)).ToArray();

    public static T[] SubForEach<T>(this T[] array1, T[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => (T)((dynamic)a1! - (dynamic)a2!)).ToArray();

    public static T[] ProductForEach<T>(this T[] array1, T[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => (T)((dynamic)a1! * (dynamic)a2!)).ToArray();

    public static T[] DivForEach<T>(this T[] array1, T[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => (T)((dynamic)a1! / (dynamic)a2!)).ToArray();


    // ★★★★★★★★★★★★★★★ chainable (specific, faster)

    public static double[] AddForEach(this double[] array1, double[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => a1 + a2).ToArray();

    public static double[] SubForEach(this double[] array1, double[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => a1 - a2).ToArray();

    public static double[] ProductForEach(this double[] array1, double[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => a1 * a2).ToArray();

    public static double[] DivForEach(this double[] array1, double[] array2)
        => Enumerable.Zip(array1, array2, (a1, a2) => a1 / a2).ToArray();


    // ★★★★★★★★★★★★★★★ 

}
