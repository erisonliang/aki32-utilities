
using System;

namespace Aki32_Utilities.Extensions;
public class ProgressCounter
{
    public DateTime StartTime { get; set; }

    public int MaxCount { get; set; }

    public bool ConsoleOutput { get; set; }

    public ProgressCounter(int maxCount, bool? consoleOutput = null)
    {
        StartTime = DateTime.Now;
        MaxCount = maxCount;

        ConsoleOutput = consoleOutput == null ? UtilConfig.ConsoleOutput : consoleOutput.Value;

        if (!ConsoleOutput)
            return;

        Console.WriteLine();
        Console.WriteLine();
    }

    public void WriteCurrentState(int currentStep, int? maxOverwrite = null)
    {
        if (!ConsoleOutput)
            return;

        ConsoleExtension.ClearCurrentConsoleLine();
        Console.Write($"Processing… {100d * currentStep / MaxCount:F1} % ( {currentStep} / {MaxCount} steps)");
    }

    public void WriteDone()
    {
        if (!ConsoleOutput)
            return;

        ConsoleExtension.ClearCurrentConsoleLine();
        Console.Write($"Done! 100 % ( {MaxCount} / {MaxCount} steps)");
        Console.WriteLine();
        Console.WriteLine();
    }
}