

namespace Aki32_Utilities.Console_App_Utilities.OwesomeModels;
/// <summary>
/// TimeHistory row data.
/// </summary>
/// <remarks>
/// Based on TimeHistory.
/// Guarantee [0] exitance
/// </remarks>
public class TimeHistoryStep : TimeHistory
{

    // ★★★★★★★★★★★★★★★ props

    public new double this[string key]
    {
        get
        {
            if (ContentsTable.Keys.Contains(key))
                return ContentsTable[key][0];

            ContentsTable.Add(key, new double[] { 0 });
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return ContentsTable[key][0];
        }
        set
        {
            if (ContentsTable.Keys.Contains(key))
                ContentsTable[key][0] = value;
            else
                ContentsTable.Add(key, new double[] { value });
        }
    }


    // ★★★★★★★★★★★★★★★ useful values

    #region default keys

    public new double t
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
    public new double x
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
    public new double xt
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
    public new double xtt
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
    public new double ytt
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
    public new double xtt_plus_ytt
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
    public new double f
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
    public new double memo
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

    public new double a
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
    public new double v
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
    public new double mu
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

    public new double Sd
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
    public new double Sv
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
    public new double Sa
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


    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// constructor
    /// </summary>
    public TimeHistoryStep()
    {
        ContentsTable = new Dictionary<string, double[]>();
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// clone
    /// </summary>
    /// <returns></returns>
    public new TimeHistoryStep Clone()
    {
        var newHistoryStep = new TimeHistoryStep();
        foreach (var key in ContentsTable.Keys)
            newHistoryStep.ContentsTable[key] = ContentsTable[key];
        return newHistoryStep;
    }


    // ★★★★★★★★★★★★★★★ 

}
