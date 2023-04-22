using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    public class RBFKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public RBFKernel(double lengthScale)
        {
            LengthScale = lengthScale;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return Math.Exp(to);
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2, bool isSameIndex)
        {
            throw new NotImplementedException();
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {





        }


        // ★★★★★★★★★★★★★★★

    }
}
