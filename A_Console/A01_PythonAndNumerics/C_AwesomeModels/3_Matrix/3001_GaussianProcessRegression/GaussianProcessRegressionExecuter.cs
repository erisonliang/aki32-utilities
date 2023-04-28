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
    /// <param name="terminatingRatio">Terminating threshold of difference of values</param>
    /// <param name="learningRate"></param>
    /// <returns>Optimize history</returns>
    public TimeHistory OptimizeParameters(DenseVector X_train, DenseVector Y_train,
        int maxTryCount = 2000,
        double terminatingRatio = 0.00001d,
        double learningRate = 0.001d,
        int wideOptimizeSize = 8,
        bool executeWideOptimization = true,
        bool executePreciseOptimization = true
        )
    {
        var SSHistory = new TimeHistory();

        try
        {
            // wide optimize
            if (executeWideOptimization)
            {
                var maxCount = HyperParameters.Count * (2 * wideOptimizeSize + 1);
                using var progress = new ProgressManager(maxCount);
                Console.WriteLine("wide optimization");
                progress.StartAutoWrite();

                // 全てのパラメーターに対して，初期値の周辺の値を代入してみて比較する。
                foreach (var targetParam in HyperParameters)
                {
                    var initialParamValue = Kernel.GetParameterValue(targetParam)!.Value;

                    var tryingParamValues = new List<double>
                    {
                        initialParamValue
                    };

                    for (int i = 1; i < wideOptimizeSize; i++)
                    {
                        var weight = (double)i / wideOptimizeSize;
                        tryingParamValues.Add(initialParamValue * weight);
                        tryingParamValues.Add(initialParamValue / weight);
                    }

                    var results = new List<(double ParamValue, double SSE)>();

                    foreach (var tryingParamValue in tryingParamValues)
                    {
                        Kernel.SetParameterValue(targetParam, tryingParamValue);

                        Fit(X_train, Y_train);

                        var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
                        var dK = Kernel.CalcKernelGrad(X_train, Y_train, targetParam);
                        var difference = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv).Multiply(dK); // 二乗和誤差

                        results.Add((tryingParamValue, Math.Abs(difference.Diagonal().Sum())));
                        progress.CurrentStep++;
                    }

                    var settingValue = results.MinBy(r => r.SSE).ParamValue;
                    Kernel.SetParameterValue(targetParam, settingValue);

                }

                progress.WriteDone();
            }

            // precise optimize
            if (executePreciseOptimization)
            {
                using var progress = new ProgressManager(maxTryCount);
                Console.WriteLine("precise optimization");
                progress.StartAutoWrite();
                var signSwap = 1d; //for emergency

                for (int i = 0; i < maxTryCount; i++)
                {
                    var step = new TimeHistoryStep();

                    // optimize
                    foreach (var targetParam in HyperParameters)
                    {
                        Fit(X_train, Y_train);

                        var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
                        var dK = Kernel.CalcKernelGrad(X_train, Y_train, targetParam);
                        var difference = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv).Multiply(dK); // 二乗和誤差
                        var addingValue = learningRate * signSwap * difference.Diagonal().Sum();

                        var originalValue = Kernel.GetParameterValue(targetParam)!.Value;
                        Kernel.SetParameterValue(targetParam, originalValue + addingValue);

                        step[targetParam.ToString()] = originalValue;
                    }

                    SSHistory.AppendStep(step);

                    // terminate check
                    if (SSHistory.DataRowCount >= 2)
                    {
                        var TerminatingFlag = true;

                        foreach (var column in SSHistory.Columns)
                        {
                            var c = SSHistory[column][SSHistory.DataRowCount - 1];
                            var p = SSHistory[column][SSHistory.DataRowCount - 2];
                            if (Math.Abs((c - p) / p) > terminatingRatio)
                                TerminatingFlag = false;
                        }
                        if (TerminatingFlag)
                            throw new OperationCanceledException("terminated");

                    }
                    progress.CurrentStep++;

                }
                progress.WriteDone();

            }

        }
        catch (OperationCanceledException)
        {
            // terminated
        }
        catch (Exception)
        {
            throw;
        }

        Console.WriteLine($"★ Parameters optimized.\r\n");
        Console.WriteLine(this.ToString());

        return SSHistory;
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
        var X_train = new double[] { 0, 0.5, 1, 1.5, 2, 2, 2, 5, 5, 5, 5.5, 5.5, 5.5, 6, 6, 6, 6.5, 6.5, 6.5, 7, 7, 7, 8, 8, 8, 8 };
        var Y_train = X_train.Select(x => f(x, true)).ToArray();
        var X_predict = EnumerableExtension.Range_WithStep(0, 10, 0.1).ToArray();
        var Y_true = X_predict.Select(x => f(x)).ToArray();


        // build model
        var k1 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
        var k2 = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
        var k3 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
        var kernel = k1 * k2 + k3;
        var gpr = new GaussianProcessRegressionExecuter(kernel);


        // (optional) optimize parameters
        var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train);
        PyPlotWrapper.LinePlot.DrawSimpleGraph(optimizeHistory[1]);


        // fit and predict
        (var Y_predict, var Y_cov) = gpr.FitAndPredict(X_train, Y_train, X_predict);
        var Y_std = Y_cov.Select(x => Math.Sqrt(x)).ToArray();
        var Y_95CI = Y_std.ProductForEach(1.96);
        var Y_99CI = Y_std.ProductForEach(2.58);


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
