using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static partial class ChainableExtensions
{
    public static TimeHistory Wavelet(this TimeHistory inputHistory, string targetIndex, int maxLevel = 2, int D = 2)
    {
        // preprocess
        if (!inputHistory.Columns.Contains(targetIndex))
            throw new InvalidDataException($"\"{targetIndex}\" index (designated by \"targetIndex\") is required in inputHistory");


        // main
        var resultHistory = WaveletExecuter.Execute_DaubechiesWavelet(inputHistory[targetIndex], maxLevel, D);


        // post process
        resultHistory.inputDir = inputHistory.inputDir;
        resultHistory.Name = $"{inputHistory.Name}_Wavelet";
        return resultHistory;
    }
}
