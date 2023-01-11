using Python.Runtime;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Python Interpreter
/// </summary>
public static class PythonController
{
    // ★★★★★★★★★★★★★★★ init

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
    public static void Init(DirectoryInfo pythonPath, string dllName = @"python310.dll", string[]? myPackages = null)
    {
        throw new NotImplementedException();



        // python環境にパスを通す。pythonの場所，もしくは仮想環境のルート！
        var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\aki32\AppData\Local\Programs\Python\Python38");

        // だいたい，python38.dll (Windows), libpython3.8.dylib (Mac), libpython3.8.so (most other *nix)。これしないと TypeInitializationException 吐く。
        Runtime.PythonDLL = "python38.dll";

        // pythonnetがpython本体のDLLおよび依存DLLを見つけられるようにする。
        AddEnvPath(
          PYTHON_HOME,
          Path.Combine(PYTHON_HOME, @"DLLs")
        );

        // python環境に、PYTHON_HOME(標準pythonライブラリの場所)を設定
        PythonEngine.PythonHome = PYTHON_HOME;

        // python環境に、PYTHON_PATH(モジュールファイルのデフォルトの検索パス)を設定
        PythonEngine.PythonPath = string.Join(
          Path.PathSeparator.ToString(),
          new string[] {
                  PythonEngine.PythonPath, // 元の設定を残す
                  Path.Combine(PYTHON_HOME, "Lib", "site-packages"),
                  
                  //pipで入れたパッケージはここ ↑ に入る。
                  //Path.Combine(@"C:\foo\bar\my_packages"), //※自作コードの場所！
                  //Path.Combine(@"C:\foo\bar\my_packages"), //※自作コードの場所！
                  //Path.Combine(@"C:\foo\bar\my_packages"), //※自作コードの場所！
          }
        );

        // 初期化 (明示的に呼ばなくても内部で自動実行されるようだが、一応呼ぶ)
        PythonEngine.Initialize();











        //Runtime.PythonDLL = dllName;

        //var pythonPathEnvVar = Environment.ExpandEnvironmentVariables(pythonPath.FullName);

        //// pythonnetがpython本体のDLLおよび依存DLLを見つけられるようにする。
        //AddEnvPath(
        //  pythonPathEnvVar,
        //  Path.Combine(pythonPathEnvVar, @"DLLs")
        //);

        //// python環境に、pythonPathEnvVar(標準pythonライブラリの場所)を設定
        ////PythonEngine.PythonHome = pythonPathEnvVar;
        //Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonPathEnvVar, EnvironmentVariableTarget.Process);


        //// python環境に、PYTHON_PATH(モジュールファイルのデフォルトの検索パス)を設定
        //var packages = new List<string>
        //{
        //    PythonEngine.PythonPath, // 元の設定を残す
        //    Path.Combine(pythonPathEnvVar, "Lib", "site-packages"), //pipで入れたパッケージはここに入る？
        //};
        //if (myPackages != null) packages.AddRange(myPackages);
        //PythonEngine.PythonPath = string.Join(Path.PathSeparator.ToString(), packages);

        //// 初期化 (明示的に呼ばなくても内部で自動実行されるようだが、一応呼ぶ)
        //PythonEngine.Initialize();

    }
    /// <summary>
    /// プロセスの環境変数PATHに、指定されたディレクトリを追加する(パスを通す)。
    /// </summary>
    /// <param name="paths">PATHに追加するディレクトリ。</param>
    private static void AddEnvPath(params string[] paths)
    {
        var envPaths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator).ToList();
        foreach (var path in paths)
            if (path.Length > 0 && !envPaths!.Contains(path))
                envPaths.Insert(0, path);

        Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator.ToString(), envPaths!), EnvironmentVariableTarget.Process);
    }

    public static void Run(Action pythonAction)
    {
        // Global Interpreter Lockを取得
        using (var gil = Py.GIL())
        {
            pythonAction();
        }
    }

    /// <summary>
    /// shutdown after not being used
    /// </summary>
    public static void Close()
    {
        try
        {
            PythonEngine.Shutdown();
            Console.WriteLine("python shutdown");
        }
        catch (Exception)
        {
        }
    }

    
    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// python handling example
    /// </summary>
    /// <param name="paths"></param>
    public static void PythonExample()
    {
//        // ★★★★★ pythonコードを直接指定して実行
//        Console.WriteLine("=======================================");
//        PythonEngine.RunSimpleString(@"
//import sys
//import pprint
//print()
//pprint.pprint(f'module path ={sys.path}')
//");


//        // ★★★★★ 自作コードも叩ける（環境変数の初期化のところに自作コードの場所の指定が必要！）
//        //Console.WriteLine("=======================================");
//        //dynamic myMath = Py.Import("my_awesome_lib.my_math"); // "from my_awesome_lib import my_math"
//        //dynamic calculator = myMath.Calculator(5, 7); // クラスのインスタンスを生成
//        //Console.WriteLine($"5 + 7 = {calculator.add()}"); // クラスのメソッド呼び出し
//        //Console.WriteLine($"sum(1,2,3,4,5) = {myMath.Calculator.sum(new[] { 1, 2, 3, 4, 5 })}"); //staticメソッドも当然呼べる
//        //dynamic dict = myMath.GetDict(); // 辞書型を返す関数呼び出し
//        //Console.WriteLine(dict[3]); // 辞書からキーを指定して読み取り
//        //Console.ReadKey();



//        // ★★★★★ 普通のpythonのC#での記述！
//        Console.WriteLine("=======================================");

//        dynamic np = Py.Import("numpy");
//        Console.WriteLine($"np.cos(np.pi * 2) = {np.cos(np.pi * 2)}");

//        dynamic sin = np.sin;
//        Console.WriteLine($"sin(5) = {sin(5)}");

//        double c = np.cos(5) + sin(5);
//        Console.WriteLine($"c = {c}");

//        dynamic a = np.array(new List<int> { 1, 2, 3 });
//        Console.WriteLine($"a.dtype = {a.dtype}");

//        dynamic b = np.array(new List<int> { 6, 5, 4 }, dtype: np.int32);
//        Console.WriteLine($"b.dtype = {b.dtype}");
//        Console.WriteLine($"a * b = {a * b}");


//        // ★★★★★ 
//        Console.WriteLine("=======================================");
    }



    // ★★★★★★★★★★★★★★★ 

}