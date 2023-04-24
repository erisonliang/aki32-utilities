using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination with addition
    /// </summary>
    public class AdditionOperationKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public KernelBase LeftChild { get; set; }
        public KernelBase RightChild { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        internal AdditionOperationKernel()
        {

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            return LeftChild.CalcKernel(m1, m2) + RightChild.CalcKernel(m1, m2);
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
            return $"{LeftChild.ToString()}+{RightChild.ToString()}";
        }


        // ★★★★★★★★★★★★★★★

    }
}
