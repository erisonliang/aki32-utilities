using System.Linq.Expressions;

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
    public class CombinedKernel : IKernel
    {

        // ★★★★★★★★★★★★★★★ props

        public IKernel? LeftChild { get; set; }
        public IKernel? RightChild { get; set; }
        public ExpressionType ChildKernelsOperator { get; set; }
        
        
        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2)
        {
            throw new NotImplementedException();
        }

        internal override double CalcGradKernel_Parameter1(double x1, double x2)
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
