using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    partial class Member
    {
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
        private void SaveVisualizeBeam(string imageName, Func<MemberPiece, string> displayStringDefinition, string nullValue = " ")
        {
            // 結果文字列を出力
            try
            {
                using (var sw = new StreamWriter(PathVisualizedBeamResult.FullName, true, Encoding.UTF8))
                {
                    Console.WriteLine();
                    Console.WriteLine(imageName);
                    sw.WriteLine();
                    sw.WriteLine(imageName);

                    for (int iH = 0; iH < DivH; iH++)
                    {
                        string s = "";
                        for (int iL = 0; iL < DivL; iL++)
                        {
                            if (iL % (DivL * nullValue.Length / 150) != 0)
                                continue;

                            if (this.s[iL].p[iH].IsBroken)
                                s += nullValue;
                            else if (this.s[iL].p[iH].E_n < 1)
                                s += nullValue;
                            else
                                s += displayStringDefinition(this.s[iL].p[iH]);
                        }
                        Console.WriteLine(s);
                        sw.WriteLine(s);
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                }

                Console.WriteLine($"出力：{PathVisualizedBeamResult.Name}");
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

                using (var sw = new StreamWriter(PathMPhiResult.FullName, true, Encoding.UTF8))
                    sw.WriteLine(resultLine);

                Console.WriteLine($"出力：{PathMPhiResult.Name}");
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

                using (var sw = new StreamWriter(PathDangerousSectionResult.FullName, true, Encoding.UTF8))
                    sw.WriteLine(resultLine);

                Console.WriteLine($"出力：{PathDangerousSectionResult.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("途中で出力に失敗。");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
