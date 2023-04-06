using Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class SDoFModel
{

    // ★★★★★★★★★★★★★★★ props

    public ElastoplasticCharacteristicBase EP { get; set; }

    public double h { get; set; }
    public double m { get; set; }

    public double w => Math.Sqrt(EP.CurrentK / m);
    public double T => 2 * Math.PI / w;

    public double wo;
    public double To;

    // ★★★★★★★★★★★★★★★ inits

    private SDoFModel(double m, double h, ElastoplasticCharacteristicBase ep)
    {
        EP = ep;

        this.h = h;
        this.m = m;

        wo = w;
        To = T;
    }
    public static SDoFModel FromM(double m, double h = 0, ElastoplasticCharacteristicBase? ep = null)
    {
        if (ep == null)
            ep = new ElasticModel(1);
        return new SDoFModel(m, h, ep);
    }
    public static SDoFModel FromT(double T, double h = 0, ElastoplasticCharacteristicBase? ep = null)
    {
        if (ep == null)
            ep = new ElasticModel(1);
        var initialwo = 2 * Math.PI / T;
        var m = ep.K1 / (initialwo * initialwo);
        return new SDoFModel(m, h, ep);
    }

    // ★★★★★★★★★★★★★★★ dynamic methods

    public TimeHistory Calc(TimeHistory wave, ITimeHistoryAnalysisModel thaModel)
    {
        return thaModel.Calc(this, wave);
    }

    // ★★★★★★★★★★★★★★★ static methods

    /// <summary>
    /// Calculate response spectra for lists of T and h
    /// </summary>
    /// <param name="TList"></param>
    /// <param name="hList"></param>
    /// <param name="wave"></param>
    /// <param name="thaModel"></param>
    /// <param name="ep"></param>
    /// <returns>
    /// List<TimeHistory> {Sd, Sv, Sa};
    /// </returns>
    public static List<TimeHistory> CalcResponseSpectrum(double[] TList, double[] hList, TimeHistory wave, ITimeHistoryAnalysisModel thaModel,
        ElastoplasticCharacteristicBase? ep = null,
        int maxDegreeOfParallelism = 15
        )
    {
        ep ??= new ElasticModel(1);

        var SdList = new TimeHistory("Sd");
        var SvList = new TimeHistory("Sv");
        var SaList = new TimeHistory("Sa");

        using var progress = new ProgressManager(TList.Length * hList.Length);
        progress.StartAutoWrite(100);

        var option = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
        Parallel.ForEach(TList, option, T =>
        {
            var epc = ep.Clone() as ElastoplasticCharacteristicBase;
            var Sd = new TimeHistoryStep();
            var Sv = new TimeHistoryStep();
            var Sa = new TimeHistoryStep();

            Sd["T"] = T;
            Sv["T"] = T;
            Sa["T"] = T;

            foreach (var h in hList)
            {
                var targetStructure = FromT(T, h, epc);
                var resultSpectrum = targetStructure.Calc(wave, thaModel).GetSpectrumSet();
                Sd[$"h={h:F4}"] = resultSpectrum.Sd;
                Sv[$"h={h:F4}"] = resultSpectrum.Sv;
                Sa[$"h={h:F4}"] = resultSpectrum.Sa;

                progress.CurrentStep++;
            }

            lock (SdList)
                SdList.AppendStep(Sd);
            lock (SvList)
                SvList.AppendStep(Sv);
            lock (SaList)
                SaList.AppendStep(Sa);

        });

        SdList.OrderBy("T");
        SvList.OrderBy("T");
        SaList.OrderBy("T");

        progress.WriteDone();

        return new List<TimeHistory> { SdList, SvList, SaList };
    }

    // ★★★★★★★★★★★★★★★ 

}
