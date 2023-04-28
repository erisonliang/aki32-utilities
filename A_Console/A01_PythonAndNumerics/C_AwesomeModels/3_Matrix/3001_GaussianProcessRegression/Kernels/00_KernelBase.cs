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

        internal Guid KernelID { get; set; } = Guid.NewGuid();
        internal HyperParameter[] HyperParameters { get; set; } = Array.Empty<HyperParameter>();


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// カーネルの演算結果を取得します。
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <returns></returns>
        internal abstract DenseMatrix CalcKernel(DenseVector X1, DenseVector X2);

        /// <summary>
        /// カーネルの微分演算結果を取得します。
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="targetParameter"></param>
        /// <returns></returns>
        internal abstract DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, HyperParameter targetParameter);

        /// <summary>
        /// グラム行列（カーネル行列）などを作成します。
        /// </summary>
        /// <param name="XTrain"></param>
        /// <returns></returns>
        internal void Fit(DenseVector XTrain, DenseVector Y_train)
        {
            this.X = XTrain;

            // kernel
            Ktt = CalcKernel(XTrain, XTrain);
            Ktt_Inv = (DenseMatrix)Ktt.Inverse();
            Ktt_Inv_Y = Ktt_Inv * Y_train;

        }

        /// <summary>
        /// 回帰を適用します。
        /// </summary>
        /// <param name="X_predict"></param>
        /// <returns></returns>
        internal (DenseVector Y_predict, DenseVector cov) Predict(DenseVector X_predict)
        {
            if (Ktt_Inv is null)
                throw new InvalidOperationException("No available fitted data found. Consider to call \"Fit()\" first!");

            var Ktp = CalcKernel(X, X_predict);
            var Kpp = CalcKernel(X_predict, X_predict);

            var Y_predict = (DenseVector)(Ktp.Transpose() * Ktt_Inv_Y);
            var cov = (DenseVector)(Kpp - Ktp.Transpose() * Ktt_Inv * Ktp).Diagonal();

            return (Y_predict, cov);

        }

        /// <summary>
        /// カーネルを表現する文字列を返します。
        /// </summary>
        public new abstract string ToString();

        /// <summary>
        /// 自身と全ての子カーネルを取得します。
        /// </summary>
        /// <returns></returns>
        internal KernelBase[] GetAllChildrenKernelsAndSelf()
        {
            var children = new List<KernelBase>();

            void ProcessKernelRecursive(KernelBase k)
            {
                if (k is OperationKernelBase ok)
                {
                    ProcessKernelRecursive(ok.LeftChild);
                    ProcessKernelRecursive(ok.RightChild);
                }
                else
                {
                    children.Add(k);
                }
            }

            ProcessKernelRecursive(this);
            return children.ToArray();
        }

        public abstract string ToInitialStateString();


        // ★★★★★★★★★★★★★★★ operators

        public static AdditionOperationKernel operator +(KernelBase left, KernelBase right)
        {
            return
                new AdditionOperationKernel
                {
                    LeftChild = left,
                    RightChild = right,
                };
        }
        public static MultiplicationOperationKernel operator *(KernelBase left, KernelBase right)
        {
            if (left is not ConstantKernel && right is not ConstantKernel)
                throw new InvalidOperationException("Either of multiplying kernels have to be ConstantKernel!");

            return
                new MultiplicationOperationKernel
                {
                    LeftChild = left,
                    RightChild = right,
                };
        }


        // ★★★★★★★★★★★★★★★

    }
}
