using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Auto Regressive Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class AutoRegressiveKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double Rho { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public AutoRegressiveKernel(double rho)
        {
            Rho = rho;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            throw new NotImplementedException();
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
            return $"ARK(#{Rho:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
