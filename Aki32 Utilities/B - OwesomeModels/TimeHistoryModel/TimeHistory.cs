using System.Text;

namespace Aki32_Utilities.OwesomeModels;
public class TimeHistory : ICloneable
{
    private Dictionary<string, double[]> data;
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

    #region fields

    private DirectoryInfo __inputDir = null;
    public string __resultFileName = "result";

    #endregion

    #region useful coefs

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

    #endregion

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
    public double[] xtt_ytt
    {
        get
        {
            return this["xtt_ytt"];
        }
        set
        {
            this["xtt_ytt"] = value;
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
    public double[] memo1
    {
        get
        {
            return this["memo1"];
        }
        set
        {
            this["memo1"] = value;
        }
    }

    #endregion

    #region initializers

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistory()
    {
        data = new Dictionary<string, double[]>();
    }

    /// <summary>
    /// Construct from csv
    /// </summary>
    /// <param name="inputCsvPath"></param>
    /// <exception cref="Exception"></exception>
    public static TimeHistory FromCsv(FileInfo inputCsv, string[] overwriteHeaders = null)
    {
        var history = new TimeHistory() { __inputDir = inputCsv.Directory };

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
                var column = data.Select(x => double.Parse(x[i])).ToArray();
                history.data[headers[i]] = column;
            }

            // Get TimeStep
            var ts = history.TimeStep;
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
    public object Clone()
    {
        var newHistory = new TimeHistory();

        foreach (var key in data.Keys)
            newHistory.data[key] = (double[])data[key].Clone();

        newHistory.__inputDir = __inputDir;

        return newHistory;
    }

    #endregion

    public TimeHistoryStep GetStep(int i)
    {
        var step = new TimeHistoryStep();
        foreach (var key in data.Keys)
            step.data[key] = this[key][i];
        return step;
    }
    public void SetStep(int i, TimeHistoryStep step)
    {
        foreach (var key in step.data.Keys)
            this[key][i] = step.data[key];
    }


    /// <summary>
    /// Output wave history to csv
    /// </summary>
    /// <param name="outputFilePath"></param>
    public void OutputWaveHistoryToCsv(FileInfo outputFile = null)
    {
        if (outputFile == null)
            outputFile = new FileInfo(Path.Combine(__inputDir.FullName, "output", $"result - {__resultFileName}.csv"));
        if (!outputFile.Directory.Exists)
            outputFile.Directory.Create();

        using var sw = new StreamWriter(outputFile.FullName);

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


    #region specific use

    /// <summary>
    /// Get response spectrum set from
    /// </summary>
    /// <returns></returns>
    public TimeHistoryStep GetSpectrumSet(double T)
    {
        var step = new TimeHistoryStep();
        step["T"] = T;
        step["Sd"] = x.Max(Math.Abs);
        step["Sv"] = xt.Max(Math.Abs);
        step["Sa"] = xtt_ytt.Max(Math.Abs);
        return step;
    }

    #endregion
}
