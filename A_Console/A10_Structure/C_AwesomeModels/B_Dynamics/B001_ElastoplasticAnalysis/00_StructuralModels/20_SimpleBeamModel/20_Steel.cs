using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class SimpleBeamModel
{
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
        /// 鋼材の真応力度-真歪度関係(骨格曲線)における降伏点
        /// ※ 塑性化の開始を判断する応力度なので、降伏点ではなく比例限界を入力しておく
        /// </summary>
        public int Sig_y_t { get; set; }
        /// <summary>
        /// バウシンガー関連の計数。.md 参照
        /// </summary>
        public double BCF { get; set; }
        /// <summary>
        /// バウシンガー関連の計数。.md 参照
        /// </summary>
        public double ALF { get; set; }

        /// <summary>
        /// 鋼材の真応力度-真歪度関係(骨格曲線)のデーター数
        /// </summary>
        public int DataCount => Steps.Count;

        /// <summary>
        /// 鋼材の破断応力度(真応力度最大値)
        /// </summary>
        public double Sig_u_t => Steps.Last().Sig_t;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">部材名</param>
        /// <param name="monoEPDataFile">単調載荷時の弾塑性特性データの場所</param>
        /// <param name="sig_y_t">規準となる降伏応力度の手動入力</param>
        /// <param name="BCF"></param>
        /// <param name="ALF"></param>
        public Steel(string name, FileInfo monoEPDataFile, int sig_y_t, double BCF, double ALF)
        {
            Name = name;
            Steps = new List<SteelStepInfo>();

            //データ読み込み
            var EP = TimeHistory.FromCsv(monoEPDataFile);

            double prev_sig_t = 0d;
            double prev_eps_t = 0d;

            for (int i = 0; i < EP.DataRowCount; i++)
            {
                var eps_t = EP[0][i];
                var sig_t = EP[1][i];

                if (eps_t == 0)
                    continue;

                var e_t = (sig_t - prev_sig_t) / (eps_t - prev_eps_t);
                var e_n_t = (sig_t / Math.Exp(eps_t) - prev_sig_t / Math.Exp(prev_eps_t)) / ((Math.Exp(eps_t) - 1) - (Math.Exp(prev_eps_t) - 1));
                var e_n_c = (-sig_t / Math.Exp(-eps_t) + prev_sig_t / Math.Exp(-prev_eps_t)) / ((Math.Exp(-eps_t) - 1) - (Math.Exp(-prev_eps_t) - 1));

                Steps.Add(new SteelStepInfo(eps_t, sig_t, e_t, e_n_t, e_n_c));

                prev_eps_t = eps_t;
                prev_sig_t = sig_t;
            }

            Sig_y_t = sig_y_t;

            this.BCF = BCF;
            this.ALF = ALF;
        }

    }
}
