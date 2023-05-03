

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class Material
{

    // ★★★★★★★★★★★★★★★ props

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
        /// 標本の名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 鋼材のσ-ε関係の諸元の集合体。(0,0)を含む！
        /// </summary>
        public List<SteelStepInfo> Steps { get; set; }

        /// <summary>
        /// 鋼材の σt-εt 関係における比例限界点。
        /// ※ 塑性化の開始を判断する応力度なので、降伏点ではなく比例限界。
        /// </summary>
        public double Sig_p_t { get; set; }

        /// <summary>
        /// 鋼材の破断応力度(真応力度最大値)
        /// </summary>
        public double Sig_u_t => Steps.Last().Sig_t;


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sig_list"></param>
        /// <param name="eps_list"></param>
        /// <param name="stressType"></param>
        /// <param name="sigma_p_t"></param>
        /// <param name="cutAfterWeaken">真応力が低下しているのを発見したら，それ以降のデータを無視（その段階で破断）</param>
        public Steel(string name, double[] sig_list, double[] eps_list, StressType stressType, double? sigma_p_t = null, bool cutAfterWeaken = true)
        {
            Name = name;
            Steps = new List<SteelStepInfo>();

            if (eps_list[0] != 0) // (0,0) のデータが最初にあってもなくても対応できるようにしてる。
                Steps.Add(new SteelStepInfo(0, 0, stressType));

            for (int i = 0; i < eps_list.Length; i++)
            {
                var eps_t = eps_list[i];
                var sig_t = sig_list[i];
                var addingStep = new SteelStepInfo(eps_t, sig_t, stressType);

                if (cutAfterWeaken
                    && i > 0
                    && Steps.Last().Sig_t > addingStep.Sig_t)
                    break;

                Steps.Add(addingStep);

            }

            Sig_p_t = sigma_p_t ?? Steps.First().Sig_t;

        }


        // ★★★★★★★★★★★★★★★ classes

        /// <summary>
        /// 折れ点位置の情報
        /// </summary>
        public class SteelStepInfo
        {
            /// <summary>
            /// εn
            /// </summary>
            public double Eps_n { get; set; }
            /// <summary>
            /// σn
            /// </summary>
            public double Sig_n { get; set; }

            /// <summary>
            /// εt
            /// </summary>
            public double Eps_t { get; set; }
            /// <summary>
            /// σt
            /// </summary>
            public double Sig_t { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SteelStepInfo(double eps, double sig, StressType stressType)
            {
                switch (stressType)
                {
                    case StressType.Nominal:
                        {
                            Sig_n = sig;
                            Eps_n = eps;
                            Sig_t = Sig_n * (1 + Eps_n);
                            Eps_t = Math.Log(1 + Eps_n);
                        }
                        break;
                    case StressType.True:
                        {
                            Sig_t = sig;
                            Eps_t = eps;
                            Eps_n = Math.Exp(Eps_t) - 1; // * Eps first!
                            Sig_n = Sig_t / (1 + Eps_n);
                        }
                        break;
                }
            }
        }

        public enum StressType
        {
            Nominal,
            True,
        }

    }


    // ★★★★★★★★★★★★★★★

}
