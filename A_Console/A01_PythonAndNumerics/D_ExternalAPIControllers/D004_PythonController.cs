using Python.Runtime;
using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
/// <summary>
/// Mainly Sugar for pythonnet (Python.Runtime.dll).
/// ※ You need to include reference to Python.Runtime.dll (can be found in Repository > 9_Assets > dlls) in your project. 
/// </summary>
public static partial class PythonController
{
    // ★★★★★★★★★★★★★★★ prop

    public static bool Activated { get; private set; } = false;
    public static string DllName { get; set; } = "python310.dll";
    public static string PythonPath { get; set; }
    public static List<string> AdditionalPath { get; set; } = new List<string>();
    public static Py.GILState GIL;


    // ★★★★★★★★★★★★★★★ main

    public static void Initialize(bool reInit = false)
    {
        // ★
        if (Activated) // already activated
        {
            if (reInit)
                Shutdown();
            else
                return;
        }

        // ★ セット
        Activated = true;

        // ★ Dllの名前を明示
        try
        {
            Runtime.PythonDLL = DllName;
        }
        catch (Exception ex)
        {
            throw new Exception($"Required to set correct python-dll-name to PythonController.DllName first. {ex.Message}");
        }

        // ★ pythonnetがpython本体のDLLおよび依存DLLを見つけられるようにする。
        //    使用しようとしているPythonをPATHに登録してない場合に呼ぶことを想定。
        try
        {
            if (!string.IsNullOrEmpty(PythonPath))
            {
                var pythonPathEnvVar = Environment.ExpandEnvironmentVariables(PythonPath);
                SystemExtension.AddEnvPath("PATH", new string[]
                {
                pythonPathEnvVar,
                Path.Combine(pythonPathEnvVar, @"DLLs"),
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Required to set correct python-executable-path to PythonController.PythonPath first. {ex.Message}");
        }


        // ★ 初期化 (明示的に呼ばなくても内部で自動実行されるようだが、一応呼ぶ)
        PythonEngine.Initialize();

        // ★ Global Interpreter Lockを取得
        GIL = Py.GIL();

        // ★ 追加のパスを通す。自作のコードを呼ぶときなど。
        if (AdditionalPath != null && AdditionalPath.Count > 0)
        {
            dynamic sys = Py.Import("sys");
            foreach (var ap in AdditionalPath)
                sys.path.append(ap);
        }

    }

    public static void Shutdown()
    {
        try
        {
            GIL?.Dispose();
        }
        catch (Exception)
        {
        }

        try
        {
            PythonEngine.Shutdown();
        }
        catch (Exception)
        {
        }

        Activated = false;
    }


    // ★★★★★★★★★★★★★★★ sugar

    public static dynamic Import(string name)
    {
        if (!Activated)
            throw new Exception("Required to call PythonController.Initialize() first");
        return Py.Import(name);

    }
    public static int RunSimpleString(string code)
    {
        if (!Activated)
            throw new Exception("Required to call PythonController.Initialize() first");
        return PythonEngine.RunSimpleString(code);
    }


    // ★★★★★★★★★★★★★★★ examples

    /// <summary>
    /// pythonコードを直接指定して実行
    /// </summary>
    public static void PythonExample_WithStringInvoke()
    {
        UtilPreprocessors.PreprocessBasic();

        RunSimpleString(@"
import numpy as np
print(f'np.cos(np.pi/4) = {np.cos(np.pi/4)}')
");

    }

    /// <summary>
    /// 普通のpythonのC#での記述！
    /// </summary>
    /// <param name="paths"></param>
    public static void PythonExample_WithDynamicInvoke()
    {
        UtilPreprocessors.PreprocessBasic();

        //dynamic myMath = Py.Import("my_awesome_lib.my_math"); // ← "from my_awesome_lib import my_math"
        dynamic np = Import("numpy");
        Console.WriteLine($"np.cos(np.pi/4) = {np.cos(np.pi / 4)}");

        dynamic sin = np.sin;
        Console.WriteLine($"sin(5) = {sin(5)}");

        double c = np.cos(5) + sin(5);
        Console.WriteLine($"c = {c}");

        dynamic a = np.array(new List<int> { 1, 2, 3 });
        Console.WriteLine($"a.dtype = {a.dtype}");

        dynamic b = np.array(new List<int> { 6, 5, 4 }, dtype: np.int32);
        Console.WriteLine($"b.dtype = {b.dtype}");
        Console.WriteLine($"a * b = {a * b}");

        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ 

}