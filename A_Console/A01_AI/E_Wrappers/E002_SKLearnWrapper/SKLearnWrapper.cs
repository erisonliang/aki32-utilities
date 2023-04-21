using System.Linq;

using Aki32Utilities.ConsoleAppUtilities.General;

using Tensorflow;

using static Aki32Utilities.ConsoleAppUtilities.AI.SKLearnWrapper;

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

        // ★★★★★★★★★★★★★★★ props

        public static dynamic GP => PythonController.Import("_gpr.GaussianProcessRegressor");


        // ★★★★★★★★★★★★★★★ init

        public GaussianProcess(DirectoryInfo? gaussianProcessFolder = null)
        {
        
        }


        // ★★★★★★★★★★★★★★★ methods

        public  int Fit(dynamic kernel, double[] x, double[] y, double[] predictX)
        {







            return 1;
        }
    }

}