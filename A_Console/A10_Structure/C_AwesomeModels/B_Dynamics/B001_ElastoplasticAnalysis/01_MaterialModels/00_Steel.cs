using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class Material
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// 用語の定義
    /// 
    /// εt = 真歪度
    /// σt = 真応力度
    /// 
    /// εn = 工学的歪度（公称歪度）
    /// σn = 工学的応力度（公称応力度）
    /// 
    /// </remarks>
    public class Steel
    {
        /// <summary>
        /// テストピースの名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 鋼材のσ-ε関係の諸元の集合体
        /// </summary>
        public List<SteelStepInfo> Steps { get; set; }

        /// <summary>
        /// 鋼材の σt-εt 関係における比例限界点。
        /// ※ 塑性化の開始を判断する応力度なので、降伏点ではなく比例限界。
        /// </summary>
        public int Sig_p_t { get; set; }

        /// <summary>
        /// 鋼材の破断応力度(真応力度最大値)
        /// </summary>
        public double Sig_u_t => Steps.Last().Sig_t;

        public void Example(FileInfo monoEPDataFile)
        {
            var EP = TimeHistory.FromCsv(monoEPDataFile);
            new Steel("Steel_001", EP[0], EP[1]);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">部材名</param>
        /// <param name="eps_t_list"> σt-εt 関係の εt のリスト。</param>
        /// <param name="sig_t_list"> σt-εt 関係の σt のリスト。</param>
        public Steel(string name,double[] eps_t_list, double[] sig_t_list)
        {
            Name = name;
            Steps = new List<SteelStepInfo>();

            //データ読み込み

            double prev_sig_t = 0d;
            double prev_eps_t = 0d;

            for (int i = 0; i < eps_t_list.Length; i++)
            {
                var eps_t = eps_t_list[i];
                var sig_t = sig_t_list[i];

                if (eps_t == 0) // (0,0) のデータが最初にあってもなくても対応できるようにしてる。
                    continue;

                var e_t = (sig_t - prev_sig_t) / (eps_t - prev_eps_t);
                var e_n_t = (sig_t / Math.Exp(eps_t) - prev_sig_t / Math.Exp(prev_eps_t)) / ((Math.Exp(eps_t) - 1) - (Math.Exp(prev_eps_t) - 1));
                var e_n_c = (-sig_t / Math.Exp(-eps_t) + prev_sig_t / Math.Exp(-prev_eps_t)) / ((Math.Exp(-eps_t) - 1) - (Math.Exp(-prev_eps_t) - 1));

                Steps.Add(new SteelStepInfo(eps_t, sig_t, e_t, e_n_t, e_n_c));

                prev_eps_t = eps_t;
                prev_sig_t = sig_t;
            }

            Sig_p_t = sig_y_t;
        }


        public class SteelStepInfo
        {
            /// <summary>
            /// 鋼材の歪度(εt-真歪度関係_骨格曲線)
            /// </summary>
            public double Eps_t { get; set; }
            /// <summary>
            /// 鋼材の応力度(真応力度-真歪度関係_骨格曲線)
            /// </summary>
            public double Sig_t { get; set; }

            /// <summary>
            /// 鋼材の剛性(真応力度-真歪度関係_骨格曲線)
            /// </summary>
            public double E_t { get; set; }
            /// <summary>
            /// 鋼材の剛性(工学応力度-工学歪度関係_引張側骨格曲線)
            /// </summary>
            public double E_n_t { get; set; }
            /// <summary>
            /// 鋼材の剛性(工学応力度-工学歪度関係_圧縮側骨格曲線)
            /// </summary>
            public double E_n_c { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="eps_t"></param>
            /// <param name="sig_t"></param>
            /// <param name="e_t"></param>
            /// <param name="e_n_t"></param>
            /// <param name="e_n_c"></param>
            public SteelStepInfo(double eps_t, double sig_t, double e_t, double e_n_t, double e_n_c)
            {
                Eps_t = eps_t;
                Sig_t = sig_t;
                E_t = e_t;
                E_n_t = e_n_t;
                E_n_c = e_n_c;
            }

        }

    }


    // ★★★★★★★★★★★★★★★

}
