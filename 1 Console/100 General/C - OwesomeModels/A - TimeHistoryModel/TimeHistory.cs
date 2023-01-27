using System.Text;

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Time history data table with its columns and rows dynamically and automatically expands
/// </summary>
/// <remarks>
/// decided not to use IClonable
/// </remarks>
public class TimeHistory
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Name of this time history instance
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// </summary>
    protected Dictionary<string, double[]> ContentsTable = new Dictionary<string, double[]>();
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
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return ContentsTable[key];
        }
        set
        {
            if (ContentsTable.ContainsKey(key))
            {
                ContentsTable[key] = value;
            }
            else
            {
                var newData = new double[Math.Max(DataRowCount, value.Length)];
                for (int i = 0; i < value.Length; i++)
                    newData[i] = value[i];
                ContentsTable.Add(key, newData);
            }
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
        var history = new TimeHistory()
        {
            Name = Path.GetFileNameWithoutExtension(inputCsv.Name),
            inputDir = inputCsv.Directory!,
        };

        try
        {
            var input = File.ReadLines(inputCsv.FullName, Encoding.UTF8).ToArray();

            // Get Data
            var headers = input
                     .Take(1)
                     .Select(x => x.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                     .First();

            if (overwriteHeaders != null && overwriteHeaders.Length > 0)
                headers = overwriteHeaders;

            var data = input
                .Skip(1)
                .Select(x => x.Split(new string[] { "," }, StringSplitOptions.None))
                .ToArray();

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

        }
        catch (Exception e)
        {
            throw new Exception($"Failed to read input csv : {e}");
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
    /// Draw Line Graph
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public TimeHistory DrawGraph(string yName, string xName = "", ChartType type = ChartType.Line)
    {
        try
        {
            var vertical = this[yName];

            var layout = new Layout.Layout
            {
                title = Name,
                xaxis = new Xaxis { title = xName },
                yaxis = new Yaxis { title = yName }
            };

            var horizontal =
                string.IsNullOrEmpty(xName) ?
                Enumerable.Range(0, vertical.Length).Select(i => (double)i) :
                this[xName];
            var points = Enumerable.Zip(horizontal, vertical).Select(x => new Tuple<double, double>(x.First, x.Second));
            PlotlyChart chart = type switch
            {
                ChartType.Scatter => Chart.Scatter(points),
                ChartType.Line => Chart.Line(points),
                _ => throw new NotImplementedException(),
            };
            chart.WithLayout(layout);
            chart.Show();

            Console.WriteLine("Graph has been drawn and opened in the default browser");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to draw graph: {ex.Message}");
        }

        return this;
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
        var outputFile = new FileInfo(Path.Combine(outputDir.FullName, $"{Name}.csv"));
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
        get
        {
            return this["t"];
        }
        set
        {
            this["t"] = value;
        }
    }
    public double[] x
    {
        get
        {
            return this["x"];
        }
        set
        {
            this["x"] = value;
        }
    }
    public double[] xt
    {
        get
        {
            return this["xt"];
        }
        set
        {
            this["xt"] = value;
        }
    }
    public double[] xtt
    {
        get
        {
            return this["xtt"];
        }
        set
        {
            this["xtt"] = value;
        }
    }
    public double[] ytt
    {
        get
        {
            return this["ytt"];
        }
        set
        {
            this["ytt"] = value;
        }
    }
    public double[] xtt_plus_ytt
    {
        get
        {
            return this["xtt+ytt"];
        }
        set
        {
            this["xtt+ytt"] = value;
        }
    }
    public double[] f
    {
        get
        {
            return this["f"];
        }
        set
        {
            this["f"] = value;
        }
    }
    public double[] memo
    {
        get
        {
            return this["memo"];
        }
        set
        {
            this["memo"] = value;
        }
    }

    public double[] a
    {
        get
        {
            return this["a"];
        }
        set
        {
            this["a"] = value;
        }
    }
    public double[] v
    {
        get
        {
            return this["v"];
        }
        set
        {
            this["v"] = value;
        }
    }
    public double[] mu
    {
        get
        {
            return this["mu"];
        }
        set
        {
            this["mu"] = value;
        }
    }

    public double[] Sd
    {
        get
        {
            return this["Sd"];
        }
        set
        {
            this["Sd"] = value;
        }
    }
    public double[] Sv
    {
        get
        {
            return this["Sv"];
        }
        set
        {
            this["Sv"] = value;
        }
    }
    public double[] Sa
    {
        get
        {
            return this["Sa"];
        }
        set
        {
            this["Sa"] = value;
        }
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
                return ContentsTable[ContentsTable.Keys.First()].Length;
        }
    }
    public string[] Columns
    {
        get
        {
            return ContentsTable.Keys.ToArray();
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
            while (DataRowCount < i)
                AppendStep(new TimeHistoryStep());
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

        var keys = step.ContentsTable.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];

            if (addingIndex >= this[key].Length)
            {
                var targetList = this[key];
                while (targetList.Length < addingIndex)
                    targetList = targetList.Append(0).ToArray();
                targetList = targetList.Append(step[key]).ToArray();
                this[key] = targetList;
            }
            else
            {
                this[key][addingIndex] = step[key];
            }
        }

        return this;
    }
    public TimeHistory DropStep(params int[] droppingSteps)
    {
        foreach (var key in ContentsTable.Keys)
        {
            var dataColumnList = ContentsTable[key].ToList();
            foreach (var droppingStep in droppingSteps)
                dataColumnList.RemoveAt(droppingStep);
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
        DeplicateColumn(targetColumnName, newColumnName);
        DropColumn(targetColumnName);

        return this;
    }
    public TimeHistory DeplicateColumn(string baseColumnName, string newColumnName)
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

    private double __timeStep = 0;
    public double TimeStep
    {
        get
        {
            if (__timeStep == 0)
            {
                try
                {
                    __timeStep = t[1] - t[0];
                }
                catch (Exception)
                {
                }
            }
            return __timeStep;
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
