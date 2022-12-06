using Aki32Utilities.ConsoleAppUtilities.OwesomeModels;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public interface ITimeHistoryAnalysisModel
{
    
    // ★★★★★★★★★★★★★★★ methods

    public TimeHistory Calc(SDoFModel model, TimeHistory wave);

    // ★★★★★★★★★★★★★★★ 

}
