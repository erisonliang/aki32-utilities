using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    /// <summary>
    /// ConstantKernel() * RBFKernel() + WhiteNoiseKernel()
    /// </summary>
    public class GeneralKernel : KernelBase
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

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var noise = isSameIndex ? NoiseLambda : 0;
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return Math.Exp(to) + noise;
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return -2 * to * Math.Exp(to) / Math.Pow(LengthScale, 3);
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
            double tryCount = 100,
            double learning_rate = 0.05
            )
        {
            this.X = X;

            for (int k = 0; k < tryCount; k++)
            {
                var K = CalcKernel(X, X);
                var KInv = (DenseMatrix)K.Inverse();
                var KInvY = KInv * Y;

                var AnsMat = KInvY.ToColumnMatrix() * KInvY.ToRowMatrix();

                var dK = CalcGradKernel_Parameter1(X, X);
                var mm = (AnsMat - KInv).Multiply(dK);
                double tr = 0;
                for (int i = 0; i < N; i++)
                    tr += mm[i, i];
                LengthScale += tr * learning_rate;
            }

        }


        // ★★★★★★★★★★★★★★★ 

    }
}
