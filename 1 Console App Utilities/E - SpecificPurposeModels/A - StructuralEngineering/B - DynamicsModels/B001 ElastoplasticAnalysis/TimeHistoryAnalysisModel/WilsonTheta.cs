using Aki32_Utilities.Console_App_Utilities.OwesomeModels;

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.StructuralEngineering;
public class WilsonTheta : ITimeHistoryAnalysisModel
{

    // ★★★★★★★★★★★★★★★ props

    public double theta { get; set; }
    public double beta { get; set; }
    public double gamma { get; set; }

    public double ConvergeThre { get; set; } = 1e-6;

    // ★★★★★★★★★★★★★★★ inits

    public WilsonTheta(double theta = 1.4, double beta = 0.25, double gamma = 0.5)
    {
        this.theta = theta;
        this.beta = beta;
        this.gamma = gamma;
    }

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run wilson theta method
    /// </summary>
    public TimeHistory Calc(SDoFModel model, TimeHistory wave)
    {
        var resultHistory = wave.Clone();
        resultHistory.Name = $"result_{wave.Name}_{model.EP.GetType().Name}_{GetType().Name}";

        var epModel = model.EP;
        var m = model.m;
        var dt = wave.TimeStep;
        var tdt = theta * dt;

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

                var A0 = (1 + 2 * h * w * tdt * gamma + w2 * tdt * tdt * beta) * theta;
                var A1 = -w2;
                var A2 = -(2 * h * w + w2 * tdt);
                var A3 = -(2 * h * w * tdt * (1 - gamma * theta) + w2 * tdt * tdt * (0.5 - beta * theta) + (1 - theta));
                var A4 = -(1 - theta);
                var A5 = -theta;
                n.xtt = (A1 * c.x + A2 * c.xt + A3 * c.xtt + A4 * c.ytt + A5 * n.ytt) / A0;

                n.xtt_plus_ytt = n.xtt + n.ytt;
                n.xt = c.xt + ((1 - gamma) * c.xtt + gamma * n.xtt) * dt;
                n.x = c.x + c.xt * dt + ((0.5 - beta) * c.xtt + beta * n.xtt) * dt * dt;

                n.f = epModel.TryCalcNextF(n.x);
                epModel.AdoptNextPoint();
                resultHistory.SetStep(i + 1, n);
            }

        }
        else
        {
            // converging calc
            // 1, let var a1
            // 2, calc wilson to get x1
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
                    n.xt = c.xt + ((1 - gamma) * c.xtt + gamma * a1) * tdt;
                    n.x = c.x + c.xt * tdt + ((0.5 - beta) * c.xtt + beta * a1) * tdt * tdt;

                    var R = epModel.TryCalcNextF(n.x);
                    var w = Math.Sqrt(epModel.NextAverageK / m);
                    var h = model.h + epModel.Next_heq;
                    var D = 2 * h * w * c.xt * m;
                    var a2 = -n.ytt - (D + R) / m;

                    //Console.WriteLine($"{a1,10}, {a2,10} = {Math.Abs(a1 - a2),10}");
                    if (Math.Abs(a1 - a2) < ConvergeThre)
                    {
                        n.xtt = (1 - theta) * c.xtt + theta * a1;
                        n.xt = c.xt + ((1 - gamma) * c.xtt + gamma * n.xtt) * dt;
                        n.x = c.x + c.xt * dt + ((0.5 - beta) * c.xtt + beta * n.xtt) * dt * dt;
                        n.xtt_plus_ytt = n.xtt + n.ytt;

                        R = epModel.TryCalcNextF(n.x);
                        n.f = R;
                        h = model.h + epModel.Next_heq;
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
