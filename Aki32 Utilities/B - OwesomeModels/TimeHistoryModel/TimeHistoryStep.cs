

namespace Aki32_Utilities.OwesomeModels;
/// <summary>
/// 時系列データのうちの一行。
/// </summary>
/// <remarks>
/// TimeHistoryを流用し，[0]の存在が保証されるモデルとする。
/// </remarks>
public class TimeHistoryStep : TimeHistory
{
    public new double this[string key]
    {
        get
        {
            if (data.Keys.Contains(key))
                return data[key][0];

            data.Add(key, new double[] { 0 });
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return data[key][0];
        }
        set
        {
            if (data.Keys.Contains(key))
                data[key][0] = value;
            else
                data.Add(key, new double[] { value });
        }
    }

    #region useful values

    #region default keys

    public double t
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
    public double x
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
    public double xt
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
    public double xtt
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
    public double ytt
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
    public double f
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
    public double memo1
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

    #endregion

    #region initializers

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistoryStep()
    {
        data = new Dictionary<string, double[]>();
    }

    /// <summary>
    /// clone
    /// </summary>
    /// <returns></returns>
    public new object Clone()
    {
        var newHistoryStep = new TimeHistoryStep();
        foreach (var key in data.Keys)
            newHistoryStep.data[key] = data[key];
        return newHistoryStep;
    }

    #endregion

}
