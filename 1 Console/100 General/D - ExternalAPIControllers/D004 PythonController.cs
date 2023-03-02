using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;

using LibGit2Sharp;

using Python.Runtime;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistory;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Mainly Sugar for pythonnet (Python.Runtime.dll).
/// ※ You need to include reference to Python.Runtime.dll (can be found in Project > Properties > Assets) in your project 
/// </summary>
public static class PythonController
{
    // ★★★★★★★★★★★★★★★ prop

    public static bool Activated { get; set; } = false;
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
        Activated = true;
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

        Activated = false;
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


    // ★★★★★★★★★★★★★★★ classes

    public class PyplotController
    {

        // ★★★★★★★★★★★★★★★ props

        // ★★★★★ 全体

        public string FontName { get; set; } = "MS Gothic";

        public double FigHeight { get; set; } = 10;
        public double FigWidth { get; set; } = 15;

        public double PlotWSpace { get; set; } = 0;
        public double PlotHSpace { get; set; } = 0;

        public bool IsTightLayout { get; set; } = false;


        // ★★★★★ サブプロット，外側

        public string Title { get; set; } = "";
        public int TitleSize { get; set; } = 40;

        public string XLabel { get; set; } = "";
        public string YLabel { get; set; } = "";
        public int XLabelSize { get; set; } = 30;
        public int YLabelSize { get; set; } = 30;

        public int XYLabelTickSize { get; set; } = 20;

        public Range? XLim { get; set; }
        public Range? YLim { get; set; }


        // ★★★★★ サブプロット，内側

        public string LineStyle { get; set; } = "solid"; // solid, dashed, dashdot, dotted, --, -, :
        public string LegendLabel { get; set; } = "";

        public double? GraphMargins { get; set; } = null;
        public bool HasGrid { get; set; } = true;

        public string Marker { get; set; } = "."; // . , o v ^ < > ...
        public int Linewidth { get; set; } = 4;


        // ★★★★★★★★★★★★★★★ methods

        public FileInfo LinePlot(FileInfo outputFile, double[] x, double[] y)
        {
            // preprocess
            if (!Activated)
                throw new Exception("Required to call PythonController.Initialize() first");

            // ★★★★★ 全体

            dynamic plt = Import("matplotlib.pyplot");

            plt.subplots_adjust(wspace: PlotWSpace, hspace: PlotHSpace);

            dynamic fig = plt.figure(figsize: new double[] { FigWidth, FigHeight });

            // ★★★★★ サブプロット，外側

            dynamic ax = fig.add_subplot(111);

            // タイトル
            if (!string.IsNullOrEmpty(Title))
                ax.set_title(Title, fontname: FontName, size: TitleSize);

            // 軸ラベル
            if (!string.IsNullOrEmpty(XLabel))
                ax.set_xlabel(XLabel, size: XLabelSize, fontname: FontName);
            if (!string.IsNullOrEmpty(YLabel))
                ax.set_ylabel(YLabel, size: YLabelSize, fontname: FontName);

            // 軸目盛
            ax.tick_params(axis: 'x', labelsize: XYLabelTickSize);
            if (XLim.HasValue)
                ax.set_xlim(XLim.Value.Start.Value, XLim.Value.End.Value);
            ax.tick_params(axis: 'y', labelsize: XYLabelTickSize);
            if (YLim.HasValue)
                ax.set_ylim(YLim.Value.Start.Value, YLim.Value.End.Value);


            // ★★★★★ サブプロット，内側

            // プロット
            ax.plot(x, y,
                linestyle: LineStyle,
                label: LegendLabel,
                marker: Marker,
                linewidth: Linewidth
                );

            // 凡例
            if (!string.IsNullOrEmpty(LegendLabel))
                ax.legend(prop: new Dictionary<string, string> { { "family", FontName } });

            // グラフから枠線までの距離
            if (GraphMargins.HasValue)
                ax.margins(GraphMargins!);

            // grid
            ax.grid(HasGrid);


            // ★★★★★ 最後に呼ぶべきもの

            // レイアウト詰め
            if (IsTightLayout)
                fig.tight_layout();

            // 保存，解放
            plt.savefig(outputFile.FullName);
            plt.clf();
            plt.close();

            return outputFile;
        }

        public FileInfo ScatterPlot(FileInfo outputFile, double[] x, double[] y)
        {
            // preprocess
            if (!Activated)
                throw new Exception("Required to call PythonController.Initialize() first");

            dynamic plt = Import("matplotlib.pyplot");
            dynamic fig = plt.figure(figsize: new double[] { FigWidth, FigHeight });
            dynamic ax = fig.add_subplot(111);


            // プロット
            ax.scatter(x, y);

            // 保存，解放
            plt.savefig(outputFile.FullName);
            plt.clf();
            plt.close();

            return outputFile;
        }


    }


    // ★★★★★★★★★★★★★★★ 

}