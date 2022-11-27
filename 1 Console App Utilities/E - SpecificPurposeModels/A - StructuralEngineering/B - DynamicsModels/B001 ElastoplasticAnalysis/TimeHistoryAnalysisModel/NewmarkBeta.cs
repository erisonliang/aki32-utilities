using Aki32_Utilities.StructuralEngineering;
using Aki32_Utilities.OwesomeModels;

namespace Aki32_Utilities.StructuralEngineering;
public class NewmarkBetaModel : ITimeHistoryAnalysisModel
{

    // ★★★★★★★★★★★★★★★ props

    public double beta { get; set; }
    public double gamma { get; set; }

    public double ConvergeThre { get; set; } = 1e-6;


    // ★★★★★★★★★★★★★★★ inits

    public NewmarkBetaModel(double beta, double gamma = 0.5)
    {
        this.beta = beta;
        this.gamma = gamma;
    }

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run newmark beta method
    /// </summary>
    public TimeHistory Calc(SDoFModel model, TimeHistory wave)
    {
        var resultHistory = wave.Clone();
        resultHistory.Name = $"result_{wave.Name}_{model.EP.GetType().Name}_{GetType().Name}";

        var epModel = model.EP;
        var m = model.m;
        var dt = wave.TimeStep;

        if (model.EP is ElasticModel)
        {
            // simple calc

            var h = model.h;
            var w = model.w;
            var w2 = w * w;

            for (int i = 0; i < resultHistory.DataRowCount - 1; i++)
            {
                var c = resultHistory.GetStep(i);
                var n = resultHistory.GetStep(i + 1);

                var xtt_nume1 = n.ytt + 2 * h * w * (c.xt + 0.5 * c.xtt * dt);
                var xtt_nume2 = w2 * (c.x + c.xt * dt + (0.5 - beta) * c.xtt * dt * dt);
                var xtt_nume = xtt_nume1 + xtt_nume2;
                var xtt_denom = 1 + h * w * dt + beta * w2 * dt * dt;
                n.xtt = -xtt_nume / xtt_denom;
                n.xt = c.xt + 0.5 * (c.xtt + n.xtt) * dt;
                n.x = c.x + c.xt * dt + ((0.5 - beta) * c.xtt + beta * n.xtt) * dt * dt;
                n.xtt_plus_ytt = n.xtt + n.ytt;
                n.f = epModel.TryCalcNextF(n.x);
                epModel.AdoptNextPoint();
                resultHistory.SetStep(i + 1, n);
            }

        }
        else
        {
            // converging calc
            // 1, let var a1
            // 2, calc newmark to get x1
            // 3, calc epModel to get a2
            // 4, converge a1 and a2 with repeating step 1-3 with changing a1

            for (int i = 0; i < resultHistory.DataRowCount - 1; i++)
            {
                var c = resultHistory.GetStep(i);
                var n = resultHistory.GetStep(i + 1);

                var a1 = c.xtt;
                //Console.WriteLine($"{i,4}========================================");
                while (true)
                {
                    n.xt = c.xt + ((1 - gamma) * c.xtt + gamma * a1) * dt;
                    n.x = c.x + c.xt * dt + ((0.5 - beta) * c.xtt + beta * a1) * dt * dt;

                    var R = epModel.TryCalcNextF(n.x);
                    var w = Math.Sqrt(epModel.NextAverageK / m);
                    var h = model.h + epModel.Next_heq;
                    var D = 2 * h * w * c.xt * m;
                    var a2 = -n.ytt - (D + R) / m;

                    //Console.WriteLine($"{a1,10}, {a2,10} = {Math.Abs(a1 - a2),10}");
                    if (Math.Abs(a1 - a2) < ConvergeThre)
                    {
                        n.f = R;
                        n.xtt = a1;
                        n.xtt_plus_ytt = n.xtt + n.ytt;
                        n["h"] = h;
                        n["K"] = epModel.NextAverageK;
                        break;
                    }
                    else
                    {
                        a1 = a2;
                        continue;
                    }
                }

                epModel.AdoptNextPoint();
                resultHistory.SetStep(i + 1, n);
            }

        }

        return resultHistory;
    }

    // ★★★★★★★★★★★★★★★

}
