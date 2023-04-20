using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public interface ITimeHistoryAnalysisModel
{

    // ★★★★★★★★★★★★★★★ methods

    public TimeHistory Calc(SDoFModel model, TimeHistory wave);

    // ★★★★★★★★★★★★★★★ 

}
