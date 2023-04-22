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








        // ★★★★★★★★★★★★★★★

    }
}
