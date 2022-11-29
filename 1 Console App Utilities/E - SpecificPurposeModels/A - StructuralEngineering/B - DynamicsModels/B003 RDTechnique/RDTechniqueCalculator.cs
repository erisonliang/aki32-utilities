using Aki32_Utilities.OwesomeModels;

namespace Aki32_Utilities.SpecificPurposeModels.StructuralEngineering;
public class RDTechniqueCalculator
{

    // ★★★★★★★★★★★★★★★ props

    public TimeHistory InputHistory { get; set; }
    public TimeHistory ResultHistory { get; set; }

    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// Forbid public instantiate
    /// </summary>
    private RDTechniqueCalculator() { }

    /// <summary>
    /// Construct from csv
    /// </summary>
    /// <param name="inputCsvPath"></param>
    /// <exception cref="Exception"></exception>
    public static RDTechniqueCalculator FromCsv(FileInfo inputCsv)
    {
        var rd = new RDTechniqueCalculator();
        rd.InputHistory = TimeHistory.FromCsv(inputCsv, new string[] { "t", "v" });
        return rd;
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Calculate RD Technique
    /// </summary>
    /// <returns></returns>
    public TimeHistory Calc(int resultStepLength, int maxOverlayCount = int.MaxValue, int skipingInitialPeakCount = 0)
    {
        // init
        ResultHistory = InputHistory.Clone();
        ResultHistory.DropAllColumns();
        ResultHistory.t = Enumerable.Range(0, resultStepLength).Select(x => x * InputHistory.TimeStep).ToArray();
        ResultHistory.Name += "_RDTechnique";
        int peakCount = 0;
        int overlayCount = 0;


        // main
        // ごり押し。極大を認識したら結果に足し合わせる。
        for (int i = 1; i < InputHistory.DataRowCount - resultStepLength - 1; i++)
        {
            if (maxOverlayCount <= overlayCount)
            {
                Console.WriteLine($"Successfully stopped: overlayCount count reached maxOverlayCount");
                break;
            }

            var lastStep = InputHistory.GetStep(i - 1);
            var currentStep = InputHistory.GetStep(i);
            var nextStep = InputHistory.GetStep(i + 1);

            // Simple peak detection
            if (lastStep.v < currentStep.v && currentStep.v >= nextStep.v)
            {
                peakCount++;

                // Skip when in skipping state 
                if (skipingInitialPeakCount >= peakCount)
                    continue;

                // Overlay
                overlayCount++;
                for (int j = 0; j < resultStepLength; j++)
                {
                    ResultHistory.v[j] += InputHistory.v[i + j];
                }
            }
        }

        #region Show results

        if (skipingInitialPeakCount > 0)
            Console.WriteLine($"Skipped  : {peakCount - overlayCount} / {skipingInitialPeakCount}");

        if (maxOverlayCount == int.MaxValue)
            Console.WriteLine($"Overlayed: {overlayCount}");
        else
            Console.WriteLine($"Overlayed: {overlayCount} / {maxOverlayCount}");

        Console.WriteLine();

        #endregion

        return ResultHistory;
    }

    /// <summary>
    /// Calc attenuation constant from result
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public double CalcAttenuationConstant(int checkPeakCount, bool consoleOutput = false)
    {
        var peaks = new List<double>();
        var attenuations = new List<double>();
        int peakCount = 0;

        void AddPeak(TimeHistoryStep addingStep)
        {
            if (consoleOutput)
                Console.WriteLine($"Peak {peaks.Count}: t={addingStep.t:F2}, v={addingStep.v}");
            peaks.Add(addingStep.v);
        }

        AddPeak(ResultHistory.GetStep(0));

        // Find peaks
        for (int i = 1; i < ResultHistory.DataRowCount - 1; i++)
        {
            if (checkPeakCount <= peakCount)
                break;

            var lastStep = ResultHistory.GetStep(i - 1);
            var currentStep = ResultHistory.GetStep(i);
            var nextStep = ResultHistory.GetStep(i + 1);

            // Simple peak detection
            if (lastStep.v < currentStep.v && currentStep.v >= nextStep.v)
            {
                peakCount++;
                AddPeak(currentStep);
            }
        }


        if (consoleOutput)
            Console.WriteLine();

        // Calc attenuations
        var initialPeak = peaks[0];
        for (int i = 1; i < peaks.Count; i++)
        {
            var currentPeak = peaks[i];
            var delta = 1d / i * Math.Log(initialPeak / currentPeak);
            var attenuation = delta / 2 / Math.PI;
            if (consoleOutput)
                Console.WriteLine($"Attenuation {i}: h={attenuation:F4}");
            attenuations.Add(attenuation);
        }

        return attenuations.Last();
    }


    // ★★★★★★★★★★★★★★★ sugar

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileInfo SaveResultHistoryToCsv(FileInfo? outputFile = null)
    {
        ResultHistory.SaveToCsv(outputFile);
        return outputFile;
    }


    // ★★★★★★★★★★★★★★★ 

}
