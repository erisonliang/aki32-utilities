using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    partial class Member
    {
        // ★★★★★ 重要

        /// <summary>
        /// 梁の微小厚さ断面の集合体
        /// </summary>
        public MemberSection[] s { get; set; }
        /// <summary>
        /// 梁の微小厚さ断面の集合体（前ステップ）
        /// </summary>
        public MemberSection[] prev_s { get; set; }

        // ★★★★★ 部材固有情報

        /// <summary>
        /// 部材断面形状
        /// </summary>
        public SectionType SectionType { get; set; }

        /// <summary>
        /// 軸方向の分割数
        /// </summary>
        public int DivL { get; set; }
        /// <summary>
        /// 成方向の分割数 
        /// </summary>
        public int DivH { get; set; }


        /// <summary>
        /// 部材断面成
        /// </summary>
        public double H { get; set; }
        /// <summary>
        /// 部材断面幅
        /// </summary>
        public double B { get; set; }
        /// <summary>
        /// 部材材長
        /// </summary>
        public double L { get; set; }


        /// <summary>
        /// ウェブ板厚（矩形の場合でも純粋な厚さを入力）
        /// </summary>
        public double tw { get; set; }
        /// <summary>
        /// フランジ板厚（断面積計算などのため，矩形の場合，ウェブ厚さの２倍が返ってくる）
        /// </summary>
        public double tw_total
        {
            get
            {
                return SectionType switch
                {
                    SectionType.H => tw,
                    SectionType.Rectangle => 2 * tw,
                    _ => double.NaN,
                };
            }
        }
        /// <summary>
        /// フランジ板厚
        /// </summary>
        public double tf { get; set; }


        /// <summary>
        /// 剪断弾性係数
        /// </summary>
        public double G { get; set; }
        /// <summary>
        /// 断面積
        /// </summary>
        public double A { get; set; }
        /// <summary>
        /// ウェブの断面積
        /// </summary>
        public double Aw { get; set; }
        /// <summary>
        /// ウェブの断面積（せん断変形用）
        /// </summary>
        public double Aw_s { get; set; }
        /// <summary>
        /// フランジの断面積 
        /// </summary>
        public double Af { get; set; }

        /// <summary>
        /// 断面２次モーメント 
        /// </summary>
        public double I { get; set; }


        /// <summary>
        /// 微小要素の長さ（L方向）
        /// </summary>
        public double dL { get; set; }
        /// <summary>
        /// 微小要素の長さ（L方向）（変形後）
        /// </summary>
        public double dLx { get; set; }

        /// <summary>
        /// 降伏軸力
        /// </summary>
        public double Py { get; set; }


        /// <summary>
        /// 全塑性モーメント(軸力考慮済)  
        /// </summary>
        public double Mp { get; set; }

        /// <summary>
        /// δp (軸力考慮済)  
        /// </summary>
        public double DelP { get; set; }


        /// <summary>
        /// Ｍp／Ｌ (軸力考慮済)  
        /// </summary>
        public double Qp { get; set; }


        // ★★★★★ 代表値

        /// <summary>
        /// 降伏点(代表値)
        /// </summary>
        public double Sig_y { get; set; }
        /// <summary>
        /// ヤング率
        /// </summary>
        public double E { get; set; }

        // ★★★★★ メモ

        /// <summary>
        /// 与える軸力の，部材の降伏軸力に対する比
        /// </summary>
        public int N_ratio { get; set; }
        /// <summary>
        /// 与える軸力
        /// </summary>
        public double P { get; set; }
        /// <summary>
        /// せん断力
        /// </summary>
        public double Q { get; set; }

        /// <summary>
        /// 累積変形
        /// </summary>
        public double TotalDelH { get; set; }

        /// <summary>
        /// せん断変形を考慮
        /// </summary>
        public bool ConsiderQDef { get; set; }

        // ★★★★★ 前ステップとの差のメモ

        /// <summary>
        /// 軸力の増分
        /// </summary>
        public double dP { get; set; }
        /// <summary>
        /// DQ:せん断力の増分 
        /// </summary>
        public double dQ { get; set; }
        /// <summary>
        /// モーメントの増分
        /// </summary>
        public double dM { get; set; }

        // ★★★★★ IO情報

        /// <summary>
        /// モデル生成時刻
        /// </summary>
        public DateTime ModelCreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// ベースとなるディレクトリのパス
        /// 主に外部入力データが入る
        /// </summary>
        public DirectoryInfo BaseDir { get; set; }

        /// <summary>
        /// 出力先ディレクトリのパス
        /// </summary>
        public DirectoryInfo OutputDir => BaseDir.GetChildDirectoryInfo("結果出力 - " + ModelCreateTime.ToString("yyyy-MM-dd_HH-mm-ss")).CreateAndPipe();

        /// <summary>
        /// 結果表示パス１
        /// </summary>
        public FileInfo PathMPhiResult => OutputDir.GetChildFileInfo("Result_M_Phi.csv");

        /// <summary>
        /// 結果表示パス２
        /// </summary>
        public FileInfo PathDangerousSectionResult => OutputDir.GetChildFileInfo("Result_DangerousSection.csv");

        /// <summary>
        /// 結果表示パス３
        /// </summary>
        public FileInfo PathVisualizedBeamResult => OutputDir.GetChildFileInfo("Result_Visualized Beam.txt");

    }

    /// <summary>
    /// 断面タイプ
    /// </summary>
    enum SectionType { H, Rectangle }
}