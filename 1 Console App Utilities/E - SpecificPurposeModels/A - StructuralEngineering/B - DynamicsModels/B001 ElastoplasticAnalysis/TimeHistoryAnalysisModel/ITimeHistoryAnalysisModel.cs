using Aki32_Utilities.OwesomeModels;

namespace Aki32_Utilities.StructuralEngineering.DynamicsModels;
public interface ITimeHistoryAnalysisModel
{
    
    // ★★★★★★★★★★★★★★★ methods

    public TimeHistory Calc(SDoFModel model, TimeHistory wave);

    // ★★★★★★★★★★★★★★★ 

}
