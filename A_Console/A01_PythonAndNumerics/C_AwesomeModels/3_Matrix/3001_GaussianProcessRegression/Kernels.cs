using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ kernels

    public interface IKernel
    {
        public double Evaluate(double x);
    }

    public class DefaultKernel
    {
        public double LengthScale { get; set; }
        public double NoiseLambda { get; set; }

        public DefaultKernel(double lengthScale, double noiseLambda)
        {
            LengthScale = lengthScale;
            NoiseLambda = noiseLambda;
        }

        public double Kernel(double x1, double x2, double l)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / l, 2) / 2;
            return Math.Exp(to);
        }

        public double dKernel_dl(double x1, double x2, double l)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / l, 2) / 2;
            return -2 * to * Math.Exp(to) / Math.Pow(l, 3);
        }

    }



    public class RBFKernel
    {
        public double LengthScale { get; set; }

        public double Kernel(double x1, double x2, double l)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / l, 2) / 2;
            return Math.Exp(to);
        }

        public double dKernel_dl(double x1, double x2, double l)
        {
            var d = x1 - x2;
            var to = -Math.Pow(d / l, 2) / 2;
            return -2 * to * Math.Exp(to) / Math.Pow(l, 3);
        }

    }


    // ★★★★★★★★★★★★★★★

}
