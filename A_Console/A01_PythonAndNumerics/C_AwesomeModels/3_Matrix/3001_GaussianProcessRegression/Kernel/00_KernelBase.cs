using System.Linq.Expressions;

using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ kernels

    public abstract class KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        internal DenseVector X { get; set; }
        internal DenseMatrix K { get; set; }
        internal DenseMatrix KInv { get; set; }
        internal DenseVector KInvY { get; set; }
        internal int N => X.Count;


        // ★★★★★★★★★★★★★★★ methods

        internal abstract double CalcKernel(double x1, double x2, bool isSameIndex);
        internal DenseMatrix CalcKernel(DenseVector x1, DenseVector x2)
        {
            var n1 = x1.Count;
            var n2 = x2.Count;
            var ks = new DenseMatrix(n1, n2);
            for (int i1 = 0; i1 < n1; i1++)
                for (int i2 = 0; i2 < n2; i2++)
                    ks[i1, i2] = CalcKernel(x1[i1], x2[i2], n1 == n2 && i1 == i2);
            return ks;
        }
        internal abstract double CalcGradKernel_Parameter1(double x1, double x2, bool isSameIndex);
        internal DenseMatrix CalcGradKernel_Parameter1(DenseVector x1, DenseVector x2)
        {
            var n1 = x1.Count;
            var n2 = x2.Count;
            var ks = new DenseMatrix(n1, n2);
            for (int i1 = 0; i1 < n1; i1++)
                for (int i2 = 0; i2 < n2; i2++)
                    ks[i1, i2] = CalcGradKernel_Parameter1(x1[i1], x2[i2], n1 == n2 && i1 == i2);
            return ks;
        }

        /// <summary>
        /// グラム行列（カーネル行列）などを作成します。
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        internal void Fit(DenseVector X, DenseVector Y)
        {
            this.X = X;

            // kernel
            K = CalcKernel(X, X);
            KInv = (DenseMatrix)K.Inverse();
            KInvY = KInv * Y;
        }
        /// <summary>
        /// 回帰を適用します。
        /// </summary>
        /// <param name="predictX"></param>
        /// <returns></returns>
        internal (DenseVector mu, DenseVector sig) Predict(DenseVector predictX)
        {
            if (KInv is null)
                throw new InvalidOperationException("No available fitted data found. Consider to call \"Fit()\" first!");

            var ks = CalcKernel(X, predictX);

            // 期待値 K * K^(-1) * Y
            var mus = KInvY * ks;

            // 分散 k - (K * K^(-1)) ⊙ K
            var v = KInv * ks;
            var sigmas = (DenseVector)(CalcKernel(predictX, predictX) - v.Transpose() * v).Diagonal();
            
            return (mus, sigmas);
        }

        /// <summary>
        /// ハイパーパラメーターを最適化します。
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="tryCount"></param>
        /// <param name="learning_rate"></param>
        internal abstract void OptimizeParameters(DenseVector X, DenseVector Y, double tryCount, double learning_rate);


        // ★★★★★★★★★★★★★★★ operators

        public static AddedKernel operator +(KernelBase left, KernelBase right)
        {
            return
                new AddedKernel
                {
                    LeftChild = left,
                    RightChild = right,
                };
        }
        public static MultipliedKernel operator *(KernelBase left, KernelBase right)
        {
            if (left is not ConstantKernel && right is not ConstantKernel)
                throw new InvalidOperationException("Either of multiplying kernels have to be ConstantKernel!");

            return
                new MultipliedKernel
                {
                    LeftChild = left,
                    RightChild = right,
                };
        }


        // ★★★★★★★★★★★★★★★

    }
}
