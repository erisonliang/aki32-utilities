using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
/// <summary>
/// 
/// Big summary of https://github.com/dotnet/machinelearning-samples
/// Almost all the examples and data is form above.
/// Examples are tagged by me as found in enum MLNetExampleScenario 
/// 
/// </summary>
public partial class MLNetExampleSummary : MLNetHandler
{

    // ★★★★★★★★★★★★★★★ props

    public MLNetExampleScenario Scenario { get; set; }

    public FileInfo ModelFile { get; set; }


    // ★★★★★★★★★★★★★★★ specific

    public int SpikeDitectionSize = 36;


    // ★★★★★★★★★★★★★★★ 

}
