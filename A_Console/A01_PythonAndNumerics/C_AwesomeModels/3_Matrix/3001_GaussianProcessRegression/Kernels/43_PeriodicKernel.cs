using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Periodic Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class PeriodicKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }
        public double P { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public PeriodicKernel(double lengthScale, double p)
        {
            LengthScale = lengthScale;
            P = p;

            HyperParameters = new string[]
            {
                nameof(LengthScale),
                nameof(P),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            var K = new DenseMatrix(m1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    var d = Math.Abs(m1[i1] - v2[i2]);
                    var to = -2 * Math.Pow(Math.Sin(Math.PI * d / P) / LengthScale, 2);
                    K[i1, i2] = Math.Exp(to);
                }
            }

            return K;
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector v1, DenseVector v2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(LengthScale) => CalcKernelGrad_LengthScale(v1, v2),
                    nameof(P) => CalcKernelGrad_P(v1, v2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(v1, v2);
            }
        }

        internal DenseMatrix CalcKernelGrad_LengthScale(DenseVector v1, DenseVector v2)
        {

            var K = new DenseMatrix(v1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_P(DenseVector v1, DenseVector v2)
        {

            var K = new DenseMatrix(v1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        public override string ToString()
        {
            return $"P({LengthScale:F3}, {P:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}