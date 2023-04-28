

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    public static class GeneralKernels
    {
        // ★★★★★★★★★★★★★★★ methods (static)

        /// <summary>
        /// super basic one
        /// </summary>
        public static KernelBase GeneralKernel001(double constantWeight, double lengthScale, double noiseLambda)
            => new ConstantKernel(constantWeight) * new SquaredExponentialKernel(lengthScale)
            + new WhiteNoiseKernel(noiseLambda);

        /// <summary>
        /// super basic one
        /// </summary>
        public static KernelBase GeneralKernel002(double constantWeight1, double lengthScale, double constantWeight2, double linearWeight, double noiseLambda)
            => new ConstantKernel(constantWeight1) * new SquaredExponentialKernel(lengthScale)
            + new ConstantKernel(constantWeight2) * new LinearKernel(linearWeight)
            + new WhiteNoiseKernel(noiseLambda);


        // ★★★★★★★★★★★★★★★ 

    }
}
