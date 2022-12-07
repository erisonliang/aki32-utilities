﻿using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.OwesomeModels;

using DocumentFormat.OpenXml.EMMA;

using MathNet.Numerics.Distributions;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Calc integrate and add result as a new column
    /// </summary>
    /// <returns></returns>
    public static TimeHistory CalcIntegrate_Simple(this TimeHistory inputHistory, string targetIndex, params string[] newIndexes)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic();
        if (newIndexes.Length == 0)
            newIndexes = new string[] { $"{targetIndex}_Integrated" };


        // main
        var dt = inputHistory.TimeStep;
        var indexChain = new List<string> { targetIndex };
        indexChain.AddRange(newIndexes);

        for (int index = 0; index < indexChain.Count - 1; index++)
        {
            var oldIndex = indexChain[index];
            var newIndex = indexChain[index + 1];

            for (int targetRow = 0; targetRow < inputHistory.DataRowCount - 1; targetRow++)
            {
                var c = inputHistory.GetStep(targetRow);
                var n = inputHistory.GetStep(targetRow + 1);

                n[newIndex] = c[newIndex] + 0.5 * (c[oldIndex] + n[oldIndex]) * dt;

                inputHistory.SetStep(targetRow + 1, n);
            }
        }


        // post process
        return inputHistory;
    }

}
