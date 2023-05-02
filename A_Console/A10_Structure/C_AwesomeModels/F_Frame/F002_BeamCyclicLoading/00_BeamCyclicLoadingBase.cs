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

            // ★★★★★ 鋼材情報作成（フランジとウェブそれぞれ）
            var steels = new List<Steel>();
            {
                steels.Add(new Steel("F", inputDataDir.GetChildFileInfo("Steel_Flange.csv"), 345, 0.33, 0.67));
                steels.Add(new Steel("W", inputDataDir.GetChildFileInfo("Steel_Web.csv"), 420, 0.33, 0.67));
            }

            // ★★★★★ 梁作成（梁の分割方法を変更するには，Memberクラスのコンストラクタにて行う。）

            //（高精度 → dLを5にする？）
            // 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            var m = new Member(

                steels: steels,
                sectionType: SectionType.H,

                L: 2050,
                dL: 10,
                Ls: 40,
                Lw: 10,

                H: 600,
                divH: 50,
                Hs: 35,
                dHs: 5,
                divHf: 5,
                tf: 17,

                B: 200,
                tw: 11,

                sig_y: 345.4165295,
                E: 205000,
                n_ratio: 0,
                data_dir: inputDataDir.FullName,

                ConsiderQDef: true
                );

            // ★★★★★ 目標変位データ
            var target_delH_list = TimeHistory.FromCsv(inputDataDir.GetChildFileInfo($"目標変位 {item}.txt"))[0].ToList();

            // ★★★★★ 載荷実行！
            m.Calc(target_delH_list);

        }

    }


    // ★★★★★★★★★★★★★★★ 

}
