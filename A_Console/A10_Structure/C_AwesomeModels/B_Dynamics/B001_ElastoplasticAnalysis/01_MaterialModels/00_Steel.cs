

using System.Diagnostics.Metrics;

using Aki32Utilities.ConsoleAppUtilities.General;

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
        /// 鋼材のσ-ε関係の集合体。(0,0)を含む！
        /// </summary>
        public TimeHistory SigEps { get; set; }
        public SteelStepInfo GetSigEpsAt(int i) => (SteelStepInfo)SigEps.GetStep(i);
        public double[] Eps_n => SigEps["Eps_n"];
        public double[] Sig_n => SigEps["Sig_n"];
        public double[] Eps_t => SigEps["Eps_t"];
        public double[] Sig_t => SigEps["Sig_t"];

        /// <summary>
        /// 鋼材の σt-εt 関係における比例限界点。
        /// ※ 塑性化の開始を判断する応力度なので、降伏点ではなく比例限界。
        /// </summary>
        public double Sig_p_t { get; set; }

        /// <summary>
        /// 鋼材の破断応力度(真応力度最大値)
        /// </summary>
        public double Sig_u_t => ((SteelStepInfo)SigEps.GetTheLastStep()).Sig_t;


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
            SigEps = new TimeHistory(name);

            if (eps_list[0] != 0) // (0,0) のデータが最初にあってもなくても対応できるようにしてる。
                SigEps.AppendStep(new SteelStepInfo(0, 0, stressType).StepTable);

            for (int i = 0; i < eps_list.Length; i++)
            {
                var eps_t = eps_list[i];
                var sig_t = sig_list[i];
                var addingStep = new SteelStepInfo(eps_t, sig_t, stressType);

                if (cutAfterWeaken
                    && i > 0
                    && (((SteelStepInfo)SigEps.GetTheLastStep()).Sig_t > addingStep.Sig_t))
                    break;

                SigEps.AppendStep(addingStep);

            }

            Sig_p_t = sigma_p_t ?? GetSigEpsAt(1).Sig_t;

        }


        // ★★★★★★★★★★★★★★★ classes

        /// <summary>
        /// 折れ点位置の情報
        /// </summary>
        public class SteelStepInfo
        {
            public TimeHistoryStep StepTable { get; set; } = new TimeHistoryStep();

            /// <summary>
            /// εn
            /// </summary>
            public double Eps_n { get => StepTable["Eps_n"]; set => StepTable["Eps_n"] = value; }
            /// <summary>
            /// σn
            /// </summary>
            public double Sig_n { get => StepTable["Sig_n"]; set => StepTable["Sig_n"] = value; }

            /// <summary>
            /// εt
            /// </summary>
            public double Eps_t { get => StepTable["Eps_t"]; set => StepTable["Eps_t"] = value; }
            /// <summary>
            /// σt
            /// </summary>
            public double Sig_t { get => StepTable["Sig_t"]; set => StepTable["Sig_t"] = value; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            private SteelStepInfo(TimeHistoryStep ths)
            {
                StepTable = ths;
            }

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



            public static implicit operator TimeHistoryStep(SteelStepInfo step)
            {
                return step.StepTable;
            }

            public static explicit operator SteelStepInfo(TimeHistoryStep ths)
            {
                return new SteelStepInfo(ths);
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
