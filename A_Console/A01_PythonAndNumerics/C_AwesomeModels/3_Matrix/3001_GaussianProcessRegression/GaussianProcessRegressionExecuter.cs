using System.ComponentModel.Design.Serialization;

using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Numerics.LinearAlgebra.Double;

using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.PyPlotWrapper;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{

    // ★★★★★★★★★★★★★★★ props

    public KernelBase Kernel { get; set; }

    internal List<(Guid, string)> HyperParameters { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public GaussianProcessRegressionExecuter(KernelBase kernel)
    {
        Kernel = kernel;
        HyperParameters = new List<(Guid, string)>();

        void ProcessKernelRecursive(KernelBase k)
        {
            if (k is OperationKernelBase ok)
            {
                ProcessKernelRecursive(ok.LeftChild);
                ProcessKernelRecursive(ok.RightChild);
            }
            else
            {
                HyperParameters.AddRange(k.HyperParameters.Select(kHP => (k.KernelID, kHP)));
            }
        }

        ProcessKernelRecursive(Kernel);

        Console.WriteLine($"{nameof(GaussianProcessRegressionExecuter)} instance created.\r\n");
        Console.WriteLine(this.ToString());

    }


    // ★★★★★★★★★★★★★★★ methods

    public (double[] Y_predict, double[] cov) FitAndPredict(double[] X_train, double[] Y_train, double[] X_predict)
    {
        var _X = new DenseVector(X_train);
        var _Y = new DenseVector(Y_train);
        var _X_predict = new DenseVector(X_predict);

        var result = FitAndPredict(_X, _Y, _X_predict);

        var Y_predict = result.Y_predict.ToArray();
        var cov = result.Cov.ToArray();

        return (Y_predict, cov);
    }

    public (DenseVector Y_predict, DenseVector Cov) FitAndPredict(DenseVector X_train, DenseVector Y_train, DenseVector X_predict)
    {
        Fit(X_train, Y_train);
        return Predict(X_predict);
    }

    public void Fit(DenseVector X, DenseVector Y) => Kernel.Fit(X, Y);

    public (DenseVector Y_predict, DenseVector cov) Predict(DenseVector X_predict) => Kernel.Predict(X_predict);

    public void OptimizeParameters(DenseVector X_train, DenseVector Y_train,
        double tryCount = 100,
        double learning_rate = 0.05
        )
    {
        // target = length

        var N = X_train.Count;

        var targetParam = HyperParameters.First(p => p.Item2 == "LengthScale");
        double targetParamValue = 1;

        for (int k = 0; k < tryCount; k++)
        {
            Fit(X_train, Y_train);

            var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
            var dK = Kernel.CalcKernelGrad(X_train, Y_train, targetParam);

            var mm = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv).Multiply(dK);

            var addingValue = learning_rate * mm.Diagonal().Sum();
            Kernel.AddValueToParameter(addingValue, targetParam);
        }
    }


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
        var X_train = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 5.2, 5.3, 5.4, 5.6, 5.8, 6, 7, 8, 8, 8, 8 };
        var Y_train = X_train.Select(x => f(x, true)).ToArray();
        var X_predict = EnumerableExtension.Range_WithStep(0, 10, 0.1).ToArray();
        var Y_true = X_predict.Select(x => f(x)).ToArray();

        var k1 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
        var k2 = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
        var k3 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
        var kernel = k1 * k2 + k3;

        var gpr = new GaussianProcessRegressionExecuter(kernel);
        //gpr.OptimizeParameters(X, Y);
        (var Y_predict, var cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
        var Y_Std = cov.Select(x => Math.Sqrt(x)).ToArray();
        var Y_95CI = Y_Std.ProductForEach(1.96);
        var Y_99CI = Y_Std.ProductForEach(2.58);

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
                    new ScatterPlot(Array.Empty<double>(),Array.Empty<double>()){ MarkerColor="yellow", MarkerSize=100, LegendLabel=kernel.ToString()},

                    new LinePlot(X_predict, Y_true) { LineColor="g", LineWidth=3, LegendLabel="True f(X)= Xsin(X)"},
                    new ScatterPlot(X_train, Y_train) { MarkerSize=130, MarkerColor="g", LegendLabel="Observed Data"},

                    new LinePlot(X_predict, Y_predict) { LineColor="red", LineWidth=3, LegendLabel="Predicted Mean"},

                    new FillBetweenPlot(X_predict, Y_predict.AddForEach(Y_95CI), Y_predict.SubForEach(Y_95CI)) {FillColor="red", Alpha=0.20, LegendLabel="95% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(CI95)) { LineColor="red", LineStyle="-", Alpha=0.20 },
                    //new LinePlot(predictX, predictY.SubForEach(CI95)) { LineColor="red", LineStyle="-", Alpha=0.20 },

                    new FillBetweenPlot(X_predict, Y_predict.AddForEach(Y_99CI), Y_predict.SubForEach(Y_99CI)) {FillColor="red", Alpha=0.10, LegendLabel="99% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(CI99)) { LineColor="red", LineStyle="-", Alpha=0.10 },
                    //new LinePlot(predictX, predictY.SubForEach(CI99)) { LineColor="red", LineStyle="-", Alpha=0.10 },

                    //new TextPlot(10,8,kernel.ToString()){ HorizontalAlignment="right"},
                
                },
            }
        }.Run(outputImageFile, preview);

    }

    public override string ToString()
    {
        var s = "";

        s += $"Kernel:\r\n";
        s += $"  {Kernel.ToString()}\r\n";
        s += $"\r\n";
        s += $"Hyper Parameters:\r\n";
        foreach (var hp in HyperParameters)
            s += $"  {hp}\r\n";
        s += $"\r\n";
        return s;

    }

    // ★★★★★★★★★★★★★★★

}
