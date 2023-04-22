using System.Text;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ inits


    // ★★★★★★★★★★★★★★★ methods

    public double Kernel(double x1, double x2, double l)
    {
        var d = x1 - x2;
        var to = -(d * d) / (2 * l * l);
        return Math.Exp(to);
    }
    public double dKernel_dl(double x1, double x2, double l)
    {
        var d = x1 - x2;
        var to = -(d * d) / (2 * l * l);
        return to * Math.Exp(to) * (-2) / (l * l * l);

        //var d = x1 - x2;
        //return (d * d * -t) * Math.Exp(d * d * -t);
    }

    public (double[] predictY, double[] sigmas) FitAndPredict(DefaultKernel kernel, double[] X, double[] Y, double[] predictX)
    {
        var _X = new DenseVector(X);
        var _Y = new DenseVector(Y);
        var _predictX = new DenseVector(predictX);

        var _predictY = FitAndPredict(kernel, _X, _Y, _predictX);

        var mus = _predictY.predictY.ToArray();
        var sigmas = _predictY.sigmas.ToArray();

        return (mus, sigmas);
    }

    public (DenseVector predictY, DenseVector sigmas) FitAndPredict(DefaultKernel kernel, DenseVector X, DenseVector Y, DenseVector predictX)
    {
        var KInv = Fit(kernel, X);
        return Predict(kernel, X, Y, predictX, KInv);
    }

    public DenseMatrix Fit(DefaultKernel kernel, DenseVector X)
    {
        var N = X.Count;

        // K グラム行列
        var K = new DenseMatrix(N);

        // kernel
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                K[i, j] = Kernel(X[i], X[j], kernel.LengthScale);

        // noise
        for (int i = 0; i < N; i++)
            K[i, i] += kernel.NoiseLambda;

        // K^(-1)
        var KInv = K.Inverse();

        return (DenseMatrix)KInv;
    }

    public (DenseVector predictY, DenseVector sigmas) Predict(DefaultKernel kernel, DenseVector X, DenseVector Y, DenseVector predictX, DenseMatrix KInv)
    {
        var N = X.Count;

        var Ans = KInv * Y;

        var predictN = predictX.Count;
        var mus = new DenseVector(predictN);
        var sigmas = new DenseVector(predictN);
        for (int i = 0; i < predictN; i++)
        {
            var k = new DenseVector(N);
            for (int j = 0; j < N; j++)
                k[j] = Kernel(X[j], predictX[i],kernel.LengthScale);

            // 期待値 K * K^(-1)
            mus[i] = k * Ans;

            // 分散 k - (K x G^(-1)) ⊙ G
            var v = KInv * k;
            sigmas[i] = Kernel(predictX[i], predictX[i], kernel.LengthScale) + kernel.NoiseLambda - v * v;
        }

        return (mus, sigmas);
    }

    public double GetOptimizedL(DefaultKernel kernel, DenseVector X, DenseVector Y,
        double tryCount = 100,
        double learning_rate = 0.05
        )
    {
        var N = X.Count;
        double l = 1;

        for (int k = 0; k < tryCount; k++)
        {
            var K = new DenseMatrix(N);
            var dK = new DenseMatrix(N);

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    K[i, j] = Kernel(X[i], X[j], l);
                    dK[i, j] = dKernel_dl(X[i], X[j], l);
                }

            // noise
            for (int i = 0; i < N; i++)
                K[i, i] += kernel.NoiseLambda;

            // K^(-1)
            var KInv = K.Inverse();
            var Ans = KInv * Y;

            var AnsMat = new DenseMatrix(N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    AnsMat[i, j] = Ans[i] * Ans[j];

            var mm = (AnsMat - KInv).Multiply(dK);
            double tr = 0;
            for (int i = 0; i < N; i++)
                tr += mm[i, i];
            l += tr * learning_rate;
        }

        return l;

    }


    // ★★★★★★★★★★★★★★★

}
