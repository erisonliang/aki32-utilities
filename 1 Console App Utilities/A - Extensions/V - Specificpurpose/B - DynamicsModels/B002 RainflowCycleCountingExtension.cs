using Aki32_Utilities.Extensions;

using System.Text;

namespace StructuralEngineering_Utilities.DynamicsModels;
public static partial class Extensions
{

    /// <summary>
    /// Rainflow
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_Rainflow/{inputFile.Name}</param>
    /// <returns></returns>
    public static FileInfo Rainflow(this FileInfo inputFile, FileInfo? outputFile, double C, double beta, bool consoleOutput = false, bool outputRainFlowResultHistory = true, bool outputRainBranches = false, FileInfo? outputFileForRainBranches = null)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS

        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName!, "output_Rainflow", inputFile.Name));
        if (!outputFile.Directory!.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        if (outputRainBranches)
        {
            if (outputFileForRainBranches is null)
                outputFileForRainBranches = new FileInfo(Path.Combine(inputFile.DirectoryName!, "output_Rainflow", $"{Path.GetFileNameWithoutExtension(inputFile.Name)}_Branches.csv"));
            if (!outputFileForRainBranches.Directory!.Exists) outputFileForRainBranches.Directory.Create();
            if (outputFileForRainBranches.Exists) outputFileForRainBranches.Delete();
        }

        // main
        var rainflow = RainflowCalculator.FromCsv(inputFile);
        rainflow.CalcRainflow(C, beta, consoleOutput);

        // output
        if (outputRainFlowResultHistory && outputRainBranches)
        {
            // If both, SaveResultHistoryToCsv path will be prior 
            rainflow.SaveResultHistoryToCsv(outputFile);
            rainflow.SaveRainBranchesToCsv(outputFileForRainBranches);
        }
        else if (outputRainBranches)
        {
            rainflow.SaveRainBranchesToCsv(outputFile);
        }
        else
        {
            // Even if neither is selected, SaveResultHistoryToCsvwill be saved.
            rainflow.SaveResultHistoryToCsv(outputFile);
        }

        return outputFile;
    }

    /// <summary>
    /// Rainflow
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_Rainflow</param>
    /// <returns></returns>
    public static DirectoryInfo Rainflow_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, double C, double beta,
        bool outputRainFlowResultHistory = true,
        bool outputRainBranches = false,
        DirectoryInfo? outputDirForRainBranches = null,
        int maxDegreeOfParallelism = 999)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);
        if (outputRainFlowResultHistory && outputRainBranches)
        {
            outputDirForRainBranches ??= outputDir;
            if (!outputDirForRainBranches!.Exists)
                outputDirForRainBranches.Create();
        }


        // main
        var ProcessOne = (FileInfo inF, FileInfo outF) =>
        {
            if (outputRainFlowResultHistory && outputRainBranches)
            {
                inF.Rainflow(outF, C, beta,
                    outputRainFlowResultHistory: outputRainFlowResultHistory,
                    outputRainBranches: outputRainBranches,
                    outputFileForRainBranches: new FileInfo(Path.Combine(outputDirForRainBranches!.FullName, $"{Path.GetFileNameWithoutExtension(inF.Name)}_Branches.csv"))
                    );
            }
            else
            {
                inF.Rainflow(outF, C, beta,
                    outputRainFlowResultHistory: outputRainFlowResultHistory,
                    outputRainBranches: outputRainBranches);
            }
        };

        inputDir.Loop(outputDir,
            ProcessOne,
            maxDegreeOfParallelism: maxDegreeOfParallelism
            );


        return outputDir;

    }

}
