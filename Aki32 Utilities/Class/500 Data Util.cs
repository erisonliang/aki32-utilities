using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Class;
internal static class DataUtil
{

    // ★★★★★★★★★★★★★★★ 511 Transpose2DArray

    /// <summary>
    /// Transpose 2D Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputData"></param>
    /// <returns></returns>
    internal static T[][] Transpose2DArray<T>(this T[][] inputData)
    {
        // main
        var columnCount = inputData.Length;
        var rowCount = inputData.Max(x => x.Length);
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
                    line[j] = default;
                }
            }
            outputData[i] = line;
        }

        return outputData;
    }

    // ★★★★★★★★★★★★★★★ 

}
