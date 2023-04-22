using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ inits

    // ★★★★★★★★★★★★★★★ kernels

    public class RBFKernel
    {



    }


    // ★★★★★★★★★★★★★★★ methods

    public double Kernel(double x1, double x2, double t)
    {
        var d = x1 - x2;
        return Math.Exp(d * d * -t);
    }
    public double dKernel(double x1, double x2, double t)
    {
        var d = x1 - x2;
        return (d * d * -t) * Math.Exp(d * d * -t);


        //var d = x1 - x2;
        //return (d * d * -t) * Math.Exp(d * d * -t);
    }

    public (double[] predictY, double[] sigmas) FitAndPredict(double[] X, double[] Y, double[] predictX,
        double t = 3,
        double noiseLambda = 1 / 30d
        )
    {
        var _X = new DenseVector(X);
        var _Y = new DenseVector(Y);
        var _predictX = new DenseVector(predictX);

        var _predictY = FitAndPredict(_X, _Y, _predictX, t, noiseLambda);

        var mus = _predictY.predictY.ToArray();
        var sigmas = _predictY.sigmas.ToArray();

        return (mus, sigmas);
    }

    public (DenseVector predictY, DenseVector sigmas) FitAndPredict(DenseVector X, DenseVector Y, DenseVector predictX,
        double t = 3,
        double noiseLambda = 1 / 30d
        )
    {
        var KInv = Fit(X, t, noiseLambda);
        return Predict(X, Y, predictX, KInv, t, noiseLambda);
    }

    public DenseMatrix Fit(DenseVector X,
        double t = 3,
        double noiseLambda = 1 / 30d
        )
    {
        var N = X.Count;

        // K グラム行列
        var K = new DenseMatrix(N);

        // kernel
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                K[i, j] = Kernel(X[i], X[j], t);

        // noise
        for (int i = 0; i < N; i++)
            K[i, i] += noiseLambda;

        // K^(-1)
        var KInv = K.Inverse();

        return (DenseMatrix)KInv;
    }

    public (DenseVector predictY, DenseVector sigmas) Predict(DenseVector X, DenseVector Y, DenseVector predictX, DenseMatrix KInv,
       double t = 3,
       double noiseLambda = 1 / 30d
       )
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
                k[j] = Kernel(X[j], predictX[i], t);

            // 期待値 K * K^(-1)
            mus[i] = k * Ans;

            // 分散 k - (K x G^(-1)) ⊙ G
            var v = KInv * k;
            sigmas[i] = Kernel(predictX[i], predictX[i], t) + noiseLambda - v * v;
        }

        return (mus, sigmas);
    }

    public double GetOptimizedT(DenseVector X, DenseVector Y,
        double noiseLambda = 1 / 30d,
        double tryCount = 100,
        double learning_rate = 0.05
        )
    {
        var N = X.Count;
        double t = 1;

        for (int k = 0; k < tryCount; k++)
        {
            var K = new DenseMatrix(N);
            var dK = new DenseMatrix(N);

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    K[i, j] = Kernel(X[i], X[j], t);
                    dK[i, j] = dKernel(X[i], X[j], t);
                }

            // noise
            for (int i = 0; i < N; i++)
                K[i, i] += noiseLambda;

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
            t += tr * learning_rate;
        }

        return t;

    }


    // ★★★★★★★★★★★★★★★

}
