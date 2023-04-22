using System.Linq.Expressions;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ kernels

    public class IKernel
    {

        // ★★★★★★★★★★★★★★★ props

        public DenseVector X { get; set; }
        public DenseMatrix K { get; set; }
        public DenseMatrix KInv { get; set; }
        public DenseVector KInvY { get; set; }
        public int N => X.Count;


        // ★★★★★★★★★★★★★★★ methods



        // ★★★★★★★★★★★★★★★ operators

        public static CombinedKernel operator +(IKernel left, IKernel right)
        {
            return
                new CombinedKernel
                {
                    LeftChild = left,
                    RightChild = right,
                    ChildKernelsOperator = ExpressionType.Add,
                };
        }
        public static CombinedKernel operator *(IKernel left, IKernel right)
        {
            if (left is not ConstantKernel && right is not ConstantKernel)
                throw new InvalidOperationException("Either of multiplying kernels have to be ConstantKernel!");

            return
                new CombinedKernel
                {
                    LeftChild = left,
                    RightChild = right,
                    ChildKernelsOperator = ExpressionType.Multiply,
                };
        }


        // ★★★★★★★★★★★★★★★

    }
}
