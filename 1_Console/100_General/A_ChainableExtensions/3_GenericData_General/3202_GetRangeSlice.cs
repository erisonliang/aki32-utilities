

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ get slice

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSlice<T>(this T[,] inputData, Range r0, Range r1)
    {
        // main
        (var r0From, var r0Len) = r0.GetOffsetAndLength(inputData.GetLength(0));
        (var r1From, var r1Len) = r1.GetOffsetAndLength(inputData.GetLength(1));

        var result = new T[r0Len, r1Len];

        for (int d0 = 0; d0 < r0Len; d0++)
            for (int d1 = 0; d1 < r1Len; d1++)
                result[d0, d1] = inputData[r0From + d0, r1From + d1];


        // post process
        return result;
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Range r0, Range r1, Range r2)
    {
        // main
        (var r0From, var r0Len) = r0.GetOffsetAndLength(inputData.GetLength(0));
        (var r1From, var r1Len) = r1.GetOffsetAndLength(inputData.GetLength(1));
        (var r2From, var r2Len) = r2.GetOffsetAndLength(inputData.GetLength(2));

        var result = new T[r0Len, r1Len, r2Len];

        for (int d0 = 0; d0 < r0Len; d0++)
            for (int d1 = 0; d1 < r1Len; d1++)
                for (int d2 = 0; d2 < r2Len; d2++)
                    result[d0, d1, d2] = inputData[r0From + d0, r1From + d1, r2From + d2];


        // post process
        return result;
    }


    // ★★★★★★★★★★★★★★★ shrink

    /// <summary>
    /// get shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] ShrinkDimension<T>(this T[,] inputData, int targetDimension)
    {
        // preprocess
        if (inputData.GetLength(targetDimension) != 1)
            throw new InvalidDataException($"Length of targetDimension of inputData needs to be 1");
        var d0Len = inputData.GetLength(0);
        var d1Len = inputData.GetLength(1);


        // main
        if (targetDimension == 0)
        {
            var result = new T[d1Len];

            for (int d1 = 0; d1 < d1Len; d1++)
                result[d1] = inputData[0, d1];

            return result;
        }
        else if (targetDimension == 1)
        {
            var result = new T[d0Len];

            for (int d0 = 0; d0 < d0Len; d0++)
                result[d0] = inputData[d0, 0];

            return result;
        }
        else
            throw new InvalidDataException("dim must be 0 or 1");

    }

    /// <summary>
    /// get shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] ShrinkDimension<T>(this T[,,] inputData, int targetDimension)
    {
        // preprocess
        if (inputData.GetLength(targetDimension) != 1)
            throw new InvalidDataException($"Length of targetDimension of inputData needs to be 1");
        var d0Len = inputData.GetLength(0);
        var d1Len = inputData.GetLength(1);
        var d2Len = inputData.GetLength(2);

        // main
        if (targetDimension == 0)
        {
            var result = new T[d1Len, d2Len];

            for (int d1 = 0; d1 < d1Len; d1++)
                for (int d2 = 0; d2 < d2Len; d2++)
                    result[d1, d2] = inputData[0, d1, d2];

            return result;
        }
        else if (targetDimension == 1)
        {
            var result = new T[d0Len, d2Len];

            for (int d0 = 0; d0 < d0Len; d0++)
                for (int d2 = 0; d2 < d2Len; d2++)
                    result[d0, d2] = inputData[d0, 0, d2];

            return result;
        }
        else if (targetDimension == 2)
        {
            var result = new T[d0Len, d1Len];

            for (int d0 = 0; d0 < d0Len; d0++)
                for (int d1 = 0; d1 < d1Len; d1++)
                    result[d0, d1] = inputData[d0, d1, 0];

            return result;
        }
        else
            throw new InvalidDataException("dim must be 0, 1 or 2");

    }


    // ★★★★★★★★★★★★★★★ get slice expansion

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSlice<T>(this T[,] inputData, Index i0, Range r1)
    {
        // main
        var r0 = i0.ConvertToSingleRange(inputData.GetLength(0));
        return inputData.GetRangeSlice(r0, r1);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSlice<T>(this T[,] inputData, Range r0, Index i1)
    {
        // main
        var r1 = i1.ConvertToSingleRange(inputData.GetLength(1));
        return inputData.GetRangeSlice(r0, r1);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Index i0, Range r1, Range r2)
    {
        // main
        var r0 = i0.ConvertToSingleRange(inputData.GetLength(0));
        return inputData.GetRangeSlice(r0, r1, r2);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Range r0, Index i1, Range r2)
    {
        // main
        var r1 = i1.ConvertToSingleRange(inputData.GetLength(1));
        return inputData.GetRangeSlice(r0, r1, r2);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Range r0, Range r1, Index i2)
    {
        // main
        var r2 = i2.ConvertToSingleRange(inputData.GetLength(2));
        return inputData.GetRangeSlice(r0, r1, r2);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Index i0, Index i1, Range r2)
    {
        // main
        var r0 = i0.ConvertToSingleRange(inputData.GetLength(0));
        var r1 = i1.ConvertToSingleRange(inputData.GetLength(1));
        return inputData.GetRangeSlice(r0, r1, r2);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Range r0, Index i1, Index i2)
    {
        // main
        var r1 = i1.ConvertToSingleRange(inputData.GetLength(1));
        var r2 = i2.ConvertToSingleRange(inputData.GetLength(2));
        return inputData.GetRangeSlice(r0, r1, r2);
    }

    /// <summary>
    /// get sliced array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,,] GetRangeSlice<T>(this T[,,] inputData, Index i0, Range r1, Index i2)
    {
        // main
        var r0 = i0.ConvertToSingleRange(inputData.GetLength(0));
        var r2 = i2.ConvertToSingleRange(inputData.GetLength(2));
        return inputData.GetRangeSlice(r0, r1, r2);
    }


    // ★★★★★★★★★★★★★★★ get slice and shrink expansion

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] GetRangeSliceAndShrink<T>(this T[,] inputData, Index i0, Range r1) => inputData.GetRangeSlice(i0, r1).ShrinkDimension(0);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] GetRangeSliceAndShrink<T>(this T[,] inputData, Range r0, Index i1) => inputData.GetRangeSlice(r0, i1).ShrinkDimension(1);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSliceAndShrink<T>(this T[,,] inputData, Index i0, Range r1, Range r2) => inputData.GetRangeSlice(i0, r1, r2).ShrinkDimension(0);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSliceAndShrink<T>(this T[,,] inputData, Range r0, Index i1, Range r2) => inputData.GetRangeSlice(r0, i1, r2).ShrinkDimension(1);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[,] GetRangeSliceAndShrink<T>(this T[,,] inputData, Range r0, Range r1, Index i2) => inputData.GetRangeSlice(r0, r1, i2).ShrinkDimension(2);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] GetRangeSliceAndShrink<T>(this T[,,] inputData, Index i0, Index i1, Range r2) => inputData.GetRangeSlice(i0, i1, r2).ShrinkDimension(1).ShrinkDimension(0);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] GetRangeSliceAndShrink<T>(this T[,,] inputData, Range r0, Index i1, Index i2) => inputData.GetRangeSlice(r0, i1, i2).ShrinkDimension(2).ShrinkDimension(1);

    /// <summary>
    /// get sliced and shrunk array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static T[] GetRangeSliceAndShrink<T>(this T[,,] inputData, Index i0, Range r1, Index i2) => inputData.GetRangeSlice(i0, r1, i2).ShrinkDimension(2).ShrinkDimension(0);


    // ★★★★★★★★★★★★★★★ helper

    private static Range ConvertToSingleRange(this Index index, int arrayLength)
    {
        var _index = index.GetOffset(arrayLength);
        return _index..(_index + 1);
    }


    // ★★★★★★★★★★★★★★★

}
