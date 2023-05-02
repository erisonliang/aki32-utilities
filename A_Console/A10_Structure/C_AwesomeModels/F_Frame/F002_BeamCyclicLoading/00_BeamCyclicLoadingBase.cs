

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{

    // ★★★★★★★★★★★★★★★ examples

    /// <summary>
    /// AB-W 一気に処理。
    /// </summary>
    public static void RunExampleModel1(DirectoryInfo inputDataDir)
    {
        var coef = new string[] { "2.81" };
        //var coef = new string[] { "2.81", "2.21", "1.75", "1.32", "0.75" };

        foreach (var item in coef)
        {
            // 基本的に入力する部分はこのファイルだけでOK！！あとは適宜材料試験の.csvファイルや目標変位の.txtファイルを作成。
            // 引き継ぐ側が嫌にならないように，あまりオブジェクト指向的でない形で書いてます。

            // ★★★★★ 鋼材情報作成
            var steels = new List<Steel>();
            {
                // フランジ
                steels.Add(new Steel("F", inputDataDir.GetChildFileInfo("Steel_F.csv"), 345, 0.33, 0.67));

                // ウェブ
                steels.Add(new Steel("W", inputDataDir.GetChildFileInfo("Steel_W.csv"), 420, 0.33, 0.67));
            }

            // ★★★★★ 梁作成（梁の分割方法を変更するには，Memberクラスのコンストラクタにて行う。）

            // 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            var m = new Member(
                steels: steels,
                sectionType: SectionType.H,
                dL: 10,
                divH: 50,
                H: 600,
                B: 200,
                L: 2050,
                tw: 11,
                tf: 17,
                sig_y: 345.4165295,
                E: 205000,
                n_ratio: 0,
                ConsiderQDef: true,
                data_dir: inputDataDir.FullName,
                DivHf: 5,
                Hs: 35,
                dHs: 5,
                Ls: 40,
                Lw: 10
                );

            // ★★★★★ 目標変位データ
            var target_delH_list = TimeHistory.FromCsv(inputDataDir.GetChildFileInfo($"目標変位 {item}.txt"))[0].ToList();

            // ★★★★★ 載荷実行！
            m.Calc(target_delH_list);

        }

        Console.WriteLine();
        Console.WriteLine(" ★ 終了 ★ ");
        Console.WriteLine();
        Console.ReadLine();
    }

    [Obsolete("Not implemented. Do not call")]
    public static void RunExampleModel_Keep()
    {
        throw new NotImplementedException();

        // AB-A シリーズ 一気に処理。
        {
            //var coef = new string[] { "3.00", "2.50", "2.00", "1.50", "1.25", "1.00", "0.75" };

            //foreach (var item in coef)
            //{
            //    // 基本的に入力する部分はこのファイルだけでOK！！あとは適宜材料試験の.csvファイルや目標変位の.txtファイルを作成。
            //    // 引き継ぐ側が嫌にならないように，あまりオブジェクト指向的でない形で書いてます。

            //    // ★★★★★ 基本情報
            //    var data_dir = @"C:\Users\zilli\Dropbox\Learning\提出物類\東大\2 建築学科\# 卒業研究\11 略算プログラム\梁の繰り返し載荷\卒業研究データ";

            //    // ★★★★★ 鋼材情報作成
            //    var steels = new List<Steel>();
            //    {
            //        // フランジ
            //        steels.Add(new Steel("F", Path.Combine(data_dir, "Steel_F.csv"), 364, 0.33, 0.67));

            //        // ウェブ
            //        steels.Add(new Steel("W", Path.Combine(data_dir, "Steel_W.csv"), 415, 0.33, 0.67));
            //    }


            //    // ★★★★★ 梁作成（梁の分割方法を変更するには，Memberクラスのコンストラクタにて行う。）
            //    var section_type = SectionType.H;


            //    // 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            //    var dL = 10;
            //    var DivH = 50;

            //    var H = 500;
            //    var B = 200;
            //    var L = 2260;

            //    var tw = 10;
            //    var tf = 16;

            //    var Sig_y = 363.96;
            //    var E = 200255;

            //    var N_ratio = 0;
            //    var ConsiderQDef = true;

            //    int DivHf = 5;
            //    int Hs = 35;
            //    int dHs = 5;
            //    int Ls = 40;
            //    int Lw = 10;

            //    var m = new Member(steels, section_type, dL, DivH, H, B, L, tw, tf, Sig_y, E, N_ratio, ConsiderQDef, data_dir, DivHf, Hs, dHs, Ls, Lw);

            //    // ★★★★★ 目標変位データ
            //    var target_delH_list = IO_Extension.ReadDataList1D(Path.Combine(data_dir, $"目標変位 {item}.txt")).ToList();


            //    // ★★★★★ 載荷実行！
            //    m.Calc(target_delH_list);


            //    // ★★★★★ 結果出力
            //    m.Output();

            //}

            //Console.WriteLine();
            //Console.WriteLine("All Done!!");
            //Console.ReadLine();
        }

        // AB-A シリーズ 1データだけ処理
        {
            //// 基本的に入力する部分はこのファイルだけでOK！！あとは適宜材料試験の.csvファイルや目標変位の.txtファイルを作成。
            //// 引き継ぐ側が嫌にならないように，あまりオブジェクト指向的でない形で書いてます。

            //// ★★★★★ 基本情報
            //var data_dir = @"C:\Users\zilli\Dropbox\Learning\提出物類\東大\2 建築学科\# 卒業研究\11 略算プログラム\梁の繰り返し載荷\卒業研究データ";

            //// ★★★★★ 鋼材情報作成
            //var steels = new List<Steel>();
            //{
            //    // フランジ
            //    steels.Add(new Steel("F", Path.Combine(data_dir, "Steel_F.csv"), 364, 0.33, 0.67));

            //    // ウェブ
            //    steels.Add(new Steel("W", Path.Combine(data_dir, "Steel_W.csv"), 415, 0.33, 0.67));
            //}


            //// ★★★★★ 梁作成（梁の分割方法を変更するには，Memberクラスのコンストラクタにて行う。）
            //var section_type = SectionType.H;


            //// 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            //var dL = 10;
            //var DivH = 50;

            //var H = 500;
            //var B = 200;
            //var L = 2260;

            //var tw = 10;
            //var tf = 16;

            //var Sig_y = 363.96;
            //var E = 200255;

            //var N_ratio = 0;
            //var ConsiderQDef = true;

            //int DivHf = 5;
            //int Hs = 35;
            //int dHs = 5;
            //int Ls = 40;
            //int Lw = 10;

            //var m = new Member(steels, section_type, dL, DivH, H, B, L, tw, tf, Sig_y, E, N_ratio, ConsiderQDef, data_dir, DivHf, Hs, dHs, Ls, Lw);

            //// ★★★★★ 目標変位データ
            //var target_delH_list = IO_Extension.ReadDataList1D(Path.Combine(data_dir, "目標変位.txt")).ToList();


            //// ★★★★★ 載荷実行！
            //m.Calc(target_delH_list);


            //// ★★★★★ 結果出力
            //m.Output();
            //Console.ReadLine();
        }

        // AB-A シリーズ 1データだけ処理（高精度）
        {
            //// 基本的に入力する部分はこのファイルだけでOK！！あとは適宜材料試験の.csvファイルや目標変位の.txtファイルを作成。
            //// 引き継ぐ側が嫌にならないように，あまりオブジェクト指向的でない形で書いてます。

            //// ★★★★★ 基本情報
            //var data_dir = @"C:\Users\zilli\Dropbox\Learning\提出物類\東大\2 建築学科\# 卒業研究\11 略算プログラム\梁の繰り返し載荷\卒業研究データ";

            //// ★★★★★ 鋼材情報作成
            //var steels = new List<Steel>();
            //{
            //    // フランジ
            //    steels.Add(new Steel("F", Path.Combine(data_dir, "Steel_F.csv"), 364, 0.33, 0.67));

            //    // ウェブ
            //    steels.Add(new Steel("W", Path.Combine(data_dir, "Steel_W.csv"), 415, 0.33, 0.67));
            //}


            //// ★★★★★ 梁作成（梁の分割方法を変更するには，Memberクラスのコンストラクタにて行う。）
            //var section_type = SectionType.H;


            //// XXX 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            //// OOO 今回は，L方向は5mmごとに分割，H方向には50分割ということで固定。
            //var dL = 5;
            //var DivH = 50;

            //var H = 500;
            //var B = 200;
            //var L = 2260;

            //var tw = 10;
            //var tf = 16;

            //var Sig_y = 363.96;
            //var E = 200255;

            //var N_ratio = 0;
            //var ConsiderQDef = true;

            //int DivHf = 5;
            //int Hs = 35;
            //int dHs = 5;
            //int Ls = 40;
            //int Lw = 10;

            //var m = new Member(steels, section_type, dL, DivH, H, B, L, tw, tf, Sig_y, E, N_ratio, ConsiderQDef, data_dir, DivHf, Hs, dHs, Ls, Lw);

            //// ★★★★★ 目標変位データ
            //var target_delH_list = IO_Extension.ReadDataList1D(Path.Combine(data_dir, "目標変位 1.50 - 200cycle.txt")).ToList();


            //// ★★★★★ 載荷実行！
            //m.Calc(target_delH_list);

            //Console.WriteLine();
            //Console.WriteLine(" ★ 終了 ★ ");
            //Console.WriteLine();
            //Console.ReadLine();
        }

    }


    // ★★★★★★★★★★★★★★★ 

}
