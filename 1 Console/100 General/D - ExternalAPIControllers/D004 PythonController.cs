using System.Security.AccessControl;

using Python.Runtime;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Python Interpreter
/// </summary>
public class PythonController
{
    // ★★★★★★★★★★★★★★★ prop

    public static DirectoryInfo PythonPath { get; set; }
    public static List<string> AdditionalPath { get; set; } = new List<string>();
    public static string DllName { get; set; } = @"python310.dll";
    private static Py.GILState GIL { get; set; }

    // ★★★★★★★★★★★★★★★ method

    public static void RunOnce(Action pythonAction)
    {
        Init();

        GIL = Py.GIL(); // Global Interpreter Lockを取得
        pythonAction();

        GIL.Dispose();
        PythonEngine.Shutdown();
    }

    // ★★★★★★★★★★★★★★★ samples

    /// <summary>
    /// pythonコードを直接指定して実行
    /// </summary>
    public static void PythonExample_WithStringInvoke()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("PythonExample_WithStringInvoke");
        Console.WriteLine();

        PythonEngine.RunSimpleString(@"
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

        dynamic np = Py.Import("numpy");
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

        dynamic sys = Py.Import("sys");
        foreach (var ap in AdditionalPath)
            sys.path.append(ap);

        dynamic snap = Py.Import("SNAPVisualizer");
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


    // ★★★★★★★★★★★★★★★ private

    /// <summary>
    /// initialize all
    /// </summary>
    /// <param name="pythonPath">
    /// python環境にパスを通す。pythonの場所，もしくは仮想環境のルート！
    /// @"C:\Python310", @"C:\Users\user\AppData\Local\Programs\Python\Python310",...
    /// </param>
    /// <param name="dllName">
    /// だいたい，python310.dll (Windows), libpython3.10.dylib (Mac), libpython3.10.so (most other *nix)。これしないと TypeInitializationException 吐く。
    /// </param>
    private static void Init()
    {
        // ★★★★★ 
        Runtime.PythonDLL = DllName;

        // ★★★★★ PATHに登録してない場合に使うべきかも。pythonnetがpython本体のDLLおよび依存DLLを見つけられるようにする。
        //var pythonPathEnvVar = Environment.ExpandEnvironmentVariables(PythonPath.FullName);
        //AddEnvPath("PATH", new string[]
        //{
        //    pythonPathEnvVar,
        //    Path.Combine(pythonPathEnvVar, @"DLLs"),
        //});

        // 初期化 (明示的に呼ばなくても内部で自動実行されるようだが、一応呼ぶ)
        PythonEngine.Initialize();

    }

    /// <summary>
    /// プロセスの環境変数PATHに、指定されたディレクトリを追加する(パスを通す)。
    /// </summary>
    /// <param name="paths">PATHに追加するディレクトリ。</param>
    private static void AddEnvPath(string pathName, params string[] paths)
    {
        var envPaths = Environment.GetEnvironmentVariable(pathName)?.Split(Path.PathSeparator).ToList() ?? new List<string>();
        foreach (var path in paths)
            if (path.Length > 0 && !envPaths!.Contains(path))
                envPaths.Insert(0, path);

        Environment.SetEnvironmentVariable(pathName, string.Join(Path.PathSeparator.ToString(), envPaths!), EnvironmentVariableTarget.Process);
    }


    // ★★★★★★★★★★★★★★★ 

}