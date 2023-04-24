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

        foreach (var k in Kernel.GetAllChildrenKernelsAndSelf())
            HyperParameters.AddRange(k.HyperParameters.Select(kHP => (k.KernelID, kHP)));

        Console.WriteLine($"★ {nameof(GaussianProcessRegressionExecuter)} instance created.\r\n");
        Console.WriteLine(this.ToString());

    }


    // ★★★★★★★★★★★★★★★ methods

    public (double[] Y_predict, double[] Y_Cov) FitAndPredict(double[] X_train, double[] Y_train, double[] X_predict)
    {
        var _X = new DenseVector(X_train);
        var _Y = new DenseVector(Y_train);
        var _X_predict = new DenseVector(X_predict);

        var result = FitAndPredict(_X, _Y, _X_predict);

        var Y_predict = result.Y_predict.ToArray();
        var Y_Cov = result.Y_Cov.ToArray();

        return (Y_predict, Y_Cov);
    }

    public (DenseVector Y_predict, DenseVector Y_Cov) FitAndPredict(DenseVector X_train, DenseVector Y_train, DenseVector X_predict)
    {
        Fit(X_train, Y_train);
        return Predict(X_predict);
    }

    public void Fit(DenseVector X, DenseVector Y) => Kernel.Fit(X, Y);

    public (DenseVector Y_predict, DenseVector Y_Cov) Predict(DenseVector X_predict) => Kernel.Predict(X_predict);

    /// <summary>
    /// Optimize all hyper parameters
    /// </summary>
    /// <param name="X_train"></param>
    /// <param name="Y_train"></param>
    /// <param name="maxTryCount"></param>
    /// <param name="terminatingSRSS">Terminating threshold of SRSS of SSE</param>
    /// <param name="learningRate"></param>
    /// <returns>Optimizing SRSS history</returns>
    public double[] OptimizeParameters(DenseVector X_train, DenseVector Y_train,
        int maxTryCount = 10000,
        double terminatingSRSS = 0.003d,
        double learningRate = 0.0003d
        )
    {
        using var progress = new ProgressManager(maxTryCount);
        progress.StartAutoWrite();

        // wide optimize
        {
            // 全てのパラメーター，初期値の{ 1/8, 1/4, 1/2, 1, 2, 4, 8}倍を比較する。







        }

        // precise optimize
        var SRSSHistory = new List<double>();
        for (int i = 0; i < maxTryCount; i++)
        {
            var SS = 0d;

            foreach (var targetParam in HyperParameters)
            {
                Fit(X_train, Y_train);
                var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
                var dK = Kernel.CalcKernelGrad(X_train, Y_train, targetParam);

                var SSE = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv).Multiply(dK); // 二乗和誤差

                var addingValue = learningRate * SSE.Diagonal().Sum();
                Kernel.AddValueToParameter(addingValue, targetParam);

                SS += Math.Pow(addingValue, 2);
            }

            var SRSS = Math.Sqrt(SS);
            SRSSHistory.Add(SRSS);
            if (SRSS < terminatingSRSS)
                break;

            progress.CurrentStep = i;
        }

        progress.WriteDone();

        Console.WriteLine($"★ Parameters optimized.\r\n");
        Console.WriteLine(this.ToString());

        return SRSSHistory.ToArray();

    }


    // ★★★★★★★★★★★★★★★ methods (static)

    public static void RunExampleModel(FileInfo? outputImageFile = null, bool preview = true)
    {
        // pre-process
        outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));


        // prepare data
        static double f(double x, bool withNoise = false)
        {
            var noise = withNoise ? new Random().NextDouble() - 0.5d : 0;
            return x * Math.Sin(x) + noise;
        }
        //var X = new double[] { 1, 3, 5, 6, 7, 8 };
        //var X = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 6, 7, 8 };
        //var X_train = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 5.2, 5.3, 5.4, 5.6, 5.8, 6, 7, 8, 8, 8, 8 };
        var X_train = new double[] { 1, 1.5, 2, 5, 5, 5, 5.5, 5.5, 5.5, 6, 6, 6, 6.5, 6.5, 6.5, 7, 7, 7, 8, 8, 8, 8 };
        var Y_train = X_train.Select(x => f(x, true)).ToArray();
        var X_predict = EnumerableExtension.Range_WithStep(0, 10, 0.1).ToArray();
        var Y_true = X_predict.Select(x => f(x)).ToArray();


        // build model
        var k1 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
        var k2 = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
        var k3 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
        var kernel = k1 * k2 + k3;
        var gpr = new GaussianProcessRegressionExecuter(kernel);

        {

            // (optional) optimize parameters
            var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train);
            PyPlotWrapper.LinePlot.DrawSimpleGraph(optimizeHistory.ToArray());


            // fit and predict
            (var Y_predict, var Y_Cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
            var Y_Std = Y_Cov.Select(x => Math.Sqrt(x)).ToArray();
            var Y_95CI = Y_Std.ProductForEach(1.96);
            var Y_99CI = Y_Std.ProductForEach(2.58);


            // draw
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
