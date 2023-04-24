﻿using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Auto Regressive Kernel
    /// <br/> * without σ² in front
    /// </summary>
    public class AutoRegressiveKernel : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public double Rho { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        public AutoRegressiveKernel(double rho)
        {
            Rho = rho;

            HyperParameters = new string[]
            {
                nameof(Rho),
            };

        }


        // ★★★★★★★★★★★★★★★ methods

        internal override DenseMatrix CalcKernel(DenseVector m1, DenseVector m2)
        {
            var K = new DenseMatrix(m1.Count, m2.Count);

            for (int i1 = 0; i1 < m1.Count; i1++)
            {
                for (int i2 = 0; i2 < m2.Count; i2++)
                {
                    throw new NotImplementedException();




                }
            }

            return K;
        }

        internal DenseMatrix CalcKernelGrad_Rho(DenseVector m1, DenseVector m2)
        {

            var K = new DenseMatrix(m1.Count, m2.Count);

            for (int i1 = 0; i1 < m1.Count; i1++)
            {
                for (int i2 = 0; i2 < m2.Count; i2++)
                {
                    throw new NotImplementedException();



                }
            }

            return K;
        }

        internal override void OptimizeParameters(DenseVector X, DenseVector Y,
          double tryCount = 100,
          double learning_rate = 0.05
          )
        {
            throw new NotImplementedException();





        }

        public override string ToString()
        {
            return $"AR({Rho:F3})";
        }


        // ★★★★★★★★★★★★★★★

    }
}
