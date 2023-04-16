using System.Text;

using XPlot.Plotly;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistoryBase;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Time history data table with its columns and rows dynamically and automatically expands
/// </summary>
/// <remarks>
/// decided not to use IClonable
/// </remarks>
public class TimeHistory : TimeHistoryBase
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Name of this time history instance
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// </summary>
    protected Dictionary<string, double[]> ContentsTable = new();
    /// <summary>
    /// indexer
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public double[] this[string key]
    {
        get
        {
            if (ContentsTable.ContainsKey(key))
                return ContentsTable[key];
            ContentsTable.Add(key, new double[DataRowCount]);
            return ContentsTable[key];
        }
        set
        {
            if (ContentsTable.ContainsKey(key))
                ContentsTable[key] = value;
            else
                ContentsTable.Add(key, value);
            CheckAllColumnsLength();
        }
    }
    /// <summary>
    /// indexer
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public double[] this[int index]
    {
        get
        {
            return this[Columns[index]];
        }
        set
        {
            this[Columns[index]] = value;
        }
    }


    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistory(string Name = "")
    {
        this.Name = Name;
        ContentsTable = new Dictionary<string, double[]>();
    }

    /// <summary>
    /// Construct from csv.
    /// </summary>
    /// <param name="inputCsv">first row must be a header</param>
    /// <param name="overwriteHeaders"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static TimeHistory FromCsv(FileInfo inputCsv, params string[]? overwriteHeaders)
    {
        try
        {
            var input = inputCsv.ReadCsv_Rows();
            var name = Path.GetFileNameWithoutExtension(inputCsv.Name);
            var originalDir = inputCsv.Directory!;

            var history = FromArray(input, name, originalDir, overwriteHeaders);
            return history;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to read input csv : {e}");
        }
    }

    /// <summary>
    /// Construct from excel.
    /// </summary>
    /// <param name="inputCsv">first row must be a header</param>
    /// <param name="overwriteHeaders"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static TimeHistory FromExcel(FileInfo inputExcel, string SheetName, params string[]? overwriteHeaders)
    {
        try
        {
            var name = Path.GetFileNameWithoutExtension(inputExcel.Name);
            var input = inputExcel.GetExcelSheet(SheetName).ConvertToJaggedArray();
            var originalDir = inputExcel.Directory!;

            var history = FromArray(input, name, originalDir, overwriteHeaders);
            return history;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to read input excel : {e}");
        }
    }

    /// <summary>
    /// Construct from excel.
    /// </summary>
    /// <param name="inputCsv">first row must be a header</param>
    /// <param name="overwriteHeaders"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static TimeHistory FromArray(string[][] input, string name, DirectoryInfo? originalDir, params string[]? overwriteHeaders)
    {
        var history = new TimeHistory()
        {
            Name = Path.GetFileNameWithoutExtension(name),
            inputDir = originalDir,
        };

        // Get Headers
        var headers = input.First().ToArray();
        if (overwriteHeaders != null && overwriteHeaders.Length > 0)
            headers = overwriteHeaders;
        if (string.IsNullOrEmpty(headers.LastOrDefault()))
            headers = headers[0..^1];
        var tempCount = 0;
        for (int i = 0; i < headers.Length; i++)
            if (string.IsNullOrEmpty(headers[i]))
                headers[i] = $"Unnamed: {tempCount++}";


        // Get Data
        var data = input.Skip(1).ToArray();

        for (int i = 0; i < headers.Length; i++)
        {
            var column = data
                .Select(x =>
                {
                    try
                    {
                        return double.Parse(x[i]);
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                })
                .ToArray();
            history.ContentsTable[headers[i]] = column;
        }

        return history;
    }

    /// <summary>
    /// clone
    /// </summary>
    /// <returns></returns>
    public TimeHistory Clone()
    {
        var newHistory = new TimeHistory()
        {
            Name = Name,
            inputDir = inputDir,
        };

        foreach (var key in ContentsTable.Keys)
            newHistory.ContentsTable[key] = (double[])ContentsTable[key].Clone();

        newHistory.inputDir = inputDir;

        return newHistory;
    }


    // ★★★★★★★★★★★★★★★ output

    /// <summary>
    /// Draw Line Graph on Ploty
    /// x axis will be index
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public TimeHistory DrawGraph_OnPlotly(string yName, ChartType type = ChartType.Line) => DrawGraph_OnPlotly("", yName, type);

    /// <summary>
    /// Draw Line Graph on Ploty
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public TimeHistory DrawGraph_OnPlotly(string xName, string yName, ChartType type = ChartType.Line)
    {
        try
        {
            var y = this[yName];

            var layout = new Layout.Layout
            {
                title = Name,
                xaxis = new Xaxis { title = xName },
                yaxis = new Yaxis { title = yName }
            };

            var x = string.IsNullOrEmpty(xName)
                ? Enumerable.Range(0, y.Length).Select(i => (double)i)
                : this[xName];
            var points = Enumerable.Zip(x, y).Select(x => new Tuple<double, double>(x.First, x.Second));
            PlotlyChart chart = type switch
            {
                ChartType.Scatter => Chart.Scatter(points),
                ChartType.Line => Chart.Line(points),
                _ => throw new NotImplementedException(),
            };
            chart.WithLayout(layout);
            chart.Show();

            if (UtilConfig.ConsoleOutput_Contents)
                Console.WriteLine("Graph has been drawn and opened in the default browser");
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed to draw graph: {ex.Message}", ConsoleColor.Red);
        }

        return this;
    }

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public FileInfo DrawGraph_OnPyPlot(FileInfo outputFile, string yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        return DrawGraph_OnPyPlot(outputFile, "", yName: yName,
            type: type,
            chartTitle: chartTitle,
            xLabel: xLabel,
            yLabel: yLabel,
            preview: preview);
    }

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public FileInfo DrawGraph_OnPyPlot(FileInfo outputFile, string xName, string yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        return DrawGraph_OnPyplot(outputFile, xName, new string[] { yName },
            type: type,
            chartTitle: chartTitle,
            xLabel: xLabel,
            yLabel: yLabel,
            preview: preview);
    }

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public FileInfo DrawGraph_OnPyplot(FileInfo outputFile, string xName, string[] yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        try
        {
            var x = string.IsNullOrEmpty(xName) ? null : this[xName];
            var plots = new List<PyPlotWrapper.IPlot>();
            xLabel ??= xName;
            yLabel ??= yName.Length == 1 ? yName[0] : "";

            switch (type)
            {
                case ChartType.Scatter:
                    {
                        if (yName.Length == 1)
                        {
                            plots.Add(new PyPlotWrapper.ScatterPlot(x, this[yName[0]])
                            {
                                MarkerColor = "blue",
                            });
                        }
                        else
                        {
                            foreach (var name in yName)
                                plots.Add(new PyPlotWrapper.ScatterPlot(x, this[name])
                                {
                                    LegendLabel = name,
                                });
                        }

                        new PyPlotWrapper.Figure
                        {
                            IsTightLayout = true,
                            SubPlots = new List<PyPlotWrapper.SubPlot>()
                            {
                                new PyPlotWrapper.SubPlot()
                                {
                                    XLabel = xLabel,
                                    YLabel = yLabel,
                                    Title = chartTitle,
                                    Plots = plots,
                                }
                            }

                        }.Run(outputFile, preview);
                    }
                    break;

                case ChartType.Line:
                    {
                        if (yName.Length == 1)
                        {
                            plots.Add(new PyPlotWrapper.LinePlot(x, this[yName[0]])
                            {
                                LineColor = "blue",
                            });
                        }
                        else
                        {
                            foreach (var name in yName)
                                plots.Add(new PyPlotWrapper.LinePlot(x, this[name])
                                {
                                    LegendLabel = name,
                                    LineColor = null
                                });
                        }

                        new PyPlotWrapper.Figure
                        {
                            IsTightLayout = true,
                            SubPlots = new List<PyPlotWrapper.SubPlot>()
                            {
                                new PyPlotWrapper.SubPlot()
                                {
                                    XLabel = xLabel,
                                    YLabel = yLabel,
                                    Title = chartTitle,
                                    Plots = plots,
                                }
                            }

                        }.Run(outputFile, preview);
                    }
                    break;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed to draw graph: {ex.Message}", ConsoleColor.Red);
        }

        return outputFile;
    }


    /// <summary>
    /// Output TimeHistory to csv
    /// </summary>
    /// <param name="outputFilePath"></param>
    public FileInfo SaveToCsv(FileInfo? outputFile = null)
    {
        if (outputFile == null)
        {
            if (inputDir == null)
                throw new InvalidOperationException("outputFile must be declared when input was not by FromCsv()");
            else
                outputFile = new FileInfo(Path.Combine(inputDir.FullName, "output", $"{Name}.csv"));
        }
        if (!outputFile.Directory!.Exists)
            outputFile.Directory.Create();

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        WriteToStream(sw);

        return outputFile!;
    }
    /// <summary>
    /// Output TimeHistory to csv
    /// </summary>
    /// <param name="outputFilePath"></param>
    public FileInfo SaveToCsv(DirectoryInfo outputDir)
    {
        var outputFile = outputDir.GetChildFileInfo($"{Name}.csv");
        return SaveToCsv(outputFile);
    }

    /// <summary>
    /// Output TimeHistory to console
    /// </summary>
    public TimeHistory WriteToConsole(int head = int.MaxValue)
    {
        using var sw = new StreamWriter(Console.OpenStandardOutput());
        sw.WriteLine("============================================");
        WriteToStream(sw, head);
        sw.WriteLine("============================================");

        return this;
    }

    /// <summary>
    /// Output TimeHistory to stream
    /// </summary>
    private TimeHistory WriteToStream(StreamWriter sw, int head = int.MaxValue)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS

        foreach (var key in ContentsTable.Keys)
        {
            sw.Write(key.ToString());
            sw.Write(",");
        }
        sw.WriteLine();

        var repeatingCount = Math.Min(DataRowCount, head);
        for (int i = 0; i < repeatingCount; i++)
        {
            foreach (var key in ContentsTable.Keys)
            {
                try
                {
                    sw.Write(ContentsTable[key][i]);
                }
                catch (Exception)
                {
                }
                sw.Write(",");
            }
            sw.WriteLine();
        }

        return this;
    }


    // ★★★★★★★★★★★★★★★ useful values

    #region default keys

    public double[] t
    {
        get => this[PresetIndex.t];
        set => this[PresetIndex.t] = value;
    }
    public double[] x
    {
        get => this[PresetIndex.x];
        set => this[PresetIndex.x] = value;
    }
    public double[] xt
    {
        get => this[PresetIndex.xt];
        set => this[PresetIndex.xt] = value;
    }
    public double[] xtt
    {
        get => this[PresetIndex.xtt];
        set => this[PresetIndex.xtt] = value;
    }
    public double[] y
    {

        get => this[PresetIndex.y];
        set => this[PresetIndex.y] = value;
    }
    public double[] yt
    {

        get => this[PresetIndex.yt];
        set => this[PresetIndex.yt] = value;
    }
    public double[] ytt
    {
        get => this[PresetIndex.ytt];
        set => this[PresetIndex.ytt] = value;
    }
    public double[] xtt_plus_ytt
    {

        get => this[PresetIndex.xtt_plus_ytt];
        set => this[PresetIndex.xtt_plus_ytt] = value;
    }
    public double[] f
    {
        get => this[PresetIndex.f];
        set => this[PresetIndex.f] = value;
    }
    public double[] memo
    {
        get => this[PresetIndex.memo];
        set => this[PresetIndex.memo] = value;
    }

    public double[] a
    {
        get => this[PresetIndex.a];
        set => this[PresetIndex.a] = value;
    }
    public double[] v
    {
        get => this[PresetIndex.v];
        set => this[PresetIndex.v] = value;
    }
    public double[] mu
    {
        get => this[PresetIndex.mu];
        set => this[PresetIndex.mu] = value;
    }

    public double[] Sd
    {
        get => this[PresetIndex.Sd];
        set => this[PresetIndex.Sd] = value;
    }
    public double[] Sv
    {
        get => this[PresetIndex.Sv];
        set => this[PresetIndex.Sv] = value;
    }
    public double[] Sa
    {
        get => this[PresetIndex.Sa];
        set => this[PresetIndex.Sa] = value;
    }

    #endregion

    public DirectoryInfo? inputDir = null;
    public int DataRowCount
    {
        get
        {
            if (ContentsTable.Keys.Count == 0)
                return 0;
            else
                return ContentsTable.Values.Max(v => v.Length);
        }
    }
    public string[] Columns
    {
        get
        {
            return ContentsTable.Keys.ToArray();
        }
    }

    /// <summary>
    /// Check if all the column length is the same; Append 0 if invalid.<br/>
    /// If "t" is shorter, enlengthen column at current TimeStep or designated "overwriteTimeStep".
    /// </summary>
    private void CheckAllColumnsLength(int requiredLength = 0, double? overwriteTimeStep = null)
    {
        var targetDataRowCount = Math.Max(DataRowCount, requiredLength);

        if (ContentsTable.ContainsKey("t"))
        {
            if (t.Length < targetDataRowCount)
            {
                overwriteTimeStep ??= TimeStep;
                ContentsTable["t"] = EnumerableExtension.Range_WithStepAndCount(t[0], overwriteTimeStep.Value, targetDataRowCount).ToArray();
            }
        }
        else
        {
            if (overwriteTimeStep is not null)
                ContentsTable["t"] = EnumerableExtension.Range_WithStepAndCount(0, overwriteTimeStep.Value, targetDataRowCount).ToArray();
        }

        foreach (var column in Columns)
        {
            var value = ContentsTable[column].AsEnumerable();
            var addingCount = targetDataRowCount - value.Count();
            if (addingCount == 0)
                continue;

            for (int i = 0; i < addingCount; i++)
                value = value.Append(0);

            ContentsTable[column] = value.ToArray();
        }
    }


    // ★★★★★★★★★★★★★★★ methods

    public TimeHistoryStep GetStep(int i)
    {
        var step = new TimeHistoryStep()
        {
            inputDir = inputDir,
            Name = $"{Name}_step{i}",
        };

        foreach (var key in ContentsTable.Keys)
            step[key] = this[key][i];

        return step;
    }
    public TimeHistoryStep GetTheLastStep()
    {
        return GetStep(DataRowCount - 1);
    }
    public TimeHistory SetStep(int i, TimeHistoryStep step)
    {
        if (DataRowCount <= i)
        {
            CheckAllColumnsLength(i);
            AppendStep(step);
        }
        else
        {
            foreach (var key in step.ContentsTable.Keys)
                this[key][i] = step[key];
        }

        return this;
    }
    public TimeHistory AppendStep(TimeHistoryStep step)
    {
        var addingIndex = DataRowCount;
        if (addingIndex == 0)
        {
            // 実質 init
            foreach (var key in step.ContentsTable.Keys)
                this[key] = new double[] { step[key] };
        }
        else
        {
            CheckAllColumnsLength(requiredLength: addingIndex + 1);
            foreach (var key in step.ContentsTable.Keys)
                this[key][addingIndex] = step[key];
        }

        return this;
    }
    public TimeHistory DropStep(params int[] droppingSteps)
    {
        foreach (var key in ContentsTable.Keys)
        {
            var dataColumnList = ContentsTable[key].ToList();
            for (int i = droppingSteps.Length - 1; i >= 0; i--)
                dataColumnList.RemoveAt(droppingSteps[i]);
            ContentsTable[key] = dataColumnList.ToArray();
        }

        return this;
    }
    public IEnumerable<TimeHistoryStep> GetAllSteps()
    {
        for (int i = 0; i < DataRowCount; i++)
            yield return GetStep(i);
    }
    public TimeHistory SetAllSteps(List<TimeHistoryStep> allSteps)
    {
        foreach (var key in allSteps.First().ContentsTable.Keys)
            ContentsTable[key] = allSteps.Select(x => x[key]).ToArray();
        return this;
    }

    public TimeHistory AddColumn(params string[] addingColumnNames)
    {
        foreach (var addingColumnName in addingColumnNames)
            _ = this[addingColumnName][0];

        return this;
    }
    public TimeHistory RenameColumn(string targetColumnName, string newColumnName)
    {
        DuplicateColumn(targetColumnName, newColumnName);
        DropColumn(targetColumnName);

        return this;
    }
    public TimeHistory DuplicateColumn(string baseColumnName, string newColumnName)
    {
        this[newColumnName] = this[baseColumnName];

        return this;
    }
    public TimeHistory DropColumn(params string[] droppingColumnNames)
    {
        foreach (var droppingColumnName in droppingColumnNames)
            ContentsTable.Remove(droppingColumnName);

        return this;
    }
    public TimeHistory DropAllColumns()
    {
        ContentsTable.Clear();

        return this;
    }

    public TimeHistory OrderBy(string targetColumnName, bool descending = false)
    {
        var allSteps = GetAllSteps().ToList();
        List<TimeHistoryStep> ordered;
        if (descending)
            ordered = allSteps.OrderByDescending(step => step[targetColumnName]).ToList();
        else
            ordered = allSteps.OrderBy(step => step[targetColumnName]).ToList();

        SetAllSteps(ordered);

        return this;
    }
    public TimeHistory OrderByDescending(string targetColumnName) => OrderBy(targetColumnName, true);


    // ★★★★★★★★★★★★★★★ for specific use

    public double TimeStep
    {
        get
        {
            try
            {
                return t[1] - t[0];
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }


    // ★★★★★★★★★★★★★★★ enum

    public enum ChartType
    {
        Scatter,
        Line,
    }


    // ★★★★★★★★★★★★★★★ 

}
