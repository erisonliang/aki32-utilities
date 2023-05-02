

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    /// <summary>
    /// 梁の微小厚さ断面
    /// </summary>
    class MemberSection
    {
        /// <summary>
        /// 梁の微小要素の集合体
        /// </summary>
        public MemberPiece[] p { get; set; }

        /// <summary>
        /// 曲率
        /// </summary>
        public double Phi { get; set; }
        /// <summary>
        /// 変形（ L 方向）
        /// </summary>
        public double DelL { get; set; }
        /// <summary>
        /// 変形（ H 方向）
        /// </summary>
        public double DelH { get; set; }
        /// <summary>
        /// 回転角
        /// </summary>
        public double dDelH { get; set; }
        /// <summary>
        /// 回転角の微分
        /// </summary>
        public double ddDelH { get; set; }
        /// <summary>
        /// モーメント
        /// </summary>
        public double M { get; set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="member_piece_num"></param>
        public MemberSection(int member_piece_num)
        {
            p = Enumerable.Range(0, member_piece_num).Select(_ => new MemberPiece()).ToArray();
        }

    }
}
