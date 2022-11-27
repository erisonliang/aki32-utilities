

namespace Aki32_Utilities.ChainableExtensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Transpose 2D Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[][] Transpose<T>(this T[][] inputData, T emptyItem = default!)
    {
        // main
        var columnCount = inputData.Length;
        var rowCount = inputData.Max(x => x?.Length ?? 0);
        var outputData = new T[rowCount][];
        for (int i = 0; i < rowCount; i++)
        {
            var line = new T[columnCount];
            for (int j = 0; j < columnCount; j++)
            {
                try
                {
                    line[j] = inputData[j][i];
                }
                catch (Exception)
                {
                    line[j] = emptyItem;
                }
            }
            outputData[i] = line;
        }


        // post process
        return outputData;
    }

    /// <summary>
    /// Transpose 2D Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] Transpose<T>(this T[,] inputData)
    {
        // main
        var outputData = new T[inputData.GetLength(1), inputData.GetLength(0)];
        for (int i = 0; i < inputData.GetLength(1); i++)
        {
            for (int j = 0; j < inputData.GetLength(0); j++)
            {
                try
                {
                    outputData[i, j] = inputData[j, i];
                }
                catch (Exception)
                {
                    outputData[i, j] = default!;
                }
            }
        }


        // post process
        return outputData;
    }

}
