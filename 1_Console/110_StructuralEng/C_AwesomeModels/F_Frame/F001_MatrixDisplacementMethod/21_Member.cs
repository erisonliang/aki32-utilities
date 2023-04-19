using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 部材
    /// </summary>
    public class Member
    {
        // ★★★★★★★★★★★★★★★ props

        // ★★★★★ 入力情報
        public MaterialType Material { get; set; }
        public CrossSection Section { get; set; }
        public Edge[] Edges { get; set; } = new Edge[2];
        public bool IsRealMember { get; set; } = true;
        public List<Load> Loads { get; set; }

        // ★★★★★ 算出情報
        public Stress Stress;
        public double Length { get; set; }
        public double Volume { get; set; }
        public double AngleInRad { get; set; }

        /// <summary>
        /// 部材剛性マトリックス(変換前)
        /// </summary>
        public DenseMatrix M_k;
        /// <summary>
        /// 部材変換マトリックス
        /// </summary>
        public DenseMatrix M_Converse;
        /// <summary>
        /// 部材剛性マトリックス(変換後)
        /// </summary>
        public DenseMatrix M_K;


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// </summary>
        /// <param name="section">断面形状</param>
        /// <param name="material"></param>
        /// <param name="Node1">第１端の節点番号</param>
        /// <param name="Node2">第２端の節点番号</param>
        /// <param name="Node1IsPin">第１端はピンである</param>
        /// <param name="Node2IsPin">第２端はピンである</param>
        /// <param name="Nodes">全節点情報</param>
        public Member(CrossSection section, MaterialType material, int Node1, int Node2, bool Node1IsPin, bool Node2IsPin, List<Node> Nodes)
        {
            // ★★★★★ 既知の数値を代入
            Section = section;
            Material = material;
            Loads = new List<Load>();
            Stress = new Stress();
            IsRealMember = true;


            // ★★★★★ 節点の配置決め

            // 節点の座標割り出し
            double X1 = Nodes[Node1].Point.X;
            double Y1 = Nodes[Node1].Point.Y;
            double X2 = Nodes[Node2].Point.X;
            double Y2 = Nodes[Node2].Point.Y;

            // 部材長
            Length = Math.Sqrt(Math.Pow((X1 - X2), 2) + Math.Pow((Y1 - Y2), 2));

            // -90<θ<=90となるように，節点の順番を調整
            if ((X1 == X2 && Y1 > Y2) || (X1 > X2))
            {
                (Node2, Node1) = (Node1, Node2);
                Edges[0].IsNodePin = Node2IsPin;
                Edges[1].IsNodePin = Node1IsPin;
            }
            else
            {
                Edges[0].IsNodePin = Node1IsPin;
                Edges[1].IsNodePin = Node2IsPin;
            }

            // 決定，代入
            Edges[0].Node = Node1;
            Edges[1].Node = Node2;
            Edges[0].Node_Fix = Nodes[Node1].Fix;
            Edges[1].Node_Fix = Nodes[Node2].Fix;


            // ★★★★★ 部材角算出

            // 節点の座標再割り出し
            X1 = Nodes[Node1].Point.X;
            Y1 = Nodes[Node1].Point.Y;
            X2 = Nodes[Node2].Point.X;
            Y2 = Nodes[Node2].Point.Y;
            // -90<Psi<=90
            if (Length == 0)
            {
                Length = double.MinValue;
                AngleInRad = 0;
            }
            else if (X1 == X2)
                AngleInRad = Math.PI / 2;
            else if (Y1 >= Y2)
                AngleInRad = -Math.Atan((Y1 - Y2) / (X2 - X1));
            else if (Y1 < Y2)
                AngleInRad = Math.Atan((Y2 - Y1) / (X2 - X1));
            else
                throw new Exception();

            // ★★★★★

            UpdateMatrix();
        }


        // ★★★★★★★★★★★★★★★ methods

        public void UpdateMatrix()
        {
            // ★★★★★ M_k
            double EAL = Section.E * Section.A / Length;
            double EIL = Section.E * Section.I / Length;
            double EILL = EIL / Length;
            double EILLL = EILL / Length;

            if (Edges[0].IsNodePin)
            {
                if (Edges[1].IsNodePin)
                {
                    M_k = DenseMatrix.OfArray(new double[6, 6]
                    {
                        { EAL , 0 , 0 ,-EAL , 0 , 0 },
                        { 0   , 0 , 0 , 0   , 0 , 0 },
                        { 0   , 0 , 0 , 0   , 0 , 0 },
                        {-EAL , 0 , 0 , EAL , 0 , 0 },
                        { 0   , 0 , 0 , 0   , 0 , 0 },
                        { 0   , 0 , 0 , 0   , 0 , 0 },
                    });
                }
                else
                {
                    M_k = DenseMatrix.OfArray(new double[6, 6]
                    {
                        { EAL , 0       , 0 ,-EAL , 0       , 0      },
                        { 0   , 3*EILLL , 0 , 0   ,-3*EILLL ,-3*EILL },
                        { 0   , 0       , 0 , 0   , 0       , 0      },
                        {-EAL , 0       , 0 , EAL , 0       , 0      },
                        { 0   ,-3*EILLL , 0 , 0   , 3*EILLL , 3*EILL },
                        { 0   ,-3*EILL  , 0 , 0   , 3*EILL  , 3*EIL  },
                    });
                }
            }
            else
            {
                if (Edges[1].IsNodePin)
                {
                    M_k = DenseMatrix.OfArray(new double[6, 6]
                    {
                        { EAL , 0       , 0      ,-EAL , 0       , 0 },
                        { 0   , 3*EILLL ,-3*EILL , 0   ,-3*EILLL , 0 },
                        { 0   ,-3*EILL  , 3*EIL  , 0   , 3*EILL  , 0 },
                        {-EAL , 0       , 0      , EAL , 0       , 0 },
                        { 0   ,-3*EILLL , 3*EILL , 0   , 3*EILLL , 0 },
                        { 0   , 0       , 0      , 0   , 0       , 0 },
                    });
                }
                else
                {
                    M_k = DenseMatrix.OfArray(new double[6, 6]
                    {
                        { EAL , 0        , 0      ,-EAL , 0        , 0      },
                        { 0   , 12*EILLL ,-6*EILL , 0   ,-12*EILLL ,-6*EILL },
                        { 0   ,-6*EILL   , 4*EIL  , 0   , 6*EILL   , 2*EIL  },
                        {-EAL , 0        , 0      , EAL , 0        , 0      },
                        { 0   ,-12*EILLL , 6*EILL , 0   , 12*EILLL , 6*EILL },
                        { 0   ,-6*EILL   , 2*EIL  , 0   , 6*EILL   , 4*EIL  },
                    });
                }
            }

            // ★★★★★ M_Converse
            double C = Math.Cos(AngleInRad);
            double S = Math.Sin(AngleInRad);
            M_Converse = DenseMatrix.OfArray(new double[6, 6]
            {
                    { C , S , 0 , 0 , 0 , 0 },
                    {-S , C , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 1 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , C , S , 0 },
                    { 0 , 0 , 0 ,-S , C , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 1 },
            });

            // ★★★★★ M_K
            M_K = (DenseMatrix)(M_Converse.Transpose() * M_k * M_Converse);

        }

        public void DisableMatrixes()
        {
            // ★★★★★ M_k
            M_k = DenseMatrix.OfArray(new double[6, 6]
            {
                    { 0 , 0 , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 0 },
            });

            // ★★★★★ M_Converse
            double C = Math.Cos(AngleInRad);
            double S = Math.Sin(AngleInRad);
            M_Converse = DenseMatrix.OfArray(new double[6, 6]
            {
                    { C , S , 0 , 0 , 0 , 0 },
                    {-S , C , 0 , 0 , 0 , 0 },
                    { 0 , 0 , 1 , 0 , 0 , 0 },
                    { 0 , 0 , 0 , C , S , 0 },
                    { 0 , 0 , 0 ,-S , C , 0 },
                    { 0 , 0 , 0 , 0 , 0 , 1 },
            });

            // ★★★★★ M_K
            M_K = (DenseMatrix)(M_Converse.Transpose() * M_k * M_Converse);
        }

        public double Update_Volume()
        {
            return Volume = Length * Section.A;
        }

        /// <summary>
        /// </summary>
        /// <param name="num">部材番号</param>
        public string ToString(int num)
        {
            if (!IsRealMember)
                return "";

            string A = "";
            A += "******************************************************** " + num + "\r\n";
            A += "*断面形状：\r\n" + Section.ToString() + "\r\n";
            A += "*材端情報：\r\n" + Edges[0].ToString() + Edges[1].ToString() + "\r\n";
            A += "*荷重情報：\r\n";
            foreach (var load in Loads)
                A += load.ToString() + "\r\n";
            A += "\r\n";
            A += "*応力状況：\r\n" + Stress.Print_Stress() + "\r\n";
            A += "*許容応力度：" + Stress.Print_f() + "\r\n";
            A += "*検定比：\r\n" + Stress.Print_Check() + "\r\n";
            A += "*部材剛性マトリックス：\r\n" + M_k.ToString() + "\r\n";
            return A;
        }

        // ★★★★★ 検定メソッドたち

        /// <summary>
        /// 解析。
        /// </summary>
        /// <param name="W">安全率</param>
        /// <param name="F">F値</param>
        /// <param name="term">1:終局，2:長期，3:短期</param>
        /// <returns>計算結果</returns>
        public bool Design(double W, double F)
        {
            try
            {
                switch (Material)
                {
                    case MaterialType.Steel:
                        {
                            // 検定比は，F値時を1.00とします。

                            //*****許容応力度の算出
                            //使う用語の定義
                            double Λ = Math.Sqrt(Math.PI * Math.PI * Section.E / 0.6D / F);
                            double λ = Length / Section.i;
                            double XXX = Math.Pow(λ / Λ, 2);
                            double M1 = Math.Abs(Stress.M1);
                            double M2 = Math.Abs(Stress.M2);
                            double MMM = (M1 == 0 || M2 == 0) ? 0 : (M1 > M2) ? M2 / M1 : M1 / M2;
                            double C = Math.Min(2.3D, 1.75D - 1.05D * MMM + 0.3D * Math.Pow(MMM, 2));

                            //引張許容応力度
                            double ft = F / W;

                            //剪断許容応力度
                            double fs = ft / Math.Sqrt(3);

                            //圧縮許容応力度(全長を座屈長さとする。)
                            double fc = (λ <= Λ) ?
                                F * (1D - 0.4D * XXX) / (1D + 4D / 9D * XXX) / W :
                                F * (27D / 65D) / XXX / W;

                            //曲げ許容応力度
                            double fb = Math.Max(ft, ft * (1D - 0.4D * XXX / C));

                            //*****検定比の算出
                            //使う用語の定義
                            double σb = Math.Max(Math.Abs(Stress.σb1), Math.Abs(Stress.σb2));
                            double σc = -Stress.σN;
                            double τ = Math.Abs(Stress.τ);

                            //許容応力度
                            Stress.ft = ft;
                            Stress.fc = fc;
                            Stress.fs = fs;
                            Stress.fb = fb;
                            //検定比
                            Stress.Check_C = σc / fc + σb / fb;
                            Stress.Check_T = -(σc - σb) / ft;
                            Stress.Check_Q = τ / fs;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
