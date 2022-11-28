using Aki32_Utilities.General.ChainableExtensions;

namespace Aki32_Utilities.StructuralEngineering.ChainableExtensions;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Random Decrement
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_Rainflow/{inputFile.Name}</param>
    /// <returns></returns>
    public static FileInfo CalcRandomDecrement(this FileInfo inputFile, FileInfo? outputFile)
    {
        // TODO
        throw new NotImplementedException();



        //// Define IO paths
        //var input = new FileInfo(Path.Combine(basePath, "B003 RDTechnique", @"input.csv"));

        //// Read input csv
        //var rd = RDTechniqueCalculator.FromCsv(input);

        //// Calc and show
        //rd.Calc(200);
        //rd.InputHistory.DrawLineGraph("v");
        //rd.ResultHistory.DrawLineGraph("v");

        //// Calc AttenuationConstant and show
        //var att = rd.CalcAttenuationConstant(4, true);
        //Console.WriteLine();
        //Console.WriteLine($"result h = {att}");


    }


}
