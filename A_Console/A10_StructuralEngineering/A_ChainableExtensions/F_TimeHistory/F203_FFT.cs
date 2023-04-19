using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static partial class ChainableExtensions
{
    public static TimeHistory FFT(this TimeHistory inputHistory, string targetIndex)
    {
        // preprocess
        if (!inputHistory.Columns.Contains(targetIndex))
            throw new InvalidDataException($"\"{targetIndex}\" index (designated by \"targetIndex\") is required in inputHistory");
        if (!inputHistory.Columns.Contains("t"))
            throw new InvalidDataException("\"t\" index is required in inputHistory");
        if (inputHistory.TimeStep == 0)
            throw new InvalidDataException("\"t\" have to be in correct format.");


        // main
        var resultHistory = FFTExecuter.Execute(inputHistory.TimeStep, inputHistory[targetIndex]);


        // post process
        resultHistory.inputDir = inputHistory.inputDir;
        resultHistory.Name = $"{inputHistory.Name}_FFT";
        return resultHistory;
    }
}
