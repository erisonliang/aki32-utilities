

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    public class SteelStepInfo
    {

        /// <summary>
        /// 鋼材の歪度(真応力度-真歪度関係_骨格曲線)
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
