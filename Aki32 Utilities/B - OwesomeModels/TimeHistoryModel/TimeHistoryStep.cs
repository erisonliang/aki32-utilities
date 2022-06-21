

namespace Aki32_Utilities.OwesomeModels;
public class TimeHistoryStep : ICloneable
{
    public Dictionary<string, double> data;
    public double this[string key]
    {
        get
        {
            if (data.Keys.Contains(key))
                return data[key];

            data.Add(key, 0);
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return data[key];
        }
        set
        {
            if (data.Keys.Contains(key))
                data[key] = value;
            else
                data.Add(key, value);
        }
    }

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

    #region initializers

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistoryStep()
    {
        data = new Dictionary<string, double>();
    }

    /// <summary>
    /// clone
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        var newHistoryStep = new TimeHistoryStep();
        foreach (var key in data.Keys)
            newHistoryStep.data[key] = data[key];
        return newHistoryStep;
    }

    #endregion

}
