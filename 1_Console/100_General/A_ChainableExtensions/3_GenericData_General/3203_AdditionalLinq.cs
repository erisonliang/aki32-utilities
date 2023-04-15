

using System.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    // you can simply use ReShape for doing this
    public static T Max<T>(this T[,] inputData) where T : IComparable<T> => inputData.ReShape()!.Max()!;
    public static T Min<T>(this T[,] inputData) where T : IComparable<T> => inputData.ReShape()!.Min()!;
    public static T Max<T>(this T[,,] inputData) where T : IComparable<T> => inputData.ReShape()!.Max()!;
    public static T Min<T>(this T[,,] inputData) where T : IComparable<T> => inputData.ReShape()!.Min()!;

}
