using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Periodic Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class PeriodicKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }
        public double P { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public PeriodicKernel(double lengthScale, double p)
        {
            LengthScale = lengthScale;
            P = p;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var d = Math.Abs(x1 - x2);
            var to = -2 * Math.Pow(Math.Sin(Math.PI * d / P) / LengthScale, 2);
            return Math.Exp(to);
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            throw new NotImplementedException();



        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {
            throw new NotImplementedException();





        }

        public override string ToString()
        {
            return $"PK(#{LengthScale:F3}, {P:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}