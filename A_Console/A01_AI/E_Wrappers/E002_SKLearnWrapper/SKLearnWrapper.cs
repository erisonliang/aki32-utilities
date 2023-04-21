using Aki32Utilities.ConsoleAppUtilities.General;

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public class SKLearnWrapper
{
    // ★★★★★★★★★★★★★★★ props


    // ★★★★★★★★★★★★★★★ init


    // ★★★★★★★★★★★★★★★ methods

    public static dynamic GetSKLearn() => PythonController.Import("sklearn");


    // ★★★★★★★★★★★★★★★ classes

    public class GaussianProcess
    {
        public static dynamic GetGaussianProcessRegressor() => PythonController.Import("sklearn.gaussian_process.GaussianProcessRegressor");

        public static dynamic ConstantKernel() => PythonController.Import("sklearn.gaussian_process.ConstantKernel");
        public static dynamic RBF() => PythonController.Import("sklearn.gaussian_process.RBF");
        public static dynamic WhiteKernel() => PythonController.Import("sklearn.gaussian_process.WhiteKernel");

        public static int Fit(dynamic kernel, double[] x, double[] y, double[] predictX)
        {
            var np=PythonController.Import("numpy");
            var GaussianProcessRegressor = PythonController.Import("sklearn.gaussian_process.GaussianProcessRegressor");

            var X = np.atleast_2d(x).T;
            var Y = np.atleast_2d(y).T;

            var gpm = GaussianProcessRegressor(kernel: kernel);
            gpm.fit(X, Y);

            dynamic g = gpm.predict(predictX, return_cov: true);  // 予測値と分散共分散行列
            (dynamic GPf, dynamic GPv) = (g.Item1, g.Item2);
            dynamic GPsd = np.sqrt(np.diag(GPv)); // 予測値の標準偏差


            return 1;
        }
    }

}