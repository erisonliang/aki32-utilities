﻿
namespace Aki32_Utilities.Extensions;
public class ProgressManager
{

    // ★★★★★★★★★★★★★★★ props

    public DateTime StartTime { get; set; }
    public TimeSpan ElapsedTime => DateTime.Now - StartTime;

    public int MaxStep { get; set; }

    public bool ConsoleOutput { get; set; } = true;
    public bool WritePercentage { get; set; } = true;
    public bool WriteSteps { get; set; } = true;
    public bool WriteProgressBar { get; set; } = true;
    public bool WriteElapsedTime { get; set; } = false;
    public bool WriteEstimateTime { get; set; } = true;
    public bool WriteErrorCount { get; set; } = true;

    public int ProgressBarBoxCount { get; set; } = 15;

    public List<string> ErrorMessages { get; set; } = new List<string>();


    // ★★★★★★★★★★★★★★★ inits

    public ProgressManager(int maxStep, bool? consoleOutput = null)
    {
        StartTime = DateTime.Now;
        MaxStep = maxStep;

        ConsoleOutput = consoleOutput == null ? UtilConfig.ConsoleOutput : consoleOutput.Value;

        if (!ConsoleOutput)
            return;

        Console.WriteLine();
        Console.WriteLine();
    }


    // ★★★★★★★★★★★★★★★ methods

    public void WriteCurrentState(int currentStep, bool useConsoleOverwrite = true)
    {
        if (!ConsoleOutput)
            return;

        if (useConsoleOverwrite)
            ConsoleExtension.ClearCurrentConsoleLine();

        Console.Write(GetCurrentStateString("Processing…", currentStep));
    }

    public void WriteDone(bool writeErrorMessages = true, bool useConsoleOverwrite = true)
    {
        if (!ConsoleOutput)
            return;

        if (useConsoleOverwrite)
        {
            Thread.Sleep(10); // make sure another rewrite process finished
            ConsoleExtension.ClearCurrentConsoleLine();
        }

        Console.Write(GetCurrentStateString("Done!", MaxStep));
        Console.WriteLine();
        Console.WriteLine();

        if (writeErrorMessages && ErrorMessages.Count > 0)
        {
            ConsoleExtension.WriteLineWithColor($"(!) {ErrorMessages.Count} ERRORS OCCURRED IN TOTAL.", ConsoleColor.Red);

            for (int i = 0; i < ErrorMessages.Count; i++)
                ConsoleExtension.WriteLineWithColor($"ERROR{i + 1}: {ErrorMessages[i]}", ConsoleColor.Red);

            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public void AddErrorMessage(string errorMessage) => ErrorMessages.Add(errorMessage);

    private string GetCurrentStateString(string message, double currentStep)
    {
        var s = $"{message}, ";

        var percentage = MaxStep == 0 ? 100 : 100d * currentStep / MaxStep;

        if (WriteSteps)
            s += $"{currentStep}/{MaxStep} steps, ";

        if (WritePercentage)
            s += $"{percentage:F0} %, ";

        if (WriteProgressBar)
        {
            var s1 = Enumerable.Repeat('■', (int)(percentage * ProgressBarBoxCount / 100.001 + 1)).ToString_Extension();
            var s2 = Enumerable.Repeat('＿', (int)((100 - percentage) * ProgressBarBoxCount / 100)).ToString_Extension();
            s += $"{s1}{s2}, ";
        }

        if (WriteElapsedTime)
        {
            if (ElapsedTime.TotalMinutes > 1.0)
                s += $"{ElapsedTime.TotalMinutes:F1} min elapsed, ";
            else
                s += $"{ElapsedTime.TotalSeconds:F1} sec elapsed, ";
        }

        if (WriteEstimateTime)
        {
            if (percentage > 0)
            {
                var estimated = ElapsedTime * ((100 - percentage) / percentage);
                if (estimated.TotalMinutes > 1.0)
                    s += $"{estimated.TotalMinutes:F1} min left, ";
                else
                    s += $"{estimated.TotalSeconds:F1} sec left, ";
            }
        }

        if (WriteErrorCount)
            if (ErrorMessages.Count > 0)
                s += $"(!) {ErrorMessages.Count} error{(ErrorMessages.Count == 1 ? "" : "s")}, ";

        return s.TrimEnd(' ', ',');
    }


    // ★★★★★★★★★★★★★★★

}