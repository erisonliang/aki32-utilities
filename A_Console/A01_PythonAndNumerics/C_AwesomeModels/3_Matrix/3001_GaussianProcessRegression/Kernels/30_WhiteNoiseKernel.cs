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

        public double NoiseLambda { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public WhiteNoiseKernel(double noiseLambda)
        {
            NoiseLambda = noiseLambda;

            HyperParameters = new string[]
            {
                nameof(NoiseLambda),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

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

        internal DenseMatrix CalcKernelGrad_NoiseLambda(DenseVector X1, DenseVector X2)
        {
            return DenseMatrix.Create(X1.Count, X2.Count, 0);
        }

        internal override void AddValueToParameter(double addingValue, (Guid, string) targetParameter)
        {
            if (targetParameter.Item1 == KernelID)
            {
                _ = targetParameter.Item2 switch
                {
                    nameof(NoiseLambda) => NoiseLambda += addingValue,
                    _ => throw new InvalidOperationException("No such parameter found in this kernel."),
                };
            }
        }

        public override string ToString()
        {
            return $"δ({NoiseLambda:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
