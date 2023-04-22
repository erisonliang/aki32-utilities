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

    public static void RunExampleModel(FileInfo? outputImageFile = null, bool preview = true)
    {
        outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

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
        var k2 = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
        var k3 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
        var kernel = k1 * k2 + k3;

        var gpr = new GaussianProcessRegressionExecuter(kernel);
        //gpr.OptimizeParameters(X, Y);
        (var predictY, var cov) = gpr.FitAndPredict(X, Y, predictX);
        var std1 = cov.Select(x => Math.Sqrt(x)).ToArray();
        var CI95 = std1.ProductForEach(1.96);
        var CI99 = std1.ProductForEach(2.58);

        new Figure
        {
            IsTightLayout = true,
            SubPlot = new SubPlot()
            {
                XLabel = "X",
                YLabel = "f(X)",
                Title = "ガウス過程回帰",
                LegendLocation = LegendLocation.upper_left,
                LegendFontSize = 20,
                Plots = new List<IPlot>
                {
                    // new ScatterPlot(Array.Empty<double>(),Array.Empty<double>()){ MarkerColor="k", MarkerSize=100, LegendLabel=kernel.ToString()},

                    new LinePlot(predictX, correctY) { LineColor="g", LineWidth=3, LegendLabel="True f(X)= Xsin(X)"},
                    new ScatterPlot(X, Y) { MarkerSize=130, MarkerColor="g", LegendLabel="Observed data"},

                    new LinePlot(predictX, predictY) { LineColor="red", LineWidth=3, LegendLabel=kernel.ToString()},

                    new FillBetweenPlot(predictX, predictY.AddForEach(CI95), predictY.SubForEach(CI95)) {FillColor="red", Alpha=0.20, LegendLabel="95% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(CI95)) { LineColor="red", LineStyle="-", Alpha=0.20 },
                    //new LinePlot(predictX, predictY.SubForEach(CI95)) { LineColor="red", LineStyle="-", Alpha=0.20 },

                    new FillBetweenPlot(predictX, predictY.AddForEach(CI99), predictY.SubForEach(CI99)) {FillColor="red", Alpha=0.10, LegendLabel="99% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(CI99)) { LineColor="red", LineStyle="-", Alpha=0.10 },
                    //new LinePlot(predictX, predictY.SubForEach(CI99)) { LineColor="red", LineStyle="-", Alpha=0.10 },

                    //new TextPlot(10,8,kernel.ToString()){ HorizontalAlignment="right"},
                
                },
            }
        }.Run(outputImageFile, preview);

    }


    // ★★★★★★★★★★★★★★★

}
