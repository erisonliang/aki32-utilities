using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.OwesomeModels;

using DocumentFormat.OpenXml.EMMA;

using MathNet.Numerics.Distributions;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static partial class ChainableExtensions
{

    /// <summary>
    /// SimpleIntegrate
    /// </summary>
    /// <returns></returns>
    public static TimeHistory CalcIntegrate_Simple(this TimeHistory inputHistory, string targetIndex, string newIndex)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic();


        // main
        var dt = inputHistory.TimeStep;
        for (int i = 0; i < inputHistory.DataRowCount - 1; i++)
        {
            var c = inputHistory.GetStep(i);
            var n = inputHistory.GetStep(i + 1);

            n[newIndex] = c[newIndex] + 0.5 * (c[targetIndex] + n[targetIndex]) * dt;

            inputHistory.SetStep(i + 1, n);
        }



        return inputHistory;
    }

}
