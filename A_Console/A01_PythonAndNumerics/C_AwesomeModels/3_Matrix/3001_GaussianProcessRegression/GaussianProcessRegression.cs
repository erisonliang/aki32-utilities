using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Numerics.LinearAlgebra.Double;

using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.PyPlotWrapper;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{

    // ★★★★★★★★★★★★★★★ props

    public KernelBase Kernel { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public GaussianProcessRegressionExecuter(KernelBase kernel)
    {
        Kernel = kernel;
    }


    // ★★★★★★★★★★★★★★★ methods

    public (double[] predictY, double[] cov) FitAndPredict(double[] X, double[] Y, double[] predictX)
    {
        var _X = new DenseVector(X);
        var _Y = new DenseVector(Y);
        var _predictX = new DenseVector(predictX);

        var result = FitAndPredict(_X, _Y, _predictX);

        var predictY = result.predictY.ToArray();
        var cov = result.cov.ToArray();

        return (predictY, cov);
    }

    public (DenseVector predictY, DenseVector cov) FitAndPredict(DenseVector X, DenseVector Y, DenseVector predictX)
    {
        Fit(X, Y);
        return Predict(predictX);
    }

    public void Fit(DenseVector X, DenseVector Y) => Kernel.Fit(X, Y);

    public (DenseVector predictY, DenseVector cov) Predict(DenseVector predictX) => Kernel.Predict(predictX);

    public void OptimizeParameters(DenseVector X, DenseVector Y,
        double tryCount = 100,
        double learning_rate = 0.05
        )
        => Kernel.OptimizeParameters(X, Y, tryCount, learning_rate);

    // ★★★★★★★★★★★★★★★ methods (static)

    public static void RunExampleModel()
    {

        static double f(double x, bool withNoise = false)
        {
            if (withNoise)
                return x * Math.Sin(x) + new Random().NextDouble() - 0.5d;
            else
                return x * Math.Sin(x);
        }

        //var X = new double[] { 1, 3, 5, 6, 7, 8 };
        //var X = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 6, 7, 8 };
        var X = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 5.2, 5.3, 5.4, 5.6, 5.8, 6, 7, 8, 8, 8, 8 };
        var Y = X.Select(x => f(x, true)).ToArray();
        var predictX = EnumerableExtension.Range_WithStep(0, 10, 0.1).ToArray();
        var correctY = predictX.Select(x => f(x)).ToArray();

        var k1 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
        var k2 = new GaussianProcessRegressionExecuter.RBFKernel(1d);
        var k3 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 30d);
        var kernel = k1 * k2 + k3;
        //var kernel = new GPR.GeneralKernel(lengthScale: 1d, noiseLambda: 1/10d);

        var gpr = new GaussianProcessRegressionExecuter(kernel);
        //gpr.OptimizeParameters(X, Y);
        (var predictY, var cov) = gpr.FitAndPredict(X, Y, predictX);
        var std = cov.Select(x => Math.Sqrt(x)).ToArray();

        new Figure
        {
            IsTightLayout = true,
            SubPlot = new SubPlot()
            {
                XLabel = "x",
                YLabel = "y",
                Title = "ガウス過程回帰",
                Plots = new List<IPlot>
                {
                    new ScatterPlot(X, Y) { MarkerSize=100, MarkerColor="g" },
                    new LinePlot(predictX, correctY) { LineColor="g"},
                    new LinePlot(predictX, predictY) { LineColor="red" },
                    new LinePlot(predictX, predictY.AddForEach(std)) { LineColor="red", LineStyle="-", Alpha=0.5 },
                    new LinePlot(predictX, predictY.SubForEach(std)) { LineColor="red", LineStyle="-", Alpha=0.5 },
                    new LinePlot(predictX, predictY.AddForEach(std.ProductForEach(2))) { LineColor="red", LineStyle="-", Alpha=0.2 },
                    new LinePlot(predictX, predictY.SubForEach(std.ProductForEach(2))) { LineColor="red", LineStyle="-", Alpha=0.2 },
                    new LinePlot(predictX, predictY.AddForEach(std.ProductForEach(3))) { LineColor="red", LineStyle="-", Alpha=0.1 },
                    new LinePlot(predictX, predictY.SubForEach(std.ProductForEach(3))) { LineColor="red", LineStyle="-", Alpha=0.1 },
                    new TextPlot(10,8,kernel.ToString()){ HorizontalAlignment="right"},
                },
            }
        }.Run(preview: true);

    }


    // ★★★★★★★★★★★★★★★

}
