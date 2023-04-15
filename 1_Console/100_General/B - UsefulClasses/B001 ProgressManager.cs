

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class ProgressManager : IDisposable
{

    // ★★★★★★★★★★★★★★★ props

    public DateTime StartTime { get; set; } = DateTime.Now;
    public TimeSpan ElapsedTimeFromStart => DateTime.Now - StartTime;

    public DateTime LastCountUpTime { get; set; } = DateTime.Now;
    public TimeSpan ElapsedTimeFromLastCountUp => DateTime.Now - LastCountUpTime;

    public long MaxStep { get; set; }
    private long currentStep;
    public long CurrentStep
    {
        get
        {
            return currentStep;
        }
        set
        {
            LastCountUpTime = DateTime.Now;
            currentStep = value;
        }
    }

    public bool ConsoleOutput { get; set; } = true;
    public bool WritePercentage { get; set; } = true;
    public bool WriteSteps { get; set; } = true;
    public bool WriteProgressBar { get; set; } = true;
    public bool WriteElapsedTime { get; set; } = false;
    public bool WriteEstimateTime { get; set; } = true;
    public bool WriteErrorCount { get; set; } = true;

    public int ProgressBarBoxCount { get; set; } = 15;

    public List<string> ErrorMessages { get; set; } = new List<string>();

    private bool isRewriting = false;
    private System.Timers.Timer autoWriteTimer;


    // ★★★★★★★★★★★★★★★ inits

    public ProgressManager(long maxStep, bool? consoleOutput = null)
    {
        StartTime = DateTime.Now;
        LastCountUpTime = DateTime.Now;
        MaxStep = maxStep;

        consoleOutput ??= true;
        ConsoleOutput = consoleOutput.Value && UtilConfig.ConsoleOutput_Contents;

        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();
        UtilConfig.StopTemporary_ConsoleOutput_Contents();

        if (!ConsoleOutput)
            return;

        Console.WriteLine();
        Console.WriteLine();
    }

    public void Dispose()
    {
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        UtilConfig.TryRestart_ConsoleOutput_Contents();
        GC.SuppressFinalize(this);
    }


    // ★★★★★★★★★★★★★★★ methods

    public void WriteCurrentState(long? currentStep = null, bool useConsoleOverwrite = true)
    {
        if (currentStep != null)
            CurrentStep = currentStep.Value;

        if (!ConsoleOutput)
            return;

        if (isRewriting)
            return;

        isRewriting = true;

        if (useConsoleOverwrite)
            ConsoleExtension.ClearCurrentConsoleLine();
        Console.Write(GetCurrentStateString("Processing…"));

        isRewriting = false;
    }

    public void WriteDone(bool writeErrorMessages = true, bool useConsoleOverwrite = true)
    {
        CurrentStep = MaxStep;
        autoWriteTimer?.Stop();
        autoWriteTimer?.Dispose();

        if (!ConsoleOutput)
            return;

        if (useConsoleOverwrite)
        {
            Thread.Sleep(10); // make sure another rewrite process finished
            ConsoleExtension.ClearCurrentConsoleLine();
        }

        WriteElapsedTime = true;
        Console.Write(GetCurrentStateString("Done!"));
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

    public void StartAutoWrite(int interval = 100, bool useConsoleOverwrite = true, Action? additionalAction = null)
    {
        autoWriteTimer = new System.Timers.Timer(interval);
        autoWriteTimer.Elapsed += delegate
        {
            additionalAction?.Invoke();
            WriteCurrentState(useConsoleOverwrite: useConsoleOverwrite);
        };
        autoWriteTimer.Start();
    }

    private string GetCurrentStateString(string message)
    {
        var s = $"{message}, ";

        var percentage = MaxStep == 0 ? 100 : 100d * CurrentStep / MaxStep;

        if (WriteSteps)
            s += $"{CurrentStep}/{MaxStep} steps, ";

        if (WritePercentage)
            s += $"{percentage:F0} %, ";

        if (WriteProgressBar)
        {
            var s1 = Enumerable.Repeat('■', (int)(percentage * ProgressBarBoxCount / 100.001 + 1)).ToString_Extension();
            var s2 = Enumerable.Repeat('_', (int)((100 - percentage) * ProgressBarBoxCount / 100)).ToString_Extension();
            s += $"{s1}{s2}, ";
        }

        if (WriteElapsedTime)
        {
            if (ElapsedTimeFromStart.TotalMinutes > 1.0)
                s += $"{ElapsedTimeFromStart.TotalMinutes:F1} mins elapsed, ";
            else
                s += $"{ElapsedTimeFromStart.TotalSeconds:F1} secs elapsed, ";
        }

        if (WriteEstimateTime)
        {
            if (percentage > 0)
            {
                var estimated = (ElapsedTimeFromStart - ElapsedTimeFromLastCountUp) * ((100 - percentage) / percentage) - ElapsedTimeFromLastCountUp;
                if (estimated.TotalMinutes > 1.0)
                    s += $"{estimated.TotalMinutes:F1} mins left, ";
                else if (estimated.TotalSeconds < 0.0)
                    s += $"{0:F1} secs left, ";
                else
                    s += $"{estimated.TotalSeconds:F1} secs left, ";
            }
        }

        if (WriteErrorCount)
            if (ErrorMessages.Count > 0)
                s += $"(!) {ErrorMessages.Count} error{(ErrorMessages.Count == 1 ? "" : "s")}, ";

        return s.TrimEnd(' ', ',');
    }


    // ★★★★★★★★★★★★★★★

}