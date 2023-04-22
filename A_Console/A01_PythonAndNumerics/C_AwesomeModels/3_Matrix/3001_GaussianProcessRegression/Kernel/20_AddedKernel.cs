using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    /// <summary>
    /// Multiple kernel combination
    /// </summary>
    /// <remarks>
    /// sk-learnの使ってる組み合わせ11個：https://datachemeng.com/kernel_design_in_gpr/
    /// </remarks>
    public class AddedKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public KernelBase LeftChild { get; set; }
        public KernelBase RightChild { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        internal AddedKernel()
        {
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2)
        {
            return LeftChild.CalcKernel(x1, x2) + RightChild.CalcKernel(x1, x2);
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2)
        {
            throw new NotImplementedException();
        }

        internal override void Fit(DenseVector X, DenseVector Y)
        {
            this.X = X;

            throw new NotImplementedException();
        }

        internal override (DenseVector mu, DenseVector sig) Predict(DenseVector predictX)
        {
            if (KInv is null)
                throw new InvalidOperationException("No available fitted data found. Consider to call \"Fit()\" first!");

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
