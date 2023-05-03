using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class HilberHughesTaylorModel : ITimeHistoryAnalysisModel
{

    // ★★★★★★★★★★★★★★★ props

    public double beta { get; set; }
    public double gamma { get; set; }

    public double ConvergeThre { get; set; } = 1e-6;


    // ★★★★★★★★★★★★★★★ inits

    public HilberHughesTaylorModel(double beta, double gamma = 0.5)
    {
        this.beta = beta;
        this.gamma = gamma;

        throw new NotImplementedException();

    }

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run HHT method
    /// </summary>
    public TimeHistory Calc(SDoFModel model, TimeHistory wave)
    {
        var resultHistory = wave.Clone();
        resultHistory.Name = $"{wave.Name}_{model.EP.GetType().Name}_{GetType().Name}_Result";

        var epModel = model.EP;
        var m = model.m;
        var dt = wave.TimeStep;

        if (model.EP is ElasticModel)
        {
            // simple calc

            throw new NotImplementedException();

        }
        else
        {
            // converging calc
            // 1, let var a1
            // 2, calc newmark to get x1
            // 3, calc epModel to get a2
            // 4, converge a1 and a2 with repeating step 1-3 with changing a1

            throw new NotImplementedException();

        }

        return resultHistory;
    }

    // ★★★★★★★★★★★★★★★

}
