using DocumentFormat.OpenXml.Math;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ inits

    public double Kernel(double x1, double x2, int t)
    {
        var d = x1 - x2;
        return Math.Exp(d * d * -t);
    }
    public double DKernel(double x1, double x2, int t)
    {
        var d = x1 - x2;
        return Math.Exp(d * d * -t) * (d * d);
    }


    ///// <summary>
    ///// グラム行列の逆行列を計算する.
    ///// </summary>
    ///// <param name="trainingDesignMatrix">訓練データの計画行列</param>
    ///// <param name="iKernel">カーネル</param>
    ///// <param name="hyperParameters">カーネルのハイパーパラメータ</param>
    ///// <param name="noiseLambda">グラム行列の逆行列計算のハイパーパラメータ</param>
    ///// <returns></returns>
    //public static DenseMatrix Fit(DenseMatrix trainingDesignMatrix, IKernel iKernel, double[] hyperParameters, double noiseLambda = 0)
    //{
    //    iKernel.SetHyperParameters(hyperParameters); //カーネルにハイパーパラメータをセットする

    //    if (noiseLambda < 0)
    //    { throw new FormatException("ノイズ λ は非負の実数です"); }

    //    DenseMatrix gramMatrix = iKernel.GramMatrixTrain(trainingDesignMatrix);
    //    for (int i = 0; i < gramMatrix.Row; i++)
    //    {
    //        gramMatrix[i, i] += noiseLambda;
    //    }

    //    return gramMatrix.Inverse();
    //}


    public (double[], double[]) FitAndPredict(double[] x, double[] y, double[] predictX,
        int t = 3,
        int beta = 30)
    {
        var _x = new DenseVector(x);
        var _y = new DenseVector(x);
        var _predictX = new DenseVector(x);

        var _predictY = FitAndPredict(_x, _y, predictX, t, beta);

        var mus = _predictY.mus.ToArray();
        var sigmas = _predictY.sigmas.ToArray();

        return (mus, sigmas);
    }

    public (DenseVector mus, DenseVector sigmas) FitAndPredict(DenseVector x, DenseVector y, DenseVector predictX,
        int t = 3,
        int beta = 30)
    {
        var N = x.Count;

        // グラム行列
        var K = new DenseMatrix(N * N);

        K.Inverse();

        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                K[i, j] = Kernel(x[i], x[j], t) + (i == j ? 1.0 / beta : 0);

        // グラム行列の逆行列 K^(-1)
        var CInv = K.Inverse();
        var a = CInv * y;

        var m = predictX.Count;
        var mus = new DenseVector(m);
        var sigmas = new DenseVector(m);
        for (int j = 0; j < m; j++)
        {
            var k = new DenseVector(N);
            for (int i = 0; i < N; i++)
                k[i] = Kernel(x[i], predictX[j], t);

            var v = CInv * k;

            // 期待値 K* x K^(-1)
            mus[j] = k.DotProduct(a);

            // 分散 k* - (K* x K^(-1)) ⊙ K*
            sigmas[j] = Kernel(predictX[j], predictX[j], t) + 1.0 / beta - v.DotProduct(v);
        }

        return (mus, sigmas);
    }




    //    export function optimize(
    //        x: Float64Array,
    //        y: Float64Array,
    //        beta: number = 30,
    //        m = 100,
    //        learning_rate: number = 0.05,
    //    ) : number {
    //    const n = x.length;
    //    let t = 1;
    //for (let k = 0; k<m; k++)
    //{
    //    const c = new Float64Array(n * n);
    //    const dc = new Float64Array(n * n);
    //    for (let i = 0; i<n; i++)
    //    {
    //        for (let j = 0; j<n; j++)
    //        {
    //            c[n * i + j] = kernel(x[i], x[j], t) + (i === j? 1.0 / beta : 0);
    //            dc[n * i + j] = dkernel(x[i], x[j], t);
    //}
    //    }
    //    const l = cholesky(c, n);
    //const a = solve(l, y);
    //const aa = new Float64Array(n * n);
    //for (let i = 0; i < n; i++)
    //{
    //    for (let j = 0; j < n; j++)
    //    {
    //        aa[n * i + j] = a[i] * a[j];
    //    }
    //}
    //const cinv = inv(l, n);
    //const m = mul(sub(aa, cinv), dc, n);
    //let tr = 0;
    //for (let i = 0; i < n; i++)
    //{
    //    tr = tr + m[n * i + i];
    //}
    //t = t + tr * learning_rate;
    //}
    //return t;
    //}





































    // ★★★★★★★★★★★★★★★ methods


    // ★★★★★★★★★★★★★★★

}
