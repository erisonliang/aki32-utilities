using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    partial class Member
    {
        /// <summary>
        /// コンストラクタ
        /// 
        /// 範囲の設定は資料を参照！
        /// 
        /// 通しダイアフラム形式
        /// R=35mm，長さ67mm[25+7+35]のスカラップを想定。(Ls = 70)(Lw = 40)
        /// 
        /// 柱通し＋エンドプレート形式
        /// R=35mm,長さ42mm[7+35] のスカラップを想定。(Ls = 40)(Lw = 10)
        /// 
        /// </summary>
        /// <param name="steels">鋼材リスト</param>
        /// <param name="divHf">フランジのH方向分割数</param>
        /// <param name="Hs">スカラップのH方向長さ</param>
        /// <param name="dHs">スカラップのH方向微小要素長さ</param>
        /// <param name="Ls">スカラップのL方向無力化長さ</param>
        /// <param name="Lw">溶接金属のL方向弾性化長さ</param>
        public Member(List<Steel> steels, SectionType sectionType, double dL, int divH, double H, double B, double L, double tw, double tf, double sig_y, double E, int n_ratio, bool ConsiderQDef, string data_dir, int divHf, int Hs, int dHs, int Ls, int Lw)
        {

            #region 出力ファイル

            this.BaseDir = data_dir;
            if (!Directory.Exists(OutputDir))
                Directory.CreateDirectory(OutputDir);

            // M-φ図を出力
            try
            {
                using (var sw = new StreamWriter(Path.Combine(OutputDir, PathMPhiResult), true, Encoding.UTF8))
                {
                    sw.WriteLine("Step,載荷Q,根元M,部材θ,累積変形角");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(PathMPhiResult + " 出力失敗");
                Console.WriteLine(ex.Message);
            }

            // 危険断面関連事項を出力
            try
            {
                using (var sw = new StreamWriter(Path.Combine(OutputDir, PathDangerousSectionResult), true, Encoding.UTF8))
                {
                    sw.WriteLine(",,,,,,,上フランジ,,,,骨格曲線累積塑性歪,,下フランジ,,,,骨格曲線累積塑性歪,");
                    sw.WriteLine("Step,載荷Q,根元M,部材θ,累積変形角,θ(危険断面),M(危険断面),εn,σn ,εt,σt,Σεpt,Σεpst+,εn,σn,εt,σt,Σεpt,Σεpst+");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(PathDangerousSectionResult + " 出力失敗");
                Console.WriteLine(ex.Message);
            }

            #endregion

            #region 各種初期化

            SectionType = sectionType;

            // 今回は，L方向は10mmごとに分割，H方向には50分割ということで固定。
            this.dL = dL;
            DivH = divH;

            this.H = H;
            this.B = B;
            this.L = L;

            this.tw = tw;
            this.tf = tf;

            Sig_y = sig_y;
            this.E = E;

            N_ratio = n_ratio;
            this.ConsiderQDef = ConsiderQDef;

            #endregion

            #region ★★★★★ 諸元の自動算出

            // ★★★★★ 分割数
            DivL = (int)(this.L / dL);

            // ★★★★★ 断面諸量の計算
            var Poisson = 0.3;
            G = this.E / (2 * (1 + Poisson)); // 実質，E / 2.6 となる。

            // 断面積・軸力の計算
            A = this.B * tf * 2 + (this.H - 2 * tf) * tw_total;
            Py = Sig_y * A;
            P = Py * N_ratio;
            Aw = (this.H - 2 * tf) * tw_total;
            Af = this.B * tf;

            // 全塑性モーメント（※ハイブリッド材の場合は組み直す必要あり）
            var FF = (P - Aw * Sig_y) / (Af * Sig_y) / 2;
            var WF = P / (Aw * Sig_y);

            if (sectionType == SectionType.Rectangle)
                Mp = Sig_y * (this.B * tf * (1 - FF) * (this.H - tf * (1 - FF)));
            else if (sectionType == SectionType.H)
                Mp = Sig_y * (this.B * tf * (this.H - tf) + tw_total * Math.Pow(((this.H - 2 * tf) / 2), 2) * (1 - WF) * (1 + WF));
            else
                throw new Exception("未定義");


            // PHIY=2.0*SIGY/EE/SH*[1.0-ABS[ROU]]
            // 降伏変形の計算

            I = (this.B * Math.Pow(this.H, 3) - (this.B - tw_total) * Math.Pow(this.H - 2 * tf, 3)) / 12;

            Aw_s = this.H * tw_total;
            Qp = Mp / this.L;
            DelP = Qp * Math.Pow(this.L, 3) / 3 / this.E / I;
            DelP += ConsiderQDef ? (Qp / G / Aw_s * this.L) : 0;

            #endregion

            #region ★★★★★ 梁の分割の設定

            // ★★★★★ 初期分割
            s = Enumerable.Range(0, DivL).Select(_ => new MemberSection(DivH)).ToArray();
            prev_s = Enumerable.Range(0, DivL).Select(_ => new MemberSection(DivH)).ToArray();

            // ★★★★★ 全ての微小要素に対して実行
            var DivHs = Hs / dHs;
            for (int iL = 0; iL < DivL; iL++)
            {
                for (int iH = 0; iH < DivH; iH++)
                {

                    // 全部材に材料を設定
                    if (iH < divHf)
                        s[iL].p[iH].Steel = steels.Find(x => x.Name == "F");
                    else if (iH < DivH - divHf)
                        s[iL].p[iH].Steel = steels.Find(x => x.Name == "W");
                    else
                        s[iL].p[iH].Steel = steels.Find(x => x.Name == "F");


                    // 弾塑性判定における初期降伏点
                    prev_s[iL].p[iH].RecoverSig_pos = s[iL].p[iH].Steel.Sig_y_t;
                    prev_s[iL].p[iH].RecoverSig_neg = -s[iL].p[iH].Steel.Sig_y_t;
                    s[iL].p[iH].RecoverSig_pos = s[iL].p[iH].Steel.Sig_y_t;
                    s[iL].p[iH].RecoverSig_neg = -s[iL].p[iH].Steel.Sig_y_t;


                    // 断面の分割
                    {


                        // 上フランジ[DivHf 分割]
                        if (iH < divHf)
                        {
                            s[iL].p[iH].dH = tf / divHf;
                            s[iL].p[iH].B = this.B;
                        }

                        // スカラップ位置[dHs mmずつ DivHs 分割]
                        else if (iH < (divHf + DivHs))
                        {
                            s[iL].p[iH].dH = dHs;
                            s[iL].p[iH].B = tw_total;
                        }

                        // ウェブ中間部 [残りを均等分割]
                        else if (iH < DivH - (divHf + DivHs))
                        {
                            s[iL].p[iH].dH = (this.H - 2 * (tf + dHs * DivHs)) / (DivH - 2 * (divHf + DivHs));
                            s[iL].p[iH].B = tw_total;
                        }

                        // スカラップ位置[dHs mmずつ DivHs 分割];
                        else if (iH < DivH - divHf)
                        {
                            s[iL].p[iH].dH = dHs;
                            s[iL].p[iH].B = tw_total;
                        }

                        // 下フランジ[DivHf 分割];
                        else
                        {
                            s[iL].p[iH].dH = tf / divHf;
                            s[iL].p[iH].B = this.B;
                        }
                    }

                    // 解析の初期値
                    prev_s[iL].p[iH].E_n = this.E;
                    prev_s[iL].p[iH].E_t = this.E;
                    s[iL].p[iH].E_n = this.E;
                    s[iL].p[iH].E_t = this.E;

                }
            }

            // ★★★★★ 無効領域の設定
            var DivLs = Ls / dL;
            for (int iL = 0; iL < DivLs; iL++)
            {
                // 上フランジ側
                for (int iH = divHf; iH < (divHf + DivHs); iH++)
                {
                    prev_s[iL].p[iH].E_n = 0;
                    prev_s[iL].p[iH].E_t = 0;
                    s[iL].p[iH].E_n = 0;
                    s[iL].p[iH].E_t = 0;
                }

                // 下フランジ側
                for (int iH = DivH - (divHf + DivHs); iH < DivH - divHf; iH++)
                {
                    prev_s[iL].p[iH].E_n = 0;
                    prev_s[iL].p[iH].E_t = 0;
                    s[iL].p[iH].E_n = 0;
                    s[iL].p[iH].E_t = 0;
                }

                //ウェブ中央
                {
                    //	 DO 49 IA=0,6
                    //	  DO 48 IB=17,27
                    //	 __sections[MJ].__piece[IA].ETNIA,IB]=0.0
                    //	 __sections[MJ].__piece[IA].ETTIA,IB]=0.0
                    //48     CONTINUE
                    //49    CONTINUE
                    //
                    //      IA=7  
                    //	  DO 47	IB=18,26
                    //	  __sections[MJ].__piece[IA].ETNIA,IB]=0.0
                    //	  __sections[MJ].__piece[IA].ETTIA,IB]=0.0
                    //47	  CONTINUE
                    //
                    //      IA=8  
                    //	  DO 46	IB=20,25
                    //	 __sections[MJ].__piece[IA].ETNIA,IB]=0.0
                    //	 __sections[MJ].__piece[IA].ETTIA,IB]=0.0
                    //46	  CONTINUE
                }

            }

            // ★★★★★ ダイアフラムと溶接金属を弾性に留める[降伏点10倍]
            var DivLw = Lw / dL;
            for (int iL = 0; iL < DivLw; iL++)
            {

                // 上フランジ
                for (int iH = 0; iH < divHf; iH++)
                {
                    s[iL].p[iH].RecoverSig_pos *= 10;
                    s[iL].p[iH].RecoverSig_neg *= 10;
                    prev_s[iL].p[iH].RecoverSig_pos *= 10;
                    prev_s[iL].p[iH].RecoverSig_neg *= 10;
                }

                // 下フランジ
                for (int iH = DivH - divHf; iH < DivH; iH++)
                {
                    s[iL].p[iH].RecoverSig_pos *= 10;
                    s[iL].p[iH].RecoverSig_neg *= 10;
                    prev_s[iL].p[iH].RecoverSig_pos *= 10;
                    prev_s[iL].p[iH].RecoverSig_neg *= 10;
                }

            }


            #endregion

        }

        /// <summary>
        /// 部材の繰り返し曲げ解析プログラム[N][mm]
        ///
        /// ver.7.0   on 2021/05/20 by T.Akimitsu
        ///   C#に翻訳
        ///   
        /// ver.6.31  on 20XX/XX/XX by S.YAMADA
        ///   一定軸力下における繰り返し曲げの解析
        ///   断面形状はH [もしくはBox] を想定
        ///   鋼材の応力度-歪度関係はフランジとウェブで共通：[フランジとウエブの鋼材は別々：変更可能）
        ///   ウェブのモーメント伝達効率も考慮
        ///   サブルーチン[Ｍ-φ]は軸力変動にも対応
        /// 
        /// var.1.0   on 2010/08/03 by S.YAMADA
        ///   original
        ///
        /// </summary>
        /// <param name="target_delH_list">目標変位リスト</param>
        public void Calc(List<double> target_delH_list)
        {
            // ★★★★★ 軸力の導入
            var iN_ratio = (int)(Math.Abs(N_ratio) / 0.01);
            if (iN_ratio != 0)
            {
                var dP = P / iN_ratio;

                for (int i = 0; i < iN_ratio; i++)
                {
                    for (int iL = 0; iL < DivL; iL++)
                    {

                        for (int iH = 0; iH < DivH; iH++)
                        {
                            s[iL].p[iH].Sig_n = prev_s[iL].p[iH].Sig_n + dP / A;
                        }

                        M_Phi(iL);
                        prev_s[iL].Phi = s[iL].Phi;

                        for (int iH = 0; iH < DivH; iH++)
                        {
                            prev_s[iL].p[iH].Eps_n = s[iL].p[iH].Eps_n;
                            prev_s[iL].p[iH].Sig_n = s[iL].p[iH].Sig_n;
                            prev_s[iL].p[iH].E_n = s[iL].p[iH].E_n;
                            prev_s[iL].p[iH].Eps_t = s[iL].p[iH].Eps_t;
                            prev_s[iL].p[iH].SigEpsState = s[iL].p[iH].SigEpsState;
                            prev_s[iL].p[iH].BauschingerState = s[iL].p[iH].BauschingerState;
                            prev_s[iL].p[iH].RecoverSig_pos = s[iL].p[iH].RecoverSig_pos;
                            prev_s[iL].p[iH].RecoverSig_neg = s[iL].p[iH].RecoverSig_neg;
                            prev_s[iL].p[iH].TotalEps_t = s[iL].p[iH].TotalEps_t;
                            prev_s[iL].p[iH].SigError = s[iL].p[iH].SigError;
                            prev_s[iL].p[iH].BausBoundSig_t_pos = s[iL].p[iH].BausBoundSig_t_pos;
                            prev_s[iL].p[iH].BausBoundSig_t_neg = s[iL].p[iH].BausBoundSig_t_neg;
                            prev_s[iL].p[iH].BausEps_t_pos = s[iL].p[iH].BausEps_t_pos;
                            prev_s[iL].p[iH].BausEps_t_neg = s[iL].p[iH].BausEps_t_neg;
                            prev_s[iL].p[iH].E_t = s[iL].p[iH].E_t;
                        }
                    }
                }

            }

            // ★★★★★ 解析計算
            int step_count = 0;
            SaveVisualizeBeam();
            SaveCurrentState(step_count);


            // 半サイクルの計算
            for (int iDelH = 0; iDelH < target_delH_list.Count; iDelH++)
            {
                int force_direction = 0;

                if (iDelH == 0)
                    force_direction = Math.Sign(target_delH_list[iDelH]);
                else
                    force_direction = Math.Sign(target_delH_list[iDelH] - target_delH_list[iDelH - 1]);

                double dQ = 100;

                //1ステップ分の計算
                while (true)
                {
                    step_count++;

                    // 1] 固定端におけるモーメントの増分
                    // 劣化させないので荷重制御とする
                    double dMe = Mp;

                    if (Math.Abs(prev_s[0].M) <= 0.5 * Mp)
                        dMe *= 0.02;
                    else if (Math.Abs(prev_s[0].M) <= Mp)
                        dMe *= 0.002;
                    else
                        dMe *= 0.0005;

                    s[0].M = prev_s[0].M + force_direction * dMe;

                    // 2] せん断力の仮定

                    for (int iCal = 0; iCal < int.MaxValue; iCal++)
                    {
                        // 3] 解析計算
                        // 固定端の境界条件[せん断変形角+接合部局所変形]

                        s[0].dDelH = ConsiderQDef ? (Q / G / Aw_s) : 0;

                        for (int iL = 0; iL < DivL; iL++)
                        {
                            dP = 0; //TODO: dPは今回は 0 とする。

                            if (iL == 0)
                            {
                                s[0].DelH = 0;
                                dM = force_direction * dMe;
                            }
                            else
                            {
                                // 3-1] 次の節点のモーメント・回転角・変位と次の区間の曲率
                                dLx = dL * (1 + (s[iL].p[0].Eps_n + s[iL].p[DivH - 1].Eps_n) / 2);
                                s[iL].dDelH = s[iL - 1].dDelH + s[iL - 1].ddDelH * dLx;
                                s[iL].DelH = s[iL - 1].DelH + (s[iL - 1].dDelH * dLx + s[iL - 1].ddDelH * dLx * dLx / 2);
                                s[iL].DelL = s[iL - 1].DelL + Math.Sqrt(dLx * dLx - Math.Pow(s[iL].DelH - s[iL - 1].DelH, 2));
                                s[iL].M = s[iL - 1].M + (P * (s[iL - 1].dDelH * dLx + s[iL - 1].ddDelH * dLx * dLx / 2)) - Q * dLx;

                                dM = s[iL].M - prev_s[iL].M;
                            }

                            // 3-2] 断面の曲率と各要素の歪度の増分・弾塑性判定
                            M_Phi(iL);
                            s[iL].ddDelH = s[iL].Phi;

                        }

                        // 4] 収束の判定
                        if (Math.Abs(s[DivL - 1].ddDelH) > 0.0000001)
                        {
                            // 剪断力の増分の再設定
                            if (s[DivL - 1].ddDelH < 0)
                                Q -= 0.99 * dQ;
                            else
                                Q += dQ;
                            // 収束するまで繰り返し実行
                            continue;
                        }
                        else
                        {
                            //収束
                            break;
                        }
                    }

                    TotalDelH += Math.Abs(s[DivL - 1].DelH - prev_s[DivL - 1].DelH);

                    for (int iL = 0; iL < DivL; iL++)
                    {
                        prev_s[iL].M = s[iL].M;
                        prev_s[iL].Phi = s[iL].Phi;
                        prev_s[iL].DelH = s[iL].DelH;

                        for (int iH = 0; iH < DivH; iH++)
                        {
                            prev_s[iL].p[iH].Eps_n = s[iL].p[iH].Eps_n;
                            prev_s[iL].p[iH].Sig_n = s[iL].p[iH].Sig_n;
                            prev_s[iL].p[iH].E_n = s[iL].p[iH].E_n;
                            prev_s[iL].p[iH].Eps_t = s[iL].p[iH].Eps_t;
                            prev_s[iL].p[iH].SigEpsState = s[iL].p[iH].SigEpsState;
                            prev_s[iL].p[iH].BauschingerState = s[iL].p[iH].BauschingerState;
                            prev_s[iL].p[iH].RecoverSig_pos = s[iL].p[iH].RecoverSig_pos;
                            prev_s[iL].p[iH].RecoverSig_neg = s[iL].p[iH].RecoverSig_neg;
                            prev_s[iL].p[iH].TotalEps_t = s[iL].p[iH].TotalEps_t;
                            prev_s[iL].p[iH].TotalPlasticEps_t = s[iL].p[iH].TotalPlasticEps_t;
                            prev_s[iL].p[iH].TotalEps_t_pos = s[iL].p[iH].TotalEps_t_pos;
                            prev_s[iL].p[iH].SigError = s[iL].p[iH].SigError;
                            prev_s[iL].p[iH].BausBoundSig_t_pos = s[iL].p[iH].BausBoundSig_t_pos;
                            prev_s[iL].p[iH].BausBoundSig_t_neg = s[iL].p[iH].BausBoundSig_t_neg;
                            prev_s[iL].p[iH].BausEps_t_pos = s[iL].p[iH].BausEps_t_pos;
                            prev_s[iL].p[iH].BausEps_t_neg = s[iL].p[iH].BausEps_t_neg;
                            prev_s[iL].p[iH].E_t = s[iL].p[iH].E_t;
                        }
                    }

                    //基本情報
                    if (step_count % 20 == 0)
                        SaveCurrentState(step_count);

                    // 目標変形到達の確認
                    if (force_direction * s[DivL - 1].DelH >= force_direction * target_delH_list[iDelH])
                    {
                        Console.WriteLine("★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★");
                        SaveCurrentState(step_count);
                        SaveVisualizeBeam();
                        break;
                    }
                }
            }

            Console.WriteLine("★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ 計算終了");

        }

        /// <summary>
        /// モーメント－曲率関係計算
        /// モーメントおよび軸力の増分に対する計算 
        ///  ※軸力の作用位置について修正(コメント文の※630)
        /// </summary>
        private void M_Phi(int iL)
        {
            // 積分計算
            var h = 0d; // H方向の位置（資料ではz方向）
            var F1 = 0d;
            var F2 = 0d;
            var F3 = 0d;

            for (int iH = 0; iH < DivH; iH++)
            {
                // h は 微小要素の中心に設定。
                h += s[iL].p[iH].dH / 2;

                F1 += prev_s[iL].p[iH].E_n * s[iL].p[iH].B * s[iL].p[iH].dH;
                F2 += prev_s[iL].p[iH].E_n * s[iL].p[iH].B * s[iL].p[iH].dH * h;
                F3 += prev_s[iL].p[iH].E_n * s[iL].p[iH].B * s[iL].p[iH].dH * h * h;

                // 残りの長さを後で足す。
                h += s[iL].p[iH].dH / 2;
            }

            // 引張縁を中心にモーメントの釣り合い式を作っている
            dM += dP * F2 / F1;
            var dPhi = (dM - dP * F2 / F1) / (F3 - F2 * F2 / F1);
            var dEps = dP / F1 - F2 / F1 * dPhi;
            s[iL].Phi = prev_s[iL].Phi + dPhi;


            // 断面要素における歪度
            h = 0; //リセット
            for (int iH = 0; iH < DivH; iH++)
            {
                // h は 微小要素の中心に設定。
                h += s[iL].p[iH].dH / 2;

                // 公称歪度の増分
                s[iL].p[iH].dEps_n = dEps + dPhi * h;

                // 公称歪度
                s[iL].p[iH].Eps_n = prev_s[iL].p[iH].Eps_n + s[iL].p[iH].dEps_n;

                // 真歪度への変換
                if (s[iL].p[iH].Eps_n >= -1)
                    s[iL].p[iH].Eps_t = Math.Log(1 + s[iL].p[iH].Eps_n);
                else
                    s[iL].p[iH].Eps_t = -1;

                // 残りの長さを後で足す。
                h += s[iL].p[iH].dH / 2;
            }

            // 弾塑性判定[接線剛性の変更]
            HCSS06(iL);

        }

        /// <summary>
        /// バウシンガー効果を考慮した鋼材の履歴モデルに基づく弾塑性判定
        /// </summary>
        private void HCSS06(int iL)
        {
            for (int iH = 0; iH < DivH; iH++)
            {
                var p = s[iL].p[iH];
                var prev_p = prev_s[iL].p[iH];

                //収斂計算初期状態へ戻す
                p.SigEpsState = prev_p.SigEpsState;
                p.BauschingerState = prev_p.BauschingerState;
                p.RecoverSig_pos = prev_p.RecoverSig_pos;
                p.RecoverSig_neg = prev_p.RecoverSig_neg;
                p.TotalEps_t = prev_p.TotalEps_t;
                p.TotalPlasticEps_t = prev_p.TotalPlasticEps_t;
                p.TotalEps_t_pos = prev_p.TotalEps_t_pos;
                p.BausBoundSig_t_pos = prev_p.BausBoundSig_t_pos;
                p.BausBoundSig_t_neg = prev_p.BausBoundSig_t_neg;
                p.BausEps_t_pos = prev_p.BausEps_t_pos;
                p.BausEps_t_neg = prev_p.BausEps_t_neg;
                p.E_t = prev_p.E_t;
                p.E_n = prev_p.E_n;
                p.SigError = prev_p.SigError;

                //公称応力度
                p.Sig_n = prev_p.Sig_n + p.dEps_n * prev_p.E_n;
                //真応力度への変換
                p.Sig_t = p.Sig_n * (1 + p.Eps_n);
                //差を保存しておく
                var dEps_t = p.Eps_t - prev_p.Eps_t;

                //応力状態に応じて処理
                switch (p.SigEpsState)
                {
                    // 1]弾性域
                    case SigEpsState.Elastic:
                        {
                            if (p.Sig_t >= p.RecoverSig_pos)
                            {
                                //1
                                p.E_t = p.Steel.Steps[1].E_t;
                                p.E_n = p.Steel.Steps[1].E_n_t;

                                p.SigEpsState = SigEpsState.PlasticPos;
                                p.SigError = p.Sig_t - p.RecoverSig_pos;
                            }
                            else if (p.Sig_t <= p.RecoverSig_neg)
                            {
                                //2
                                p.E_t = p.Steel.Steps[1].E_t;
                                p.E_n = p.Steel.Steps[1].E_n_c;

                                p.SigEpsState = SigEpsState.PlasticNeg;
                                p.SigError = p.Sig_t - p.RecoverSig_neg;

                            }
                        }
                        break;

                    // 2]正側塑性域
                    case SigEpsState.PlasticPos:
                        {
                            p.TotalEps_t += dEps_t;
                            p.TotalEps_t_pos += dEps_t;
                            p.TotalPlasticEps_t += dEps_t;

                            if (dEps_t < 0)
                            {
                                //3
                                p.E_t = p.Steel.Steps[0].E_t;
                                p.E_n = p.Steel.Steps[0].E_n_t;

                                p.RecoverSig_pos = p.Sig_t;
                                p.SigEpsState = SigEpsState.BausFromPos;

                                p.BausBoundSig_t_pos = p.Steel.ALF * p.RecoverSig_pos;
                                p.BausBoundSig_t_neg = p.Steel.ALF * p.RecoverSig_neg;
                                p.BauschingerState = BausState.Baus1;
                                p.BausEps_t_pos = p.Eps_t;
                                p.BausEps_t_neg = p.Eps_t + ((p.RecoverSig_neg - p.RecoverSig_pos) / p.Steel.Steps[0].E_t) - p.Steel.BCF * p.TotalEps_t;

                            }
                            // 2次剛性の変更
                            else
                            {
                                for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                {
                                    if (p.Sig_t >= p.Steel.Steps[i].Sig_t)
                                    {
                                        //4
                                        p.E_t = p.Steel.Steps[i + 1].E_t;
                                        p.E_n = p.Steel.Steps[i + 1].E_n_t;
                                    }
                                }
                            }


                        }
                        break;

                    // 3]負側塑性域
                    case SigEpsState.PlasticNeg:
                        {
                            p.TotalEps_t -= dEps_t;
                            p.TotalPlasticEps_t -= dEps_t;

                            if (dEps_t > 0)
                            {
                                //5
                                p.E_t = p.Steel.Steps[0].E_t;
                                p.E_n = p.Steel.Steps[0].E_n_c;

                                p.RecoverSig_neg = p.Sig_t;
                                p.SigEpsState = SigEpsState.BausFromNeg;

                                p.BausBoundSig_t_pos = p.Steel.ALF * p.RecoverSig_pos;
                                p.BausBoundSig_t_neg = p.Steel.ALF * p.RecoverSig_neg;
                                p.BauschingerState = BausState.Baus1;
                                p.BausEps_t_pos = p.Eps_t + (p.RecoverSig_pos - p.RecoverSig_neg) / p.Steel.Steps[0].E_t + p.Steel.BCF * p.TotalEps_t;
                                p.BausEps_t_neg = p.Eps_t;

                            }
                            // 2次剛性の変更
                            else
                            {
                                for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                {
                                    if (p.Sig_t <= -p.Steel.Steps[i].Sig_t)
                                    {
                                        //6
                                        p.E_t = p.Steel.Steps[i + 1].E_t;
                                        p.E_n = p.Steel.Steps[i + 1].E_n_c;
                                    }
                                }
                            }
                        }
                        break;

                    // 4]除荷＆バウシンガ[正→負]
                    case SigEpsState.BausFromPos:
                        {
                            switch (p.BauschingerState)
                            {

                                case BausState.Baus1:
                                    {

                                        if (p.Sig_t <= 0)
                                        {
                                            p.RecoverSig_pos = p.RecoverSig_pos - p.SigError;
                                            p.SigError = 0.0;
                                        }

                                        if (p.Sig_t >= p.RecoverSig_pos)
                                        {
                                            for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                            {
                                                if (p.Sig_t >= p.Steel.Steps[i].Sig_t)
                                                {
                                                    //7
                                                    p.E_t = p.Steel.Steps[i + 1].E_t;
                                                    p.E_n = p.Steel.Steps[i + 1].E_n_t;
                                                }
                                            }

                                            p.SigError = p.Sig_t - p.RecoverSig_pos;
                                            p.SigEpsState = SigEpsState.PlasticPos;

                                        }
                                        else if (p.Sig_t <= p.BausBoundSig_t_neg)
                                        {
                                            //8
                                            p.E_t = (p.RecoverSig_neg - p.BausBoundSig_t_neg) / (p.BausEps_t_neg - p.Eps_t);
                                            p.E_n = (p.RecoverSig_neg / Math.Exp(p.BausEps_t_neg) - p.BausBoundSig_t_neg / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_neg) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus2;
                                        }
                                    }
                                    break;

                                case BausState.Baus2:
                                    {
                                        p.TotalPlasticEps_t = p.TotalPlasticEps_t - dEps_t;

                                        if (p.Sig_t <= p.RecoverSig_neg)
                                        {
                                            for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                            {
                                                if (p.Sig_t <= -1 * p.Steel.Steps[i].Sig_t)
                                                {
                                                    //9
                                                    p.E_t = p.Steel.Steps[i + 1].E_t;
                                                    p.E_n = p.Steel.Steps[i + 1].E_n_c;
                                                }
                                            }

                                            p.SigError = p.Sig_t - p.RecoverSig_neg;
                                            p.SigEpsState = SigEpsState.PlasticNeg;
                                        }
                                        else if (dEps_t > 0)
                                        {
                                            //10
                                            p.E_t = p.Steel.Steps[0].E_t;
                                            p.E_n = p.Steel.Steps[0].E_n_c;

                                            p.BauschingerState = BausState.Baus3;
                                            p.BausBoundSig_t_neg = p.Sig_t;
                                        }
                                    }
                                    break;

                                case BausState.Baus3:
                                    {
                                        if (p.Sig_t <= p.BausBoundSig_t_neg)
                                        {
                                            p.BausBoundSig_t_pos = p.Steel.ALF * p.RecoverSig_pos;
                                            //11
                                            p.E_t = (p.RecoverSig_neg - p.BausBoundSig_t_neg) / (p.BausEps_t_neg - p.Eps_t);
                                            p.E_n = (p.RecoverSig_neg / Math.Exp(p.BausEps_t_neg) - p.BausBoundSig_t_neg / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_neg) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus2;
                                        }
                                        else if (p.Sig_t >= p.BausBoundSig_t_pos)
                                        {
                                            p.BausBoundSig_t_neg = p.Steel.ALF * p.RecoverSig_neg;
                                            //12
                                            p.E_t = (p.RecoverSig_pos - p.BausBoundSig_t_pos) / (p.BausEps_t_pos - p.Eps_t);
                                            p.E_n = (p.RecoverSig_pos / Math.Exp(p.BausEps_t_pos) - p.BausBoundSig_t_pos / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_pos) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus4;

                                        }
                                    }
                                    break;

                                case BausState.Baus4:
                                    {
                                        p.TotalPlasticEps_t = p.TotalPlasticEps_t + dEps_t;

                                        if (p.Sig_t >= p.RecoverSig_pos)
                                        {
                                            for (int IB = 0; IB < p.Steel.Num_of_Data; IB++)
                                            {
                                                if (p.Sig_t >= p.Steel.Steps[IB].Sig_t)
                                                {
                                                    //13
                                                    p.E_t = p.Steel.Steps[IB + 1].E_t;
                                                    p.E_n = p.Steel.Steps[IB + 1].E_n_t;
                                                }
                                            }

                                            p.SigError = p.Sig_t - p.RecoverSig_pos;
                                            p.SigEpsState = SigEpsState.PlasticPos;

                                        }
                                        else if (dEps_t < 0)
                                        {
                                            //14
                                            p.E_t = p.Steel.Steps[0].E_t;
                                            p.E_n = p.Steel.Steps[0].E_n_t;

                                            p.BauschingerState = BausState.Baus3;
                                            p.BausBoundSig_t_pos = p.Sig_t;
                                        }
                                    }
                                    break;

                                default:
                                    throw new Exception("BausState未定義状態");
                            }
                        }
                        break;

                    // 5]除荷＆バウシンガー[負→正]
                    case SigEpsState.BausFromNeg:
                        {
                            switch (p.BauschingerState)
                            {

                                case BausState.Baus1:
                                    {
                                        if (p.Sig_t >= 0)
                                        {
                                            p.RecoverSig_neg = p.RecoverSig_neg - p.SigError;
                                            p.SigError = 0;
                                        }

                                        if (p.Sig_t <= p.RecoverSig_neg)
                                        {
                                            for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                            {
                                                if (p.Sig_t < -1 * p.Steel.Steps[i].Sig_t)
                                                {
                                                    //15
                                                    p.E_t = p.Steel.Steps[i + 1].E_t;
                                                    p.E_n = p.Steel.Steps[i + 1].E_n_c;
                                                }
                                            }

                                            p.SigError = p.Sig_t - p.RecoverSig_neg;
                                            p.SigEpsState = SigEpsState.PlasticNeg;
                                        }
                                        else if (p.Sig_t >= p.BausBoundSig_t_pos)
                                        {
                                            //16
                                            p.E_t = (p.RecoverSig_pos - p.BausBoundSig_t_pos) / (p.BausEps_t_pos - p.Eps_t);
                                            p.E_n = (p.RecoverSig_pos / Math.Exp(p.BausEps_t_pos) - p.BausBoundSig_t_pos / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_pos) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus2;
                                        }
                                    }
                                    break;

                                case BausState.Baus2:
                                    {
                                        p.TotalPlasticEps_t = p.TotalPlasticEps_t + dEps_t;

                                        if (p.Sig_t >= p.RecoverSig_pos)
                                        {
                                            for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                            {
                                                if (p.Sig_t >= p.Steel.Steps[i].Sig_t)
                                                {
                                                    //17
                                                    p.E_t = p.Steel.Steps[i + 1].E_t;
                                                    p.E_n = p.Steel.Steps[i + 1].E_n_t;
                                                }
                                                p.SigError = p.Sig_t - p.RecoverSig_pos;
                                                p.SigEpsState = SigEpsState.PlasticPos;
                                            }
                                        }
                                        else if (dEps_t < 0)
                                        {
                                            //18
                                            p.E_t = p.Steel.Steps[0].E_t;
                                            p.E_n = p.Steel.Steps[0].E_n_t;

                                            p.BauschingerState = BausState.Baus3;
                                            p.BausBoundSig_t_pos = p.Sig_t;
                                        }

                                    }
                                    break;

                                case BausState.Baus3:
                                    {
                                        if (p.Sig_t >= p.BausBoundSig_t_pos)
                                        {

                                            p.BausBoundSig_t_neg = p.Steel.ALF * p.RecoverSig_neg;
                                            //19
                                            p.E_t = (p.RecoverSig_pos - p.BausBoundSig_t_pos) / (p.BausEps_t_pos - p.Eps_t);
                                            p.E_n = (p.RecoverSig_pos / Math.Exp(p.BausEps_t_pos) - p.BausBoundSig_t_pos / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_pos) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus2;

                                        }
                                        else if (p.Sig_t <= p.BausBoundSig_t_neg)
                                        {

                                            p.BausBoundSig_t_pos = p.Steel.ALF * p.RecoverSig_pos;
                                            //20
                                            p.E_t = (p.RecoverSig_neg - p.BausBoundSig_t_neg) / (p.BausEps_t_neg - p.Eps_t);
                                            p.E_n = (p.RecoverSig_neg / Math.Exp(p.BausEps_t_neg) - p.BausBoundSig_t_neg / Math.Exp(p.Eps_t)) / ((Math.Exp(p.BausEps_t_neg) - 1) - (Math.Exp(p.Eps_t) - 1));

                                            p.BauschingerState = BausState.Baus4;
                                        }
                                    }
                                    break;

                                case BausState.Baus4:
                                    {
                                        p.TotalPlasticEps_t = p.TotalPlasticEps_t - dEps_t;

                                        if (p.Sig_t <= p.RecoverSig_neg)
                                        {
                                            for (int i = 0; i < p.Steel.Num_of_Data; i++)
                                            {
                                                if (p.Sig_t <= -1 * p.Steel.Steps[i].Sig_t)
                                                {
                                                    //21
                                                    p.E_t = p.Steel.Steps[i + 1].E_t;
                                                    p.E_n = p.Steel.Steps[i + 1].E_n_c;
                                                }
                                            }

                                            p.SigError = p.Sig_t - p.RecoverSig_neg;
                                            p.SigEpsState = SigEpsState.PlasticNeg;

                                        }
                                        else if (dEps_t > 0)
                                        {
                                            //22
                                            p.E_t = p.Steel.Steps[0].E_t;
                                            p.E_n = p.Steel.Steps[0].E_n_c;

                                            p.BauschingerState = BausState.Baus3;
                                            p.BausBoundSig_t_neg = p.Sig_t;
                                        }
                                    }
                                    break;

                                default:
                                    throw new Exception("BausState未定義状態");
                            }
                        }
                        break;

                    default:
                        throw new Exception("SigEpsState未定義状態");
                }
            }
        }

        /// <summary>
        /// 梁を可視化します。
        /// </summary>
        private void SaveVisualizeBeam()
        {
            //SaveVisualizeBeam("dH", new Func<MemberPiece, string>(p =>
            // {
            //     return p.dH.ToString("00");
            // }), "  ");

            SaveVisualizeBeam("SigEpsState", new Func<MemberPiece, string>(p =>
            {
                return p.SigEpsState.GetHashCode().ToString();
            }));
        }
        private void SaveVisualizeBeam(string image_name, Func<MemberPiece, string> func_display, string for_null_value = " ")
        {
            // 結果文字列を出力
            try
            {
                using (var sw = new StreamWriter(PathVisualizedBeamResult, true, Encoding.UTF8))
                {
                    Console.WriteLine();
                    Console.WriteLine(image_name);
                    sw.WriteLine();
                    sw.WriteLine(image_name);

                    for (int iH = 0; iH < DivH; iH++)
                    {
                        string s = "";
                        for (int iL = 0; iL < DivL; iL++)
                        {
                            if (iL % (DivL * for_null_value.Length / 150) != 0)
                                continue;

                            if (this.s[iL].p[iH].IsBroken)
                                s += for_null_value;
                            else if (this.s[iL].p[iH].E_n < 1)
                                s += for_null_value;
                            else
                                s += func_display(this.s[iL].p[iH]);
                        }
                        Console.WriteLine(s);
                        sw.WriteLine(s);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                }

                Console.WriteLine($"出力：{Path.GetFileName(PathVisualizedBeamResult)}");
            }
            catch (Exception e)
            {
                Console.WriteLine("途中で出力に失敗。");
                Console.WriteLine(e.Message);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 梁の現在の解析中の情報を出力します。
        /// </summary>
        /// <param name="step_count"></param>
        private void SaveCurrentState(int step_count)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Step   = " + step_count);
            Console.WriteLine("載荷   = " + Q);
            Console.WriteLine("根元M  = " + s[0].M / 1000 / 1000);
            Console.WriteLine("部材φ = " + s[DivL - 1].DelH / L);
            Console.WriteLine("先端θ = " + s[DivL - 1].dDelH);

            // M-φ図を出力
            try
            {
                var resultLine = string.Join(",", new double[]
                {
                    step_count,
                    Q,
                    s[0].M / 1000 / 1000,
                    s[DivL - 1].DelH / L,
                    TotalDelH / L
                }.Select(x => x.ToString()));

                using (var sw = new StreamWriter(PathMPhiResult, true, Encoding.UTF8))
                    sw.WriteLine(resultLine);

                Console.WriteLine($"出力：{Path.GetFileName(PathMPhiResult)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("途中で出力に失敗。");
                Console.WriteLine(ex.Message);
            }

            // 危険断面関連事項を出力
            try
            {
                // 材端[スカラップ底]のデータ
                var iL = 2;
                //var iL = 5;
                var iH = 4;

                var resultLine = string.Join(",", new double[]
                {
                step_count,
                Q,
                s[0].M / 1000 / 1000,
                s[DivL - 1].DelH / L,
                TotalDelH / L,
                s[iL].ddDelH,
                s[iL].M / 1000 / 1000,
                s[iL].p[0+iH].Eps_n,
                s[iL].p[0+iH].Sig_n,
                s[iL].p[0+iH].Eps_t,
                s[iL].p[0+iH].Sig_t,
                s[iL].p[0+iH].TotalPlasticEps_t,
                s[iL].p[0+iH].TotalEps_t_pos,
                s[iL].p[DivH - 1-iH].Eps_n,
                s[iL].p[DivH - 1-iH].Sig_n,
                s[iL].p[DivH - 1-iH].Eps_t,
                s[iL].p[DivH - 1-iH].Sig_t,
                s[iL].p[DivH - 1-iH].TotalPlasticEps_t,
                s[iL].p[DivH - 1-iH].TotalEps_t_pos,
                }.Select(x => x.ToString()));

                using (var sw = new StreamWriter(PathDangerousSectionResult, true, Encoding.UTF8))
                    sw.WriteLine(resultLine);

                Console.WriteLine($"出力：{Path.GetFileName(PathDangerousSectionResult)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("途中で出力に失敗。");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
