using Aki32_Utilities.Console_App_Utilities.OwesomeModels;

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.StructuralEngineering;
public static class TimeHistoryExtension
{
    /// <summary>
    /// Get response spectrum set from
    /// </summary>
    /// <returns></returns>
    public static TimeHistoryStep GetSpectrumSet(this TimeHistory th)
    {
        return new TimeHistoryStep
        {
            Name = $"{th.Name}_SpectrumSet",
            Sd = th.x.Max(Math.Abs),
            Sv = th.xt.Max(Math.Abs),
            Sa = th.xtt_plus_ytt.Max(Math.Abs)
        };
    }

}
