

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// re-shape array data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] ReShape<T>(this IEnumerable<T> inputData, int dim0, int dim1)
    {
        // preprocess
        if (inputData.Count() != dim0 * dim1)
            throw new InvalidDataException("Check dimensions are correct");


        // main
        var result = new T[dim0, dim1];
        var d0Strand = dim1;

        for (int d0 = 0; d0 < dim0; d0++)
            for (int d1 = 0; d1 < dim1; d1++)
                result[d0, d1] = inputData.ElementAt(d0Strand * d0 + d1);


        // post process
        return result;
    }

    /// <summary>
    /// re-shape array data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] ReShape<T>(this IEnumerable<T> inputData, int dim0, int dim1, int dim2)
    {
        // preprocess
        if (inputData.Count() != dim0 * dim1 * dim2)
            throw new InvalidDataException("Check dimensions are correct");


        // main
        var result = new T[dim0, dim1, dim2];
        var d0Strand = dim1 * dim2;
        var d1Strand = dim2;

        for (int d0 = 0; d0 < dim0; d0++)
            for (int d1 = 0; d1 < dim1; d1++)
                for (int d2 = 0; d2 < dim2; d2++)
                    result[d0, d1, d2] = inputData.ElementAt(d0Strand * d0 + d1Strand * d1 + d2);


        // post process
        return result;
    }

    /// <summary>
    /// re-shape array data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] ReShape<T>(this T[,] inputData)
    {
        // main
        var result = new T[inputData.Length];
        var d0Strand = inputData.GetLength(1);

        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
                result[d0Strand * d0 + d1] = inputData[d0, d1];


        // post process
        return result;
    }

    /// <summary>
    /// re-shape array data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] ReShape<T>(this T[,,] inputData)
    {
        // main
        var result = new T[inputData.Length];

        var d0Strand = inputData.GetLength(1) * inputData.GetLength(2);
        var d1Strand = inputData.GetLength(2);

        for (int d0 = 0; d0 < inputData.GetLength(0); d0++)
            for (int d1 = 0; d1 < inputData.GetLength(1); d1++)
                for (int d2 = 0; d2 < inputData.GetLength(2); d2++)
                    result[d0Strand * d0 + d1Strand * d1 + d2] = inputData[d0, d1, d2];


        // post process
        return result;
    }

}
