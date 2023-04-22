﻿using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{
    /// <summary>
    /// ConstantKernel() * RBFKernel() + WhiteNoiseKernel()
    /// </summary>
    public class GeneralKernel : KernelBase
    {
        // ★★★★★★★★★★★★★★★ props

        public double LengthScale { get; set; }
        public double NoiseLambda { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public GeneralKernel(double lengthScale, double noiseLambda)
        {
            LengthScale = lengthScale;
            NoiseLambda = noiseLambda;
        }


        // ★★★★★★★★★★★★★★★ methods

        internal override double CalcKernel(double x1, double x2, bool isSameIndex)
        {
            var noise = isSameIndex ? NoiseLambda : 0;
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return Math.Exp(to) + noise;
        }

        internal override double CalcKernelGrad_Parameter1(double x1, double x2, bool isSameIndex)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / LengthScale, 2) / 2;
            return -2 * to * Math.Exp(to) / Math.Pow(LengthScale, 3);
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
            double tryCount = 100,
            double learning_rate = 0.05
            )
        {
            for (int k = 0; k < tryCount; k++)
            {
                Fit(X, Y);

                var KInvYMat = KInvY.ToColumnMatrix() * KInvY.ToRowMatrix();

                var dK = CalcKernelGrad_Parameter1(X, X);
                var mm = (KInvYMat - KInv).Multiply(dK);
                double tr = mm.Diagonal().Sum();
                LengthScale += tr * learning_rate;
            }
        }

        public override string ToString()
        {
            return $"GeneralKernel(LengthScale={LengthScale:F3}, NoiseLambda={NoiseLambda:F3})";
        }


        // ★★★★★★★★★★★★★★★ 

    }
}