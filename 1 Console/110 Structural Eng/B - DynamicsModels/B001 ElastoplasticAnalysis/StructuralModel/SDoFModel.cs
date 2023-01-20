using Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;
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
    public static List<TimeHistory> CalcResponseSpectrum(double[] TList, double[] hList, TimeHistory wave, ITimeHistoryAnalysisModel thaModel, ElastoplasticCharacteristicBase ep = null)
    {
        if (ep == null)
            ep = new ElasticModel(1);

        Console.WriteLine("============================================");
        Console.WriteLine("calculating…");

        var SdList = new TimeHistory("1 Sd");
        var SvList = new TimeHistory("2 Sv");
        var SaList = new TimeHistory("3 Sa");

        foreach (var T in TList)
        {
            var u = TList.ToList().IndexOf(T) * hList.Length;
            var b = TList.Length * hList.Length;
            Console.Write($"{u} / {b} ( {100 * u / b} %)");

            var Sd = new TimeHistoryStep();
            var Sv = new TimeHistoryStep();
            var Sa = new TimeHistoryStep();
            Sd["T"] = T;
            Sv["T"] = T;
            Sa["T"] = T;

            foreach (var h in hList)
            {
                var targetStructure = FromT(T, h, ep);
                var resultSpectrum = targetStructure.Calc(wave, thaModel).GetSpectrumSet();
                Sd[$"h={h:F4}"] = resultSpectrum.Sd;
                Sv[$"h={h:F4}"] = resultSpectrum.Sv;
                Sa[$"h={h:F4}"] = resultSpectrum.Sa;
            }

            SdList.AppendStep(Sd);
            SvList.AppendStep(Sv);
            SaList.AppendStep(Sa);

            ConsoleExtension.ClearCurrentConsoleLine();
        }

        Console.WriteLine("calculation finished");
        Console.WriteLine("============================================");

        return new List<TimeHistory> { SdList, SvList, SaList };
    }

    // ★★★★★★★★★★★★★★★ 

}
