using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Linear Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class LinearKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double C { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public LinearKernel(double c)
        {
            C = c;

            HyperParameters = new string[]
            {
                nameof(C),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector v1, DenseVector v2)
        {
            var K = new DenseMatrix(v1.Count, v2.Count);

            for (int i1 = 0; i1 < v1.Count; i1++)
            {
                for (int i2 = 0; i2 < v2.Count; i2++)
                {
                    K[i1, i2] = (v1[i1] - C) * (v2[i2] - C);
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
                    nameof(C) => CalcKernelGrad_C(v1, v2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(v1, v2);
            }
        }

        internal DenseMatrix CalcKernelGrad_C(DenseVector v1, DenseVector v2)
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
            return $"L({C:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
