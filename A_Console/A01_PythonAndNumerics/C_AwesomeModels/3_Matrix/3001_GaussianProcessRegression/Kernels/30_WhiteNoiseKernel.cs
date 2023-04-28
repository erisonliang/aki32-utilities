﻿using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// White Noise Kernel
    /// </summary>
    public class WhiteNoiseKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public HyperParameter NoiseLambda { get; private set; }


        // ★★★★★★★★★★★★★★★ inits

        public WhiteNoiseKernel(double noiseLambda, bool fixNoiseLambda = false)
        {
            NoiseLambda = new HyperParameter(nameof(NoiseLambda), noiseLambda, fixNoiseLambda, double.Epsilon, double.MaxValue);

            HyperParameters = new string[]
            {
                nameof(NoiseLambda),
            };
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// K = δ²
        /// </summary>
        internal override DenseMatrix CalcKernel(DenseVector X1, DenseVector X2)
        {
            if (X1.Count == X2.Count)
                return NoiseLambda * DenseMatrix.CreateIdentity(X1.Count);
            return DenseMatrix.Create(X1.Count, X2.Count, 0);
        }

        internal override DenseMatrix CalcKernelGrad(DenseVector X1, DenseVector X2, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(NoiseLambda) => CalcKernelGrad_NoiseLambda(X1, X2),
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return CalcKernel(X1, X2);
            }
        }

        /// <summary>
        /// dK/dδ = 2δ
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <returns></returns>
        internal DenseMatrix CalcKernelGrad_NoiseLambda(DenseVector X1, DenseVector X2)
        {
            if (NoiseLambda.IsFixed)
                return DenseMatrix.Create(X1.Count, X2.Count, 0);
            if (X1.Count == X2.Count)
                return 2 * Math.Sqrt(NoiseLambda) * DenseMatrix.CreateIdentity(X1.Count);
            return DenseMatrix.Create(X1.Count, X2.Count, 0);
        }

        internal override double? GetParameterValue((Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                return targetParameter.Item2 switch
                {
                    nameof(NoiseLambda) => NoiseLambda,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
            else
            {
                return null;
            }
        }
        internal override void SetParameterValue((Guid, string) targetParameter, double settingValue)
        {
            if (targetParameter.Item1 == KernelID)
            {
                _ = targetParameter.Item2 switch
                {
                    nameof(NoiseLambda) => NoiseLambda.Value = settingValue,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
        }

        public override string ToString()
        {
            return $"δ({NoiseLambda.Value:F3})";
        }

        public override string ToInitialStateString()
        {
            return $"δ({NoiseLambda.InitialValue:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
