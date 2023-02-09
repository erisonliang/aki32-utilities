

using DocumentFormat.OpenXml.Office2010.Excel;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Write 1D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void WriteToConsole<T>(this IEnumerable<T> inputData, int columnWidth = 3)
    {
        // main
        Console.WriteLine(string.Join(",", inputData.Select(x => $"{x?.ToString()?.PadLeft(columnWidth)}")));
    }

    /// <summary>
    /// Write Jagged 2D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void WriteToConsole<T>(this IEnumerable<IEnumerable<T>> inputData, int columnWidth = 3)
    {
        // main
        foreach (var lineItems in inputData)
            lineItems.WriteToConsole(columnWidth);
    }

    /// <summary>
    /// Write 2D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void WriteToConsole<T>(this T[,] inputData, int columnWidth = 3)
    {
        // main
        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
        {
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
                Console.Write($"{inputData[d0, d1]?.ToString()?.PadLeft(columnWidth)},");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Write 3D Array to console
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static void WriteToConsole<T>(this T[,,] inputData, int columnWidth = 3)
    {
        // main
        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
        {
            Console.WriteLine($" {d0}:");
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
            {
                for (int d2 = 0; d2 < inputData.GetLength(2); d2++)
                    Console.Write($"  {inputData[d0, d1, d2]?.ToString()?.PadLeft(columnWidth)},");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

}
