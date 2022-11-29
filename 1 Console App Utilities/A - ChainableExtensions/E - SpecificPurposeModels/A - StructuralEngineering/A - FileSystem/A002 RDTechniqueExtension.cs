using Aki32_Utilities.General;

namespace Aki32_Utilities.StructuralEngineering;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Random Decrement
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_Rainflow/{inputFile.Name}</param>
    /// <returns></returns>
    public static FileInfo CalcRD(this FileInfo inputFile, FileInfo? outputFile, int resultStepLength, int maxOverlayCount = int.MaxValue, int skipingInitialPeakCount = 0)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        var rd = RDTechniqueCalculator.FromCsv(inputFile);

        // calc convolution
        rd.Calc(resultStepLength, maxOverlayCount, skipingInitialPeakCount);
        rd.ResultHistory.SaveToCsv(outputFile);

        // calc attenuationConstant
        var att = rd.CalcAttenuationConstant(4, true);
        rd.ResultHistory["h"][0] = att;

        // save
        rd.ResultHistory.SaveToCsv(outputFile);


        // post process
        return outputFile!;
    }


}
