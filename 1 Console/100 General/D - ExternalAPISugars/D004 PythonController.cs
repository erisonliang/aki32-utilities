using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;
using Python.Runtime;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Mainly Sugar for pythonnet (Python.Runtime.dll).
/// ※ You need to include reference to Python.Runtime.dll (can be found in Project > Properties > Assets) in your project 
/// </summary>
public static class PythonController
{
    // ★★★★★★★★★★★★★★★ prop

    private static string DllName { get; set; }
    private static string PythonPath { get; set; }
    private static List<string> AdditionalPath { get; set; }
    private static Py.GILState GIL;


    // ★★★★★★★★★★★★★★★ main

    public static void Initialize(
        string dllName = @"python310.dll",
        string pythonPath = null,
        List<string> additionalPath = null
        )
    {
        // ★ セット
        DllName = dllName;
        PythonPath = pythonPath;
        AdditionalPath = additionalPath;

        // ★ Dllの名前を明示
        Runtime.PythonDLL = DllName;

        // ★ pythonnetがpython本体のDLLおよび依存DLLを見つけられるようにする。
        //    使用しようとしているPythonをPATHに登録してない場合に呼ぶことを想定。
        if (!string.IsNullOrEmpty(PythonPath))
        {
            var pythonPathEnvVar = Environment.ExpandEnvironmentVariables(PythonPath);
            SystemExtension.AddEnvPath("PATH", new string[]
            {
                pythonPathEnvVar,
                Path.Combine(pythonPathEnvVar, @"DLLs"),
            });
        }

        // ★ 初期化 (明示的に呼ばなくても内部で自動実行されるようだが、一応呼ぶ)
        PythonEngine.Initialize();

        // ★ Global Interpreter Lockを取得
        GIL = Py.GIL();

        // ★ 追加のパスを通す。
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
    }


    // ★★★★★★★★★★★★★★★ sugar

    public static dynamic Import(string name) => Py.Import(name);
    public static dynamic RunSimpleString(string code) => PythonEngine.RunSimpleString(code);


    // ★★★★★★★★★★★★★★★ samples

    /// <summary>
    /// pythonコードを直接指定して実行
    /// </summary>
    public static void PythonExample_WithStringInvoke()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("PythonExample_WithStringInvoke");
        Console.WriteLine();

        RunSimpleString(@"
import numpy as np
print(f'np.cos(np.pi/4) = {np.cos(np.pi/4)}')
");

        Console.WriteLine("=======================================");
        Console.WriteLine();

    }

    /// <summary>
    /// 普通のpythonのC#での記述！
    /// </summary>
    /// <param name="paths"></param>
    public static void PythonExample_WithDynamicInvoke()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("PythonExample_WithDynamicInvoke");
        Console.WriteLine();

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

        Console.WriteLine("=======================================");
        Console.WriteLine();

    }

    /// <summary>
    /// 自作コード叩く。
    /// </summary>
    /// <remarks>
    /// 
    /// 他のパッケージが入ってるところ（C:\Python310\Lib\site-packages\）にフォルダ作って，中に（__init__.py）を追加でいけた！
    /// なんか他の手段ないかな…。
    /// 
    /// もしくは，環境変数の初期化のところに自作コードの場所の指定とかすればいける？？
    /// 
    /// </remarks>
    /// <param name="paths"></param>
    public static void PythonExample_WithOwnLibraryInvoke()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("PythonExample_WithOwnLibraryInvoke");
        Console.WriteLine();

        dynamic snap = Import("SNAPVisualizer");
        dynamic a = snap.SNAPBeamVisualizer;

        //dynamic myMath = Py.Import("my_awesome_lib.my_math"); // "from my_awesome_lib import my_math"
        //dynamic calculator = myMath.Calculator(5, 7); // クラスのインスタンスを生成
        //Console.WriteLine($"5 + 7 = {calculator.add()}"); // クラスのメソッド呼び出し
        //Console.WriteLine($"sum(1,2,3,4,5) = {myMath.Calculator.sum(new[] { 1, 2, 3, 4, 5 })}"); //staticメソッドも当然呼べる
        //dynamic dict = myMath.GetDict(); // 辞書型を返す関数呼び出し
        //Console.WriteLine(dict[3]); // 辞書からキーを指定して読み取り
        //Console.ReadKey();

        Console.WriteLine("=======================================");
        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ 

}