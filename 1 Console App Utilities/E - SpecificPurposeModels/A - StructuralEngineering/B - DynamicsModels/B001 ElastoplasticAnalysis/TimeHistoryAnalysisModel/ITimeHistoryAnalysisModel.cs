using Aki32_Utilities.OwesomeModels;

namespace StructuralEngineering_Utilities.DynamicsModels;
public interface ITimeHistoryAnalysisModel
{
    
    // ★★★★★★★★★★★★★★★ methods

    public TimeHistory Calc(SDoFModel model, TimeHistory wave);

    // ★★★★★★★★★★★★★★★ 

}
