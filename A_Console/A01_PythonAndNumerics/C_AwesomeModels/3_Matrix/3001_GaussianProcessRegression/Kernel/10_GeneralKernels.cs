

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    public static class GeneralKernels
    {
        // ★★★★★★★★★★★★★★★ methods (static)

        /// <summary>
        /// Super basic one
        /// </summary>
        public static KernelBase GeneralKernel(double constantWeight, double lengthScale, double noiseLambda)
            => new ConstantKernel(constantWeight) * new SquaredExponentialKernel(lengthScale) + new WhiteNoiseKernel(noiseLambda);


        // ★★★★★★★★★★★★★★★ 

    }
}
