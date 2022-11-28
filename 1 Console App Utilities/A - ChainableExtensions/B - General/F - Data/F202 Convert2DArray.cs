

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Convert to 2D Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static T[,] ConvertTo2DArray<T>(this T[][] inputData, T emptyItem = default!)
    {
        // main
        var columnCount = inputData.Length;
        var rowCount = inputData.Max(x => x.Length);
        var outputData = new T[columnCount, rowCount];
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                try
                {
                    outputData[i, j] = inputData[i][j];
                }
                catch (Exception)
                {
                    outputData[i, j] = emptyItem;
                }
            }
        }


        // post process
        return outputData;
    }
 
    /// <summary>
    /// Convert to Jagged Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <param name="emptyItem"></param>
    /// <returns></returns>
    public static T[][] ConvertToJaggedArray<T>(this T[,] inputData)
    {
        // main
        var outputData = new T[inputData.GetLength(0)][];

        for (int i = 0; i < inputData.GetLength(0); i++)
        {
            var line = new T[inputData.GetLength(1)];
            for (int j = 0; j < inputData.GetLength(1); j++)
            {
                try
                {
                    line[j] = inputData[i, j];
                }
                catch (Exception)
                {
                    line[j] = default!;
                }
            }
            outputData[i] = line;
        }


        // post process
        return outputData;
    }

}
