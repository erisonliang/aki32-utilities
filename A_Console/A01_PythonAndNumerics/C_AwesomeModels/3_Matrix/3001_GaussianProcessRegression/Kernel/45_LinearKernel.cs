using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Linear Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class LinearKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double C { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public LinearKernel(double c)
        {
            C = c;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            return (x1 - C) * (x2 - C);
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
            return $"LK({C:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
