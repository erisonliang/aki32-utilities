using System.Text;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegression
{

    // ★★★★★★★★★★★★★★★ props

    public KernelBase Kernel { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public GaussianProcessRegression(KernelBase kernel)
    {
        Kernel = kernel;
    }


    // ★★★★★★★★★★★★★★★ methods

    public (double[] predictY, double[] sigmas) FitAndPredict(double[] X, double[] Y, double[] predictX)
    {
        var _X = new DenseVector(X);
        var _Y = new DenseVector(Y);
        var _predictX = new DenseVector(predictX);

        var _predictY = FitAndPredict(_X, _Y, _predictX);

        var mus = _predictY.predictY.ToArray();
        var sigmas = _predictY.sigmas.ToArray();

        return (mus, sigmas);
    }

    public (DenseVector predictY, DenseVector sigmas) FitAndPredict(DenseVector X, DenseVector Y, DenseVector predictX)
    {
        Fit(X, Y);
        return Predict(predictX);
    }

    public void Fit(DenseVector X, DenseVector Y)
    {
        Kernel.Fit(X, Y);
    }

    public (DenseVector predictY, DenseVector sigmas) Predict(DenseVector predictX)
    {
        return Kernel.Predict(predictX);
    }

    public void OptimizeParameters(DenseVector X, DenseVector Y,
        double tryCount = 100,
        double learning_rate = 0.05
        )
    {
        Kernel.OptimizeParameters(X, Y, tryCount, learning_rate);
    }


    // ★★★★★★★★★★★★★★★

}
