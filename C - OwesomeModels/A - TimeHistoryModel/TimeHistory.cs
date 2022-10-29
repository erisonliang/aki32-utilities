using System.Linq;
using System.Text;

using XPlot.Plotly;

namespace Aki32_Utilities.OwesomeModels;
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
    internal Dictionary<string, double[]> data = new Dictionary<string, double[]>();
    /// <summary>
    /// indexer
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public double[] this[string key]
    {
        get
        {
            if (data.Keys.Contains(key))
                return data[key];

            data.Add(key, new double[DataRowCount]);
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return data[key];
        }
        set
        {
            if (data.Keys.Contains(key))
            {
                data[key] = value;
            }
            else
            {
                var newData = new double[Math.Max(DataRowCount, value.Length)];
                for (int i = 0; i < value.Length; i++)
                    newData[i] = value[i];
                data.Add(key, newData);
            }
        }
    }


    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistory(string Name = "")
    {
        this.Name = Name;
        data = new Dictionary<string, double[]>();
    }

    /// <summary>
    /// Construct from csv.
    /// </summary>
    /// <param name="inputCsv">first row must be a header</param>
    /// <param name="overwriteHeaders"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static TimeHistory FromCsv(FileInfo inputCsv, string[]? overwriteHeaders = null)
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

            if (overwriteHeaders != null)
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
                history.data[headers[i]] = column;
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

        foreach (var key in data.Keys)
            newHistory.data[key] = (double[])data[key].Clone();

        newHistory.inputDir = inputDir;

        return newHistory;
    }


    // ★★★★★★★★★★★★★★★ output

    /// <summary>
    /// Draw Scatter Graph
    /// </summary>
    /// <param name="verticalKey">Vertical Axis</param>
    /// <param name="horizontalKey">Horizontal Axis</param>
    public void DrawScatterGraph(string verticalKey, string horizontalKey = "")
    {
        try
        {
            var vertical = this[verticalKey];

            if (string.IsNullOrEmpty(horizontalKey))
            {
                Chart.Scatter(vertical).Show();
            }
            else
            {
                var horizontal = this[horizontalKey];
                var points = Enumerable.Zip(horizontal, vertical).Select(x => new Tuple<double, double>(x.First, x.Second));
                Chart.Scatter(points).Show();
            }

            Console.WriteLine("Graph has been drawn and opened in the default browser");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to draw graph: {ex.Message}");
        }
    }
    /// <summary>
    /// Draw Line Graph
    /// </summary>
    /// <param name="verticalKey">Vertical Axis</param>
    /// <param name="horizontalKey">Horizontal Axis</param>
    public void DrawLineGraph(string verticalKey, string horizontalKey = "")
    {
        try
        {
            var vertical = this[verticalKey];

            if (string.IsNullOrEmpty(horizontalKey))
            {
                Chart.Line(vertical).Show();
            }
            else
            {
                var horizontal = this[horizontalKey];
                var points = Enumerable.Zip(horizontal, vertical).Select(x => new Tuple<double, double>(x.First, x.Second));
                Chart.Line(points).Show();
            }

            Console.WriteLine("Graph has been drawn and opened in the default browser");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to draw graph: {ex.Message}");
        }
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

        return outputFile;
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
    public void WriteToConsole()
    {
        using var sw = new StreamWriter(Console.OpenStandardOutput());
        sw.WriteLine("============================================");
        WriteToStream(sw);
        sw.WriteLine("============================================");
    }

    /// <summary>
    /// Output TimeHistory to stream
    /// </summary>
    private void WriteToStream(StreamWriter sw)
    {
        foreach (var key in data.Keys)
        {
            sw.Write(key.ToString());
            sw.Write(",");
        }
        sw.WriteLine();

        for (int i = 0; i < DataRowCount; i++)
        {
            foreach (var key in data.Keys)
            {
                try
                {
                    sw.Write(data[key][i]);
                }
                catch (Exception)
                {
                }
                sw.Write(",");
            }
            sw.WriteLine();
        }

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
            if (data.Keys.Count == 0)
                return 0;
            else
                return data[data.Keys.First()].Length;
        }
    }
    public string[] Columns
    {
        get
        {
            return data.Keys.ToArray();
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
        foreach (var key in data.Keys)
        {
            try
            {
                step[key] = this[key][i];
            }
            catch (Exception)
            {
                step[key] = double.NaN;
            }
        }
        return step;
    }
    public TimeHistoryStep GetTheLastStep()
    {
        return GetStep(DataRowCount - 1);
    }
    public void SetStep(int i, TimeHistoryStep step)
    {
        foreach (var key in step.data.Keys)
            this[key][i] = step[key];
    }
    public void AppendStep(TimeHistoryStep step)
    {
        var addingIndex = DataRowCount;

        var keys = step.data.Keys.ToArray();
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
    }

    public void DropAllColumns()
    {
        data.Clear();
    }
    public void DropColumn(params string[] droppingColumns)
    {
        foreach (var droppingColumn in droppingColumns)
            data.Remove(droppingColumn);
    }
    public void DropStep(params int[] droppingSteps)
    {
        foreach (var key in data.Keys)
        {
            var dataColumnList = data[key].ToList();
            foreach (var droppingStep in droppingSteps)
                dataColumnList.RemoveAt(droppingStep);
            data[key] = dataColumnList.ToArray();
        }
    }


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

    /// <summary>
    /// Get response spectrum set from
    /// </summary>
    /// <returns></returns>
    public TimeHistoryStep GetSpectrumSet()
    {
        var step = new TimeHistoryStep();
        step.Sd = x.Max(Math.Abs);
        step.Sv = xt.Max(Math.Abs);
        step.Sa = xtt_plus_ytt.Max(Math.Abs);
        return step;
    }


    // ★★★★★★★★★★★★★★★ 

}
