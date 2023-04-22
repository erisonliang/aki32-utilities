using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    public class ConstantKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double ConstantValue { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public ConstantKernel(double constantValue)
        {
            ConstantValue = constantValue;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            return ConstantValue;
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2, bool isSameIndex)
        {
            return 0;
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
