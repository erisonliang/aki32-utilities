

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class DynamicsUtils
{

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// make data length power of 2.
    /// </summary>
    /// <param name="inputData"></param>
    /// <param name="downSample">Cut inputData to down-sample, or append 0 to up-sample</param>
    /// <param name="N">in case you want to know, return N</param>
    /// <exception cref="InvalidDataException"></exception>
    /// <returns></returns>
    public static double[] MakeDataLengthPow2(double[] inputData, bool downSample, out int N)
    {
        // 2の累乗にカットして複素数データに変換
        double len = inputData.Length;
        int Np = 0;
        while (len > 1)
        {
            Np++;
            len /= 2;
        }
        Np--;

        if (downSample)
        {
            N = (int)Math.Pow(2, Np); // サンプル数
            return inputData.Take(N).ToArray();
        }
        else
        {
            N = (int)Math.Pow(2, Np + 1); // サンプル数
            var appending = new double[N - inputData.Length];
            var inputDataList = inputData.ToList();
            inputDataList.AddRange(appending);
            return inputDataList.ToArray();
        }
    }




    // ★★★★★★★★★★★★★★★

}
