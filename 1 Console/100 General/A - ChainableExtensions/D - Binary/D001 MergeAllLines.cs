using System.Text;

using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// merge all file's lines in one file
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="skipRowCount"></param>
    /// <returns></returns>
    public static FileInfo MergeAllLines(this DirectoryInfo inputDir, FileInfo? outputFile, int skipRowCount = 0)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir!, $"output.txt");


        // main
        var inputFiles = inputDir
            .GetFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x => (x.FullName != outputFile!.FullName))
            .Sort()
            .ToArray();

        var maxRetryCount = 5;
        var progress = new ProgressManager(inputFiles.Count());
        progress.StartAutoWrite(100);

        using var sw = new StreamWriter(outputFile!.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));
        foreach (var inputFile in inputFiles)
        {
            try
            {
                var retryCount = 0;
                while (true)
                {
                    try
                    {
                        var inputTexts = File.ReadLines(inputFile.FullName, Encoding.GetEncoding("SHIFT_JIS")).ToArray();
                        for (int i = skipRowCount; i < inputTexts.Length; i++)
                        {
                            sw.WriteLine(inputTexts[i]);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (++retryCount < maxRetryCount)
                        {
                            Thread.Sleep(100);
                            GC.Collect();
                            continue;
                        }
                        progress.AddErrorMessage($"{inputFile.FullName}, {ex.Message}");
                        break;
                    }
                }
            }
            finally
            {
                progress.CurrentStep++;
            }
        }

        progress.WriteDone();


        // post process
        return outputFile!;
    }

}