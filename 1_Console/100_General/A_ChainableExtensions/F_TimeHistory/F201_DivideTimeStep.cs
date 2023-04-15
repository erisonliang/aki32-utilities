

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Divide Data Linearly
    /// </summary>
    /// <param name="inputHistory"></param>
    /// <param name="divideBy"></param>
    /// <param name="targetIndex"></param>
    /// <returns></returns>
    public static TimeHistory GetTimeDividedHistory(this TimeHistory inputHistory, int divideBy, string targetIndex)
    {
        // preprocess
        var outputHistory = inputHistory.Clone().DropAllColumns();


        // main
        for (int i = 0; i < inputHistory.DataRowCount - 1; i++)
        {
            var current = inputHistory.GetStep(i);
            var next = inputHistory.GetStep(i + 1);

            for (int j = 0; j < divideBy; j++)
            {
                var addingRow = new TimeHistoryStep();

                addingRow.t = (current.t * (divideBy - j) + next.t * j) / divideBy;
                addingRow[targetIndex] = (current[targetIndex] * (divideBy - j) + next[targetIndex] * j) / divideBy;

                outputHistory.AppendStep(addingRow);
            }

        }

        // add the last row
        outputHistory.AppendStep(inputHistory.GetTheLastStep());


        // post process
        return outputHistory;
    }

}
