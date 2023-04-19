using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Spatial.Euclidean;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 構造物
    /// </summary>
    public class Structure
    {
        // ★★★★★★★★★★★★★★★ props

        // ★★★★★ 入力情報
        public List<Node> Nodes { get; set; }
        public List<Member> Members { get; set; }

        /// <summary>
        /// F値 - 降伏や破壊などの，到達してはいけない基準！！
        /// </summary>
        public double F { get; set; }
        /// <summary>
        /// 安全率
        /// </summary>
        public double W { get; set; }
        /// <summary>
        /// 分布荷重の精度
        /// </summary>
        public int DistributedLoadDivision { get; set; } = 99;

        // ★★★★★ デザコン
        /// <summary>
        /// 体積の総和
        /// </summary>
        public double Volume;

        // ★★★★★ メモ
        public DenseVector LoadVector;
        public DenseMatrix GlobalK;
        public DenseVector SolutionVector;
        public string WarningMessage;

        // ★★★★★ 描写用情報
        public float ZoomCoefficient; //画像拡大係数
        private long WidthRange; //構造物の幅範囲
        private long WidthMin; //構造物の左端
        private long HeightRange; //構造物の高さ範囲
        private long HeightMin; //構造物の最低高さ

        private const int Base_ALL = 5; //構造全体の余白に対する大きさの係数
        private const int Correction = 555; //数値が小さくてもきれいに見えるようにする係数

        private const int Base_r = 25; //節点半径の大きさの係数
        private const int Base_Thickness = 5; //部材・線の太さの係数
        private const int Base_Letter = 50; //文字の大きさの係数
        private const int Base_Fulcrum = 20; //支点の大きさの係数
        private const int Base_Load = 100; //荷重表示の大きさの係数
        private const int Base_M = 500; //各図の大きさの係数

        public bool DrawNodesNum; //節点番号を描写 
        public bool DrawMembersNum; //部材番号を描写
        public bool DrawLoad; //荷重を描写
        public bool DrawDiagramNum; //各図の規模を描写


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 構造物を生成します。
        /// </summary>
        /// <param name="nodes">節点の集合情報</param>
        /// <param name="members">部材の集合情報</param>
        /// <param name="distributedLoadDivision">等分布荷重の分割数(推奨：99)</param>
        public Structure(List<Node> nodes, List<Member> members, int distributedLoadDivision = 99)
        {
            // ★★★★★ 代入
            Nodes = nodes;
            Members = members;
            DistributedLoadDivision = distributedLoadDivision;

            // ★★★★★ メモ
            LoadVector = DenseVector.Create(1, 0);
            SolutionVector = DenseVector.Create(1, 0);
            GlobalK = DenseMatrix.Create(1, 1, 0);
            WarningMessage = "";

            // ★★★★★ 表示情報を初期化
            DrawNodesNum = true;
            DrawMembersNum = true;
            DrawLoad = true;
            DrawDiagramNum = true;

            // ★★★★★ ピンであるべきところはピンにする
            for (int i = 0; i < Nodes.Count; i++)
            {
                //ピンorローラー支点に対して１本だけ剛接合なら，その端は自動でピンに。
                if (!Nodes[i].Fix.ThetaZ && (Nodes[i].Fix.DeltaX || Nodes[i].Fix.DeltaY))
                {
                    int count = 0;
                    foreach (var item in Members)
                        foreach (var item2 in item.Edges)
                            if (item2.Node == i && !item2.IsNodePin) //剛接続数をカウント
                                count++;

                    if (count == 1) //剛接合数が1なら，ピン端に。
                    {
                        for (int j = 0; j < Members.Count; j++)
                            for (int k = 0; k < 2; k++)
                                if (Members[j].Edges[k].Node == i)
                                {
                                    Members[j].Edges[k].IsNodePin = true;
                                    Members[j].UpdateMatrix();
                                }
                    }
                }

                //支点でないところに対して１本だけしか剛がないないなら，その剛は自動で種類変更。
                if (!Nodes[i].Fix.ThetaZ && !Nodes[i].Fix.DeltaX && !Nodes[i].Fix.DeltaY)
                {
                    int count = 0;
                    foreach (var item in Members)
                        foreach (var item2 in item.Edges)
                            if (item2.Node == i && !item2.IsNodePin) //剛接続数をカウント
                                count++;

                    if (count == 1) //剛接合数が１だった。
                    {
                        for (int j = 0; j < Members.Count; j++)
                            for (int k = 0; k < 2; k++)
                                if (Members[j].Edges[k].Node == i)
                                {
                                    if (Nodes[Members[j].Edges[k].Node].Loads.Count == 0) //荷重がないときはピン
                                        Members[j].Edges[k].IsNodePin = true;
                                    //else
                                    //    Members[j].edge[k].NodeIsPin = false;
                                    Members[j].UpdateMatrix();
                                }
                    }
                }
            }

            // ★★★★★ 分布荷重を集中荷重に置き換える
            var MemberClone = Members;
            for (int j = 0; j < MemberClone.Count; j++) //全部材に対して実行
            {
                foreach (var item in MemberClone[j].Loads) //全荷重に対して行う
                {
                    // ★ 変数の定義
                    int StartNode = Nodes.Count;
                    var edge0 = MemberClone[j].Edges[0];
                    var edge1 = MemberClone[j].Edges[1];
                    var section0 = MemberClone[j].Section;


                    // ★ 節点と荷重

                    for (int i = 0; i < DistributedLoadDivision; i++)
                    {
                        //分割する節点を生成
                        var point0 = new Point2D(
                           (Nodes[edge0.Node].Point.X * (i + 1D) + Nodes[edge1.Node].Point.X * (DistributedLoadDivision - i)) / (DistributedLoadDivision + 1D),
                           (Nodes[edge0.Node].Point.Y * (i + 1D) + Nodes[edge1.Node].Point.Y * (DistributedLoadDivision - i)) / (DistributedLoadDivision + 1D));
                        var fix0 = new Fix(false, false, false);
                        var node0 = new Node(point0, fix0);
                        node0.IsPseudNode = true;

                        //その荷重を生成
                        Load load0 = new Load();
                        if (item.Type == LoadType.DistributedLoad_GlobalGrid)
                            load0 = Load.CreateConcentratedLoad(
                                (item.P1_H * (i + 1D) + item.P2_H * (DistributedLoadDivision - i)) * MemberClone[j].Length / (DistributedLoadDivision + 1D) / (DistributedLoadDivision + 1D),
                                (item.P1_V * (i + 1D) + item.P2_V * (DistributedLoadDivision - i)) * MemberClone[j].Length / (DistributedLoadDivision + 1D) / (DistributedLoadDivision + 1D));
                        else if (item.Type == LoadType.DistributedLoad_MemberGrid)
                            load0 = Load.CreateConcentratedLoad(0, 0,
                                (item.P1_H * (i + 1D) + item.P2_H * (DistributedLoadDivision - i)) * MemberClone[j].Length / (DistributedLoadDivision + 1D) / (DistributedLoadDivision + 1D),
                                (item.P1_V * (i + 1D) + item.P2_V * (DistributedLoadDivision - i)) * MemberClone[j].Length / (DistributedLoadDivision + 1D) / (DistributedLoadDivision + 1D),
                                MemberClone[j].AngleInRad);
                        node0.Loads.Add(load0);

                        //節点を追加
                        Nodes.Add(node0);
                    }
                    //両端節点に荷重を追加
                    Load load1 = new Load();
                    if (item.Type == LoadType.DistributedLoad_GlobalGrid)
                        load1 = Load.CreateConcentratedLoad(
                            item.P1_H * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            item.P1_V * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D));
                    else if (item.Type == LoadType.DistributedLoad_MemberGrid)
                        load1 = Load.CreateConcentratedLoad(0, 0,
                            item.P1_H * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            item.P1_V * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            MemberClone[j].AngleInRad);
                    Nodes[edge0.Node].Loads.Add(load1);
                    if (item.Type == LoadType.DistributedLoad_GlobalGrid)
                        load1 = Load.CreateConcentratedLoad(
                            item.P2_H * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            item.P2_V * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D));
                    else if (item.Type == LoadType.DistributedLoad_MemberGrid)
                        load1 = Load.CreateConcentratedLoad(0, 0,
                            item.P2_H * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            item.P2_V * MemberClone[j].Length / 2D / (DistributedLoadDivision + 1D),
                            MemberClone[j].AngleInRad);
                    Nodes[edge1.Node].Loads.Add(load1);


                    // ★ 部材

                    for (int i = StartNode; i < StartNode + DistributedLoadDivision - 1; i++)
                    {
                        //中央に部材を追加
                        var member0 = new Member(section0, i, i + 1, false, false, Nodes);
                        member0.IsRealMember = false;
                        Members.Add(member0);
                    }

                    //1端の部材を追加
                    var member1 = new Member(section0, edge0.Node, StartNode + DistributedLoadDivision - 1, edge0.IsNodePin, false, Nodes);
                    member1.IsRealMember = false;
                    Members.Add(member1);

                    //2端の部材を追加
                    var member2 = new Member(section0, edge1.Node, StartNode, edge1.IsNodePin, false, Nodes);
                    member2.IsRealMember = false;
                    Members.Add(member2);

                    //元の部材を無力化
                    Members[j].DisableMatrixes();

                    // ★
                }
            }

            // ★★★★★ ピンかどうか書いておく
            foreach (var member in Members)
                for (int i = 0; i < 2; i++)
                    if (!member.Edges[i].IsNodePin)
                        Nodes[member.Edges[i].Node].IsRigid = true;
        }

        public override string ToString()
        {
            string AAA = "";
            AAA += "\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 荷重ベクトル\r\n";
            AAA += "\r\n*荷重ベクトル = \r\n" + LoadVector.ToString() + "\r\n";
            AAA += "\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 全体剛性マトリックス\r\n";
            AAA += "\r\n*全体剛性マトリックス = \r\n" + GlobalK.ToString() + "\r\n";
            AAA += "\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 解\r\n";
            AAA += "\r\n*解 = \r\n" + SolutionVector.ToString() + "\r\n";
            AAA += "\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ メトリクス\r\n";
            AAA += "\r\n体積 S = " + Volume.ToString() + "\r\n";
            AAA += "\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 終了\r\n";
            return AAA;
        }

        // ★★★★★ 解析メソッド群

        /// <summary>
        /// 全てを解きます。
        /// </summary>
        /// <returns>実行結果</returns>
        public bool CalculateAll()
        {
            return (
                CalculateFrame() &&
                CalculateMembers() &&
                CalculateMetrix());
        }

        /// <summary>
        /// 架構を解析します。
        /// </summary>
        /// <returns></returns>
        public bool CalculateFrame()
        {
            // ★★★★★ 初期化
            WarningMessage = "";

            // ★★★★★ 未知数番号をつける
            int n_unknown = 1; //未知数番号，最終的には未知数の総数を示す

            // ★ 節点に割り振り
            foreach (var item in Nodes)
            {
                if (!item.Fix.DeltaX)
                    item.Fix.n_DeltaX = n_unknown++;
                if (!item.Fix.DeltaY)
                    item.Fix.n_DeltaY = n_unknown++;
                if (!item.Fix.ThetaZ)
                    item.Fix.n_ThetaZ = n_unknown++;
            }

            // ★ 部材に割り振り
            foreach (var item in Members)
            {
                for (int i = 0; i < item.Edges.Length; i++)
                {
                    //節点の拘束情報を材端にコピー
                    item.Edges[i].Node_Fix = Nodes[item.Edges[i].Node].Fix;
                    //剛なら，部材拘束＝節点拘束のまま
                    if (!item.Edges[i].IsNodePin)
                        Nodes[item.Edges[i].Node].IsRigid = true;
                    //ピンなら，部材の回転拘束の未知数を0にする
                    if (item.Edges[i].IsNodePin)
                        item.Edges[i].Node_Fix.n_ThetaZ = 0;
                }
            }

            // ★★★★★ 荷重ベクトル生成
            LoadVector = new DenseVector(n_unknown - 1);

            // ★ 部材荷重を足し込む（※未実装）

            // ★ 節点荷重を足し込む
            foreach (var item1 in Nodes)
            {
                foreach (var item2 in item1.Loads)
                {
                    switch (item2.Type)
                    {
                        case LoadType.Moment:
                            if (item1.Fix.n_ThetaZ > 0)
                                LoadVector[item1.Fix.n_ThetaZ - 1] += item2.M;
                            break;
                        case LoadType.ConcentratedLoad:
                            if (item1.Fix.n_DeltaY > 0)
                                LoadVector[item1.Fix.n_DeltaY - 1] += item2.P1_V;
                            if (item1.Fix.n_DeltaX > 0)
                                LoadVector[item1.Fix.n_DeltaX - 1] += item2.P1_H;
                            break;
                        default:
                            break;
                    }
                }
            }

            // ★★★★★ 全体剛性マトリックス作成
            GlobalK = new DenseMatrix(n_unknown - 1);

            Parallel.ForEach(Members, item =>
            {
                var unknowns = new int[]
                {
                    item.Edges[0].Node_Fix.n_DeltaX,
                    item.Edges[0].Node_Fix.n_DeltaY,
                    item.Edges[0].Node_Fix.n_ThetaZ,
                    item.Edges[1].Node_Fix.n_DeltaX,
                    item.Edges[1].Node_Fix.n_DeltaY,
                    item.Edges[1].Node_Fix.n_ThetaZ,
                };

                lock (GlobalK)
                    for (int i = 0; i < 6; i++)
                        for (int j = 0; j < 6; j++)
                            if (unknowns[i] != 0 && unknowns[j] != 0)
                                GlobalK[unknowns[i] - 1, unknowns[j] - 1] += item.M_K[i, j];
            });

            for (int i = 0; i < GlobalK.ColumnCount; i++)
                if (GlobalK[i, i] == 0)
                    GlobalK[i, i] = 1;

            // ★★★★★ 解く
            if (this.GlobalK.Determinant() == 0)
                return false;
            SolutionVector = (DenseVector)this.GlobalK.Solve(this.LoadVector);

            // ★★★★★ 未知数より，節点変位を算出
            foreach (var item in Nodes)
            {
                double δX = 0, δY = 0, θZ = 0;

                if (item.Fix.n_DeltaX != 0)
                    δX = SolutionVector[item.Fix.n_DeltaX - 1];
                if (item.Fix.n_DeltaY != 0)
                    δY = SolutionVector[item.Fix.n_DeltaY - 1];
                if (item.Fix.n_ThetaZ != 0)
                    θZ = SolutionVector[item.Fix.n_ThetaZ - 1];

                item.Displacement = new Displacement(δX, δY, θZ);
            }

            // ★★★★★ 材端変位と材端応力を算出
            Parallel.ForEach(Members, item =>
            {
                #region 材端変位（全体系）
                lock (SolutionVector)
                {
                    for (int i = 0; i < item.Edges.Length; i++)
                    {
                        double δX = 0, δY = 0, θZ = 0;
                        if (item.Edges[i].Node_Fix.n_DeltaX != 0)
                            δX = SolutionVector[item.Edges[i].Node_Fix.n_DeltaX - 1];
                        if (item.Edges[i].Node_Fix.n_DeltaY != 0)
                            δY = SolutionVector[item.Edges[i].Node_Fix.n_DeltaY - 1];
                        if (item.Edges[i].Node_Fix.n_ThetaZ != 0)
                            θZ = SolutionVector[item.Edges[i].Node_Fix.n_ThetaZ - 1];
                        item.Edges[i].Displacement = new Displacement(δX, δY, θZ);
                    }
                }
                #endregion

                #region 材端変位（部材系）
                DenseVector Displace = item.M_Converse * DenseVector.OfArray(new double[]
                    {
                        item.Edges[0].Displacement.DeltaX,
                        item.Edges[0].Displacement.DeltaY,
                        item.Edges[0].Displacement.ThetaZ,
                        item.Edges[1].Displacement.DeltaX,
                        item.Edges[1].Displacement.DeltaY,
                        item.Edges[1].Displacement.ThetaZ
                    });

                item.Edges[0].Displacement.DeltaX = Displace[0];
                item.Edges[0].Displacement.DeltaY = Displace[1];
                item.Edges[0].Displacement.ThetaZ = Displace[2];
                item.Edges[1].Displacement.DeltaX = Displace[3];
                item.Edges[1].Displacement.DeltaY = Displace[4];
                item.Edges[1].Displacement.ThetaZ = Displace[5];
                #endregion

                #region 材端応力（部材系）
                var Stress = item.M_k * Displace;
                item.Stress.N = -Stress[0];
                item.Stress.Q = Stress[1];
                item.Stress.M1 = Stress[2];
                item.Stress.M2 = Stress[5];
                item.Stress.UpdateStress(item.Section.A, item.Section.Z);
                #endregion
            });

            // ★★★★★ 正常終了
            return true;
        }

        /// <summary>
        /// 全部材を解析します。
        /// </summary>
        /// <returns>実行結果</returns>
        public bool CalculateMembers()
        {
            try
            {
                foreach (var item in Members)
                    item.Design(W, F);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// デザコンに必要なデータを解析します。
        /// </summary>
        /// <returns>実行結果</returns>
        public bool CalculateMetrix()
        {
            try
            {
                // ★★★★★ ここで体積など計算できる。
                Volume = 0;
                foreach (var item in Members)
                {
                    Volume += item.Update_Volume();
                    var isForTension = item.Section.I <= 0.001;
                    double max = item.Stress.Update_Check_Max(isForTension);

                    if (isForTension && max == 0)
                        WarningMessage += "<" + (Members.IndexOf(item) + 1) + ">は圧縮を受ける引張り材です。正確な結果を得るには手動で取り除いて再計算してください。\r\n";

                }

            }
            catch (Exception)
            {
                return false;
            }

            // ★★★★★ 正常終了
            return true;
        }

        // ★★★★★ 作図メソッド群

        /// <summary>
        /// 描写範囲(70%,70%)の値と
        /// 荷重の最大値を算出します。
        /// </summary>
        private void CalculateRange()
        {
            //描写範囲
            double Xmin = 0, Xmax = 0, Ymin = 0, Ymax = 0;
            foreach (var item in Nodes)
            {
                Xmin = Math.Min(Xmin, item.Point.X);
                Xmax = Math.Max(Xmax, item.Point.X);
                Ymin = Math.Min(Ymin, item.Point.Y);
                Ymax = Math.Max(Ymax, item.Point.Y);
            }

            if (Xmax - Xmin == 0)
            {
                WidthRange = Correction * Correction;
                WidthMin = 0;
            }
            else
            {
                WidthRange = (long)(Correction * (Xmax - Xmin));
                WidthMin = (long)(Correction * Xmin);
            }

            if (Ymax - Ymin == 0)
            {
                HeightRange = Correction * Correction;
                HeightMin = 0;
            }
            else
            {
                HeightRange = (long)(Correction * (Ymax - Ymin));
                HeightMin = (long)(Correction * Ymin);
            }

            ZoomCoefficient = 3333 / (float)Math.Max(WidthRange, HeightRange);
        }

        /// <summary>
        /// 描画キャンバスを生成します。
        /// </summary>
        /// <returns></returns>
        public Image MakeCanvas()
        {
            Bitmap img = new Bitmap(1, 1);
            CalculateRange();
            try
            {
                img = new Bitmap(
                    (int)Math.Max(1, ((Base_ALL + 2) * WidthRange * ZoomCoefficient / Base_ALL)),
                    (int)Math.Max(1, ((Base_ALL + 2) * HeightRange * ZoomCoefficient / Base_ALL)),
                    PixelFormat.Format16bppRgb555);
            }
            catch (Exception)
            {
                throw new OutOfMemoryException("メモリが不足しました。再度試してみてください。");
            }

            using (var g = Graphics.FromImage(img))
            {
                g.Clear(Color.Gray);
            }
            return img;
        }

        /// <summary>
        /// 架構のイメージを生成します。
        /// </summary>
        /// <returns>架構のイメージ</returns>
        public Image Draw_Frame()
        {
            return Draw_Frame(MakeCanvas());
        }

        /// <summary>
        /// 架構のイメージを上描きします。
        /// </summary>
        /// <param name="img">描画するキャンバス</param>
        /// <returns>架構のイメージ</returns>
        public Image Draw_Frame(Image img)
        {
            //重要語句の宣言！！

            int R = (int)(Math.Max(Base_r / ZoomCoefficient, 1));
            int Thickness = (int)(Math.Max(Base_Thickness / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));
            int Fulcrum = (int)(Math.Max(Base_Fulcrum / ZoomCoefficient, 1));
            int Load = (int)(Math.Max(Base_Load / ZoomCoefficient, 1));

            //描画キャンバスとペンを用意
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);
            Pen p = new Pen(Color.Black, Thickness);

            #region 支点の描写

            for (int i = 0; i < Nodes.Count; i++)
            {
                bool x = Nodes[i].Fix.DeltaX;
                bool y = Nodes[i].Fix.DeltaY;
                bool θ = Nodes[i].Fix.ThetaZ;

                if (!x && !y && !θ)
                { }
                else if (x && y && θ)
                    Draw_FixedFulcrum(g, new Point((int)(Nodes[i].Point.X), (int)(Nodes[i].Point.Y)), p, Fulcrum);
                else if (x && y && !θ)
                    Draw_PinFulcrum(g, new Point((int)(Nodes[i].Point.X), (int)(Nodes[i].Point.Y)), Brushes.DimGray, p, Fulcrum, 0);
                else if (x && !y && !θ)
                    Draw_RollerFulcrum(g, new Point((int)(Nodes[i].Point.X), (int)(Nodes[i].Point.Y)), Brushes.DimGray, p, Fulcrum, 90);
                else if (!x && y && !θ)
                    Draw_RollerFulcrum(g, new Point((int)(Nodes[i].Point.X), (int)(Nodes[i].Point.Y)), Brushes.DimGray, p, Fulcrum, 0);
                else
                    Draw_UnknownFulcrum(g, new Point((int)(Nodes[i].Point.X), (int)(Nodes[i].Point.Y)), p, Fulcrum);
            }

            #endregion

            #region 節点の描写

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!Nodes[i].IsPseudNode)
                {
                    //節点自体の描写
                    var point = WorldTranslate(Nodes[i].Point.X, Nodes[i].Point.Y, 0, 0, 0);
                    if (Nodes[i].IsRigid)
                        g.FillEllipse(Brushes.Black, point.X - R, point.Y - R, 2 * R, 2 * R);
                    else
                    {
                        g.FillEllipse(Brushes.Gray, point.X - R, point.Y - R, 2 * R, 2 * R);
                        g.DrawEllipse(p, point.X - R, point.Y - R, 2 * R, 2 * R);
                    }

                    //節点番号の描写
                    if (DrawNodesNum)
                        g.DrawString((i + 1).ToString(), new Font("MS UI Gothic", Letter), Brushes.Black, point.X - 3 * R, point.Y + 1 * R);
                }
            }

            #endregion

            #region 部材の描写
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].IsRealMember)
                {
                    //部材自体の描写
                    double X = Nodes[Members[i].Edges[0].Node].Point.X;
                    double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                    Point[] points = new Point[3];
                    points[0] = WorldTranslate(X, Y, R, 0, Members[i].AngleInRad);
                    points[1] = WorldTranslate(X, Y, Correction * Members[i].Length - R, 0, Members[i].AngleInRad);
                    points[2] = WorldTranslate(X, Y, Correction * Members[i].Length / 3, 0, Members[i].AngleInRad);
                    g.DrawLine(p, points[0], points[1]);

                    //端ピンの描写
                    for (int j = 0; j < 2; j++)
                    {
                        if (Members[i].Edges[j].IsNodePin && Nodes[Members[i].Edges[j].Node].IsRigid)
                        {
                            g.FillEllipse(Brushes.Gray, points[j].X - R / 2, points[j].Y - R / 2, R, R);
                            g.DrawEllipse(p, points[j].X - R / 2, points[j].Y - R / 2, R, R);
                        }
                    }

                    //部材番号の描写
                    if (DrawMembersNum)
                        g.DrawString("<" + (i + 1) + ">", new Font("MS UI Gothic", Letter), Brushes.Black, points[2].X, points[2].Y);
                }
            }

            #endregion

            #region 荷重の描写

            //節点に働く力

            var Ya = new Pen(Color.DarkBlue, Thickness);
            Ya.EndCap = LineCap.ArrowAnchor;
            Ya.CustomEndCap = new AdjustableArrowCap(Load * ZoomCoefficient / 73, Load * ZoomCoefficient / 70);

            foreach (var item in Nodes)
            {

                foreach (var item2 in item.Loads)
                {
                    if (item2.Type == LoadType.Moment)
                    {
                        var point = WorldTranslate(item.Point.X, item.Point.Y, 0, 0, 0);
                        if (item2.M >= 0)
                            g.DrawArc(Ya, point.X - Load / 2, point.Y - Load / 2, Load, Load, 45, -270);
                        else
                            g.DrawArc(Ya, point.X - Load / 2, point.Y - Load / 2, Load, Load, 135, 270);
                        if (DrawLoad && !item.IsPseudNode)
                            g.DrawString("M=" + item2.M.ToString("G4"), new Font("MS UI Gothic", 2 * R), Brushes.DarkBlue, point.X - Load, point.Y - Load);
                    }
                    if (item2.Type == LoadType.ConcentratedLoad)
                    {
                        var MaxLoad_P = Math.Max(Math.Abs(item2.P1_H), Math.Abs(item2.P1_V));
                        if (MaxLoad_P != 0)
                        {
                            var point1 = WorldTranslate(item.Point.X, item.Point.Y, 0, 0, 0);
                            var point2 = WorldTranslate(item.Point.X, item.Point.Y, -Load * item2.P1_H / MaxLoad_P, Load * item2.P1_V / MaxLoad_P, 0);
                            g.DrawLine(Ya, point2, point1);
                        }
                        var point3 = WorldTranslate(item.Point.X, item.Point.Y, R, R, 0);
                        if (DrawLoad && !item.IsPseudNode)
                            g.DrawString("P=(" + item2.P1_H.ToString("G4") + "," + item2.P1_V.ToString("G4") + ")", new Font("MS UI Gothic", 2 * R), Brushes.DarkBlue, point3);
                    }
                }
            }

            //部材に働く力
            foreach (var member in Members)
            {
                foreach (var load in member.Loads)
                {
                    if (DrawLoad && (load.Type == LoadType.DistributedLoad_MemberGrid || load.Type == LoadType.DistributedLoad_GlobalGrid))
                    {
                        var point1 = WorldTranslate(Nodes[member.Edges[0].Node].Point.X, Nodes[member.Edges[0].Node].Point.Y, R, 3 * R, 0);
                        g.DrawString("W=(" + load.P1_H.ToString("G4") + "," + load.P1_V.ToString("G4") + ")", new Font("MS UI Gothic", 2 * R), Brushes.DarkBlue, point1);
                        var point2 = WorldTranslate(Nodes[member.Edges[1].Node].Point.X, Nodes[member.Edges[1].Node].Point.Y, R, 3 * R, 0);
                        g.DrawString("W=(" + load.P2_H.ToString("G4") + "," + load.P2_V.ToString("G4") + ")", new Font("MS UI Gothic", 2 * R), Brushes.DarkBlue, point2);
                    }
                }
            }
            #endregion

            //メモリ解放，画像を格納
            g.Dispose();
            return img;
        }

        /// <summary>
        /// Ｎ図を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_N()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double N = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;
            var pb = new Pen(Color.Blue, Thickness);


            double Max_N = 00000000000.1;
            foreach (var item in Members)
                Max_N = Math.Max(Math.Abs(item.Stress.N), Max_N);

            for (int i = 0; i < Members.Count; i++)
            {
                //N図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, -Members[i].Stress.N * N / Max_N, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.N * N / Max_N, Members[i].AngleInRad);
                if (Members[i].Stress.N < 0)
                    g.DrawPolygon(p, points);
                else
                    g.DrawPolygon(pb, points);

                //Nの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[0] = WorldTranslate(X, Y, Correction * Members[i].Length / 2, 0, Members[i].AngleInRad);
                    g.DrawString(Members[i].Stress.N.ToString("G4"), new Font("MS UI Gothic", Letter), b, points[0].X - 3 * Letter / 2, points[0].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// Q図を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_Q()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double Q = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;
            var pb = new Pen(Color.Blue, Thickness);


            double Max_Q = 00000000000.1;
            foreach (var item in Members)
                Max_Q = Math.Max(Math.Abs(item.Stress.Q), Max_Q);

            for (int i = 0; i < Members.Count; i++)
            {
                //Q図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, -Members[i].Stress.Q * Q / Max_Q, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.Q * Q / Max_Q, Members[i].AngleInRad);
                //g.FillPolygon(b, points);
                if (Members[i].Stress.Q < 0)
                    g.DrawPolygon(p, points);
                else
                    g.DrawPolygon(pb, points);


                //Qの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[0] = WorldTranslate(X, Y, Correction * Members[i].Length / 2, 0, Members[i].AngleInRad);
                    g.DrawString(Members[i].Stress.Q.ToString("G4"), new Font("MS UI Gothic", Letter), b, points[0].X - 3 * Letter / 2, points[0].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// Ｍ図を出力成します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_M()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double M = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;


            double Max_M = 00000000000000.1;
            foreach (var item in Members)
                Max_M = Math.Max(Math.Max(Math.Abs(item.Stress.M1), Math.Abs(item.Stress.M2)), Max_M);

            for (int i = 0; i < Members.Count; i++)
            {
                //M図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, Members[i].Stress.M1 * M / Max_M, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.M2 * M / Max_M, Members[i].AngleInRad);
                g.DrawPolygon(p, points);

                //Mの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[1] = WorldTranslate(X, Y, Correction * Members[i].Length * 1 / 7, 0, Members[i].AngleInRad);
                    points[2] = WorldTranslate(X, Y, Correction * Members[i].Length * 6 / 7, 0, Members[i].AngleInRad);
                    g.DrawString((Math.Abs(Members[i].Stress.M1)).ToString("G4"), new Font("MS UI Gothic", Letter), b, points[1].X - 3 * Letter / 2, points[1].Y - Letter / 2);
                    g.DrawString((Math.Abs(Members[i].Stress.M2)).ToString("G4"), new Font("MS UI Gothic", Letter), b, points[2].X - 3 * Letter / 2, points[2].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// σN図を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_σN()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double σN = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;
            var pb = new Pen(Color.Blue, Thickness);


            double Max_σN = 00000000000.1;
            foreach (var item in Members)
                Max_σN = Math.Max(Math.Abs(item.Stress.σN), Max_σN);

            for (int i = 0; i < Members.Count; i++)
            {
                //σN図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, -Members[i].Stress.σN * σN / Max_σN, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.σN * σN / Max_σN, Members[i].AngleInRad);
                //g.FillPolygon(b, points);
                if (Members[i].Stress.σN < 0)
                    g.DrawPolygon(p, points);
                else
                    g.DrawPolygon(pb, points);

                //σNの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[0] = WorldTranslate(X, Y, Correction * Members[i].Length / 2, 0, Members[i].AngleInRad);
                    g.DrawString(Members[i].Stress.σN.ToString("G4"), new Font("MS UI Gothic", Letter), b, points[0].X - 3 * Letter / 2, points[0].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// τ図を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_τ()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double τ = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;
            var pb = new Pen(Color.Blue, Thickness);


            double Max_τ = 00000000000.1;
            foreach (var item in Members)
                Max_τ = Math.Max(Math.Abs(item.Stress.τ), Max_τ);

            for (int i = 0; i < Members.Count; i++)
            {
                //τ図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, -Members[i].Stress.τ * τ / Max_τ, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.τ * τ / Max_τ, Members[i].AngleInRad);
                //g.FillPolygon(b, points);
                if (Members[i].Stress.τ < 0)
                    g.DrawPolygon(p, points);
                else
                    g.DrawPolygon(pb, points);

                //τの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[0] = WorldTranslate(X, Y, Correction * Members[i].Length / 2, 0, Members[i].AngleInRad);
                    g.DrawString(Members[i].Stress.τ.ToString("G4"), new Font("MS UI Gothic", Letter), b, points[0].X - 3 * Letter / 2, points[0].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// σb図を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_σb()
        {
            //キャンバスの用意
            var img = MakeCanvas();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double σb = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            var p = new Pen(Color.Red, Thickness);
            var b = Brushes.Red;


            double Max_σb = 00000000000.1;
            foreach (var item in Members)
                Max_σb = Math.Max(Math.Max(Math.Abs(item.Stress.σb1), Math.Abs(item.Stress.σb2)), Max_σb);

            for (int i = 0; i < Members.Count; i++)
            {
                //σb図自体の描写
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;
                Point[] points = new Point[4];
                points[0] = WorldTranslate(X, Y, 0, Members[i].Stress.σb1 * σb / Max_σb, Members[i].AngleInRad);
                points[1] = WorldTranslate(X, Y, 0, 0, Members[i].AngleInRad);
                points[2] = WorldTranslate(X, Y, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                points[3] = WorldTranslate(X, Y, Correction * Members[i].Length, -Members[i].Stress.σb2 * σb / Max_σb, Members[i].AngleInRad);
                g.DrawPolygon(p, points);

                //σbの大きさの描写
                if (DrawDiagramNum && Members[i].IsRealMember)
                {
                    points[1] = WorldTranslate(X, Y, Correction * Members[i].Length * 1 / 7, 0, Members[i].AngleInRad);
                    points[2] = WorldTranslate(X, Y, Correction * Members[i].Length * 6 / 7, 0, Members[i].AngleInRad);
                    g.DrawString((Math.Abs(Members[i].Stress.σb1)).ToString("G4"), new Font("MS UI Gothic", Letter), b, points[1].X - 3 * Letter / 2, points[1].Y - Letter / 2);
                    g.DrawString((Math.Abs(Members[i].Stress.σb2)).ToString("G4"), new Font("MS UI Gothic", Letter), b, points[2].X - 3 * Letter / 2, points[2].Y - Letter / 2);
                }
            }

            g.Dispose();
            return Draw_Frame(img);
        }

        /// <summary>
        /// δ図(簡易)を出力します。
        /// </summary>
        /// <returns>架構のイメージ</returns>
        public Image Draw_δ()
        {
            //重要語句の宣言！！
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //描画キャンバスとペンを用意
            var img = Draw_Frame();
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);
            Pen p = new Pen(Color.Red, Thickness);
            p.DashStyle = DashStyle.Dash;

            //変形後の架構
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].Loads.Count == 0) //等変分布荷重を持っていたら描く必要なし
                {
                    //移動部材の描写
                    double fromX = Nodes[Members[i].Edges[0].Node].Point.X + Nodes[Members[i].Edges[0].Node].Displacement.DeltaX;
                    double fromY = Nodes[Members[i].Edges[0].Node].Point.Y + Nodes[Members[i].Edges[0].Node].Displacement.DeltaY;
                    double toX = Nodes[Members[i].Edges[0].Node].Point.X + Nodes[Members[i].Edges[1].Node].Displacement.DeltaX;
                    double toY = Nodes[Members[i].Edges[0].Node].Point.Y + Nodes[Members[i].Edges[1].Node].Displacement.DeltaY;
                    Point fromPoint = WorldTranslate(fromX, fromY, 0, 0, Members[i].AngleInRad);
                    Point toPoint = WorldTranslate(toX, toY, Correction * Members[i].Length, 0, Members[i].AngleInRad);
                    g.DrawLine(p, fromPoint, toPoint);
                }
            }

            g.Dispose();
            return img;
        }

        /// <summary>
        /// 危険率を出力します。
        /// </summary>
        /// <returns></returns>
        public Image Draw_Check_Max()
        {
            //キャンバスの用意
            var img = Draw_Frame(MakeCanvas());
            var g = Graphics.FromImage(img);
            g.ScaleTransform(ZoomCoefficient, ZoomCoefficient);

            //重要語句の宣言！！
            double N = Math.Max(Base_M / 2 / ZoomCoefficient, 1);
            double R = Math.Max(Base_r / ZoomCoefficient, 1);
            int Thickness = (int)(Math.Max(Base_Thickness / 2 / ZoomCoefficient, 1));
            int Letter = (int)(Math.Max(Base_Letter / ZoomCoefficient, 1));

            //ペン・ブラシの用意
            Brush b;
            for (int i = 0; i < Members.Count; i++)
            {
                double X = Nodes[Members[i].Edges[0].Node].Point.X;
                double Y = Nodes[Members[i].Edges[0].Node].Point.Y;

                //危険度の書き込み
                if (Members[i].IsRealMember)
                {
                    var point = WorldTranslate(X, Y, Correction * Members[i].Length / 2, 0, Members[i].AngleInRad);
                    var max = Members[i].Stress.Check_Max;
                    var aaa = "";

                    if (max == 0)
                    { aaa = ""; b = new SolidBrush(Color.FromArgb(200, Color.Empty)); }
                    else if (max == Members[i].Stress.Check_C)
                    { aaa = "C-"; b = new SolidBrush(Color.FromArgb(200, Color.Red)); }
                    else if (max == Members[i].Stress.Check_T)
                    { aaa = "T-"; b = new SolidBrush(Color.FromArgb(200, Color.Blue)); }
                    else if (max == Members[i].Stress.Check_Q)
                    { aaa = "S-"; b = new SolidBrush(Color.FromArgb(200, Color.Green)); }
                    else
                    { aaa = "?-"; b = new SolidBrush(Color.FromArgb(128, Color.Black)); }

                    g.DrawString(aaa + max.ToString("G3"), new Font("MS UI Gothic", Letter), b, point.X - 3 * Letter / 2, point.Y - Letter / 2);
                }
            }

            g.Dispose();
            return img;
        }

        /// <summary>
        /// 構造物の座標をイメージの座標に変換します
        /// </summary>
        /// <param name="Parent">構造全体の基準点から見た部材の基準点の位置</param>
        /// <param name="Child">部材の基準点からの移動</param>
        /// <param name="Φ">部材の傾き[Rad]</param>
        /// <returns>イメージの座標</returns>
        private Point WorldTranslate(Point Parent, Point Child, double Φ) //世界座標変換
        {
            return WorldTranslate(Parent.X, Parent.Y, Child.X, Child.Y, Φ);
        }

        /// <summary>
        /// 構造物の座標をイメージの座標に変換します
        /// </summary>
        /// <param name="Parent_X">構造全体の基準点から見た部材の基準点の位置_X</param>
        /// <param name="Parent_Y">構造全体の基準点から見た部材の基準点の位置_Y</param>
        /// <param name="Child_X">部材の基準点からの移動_X</param>
        /// <param name="Child_Y">部材の基準点からの移動_Y</param>
        /// <param name="Φ">部材の傾き[Rad]</param>
        /// <returns>イメージの座標</returns>
        private Point WorldTranslate(double Parent_X, double Parent_Y, double Child_X, double Child_Y, double Φ) //世界座標変換
        {
            double X, Y;
            X = Child_X * Math.Cos(Φ) + Child_Y * Math.Sin(Φ);
            Y = -Child_X * Math.Sin(Φ) + Child_Y * Math.Cos(Φ);
            X += WidthRange / Base_ALL + Correction * Parent_X - WidthMin;
            Y += (Base_ALL + 1) * HeightRange / Base_ALL - Correction * Parent_Y + HeightMin;
            return new Point((int)X, (int)Y);
        }

        /// <summary>
        /// 指定した点に固定支点を描きます。
        /// </summary>
        /// <param name="g">描画キャンパス</param>
        /// <param name="point">描く対象の点</param>
        /// <param name="p">ペン</param>
        /// <param name="Fulcrum">支点のサイズ</param>
        private void Draw_FixedFulcrum(Graphics g, Point point, Pen p, int Fulcrum) //固定支点を描く
        {
            var point1 = WorldTranslate(point, new Point(-3 * Fulcrum, 0), 0);
            var point2 = WorldTranslate(point, new Point(3 * Fulcrum, 0), 0);
            g.DrawLine(p, point1, point2);

            for (int i = 0; i < 6; i++)
            {
                var point3 = WorldTranslate(point, new Point((i - 3) * Fulcrum, 2 * Fulcrum), 0);
                var point4 = WorldTranslate(point, new Point((i - 2) * Fulcrum, 0), 0);
                g.DrawLine(p, point3, point4);
            }
        }

        /// <summary>
        /// 指定した点にローラー支点を描きます。
        /// </summary>
        /// <param name="g">描画キャンパス</param>
        /// <param name="point">描く対象の点</param>
        /// <param name="b">ブラシ</param>
        /// <param name="p">ペン</param>
        /// <param name="Fulcrum">支点のサイズ</param>
        /// <param name="θ">ローラー移動の方向[Deg]</param>
        private void Draw_RollerFulcrum(Graphics g, Point point, Brush b, Pen p, int Fulcrum, int θ) //ローラー支点を描く
        {
            double ψ = θ * Math.PI / 180;

            Point[] ps = new Point[]
                {
                    WorldTranslate(point, new Point(0,0),ψ ),
                    WorldTranslate(point, new Point(-2 * Fulcrum,2 * Fulcrum),ψ),
                    WorldTranslate(point, new Point(2 * Fulcrum,2 * Fulcrum), ψ)
                };
            g.FillPolygon(b, ps);
            g.DrawPolygon(p, ps);

            for (int i = 0; i < 3; i++)
            {
                var point1 = WorldTranslate(point, new Point((int)((1.5 * i - 1.5) * Fulcrum), (int)(2.5 * Fulcrum)), ψ);
                g.FillEllipse(b, point1.X - Fulcrum / 2, point1.Y - Fulcrum / 2, Fulcrum, Fulcrum);
                g.DrawEllipse(p, point1.X - Fulcrum / 2, point1.Y - Fulcrum / 2, Fulcrum, Fulcrum);
            }
        }

        /// <summary>
        /// 指定した点にピン支点を描きます。
        /// </summary>
        /// <param name="g">描画キャンパス</param>
        /// <param name="b">ブラシ</param>
        /// <param name="p">ペン</param>
        /// <param name="point">描く対象の点</param>
        /// <param name="Fulcrum">支点のサイズ</param>
        /// <param name="θ">ピン支点の向き[Deg]</param>
        private void Draw_PinFulcrum(Graphics g, Point point, Brush b, Pen p, int Fulcrum, int θ) //ピン支点を描く
        {
            Point[] ps = new Point[]
                {
                    WorldTranslate(point, new Point(0,0), θ*Math.PI/180),
                    WorldTranslate(point, new Point(-2 * Fulcrum,3 * Fulcrum),θ*Math.PI/180),
                    WorldTranslate(point, new Point(2 * Fulcrum,3 * Fulcrum), θ*Math.PI/180)
                };
            g.FillPolygon(b, ps);
            g.DrawPolygon(p, ps);
        }

        /// <summary>
        /// 指定した点に不明な支点を描きます。
        /// </summary>
        /// <param name="g">描画キャンパス</param>
        /// <param name="point">描く対象の点</param>
        /// <param name="p">ペン</param>
        /// <param name="Fulcrum">支点のサイズ</param>
        private void Draw_UnknownFulcrum(Graphics g, Point point, Pen p, int Fulcrum) //不明な支点を描く
        {
            var point1 = WorldTranslate(point, new Point(-3 * Fulcrum, 3 * Fulcrum), 0);
            var point2 = WorldTranslate(point, new Point(3 * Fulcrum, -3 * Fulcrum), 0);
            g.DrawLine(p, point1, point2);

            var point3 = WorldTranslate(point, new Point(3 * Fulcrum, 3 * Fulcrum), 0);
            var point4 = WorldTranslate(point, new Point(-3 * Fulcrum, -3 * Fulcrum), 0);
            g.DrawLine(p, point3, point4);
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
