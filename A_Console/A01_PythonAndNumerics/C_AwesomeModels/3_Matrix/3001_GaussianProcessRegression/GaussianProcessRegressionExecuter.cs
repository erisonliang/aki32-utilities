using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Numerics.LinearAlgebra.Double;

using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.PyPlotWrapper;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{

    // ★★★★★★★★★★★★★★★ props

    public KernelBase Kernel { get; set; }

    internal List<(Guid, string)> HyperParameters { get; set; }

    public const string logLikelihoodIndexName = "LogLikelihood";


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
        int maxTryCount = 20000,
        double terminatingRatio = 0.000001d,
        double learningRate = 0.0002d,
        int gridOptimizationSize = 8,
        bool executeGridOptimization = true,
        bool executePreciseOptimization = true
        )
    {
        var SSHistory = new TimeHistory();

        try
        {
            // grid optimize (TODO: make this to grid)
            if (executeGridOptimization)
            {
                var maxCount = HyperParameters.Count * HyperParameters.Count * (2 * gridOptimizationSize + 1);
                using var progress = new ProgressManager(maxCount);
                Console.WriteLine("grid optimization");
                progress.StartAutoWrite();

                // 全てのパラメーターに対して，初期値の周辺の値を代入してみて比較する。
                for (int times = 0; times < HyperParameters.Count; times++)
                {
                    foreach (var targetParam in HyperParameters)
                    {
                        var initialParamValue = Kernel.GetParameterValue(targetParam)!.Value;

                        var tryingParamValues = new List<double>
                        {
                            initialParamValue
                        };

                        for (int i = 1; i < gridOptimizationSize; i++)
                        {
                            var weight = (double)i / gridOptimizationSize;
                            tryingParamValues.Add(initialParamValue * weight);
                            tryingParamValues.Add(initialParamValue / weight);
                        }

                        var results = new List<(double ParamValue, double SSE)>();

                        foreach (var tryingParamValue in tryingParamValues)
                        {
                            Kernel.SetParameterValue(targetParam, tryingParamValue);

                            Fit(X_train, Y_train);

                            var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
                            var difference = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv);

                            results.Add((tryingParamValue, Math.Abs(difference.Diagonal().Sum())));
                            progress.CurrentStep++;
                        }

                        var settingValue = results.MinBy(r => r.SSE).ParamValue;
                        Kernel.SetParameterValue(targetParam, settingValue);

                    }
                }

                progress.WriteDone();
            }

            // precise optimize
            if (executePreciseOptimization)
            {
                using var progress = new ProgressManager(maxTryCount);
                Console.WriteLine("precise optimization");
                progress.StartAutoWrite();

                for (int i = 0; i < maxTryCount; i++)
                {
                    var step = new TimeHistoryStep();
                    step["Index"] = i;


                    // optimize
                    foreach (var targetParam in HyperParameters)
                    {
                        Fit(X_train, Y_train);

                        //original
                        var Ktt_Inv_Y_2 = (DenseMatrix)(Kernel.Ktt_Inv_Y.ToColumnMatrix() * Kernel.Ktt_Inv_Y.ToRowMatrix());
                        var dK = Kernel.CalcKernelGrad(X_train, Y_train, targetParam);
                        var difference = (Ktt_Inv_Y_2 - Kernel.Ktt_Inv).Multiply(dK); // 二乗和誤差
                        var addingValue = learningRate * difference.Trace();

                        (var originalValue, var processedValue) = Kernel.AddValueToParameter(targetParam, addingValue);
                        if (double.IsNaN(processedValue)) // 計算の発散
                        {
                            progress.StopAutoWrite();
                            throw new Exception("got NaN");
                        }
                        step[targetParam.ToString()] = originalValue;

                    }


                    // calc likelihood
                    double logDetKs = Kernel.Ktt.Determinant().Log();
                    double yKy = Y_train * Kernel.Ktt_Inv_Y;
                    double logLikelihood = -0.5 * (logDetKs + yKy + X_train.Count * Math.Log(2 * Math.PI));
                    step[logLikelihoodIndexName] = logLikelihood;
                    SSHistory.AppendStep(step);


                    // terminate check
                    if (SSHistory.DataRowCount >= 2)
                    {
                        var logLikelihoods = SSHistory[logLikelihoodIndexName];
                        var p = logLikelihoods[SSHistory.DataRowCount - 2];
                        var c = logLikelihoods[SSHistory.DataRowCount - 1];
                        if (c < p) // 尤度最大化が目標なのに下がってる場合は，最適化失敗
                        {
                            progress.StopAutoWrite();
                            throw new Exception("likelihood decreased");
                        }
                        if (Math.Abs((c - p) / p) < terminatingRatio)
                        {
                            progress.StopAutoWrite();
                            throw new OperationCanceledException("terminated");
                        }

                    }
                    progress.CurrentStep++;

                }
                progress.WriteDone();

            }

        }
        catch (OperationCanceledException)
        {
            // terminated
            Console.WriteLine();
            Console.WriteLine("Converged. Terminated!");
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
            return x * (Math.Sin(x) + 2) + noise;
        }
        //var X = new double[] { 1, 3, 5, 6, 7, 8 };
        //var X = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 6, 7, 8 };
        //var X_train = new double[] { 1, 1.1, 1.2, 1.3, 3, 5, 5.2, 5.3, 5.4, 5.6, 5.8, 6, 7, 8, 8, 8, 8 };
        var X_train = new double[] { 0, 0.5, 0.7, 0.7, 1, 1.5, 2, 2, 2, 5, 5, 5, 5, 5, 5.5, 5.5, 5.5, 6, 6, 6, 6.5, 6.5, 6.5, 6.5, 7, 7, 7, 7, 8, 8, 8, 8, 10, 10, 10 };
        var Y_train = X_train.Select(x => f(x, true)).ToArray();
        var X_predict = EnumerableExtension.Range_WithStep(0, 10, 0.1).ToArray();
        var Y_true = X_predict.Select(x => f(x)).ToArray();


        // build model
        var k11 = new GaussianProcessRegressionExecuter.ConstantKernel(1d);
        var k12 = new GaussianProcessRegressionExecuter.SquaredExponentialKernel(1d);
        var k21 = new GaussianProcessRegressionExecuter.ConstantKernel(0.1d);
        var k22 = new GaussianProcessRegressionExecuter.LinearKernel(1d);
        var k30 = new GaussianProcessRegressionExecuter.WhiteNoiseKernel(1 / 15d);
        var kernel = k11 * k12 + k21 * k22 + k30;
        var gpr = new GaussianProcessRegressionExecuter(kernel);


        // (optional) optimize parameters
        while (true)
        {
            try
            {
                var optimizeHistory = gpr.OptimizeParameters(X_train, Y_train, executeGridOptimization: false);
                optimizeHistory.DrawGraph_OnPyPlot(null, "Index", logLikelihoodIndexName, preview: true);
                //optimizeHistory.DrawGraph_OnPyPlot(null, "Index", optimizeHistory.Columns.Where(x => x.Contains("Constant")).ToArray(), preview: true);
                //optimizeHistory.DrawGraph_OnPyPlot(null, "Index", optimizeHistory.Columns.Where(x => x.Contains("Length")).ToArray(), preview: true);
                //optimizeHistory.DrawGraph_OnPyPlot(null, "Index", optimizeHistory.Columns.Where(x => x.Contains("Noise")).ToArray(), preview: true);

                break;
            }
            catch (Exception ex)
            {
                ConsoleExtension.WriteWithColor($"Failed. Trying again... (error: {ex.Message})", ConsoleColor.Red);
            }
        }


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

                    new LinePlot(X_predict, Y_true) { LineColor="g", LineWidth=3, LegendLabel="True f(X)= X(sin(X)+2)"},
                    new ScatterPlot(X_train, Y_train) { MarkerSize=130, MarkerColor="g", LegendLabel="Observed Data"},

                    new LinePlot(X_predict, Y_predict) { LineColor="red", LineWidth=3, LegendLabel="Predicted Mean"},

                    new FillBetweenPlot(X_predict, Y_predict.AddForEach(Y_95CI), Y_predict.SubForEach(Y_95CI)) {FillColor="red", Alpha=0.20, LegendLabel="95% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(Y_95CI)) { LineColor="red", LineStyle="-", Alpha=0.20 },
                    //new LinePlot(predictX, predictY.SubForEach(Y_95CI)) { LineColor="red", LineStyle="-", Alpha=0.20 },

                    new FillBetweenPlot(X_predict, Y_predict.AddForEach(Y_99CI), Y_predict.SubForEach(Y_99CI)) {FillColor="red", Alpha=0.10, LegendLabel="99% CI"},
                    //new LinePlot(predictX, predictY.AddForEach(Y_99CI)) { LineColor="red", LineStyle="-", Alpha=0.10 },
                    //new LinePlot(predictX, predictY.SubForEach(Y_99CI)) { LineColor="red", LineStyle="-", Alpha=0.10 },

                    //new TextPlot(10,8,kernel.ToString()){ HorizontalAlignment="right"},
                
                },
            }
        }.Run(outputImageFile, preview);
    }

    public override string ToString()
    {
        var s = "";

        s += $"Kernel:\r\n";
        s += $"  Initial: {Kernel.ToInitialStateString()}\r\n";
        s += $"  Optimum: {Kernel.ToString()}\r\n";
        s += $"\r\n";
        s += $"Hyper Parameters:\r\n";
        foreach (var hp in HyperParameters)
            s += $"  ({hp.Item1}, {hp.Item2}, {Kernel.GetParameterValue(hp):F5})\r\n";
        s += $"\r\n";
        return s;

    }

    // ★★★★★★★★★★★★★★★

}
