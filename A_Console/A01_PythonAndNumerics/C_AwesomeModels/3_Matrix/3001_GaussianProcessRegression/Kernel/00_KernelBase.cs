using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// All Kernels Base
    /// </summary>
    public abstract class KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        internal DenseVector X { get; set; }
        /// <summary>
        /// Ktt → K-train-train
        /// Ktp → K-train-predict
        /// Kpp → K-predict-predict
        /// </summary>
        internal DenseMatrix Ktt { get; set; }
        internal DenseMatrix Ktt_Inv { get; set; }
        internal DenseVector Ktt_Inv_Y { get; set; }
        internal int N => X.Count;


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// カーネルの演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="isSameIndex"></param>
        /// <returns></returns>
        internal abstract double CalcKernel(double x1, double x2, bool isSameIndex);
        /// <summary>
        /// カーネルの演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// カーネルの微分演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="isSameIndex"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal abstract double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex);
        /// <summary>
        /// カーネルの微分演算を表します。
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal DenseMatrix CalcKernelGrad_Parameter1(DenseVector x1, DenseVector x2)
        {
            var n1 = x1.Count;
            var n2 = x2.Count;
            var ks = new DenseMatrix(n1, n2);
            for (int i1 = 0; i1 < n1; i1++)
                for (int i2 = 0; i2 < n2; i2++)
                    ks[i1, i2] = CalcKernelGrad_Parameter1(x1[i1], x2[i2], n1 == n2 && i1 == i2);
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
            Ktt = CalcKernel(X, X);
            Ktt_Inv = (DenseMatrix)Ktt.Inverse();
            Ktt_Inv_Y = Ktt_Inv * Y;

        }
        /// <summary>
        /// 回帰を適用します。
        /// </summary>
        /// <param name="predictX"></param>
        /// <returns></returns>
        internal (DenseVector predictY, DenseVector cov) Predict(DenseVector predictX)
        {
            if (Ktt_Inv is null)
                throw new InvalidOperationException("No available fitted data found. Consider to call \"Fit()\" first!");

            var Ktp = CalcKernel(X, predictX);
            var Kpp = CalcKernel(predictX, predictX);

            var predictY = (DenseVector)(Ktp.Transpose() * Ktt_Inv_Y);
            var cov = (DenseVector)(Kpp - Ktp.Transpose() * Ktt_Inv * Ktp).Diagonal();

            return (predictY, cov);

        }

        /// <summary>
        /// ハイパーパラメーターを最適化します。
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="tryCount"></param>
        /// <param name="learning_rate"></param>
        internal abstract void OptimizeParameters(DenseVector X, DenseVector Y, double tryCount, double learning_rate);

        /// <summary>
        /// カーネルを表現する文字列を返します。
        /// </summary>
        public new abstract string ToString();


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
