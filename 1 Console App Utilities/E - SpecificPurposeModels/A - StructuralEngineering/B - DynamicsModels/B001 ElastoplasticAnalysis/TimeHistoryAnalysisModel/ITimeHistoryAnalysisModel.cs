using Aki32_Utilities.Console_App_Utilities.OwesomeModels;

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.StructuralEngineering;
public interface ITimeHistoryAnalysisModel
{
    
    // ★★★★★★★★★★★★★★★ methods

    public TimeHistory Calc(SDoFModel model, TimeHistory wave);

    // ★★★★★★★★★★★★★★★ 

}
