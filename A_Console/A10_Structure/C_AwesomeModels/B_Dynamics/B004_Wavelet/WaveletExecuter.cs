using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class WaveletExecuter
{
    /// <summary>
    /// Daubechies係数 (*√2)
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Daubechies_wavelet#The_scaling_sequences_of_lowest_approximation_order"/>
    private static double[][] DaubechiesCoefficientsBaseArray => new double[][]
    {
        new double[]{ 1, 1 },
        new double[]{ 0.6830127,1.1830127,0.3169873,-0.1830127 },
        new double[]{ 0.47046721,1.14111692,0.650365,-0.19093442,-0.12083221,0.0498175 },
        new double[]{ 0.32580343,1.01094572,0.8922014,-0.03957503, -0.26450717,0.0436163,0.0465036,-0.01498699 },
        new double[]{ 0.22641898,0.85394354,1.02432694,0.19576696, -0.34265671,-0.04560113,0.10970265,-0.0088268, -0.01779187,4.72E-03 },
    };
    /// <summary>
    /// Daubechies係数
    /// </summary>
    private static DenseMatrix DaubechiesCoefficientsMatrix
        => (DenseMatrix)(new DenseMatrix(10, 5, DaubechiesCoefficientsBaseArray.ConvertTo2DArray().ReShape()) / Math.Sqrt(2));
    /// <summary>
    /// 引数Dに入力できる数値
    /// </summary>
    private static readonly int[] AcceptingD = new int[] { 2, 4, 6, 8, 10 };

    /// <summary>
    /// DaubechiesWavelet変換。
    /// </summary>
    /// <see cref="https://truthfullscore.hatenablog.com/entry/2014/02/05/204055"/>
    /// <param name="inputData"></param>
    public static TimeHistory Execute_DaubechiesWavelet(double[] inputData,
         int maxLevel = 2,
         int D = 2
        )
    {
        // ★ 諸元
        if (!AcceptingD.Contains(D))
            throw new Exception($"D must be any of ({string.Join(", ", AcceptingD)})");
        var usingDaubechiesCoefficients = DaubechiesCoefficientsMatrix.Column(D / 2 - 1);

        var extendedInputData = DynamicsUtils.MakeDataLengthPow2(inputData, false, out int totalLength);
        var resultData = new double[totalLength];

        var resultHistory = new TimeHistory();
        resultHistory.PaddingValue = double.NaN;


        // ★ main
        for (int level = 1; level <= maxLevel; level++)
        {
            int scale = 1;
            for (int i = 0; i < level; i++)
                scale *= 2;

            //ダウンサンプリング
            for (int i = 0; i < totalLength / scale; i++)
            {
                int setApproximationIndex = i;
                int setDetailIndex = i + totalLength / scale;

                resultData[setApproximationIndex] = 0;
                resultData[setDetailIndex] = 0;

                for (int k = 0; k < D; k++)
                {
                    int getInputIndex = 2 * i + k;

                    if (getInputIndex >= 2 * totalLength / scale)
                    {
                        //配列外にデータにアクセスする際に折り返しを行う
                        int over = getInputIndex % (2 * totalLength / scale) + 1;
                        getInputIndex = (2 * totalLength / scale) - over;
                    }

                    //フィルタリング
                    resultData[setApproximationIndex] += extendedInputData[getInputIndex] * usingDaubechiesCoefficients[k];
                    resultData[setDetailIndex] += extendedInputData[getInputIndex] * Math.Pow(-1.0, k) * usingDaubechiesCoefficients[D - k - 1];
                }
            }

            // ★ 配列コピー
            Array.Copy(resultData, extendedInputData, totalLength);

            // ★ 結果出力
            for (int i = 0; i < totalLength / scale; i++)
            {
                var step = resultHistory.GetStep(i);

                step[$"Lv{level}.Approximation"] = resultData[i];
                step[$"Lv{level}.Detail"] = resultData[i + totalLength / scale];

                resultHistory.SetStep(i, step);
            }
        }

        return resultHistory;
    }
}