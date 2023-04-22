using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Constant Kernel
    /// </summary>
    public class ConstantKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double ConstantWeight { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public ConstantKernel(double constantValue)
        {
            ConstantWeight = constantValue;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            return ConstantWeight;
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            return 0;
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
            return $"CK({ConstantWeight:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
