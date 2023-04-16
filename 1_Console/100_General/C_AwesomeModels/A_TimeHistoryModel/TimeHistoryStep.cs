

namespace Aki32Utilities.ConsoleAppUtilities.General;
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
            if (ContentsTable.ContainsKey(key))
                return ContentsTable[key][0];

            ContentsTable.Add(key, new double[] { 0 });
            //Console.WriteLine($"ERROR : {key} は定義されていません。空集合を作成しました。");
            //throw new KeyNotFoundException($"{key} は定義されていません。");
            return ContentsTable[key][0];
        }
        set
        {
            if (ContentsTable.ContainsKey(key))
                ContentsTable[key][0] = value;
            else
                ContentsTable.Add(key, new double[] { value });
        }
    }


    // ★★★★★★★★★★★★★★★ useful values

    #region default keys

    public new double t
    {
        get => this[PresetIndex.t];
        set => this[PresetIndex.t] = value;
    }
    public new double x
    {
        get => this[PresetIndex.x];
        set => this[PresetIndex.x] = value;
    }
    public new double xt
    {
        get => this[PresetIndex.xt];
        set => this[PresetIndex.xt] = value;
    }
    public new double xtt
    {
        get => this[PresetIndex.xtt];
        set => this[PresetIndex.xtt] = value;
    }
    public new double y
    {

        get => this[PresetIndex.y];
        set => this[PresetIndex.y] = value;
    }
    public new double yt
    {

        get => this[PresetIndex.yt];
        set => this[PresetIndex.yt] = value;
    }
    public new double ytt
    {
        get => this[PresetIndex.ytt];
        set => this[PresetIndex.ytt] = value;
    }
    public new double xtt_plus_ytt
    {

        get => this[PresetIndex.xtt_plus_ytt];
        set => this[PresetIndex.xtt_plus_ytt] = value;
    }
    public new double f
    {
        get => this[PresetIndex.f];
        set => this[PresetIndex.f] = value;
    }
    public new double memo
    {
        get => this[PresetIndex.memo];
        set => this[PresetIndex.memo] = value;
    }

    public new double a
    {
        get => this[PresetIndex.a];
        set => this[PresetIndex.a] = value;
    }
    public new double v
    {
        get => this[PresetIndex.v];
        set => this[PresetIndex.v] = value;
    }
    public new double mu
    {
        get => this[PresetIndex.mu];
        set => this[PresetIndex.mu] = value;
    }

    public new double Sd
    {
        get => this[PresetIndex.Sd];
        set => this[PresetIndex.Sd] = value;
    }
    public new double Sv
    {
        get => this[PresetIndex.Sv];
        set => this[PresetIndex.Sv] = value;
    }
    public new double Sa
    {
        get => this[PresetIndex.Sa];
        set => this[PresetIndex.Sa] = value;
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
