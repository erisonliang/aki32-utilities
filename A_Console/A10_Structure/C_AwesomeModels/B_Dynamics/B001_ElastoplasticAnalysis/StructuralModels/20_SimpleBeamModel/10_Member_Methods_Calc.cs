using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class SimpleBeamModel
{
    partial class Member
    {
        /// <summary>
        /// コンストラクタ。
        /// 範囲の設定は資料を参照！
        /// </summary>
        /// <param name="outputDir">計算結果の出力先</param>
        /// <param name="steels">鋼材リスト</param>
        /// <param name="sectionType">断面形状</param>
        /// <param name="sig_y">材料の降伏応力度の代表値</param>
        /// <param name="E">材料の弾性係数の代表値</param>
        /// 
        /// <param name="B">梁幅（B方向長さ）</param>
        /// <param name="H">梁せい（H方向長さ）</param>
        /// <param name="L">梁長さ（L方向長さ）</param>
        /// <param name="tw">ウェブ厚さ</param>
        /// <param name="tf">フランジ厚さ</param>
        /// <param name="Hs">スカラップのH方向長さ</param>
        /// <param name="Ls">スカラップのL方向長さ</param>
        /// <param name="Lw">溶接部分のL方向長さ</param>
        /// 
        /// <param name="dL">L方向の微小要素長さ</param>
        /// <param name="divH">H方向の微小要素数</param>
        /// <param name="divHf">フランジのH方向の微小要素数</param>
        /// <param name="divHs">スカラップのH方向微小要素数</param>
        /// 
        /// <param name="n_ratio"></param>
        /// <param name="considerQDef"></param>
        /// <exception cref="Exception"></exception>
        public Member(DirectoryInfo outputDir,

            List<Steel> steels, SectionType sectionType,
            double sig_y, double E,

            double B, double H, double L, double tw, double tf,
            int Hs, int Ls, int Lw,

            double dL, int divH, int divHf, int divHs,

            int n_ratio, bool considerQDef

            )
        {

            #region 出力ファイル

            OutputDir = outputDir;

            // M-φを出力
            try
            {
                using var sw = new StreamWriter(PathMPhiResult.FullName, true, Encoding.UTF8);
                sw.WriteLine("Step,載荷Q,根元M,部材θ,累積変形角");
            }
            catch (Exception ex)
            {
                Console.WriteLine(PathMPhiResult + " 出力失敗");
                Console.WriteLine(ex.Message);
            }

            // 危険断面関連事項を出力
            try
            {
                using var sw = new StreamWriter(PathDangerousSectionResult.FullName, true, Encoding.UTF8);
                sw.WriteLine(",,,,,,,上フランジ,,,,骨格曲線累積塑性歪,,下フランジ,,,,骨格曲線累積塑性歪,");
                sw.WriteLine("Step,載荷Q,根元M,部材θ,累積変形角,θ(危険断面),M(危険断面),εn,σn ,εt,σt,Σεpt,Σεpst+,εn,σn,εt,σt,Σεpt,Σεpst+");
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
            ConsiderQDef = considerQDef;

            #endregion

            #region ★★★★★ 諸元の自動算出

            // ★★★★★ 分割数
            DivL = (int)(this.L / dL);
            var dH = (double)Hs / divHs;

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


            // 降伏変形の計算
            I = (this.B * Math.Pow(this.H, 3) - (this.B - tw_total) * Math.Pow(this.H - 2 * tf, 3)) / 12;
            Aw_s = this.H * tw_total;
            Qp = Mp / this.L;
            DelP = Qp * Math.Pow(this.L, 3) / 3 / this.E / I;
            DelP += considerQDef ? (Qp / G / Aw_s * this.L) : 0;

            #endregion

            #region ★★★★★ 梁の分割の設定

            // ★★★★★ 初期分割
            s = Enumerable.Range(0, DivL).Select(_ => new MemberSection(divH)).ToArray();
            prev_s = Enumerable.Range(0, DivL).Select(_ => new MemberSection(divH)).ToArray();

            // ★★★★★ 全ての微小要素に対して実行

            for (int iL = 0; iL < DivL; iL++)
            {
                for (int iH = 0; iH < DivH; iH++)
                {
                    // 全部材に材料を設定
                    if (iH < divHf)
                        s[iL].p[iH].Steel = steels.First(x => x.Name == "F");
                    else if (iH < DivH - divHf)
                        s[iL].p[iH].Steel = steels.First(x => x.Name == "W");
                    else
                        s[iL].p[iH].Steel = steels.First(x => x.Name == "F");


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
                        else if (iH < (divHf + divHs))
                        {
                            s[iL].p[iH].dH = dH;
                            s[iL].p[iH].B = tw_total;
                        }

                        // ウェブ中間部 [残りを均等分割]
                        else if (iH < DivH - (divHf + divHs))
                        {
                            s[iL].p[iH].dH = (this.H - 2 * (tf + dH * divHs)) / (DivH - 2 * (divHf + divHs));
                            s[iL].p[iH].B = tw_total;
                        }

                        // スカラップ位置[dHs mmずつ DivHs 分割];
                        else if (iH < DivH - divHf)
                        {
                            s[iL].p[iH].dH = dH;
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
                for (int iH = divHf; iH < (divHf + divHs); iH++)
                {
                    prev_s[iL].p[iH].E_n = 0;
                    prev_s[iL].p[iH].E_t = 0;
                    s[iL].p[iH].E_n = 0;
                    s[iL].p[iH].E_t = 0;
                }

                // 下フランジ側
                for (int iH = DivH - (divHf + divHs); iH < DivH - divHf; iH++)
                {
                    prev_s[iL].p[iH].E_n = 0;
                    prev_s[iL].p[iH].E_t = 0;
                    s[iL].p[iH].E_n = 0;
                    s[iL].p[iH].E_t = 0;
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

                        CalcMPhi(iL);
                        prev_s[iL].Phi = s[iL].Phi;

                        for (int iH = 0; iH < DivH; iH++)
                        {
                            prev_s[iL].p[iH].Eps_n = s[iL].p[iH].Eps_n;
                            prev_s[iL].p[iH].Sig_n = s[iL].p[iH].Sig_n;
                            prev_s[iL].p[iH].E_n = s[iL].p[iH].E_n;
                            prev_s[iL].p[iH].Eps_t = s[iL].p[iH].Eps_t;
                            prev_s[iL].p[iH].SigEpsState = s[iL].p[iH].SigEpsState;
                            prev_s[iL].p[iH].BausState = s[iL].p[iH].BausState;
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
                int force_direction = iDelH == 0
                    ? Math.Sign(target_delH_list[iDelH])
                    : Math.Sign(target_delH_list[iDelH] - target_delH_list[iDelH - 1]);

                double dQ = 100;

                // 1ステップ分の計算
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
                            CalcMPhi(iL); // モーメント - 曲率関係計算
                            CalcEP(iL);   // 弾塑性判定[接線剛性の変更]
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
                            prev_s[iL].p[iH].BausState = s[iL].p[iH].BausState;
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
        /// モーメント - 曲率関係計算
        /// モーメントおよび軸力の増分に対する計算 
        /// </summary>
        private void CalcMPhi(int iL)
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

        }

        /// <summary>
        /// バウシンガー効果を考慮した鋼材の履歴モデルに基づく弾塑性判定（接線剛性の変更）
        /// </summary>
        private void CalcEP(int iL)
        {
            for (int iH = 0; iH < DivH; iH++)
            {
                var ppppp = s[iL].p[iH];
                var prev_p = prev_s[iL].p[iH];

                ppppp.CalcNext(prev_p);

            }
        }
  
    }
}
