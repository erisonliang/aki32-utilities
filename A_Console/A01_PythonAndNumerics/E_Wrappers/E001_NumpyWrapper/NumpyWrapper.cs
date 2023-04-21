using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public class NumpyWrapper
{
    // ★★★★★★★★★★★★★★★ props


    // ★★★★★★★★★★★★★★★ init


    // ★★★★★★★★★★★★★★★ methods

    public static dynamic ToCorrect1DNDArray<T>(dynamic X)
    {
        dynamic np = PythonController.Import("numpy");
        return np.array(X);
    }

    public static dynamic ToCorrect2DNDArray<T>(dynamic X)
    {
        dynamic np = PythonController.Import("numpy");

        if (X is T[][] X1)
        {
            return np.array(X1);
        }
        else if (X is T[,] X2)
        {
            return np.array(X2);
        }
        else
        {
            var X3 = X.ToJaggedArray<T>();
            return np.array(X3);
        }
    }

    public static (double[,] XGrid, double[,] YGrid) GetMeshGrid(double[] X, double[] Y)
    {
        var XYGrid = X.SelectMany(x => Y, (x, y) => (x, y));
        var XGrid = XYGrid.Select(xy => xy.x).ToArray().ReShape(X.Length, Y.Length);
        var YGrid = XYGrid.Select(xy => xy.y).ToArray().ReShape(X.Length, Y.Length);
        return (XGrid, YGrid);
    }


    // ★★★★★★★★★★★★★★★ 

}