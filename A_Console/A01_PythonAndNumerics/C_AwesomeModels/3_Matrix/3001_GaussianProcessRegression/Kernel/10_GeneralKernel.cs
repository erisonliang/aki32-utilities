using System.Linq.Expressions;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    /// <summary>
    /// ConstantKernel() * RBFKernel() + WhiteNoiseKernel()
    /// </summary>
    public class GeneralKernel : IKernel
    {
        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }
        public double NoiseLambda { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public GeneralKernel(double lengthScale, double noiseLambda)
        {
            LengthScale = lengthScale;
            NoiseLambda = noiseLambda;
        }


        // ★★★★★★★★★★★★★★★ methods

        private double CalcKernel(double x1, double x2)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return Math.Exp(to);
        }

        private DenseVector CalcKernel(DenseVector x1, double x2)
        {
            var n1 = x1.Count;
            var ks = new DenseVector(n1);
            for (int i = 0; i < n1; i++)
                ks[i] = CalcKernel(x1[i], x2);
            return ks;
        }

        private DenseMatrix CalcKernel(DenseVector x1, DenseVector x2)
        {
            var n1 = x1.Count;
            var n2 = x2.Count;
            var ks = new DenseMatrix(n1, n2);
            for (int i = 0; i < n2; i++)
                ks.SetColumn(i, CalcKernel(x1, x2[i]));
            return ks;
        }

        private double dKernel_dl(double x1, double x2)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return -2 * to * Math.Exp(to) / Math.Pow(LengthScale, 3);
        }

        /// <summary>
        /// グラム行列（カーネル行列）を作成します。
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        internal void Fit(DenseVector X, DenseVector Y)
        {
            this.X = X;

            // K グラム行列
            K = new DenseMatrix(N);

            // kernel
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    K[i, j] = CalcKernel(X[i], X[j]);

            // noise
            for (int i = 0; i < N; i++)
                K[i, i] += NoiseLambda;

            KInv = (DenseMatrix)K.Inverse();
            KInvY = KInv * Y;
        }

        internal (DenseVector mu, DenseVector sig) Predict(DenseVector predictX)
        {
            if (KInv is null)
                throw new InvalidOperationException("No available fitted data found. Consider to call \"Fit()\" first!");

            var predictN = predictX.Count;
            var mus = new DenseVector(predictN);
            var sigmas = new DenseVector(predictN);

            for (int i = 0; i < predictN; i++)
            {
                var ks = CalcKernel(X, predictX[i]);

                // 期待値 K * K^(-1)
                mus[i] = ks * KInvY;

                // 分散 k - (K x G^(-1)) ⊙ G
                var v = KInv * ks;
                sigmas[i] = CalcKernel(predictX[i], predictX[i]) + NoiseLambda - v * v;
            }

            return (mus, sigmas);
        }

        internal void OptimizeParameters(DenseVector X, DenseVector Y,
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
                        K[i, j] = CalcKernel(X[i], X[j]);
                        dK[i, j] = dKernel_dl(X[i], X[j]);
                    }

                // noise
                for (int i = 0; i < N; i++)
                    K[i, i] += NoiseLambda;

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
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
