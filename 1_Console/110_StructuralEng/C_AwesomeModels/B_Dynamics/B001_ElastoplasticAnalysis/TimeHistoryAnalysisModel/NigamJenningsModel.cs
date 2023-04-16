using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class NigamJenningsModel : ITimeHistoryAnalysisModel
{

    // ★★★★★★★★★★★★★★★ props

    public double? DesiredTimeStep { get; set; } // TODO: 未実装
    public double ConvergeThre { get; set; } = 1e-6;


    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// constructor
    /// </summary>
    public NigamJenningsModel(double? desiredTimeStep = null)
    {
        DesiredTimeStep = desiredTimeStep;
    }

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run Nigam-Jenning's method
    /// </summary>
    public TimeHistory Calc(SDoFModel model, TimeHistory wave)
    {
        var resultHistory = wave.Clone();
        resultHistory.Name = $"result_{wave.Name}_{model.EP.GetType().Name}_{GetType().Name}";

        var epModel = model.EP;
        var h = model.h;
        var m = model.m;
        var TimeStep = wave.TimeStep;

        var h1 = Math.Sqrt(1.0 - h * h); // √1-h2
        var h2 = 2 * h * h - 1;          // 2h2-1

        if (model.EP is ElasticModel)
        {
            // simple calc

            #region 係数の計算

            // 便利
            var w = model.w;
            var w2 = w * w;
            var w3 = w * w * w;
            var wd = h1 * w;
            var e = Math.Pow(Math.E, -1.0 * h * w * TimeStep);
            var sin = Math.Sin(wd * TimeStep);
            var cos = Math.Cos(wd * TimeStep);

            // nigam の係数
            var a11 = e * (h / h1 * sin + cos);

            var a12 = e / wd * sin;

            var a21 = -e * w / h1 * sin;

            var a22 = e * (cos - h / h1 * sin);

            var b11 =
                e
                *
                (
                    (h2 / w2 / TimeStep + h / w) * sin / wd
                    +
                    (2 * h / w3 / TimeStep + 1 / w2) * cos
                )
                -
                2 * h / w3 / TimeStep
                ;

            var b12 =
                -e
                *
                (
                    h2 / w2 / TimeStep * sin / wd
                    +
                    2 * h / w3 / TimeStep * cos
                )
                -
                1 / w2
                +
                2 * h / w3 / TimeStep;


            var b21 =
                e
                *
                (
                    (h2 / w2 / TimeStep + h / w) * (cos - h / h1 * sin)
                    -
                    (2 * h / w3 / TimeStep + 1 / w2) * (wd * sin + h * w * cos)
                )
                +
                1 / w2 / TimeStep
                ;

            var b22 =
               -e
               *
               (
                   h2 / w2 / TimeStep * (cos - h / h1 * sin)
                   -
                   2 * h / w3 / TimeStep * (wd * sin + h * w * cos)
               )
               -
               1 / w2 / TimeStep
               ;

            #endregion

            for (int i = 0; i < resultHistory.DataRowCount - 1; i++)
            {
                var c = resultHistory.GetStep(i);
                var n = resultHistory.GetStep(i + 1);

                n.x = a11 * c.x + a12 * c.xt + b11 * c.ytt + b12 * n.ytt;
                n.xt = a21 * c.x + a22 * c.xt + b21 * c.ytt + b22 * n.ytt;
                n.xtt = -n.ytt - 2 * h * w * n.xt - w2 / n.x;  // wo2*x → F/m
                n.xtt_plus_ytt = n.xtt + n.ytt;
                n.f = epModel.TryCalcNextF(n.x);
                epModel.AdoptNextPoint();

                resultHistory.SetStep(i + 1, n);
            }

        }
        else if (model.EP is BilinearModel)
        {
            // TODO:
            // nigam for bilinear theory available here
            // http://library.jsce.or.jp/Image_DB/eq1994/proc/00061/13-0022.pdf
            throw new NotImplementedException("nigam-jennings for bi-linear model is not implemented yet");
        }
        else
        {
            throw new NotImplementedException("nigam-jennings is not for elastoplastic models");
        }

        return resultHistory;
    }

    // ★★★★★★★★★★★★★★★

}
