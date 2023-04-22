using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    /// <summary>
    /// Multiple kernel combination
    /// </summary>
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

        /// <summary>
        /// カーネルの演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="isSameIndex"></param>
        /// <returns></returns>
        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            return LeftChild.CalcKernel(x1, x2, isSameIndex) + RightChild.CalcKernel(x1, x2, isSameIndex);
        }

        /// <summary>
        /// カーネルの微分演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="isSameIndex"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
            return $"{LeftChild.ToString()} + {RightChild.ToString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
