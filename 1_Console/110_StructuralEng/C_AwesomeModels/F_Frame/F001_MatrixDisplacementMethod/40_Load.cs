

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 荷重
    /// </summary>
    public struct Load
    {

        // ★★★★★★★★★★★★★★★ props

        public LoadType Type { get; set; } //荷重の種類

        public double M { get; set; } //モーメント荷重の大きさ

        public double P1_H { get; set; } //集中荷重は"P1_"のほうのみ使う。
        public double P2_H { get; set; } //分布荷重は４つとも使う。
        public double P1_V { get; set; }
        public double P2_V { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 全体系集中荷重を生成します。
        /// 上，右が正です。
        /// </summary>
        /// <param name="P_H">水平方向の荷重</param>
        /// <param name="P_V">鉛直方向の荷重</param>
        public static Load CreateConcentratedLoad(double P_H, double P_V)
        {
            return new Load()
            {
                Type = LoadType.ConcentratedLoad,
                M = 0,
                P1_H = P_H,
                P1_V = P_V,
                P2_H = 0,
                P2_V = 0,
            };
        }

        /// <summary>
        /// 荷重種類＝P で，全体系集中荷重を生成します。
        /// 上，右が正です。
        /// ※回転の単位に注意！
        /// </summary>
        /// <param name="P_Φ">載荷方向[Deg]</param>
        /// <param name="P_r">載荷荷重</param>
        /// <param name="P_H">水平方向の荷重</param>
        /// <param name="P_V">鉛直方向の荷重</param>
        /// <param name="Φ">更に回転[Rad]</param>
        public static Load CreateConcentratedLoad(double P_Φ, double P_r, double P_H, double P_V, double Φ)
        {
            double Ph = P_r * Math.Cos(P_Φ * Math.PI / 180) + P_H;
            double Pv = P_r * Math.Sin(P_Φ * Math.PI / 180) + P_V;

            return new Load()
            {
                Type = LoadType.ConcentratedLoad,
                M = 0,
                P1_H = -Math.Sin(Φ) * Pv + Math.Cos(Φ) * Ph,
                P1_V = +Math.Cos(Φ) * Pv + Math.Sin(Φ) * Ph,
                P2_H = 0,
                P2_V = 0,
            };
        }

        /// <summary>
        /// 荷重種類＝M で，モーメント荷重を生成します。
        /// 時計回りが正です。
        /// </summary>
        /// <param name="M">曲げ荷重の大きさ</param>
        public static Load CreateMoment(double M)
        {
            return new Load()
            {
                Type = LoadType.Moment,
                M = M,
                P1_H = 0,
                P1_V = 0,
                P2_H = 0,
                P2_V = 0,
            };
        }

        /// <summary>
        /// 等変分布を生成します。
        /// </summary>
        /// <param name="isBasedOnGlobalGrid">基準座標が全体座標か部材座標かを指定</param>
        /// <param name="W1_H">第１節点での水平荷重の大きさ</param>
        /// <param name="W1_V">第１節点での鉛直荷重の大きさ</param>
        /// <param name="W2_H">第２節点での水平荷重の大きさ</param>
        /// <param name="W2_V">第２節点での鉛直荷重の大きさ</param>
        public static Load CreateDistributedLoad(bool isBasedOnGlobalGrid, double W1_H, double W1_V, double W2_H, double W2_V)
        {
            var loadType = isBasedOnGlobalGrid
                ? LoadType.DistributedLoad_GlobalGrid
                : LoadType.DistributedLoad_MemberGrid
                ;

            return new Load()
            {
                Type = loadType,
                M = 0,
                P1_H = W1_H,
                P1_V = W1_V,
                P2_H = W2_H,
                P2_V = W2_V,
            };
        }

        /// <summary>
        /// 等変分布を生成します。
        /// 部材系は，材端を原点と第４・１象限においたとき，'反'時計回りが正です。
        /// </summary>
        /// <param name="isBasedOnGlobalGrid">基準座標が全体座標か部材座標かを指定</param>
        /// <param name="W1_H">第１節点での水平荷重の大きさ</param>
        /// <param name="W1_V">第１節点での鉛直荷重の大きさ</param>
        /// <param name="W2_H">第２節点での水平荷重の大きさ</param>
        /// <param name="W2_V">第２節点での鉛直荷重の大きさ</param>
        public static Load CreateDistributedLoad(bool isBasedOnGlobalGrid, double W1_Φ, double W1_r, double W1_H, double W1_V, double W2_Φ, double W2_r, double W2_H, double W2_V)
        {
            var loadType = isBasedOnGlobalGrid
                ? LoadType.DistributedLoad_GlobalGrid
                : LoadType.DistributedLoad_MemberGrid
                ;

            return new Load()
            {
                Type = loadType,
                M = 0,
                P1_H = W1_r * Math.Cos(W1_Φ * Math.PI / 180) + W1_H,
                P1_V = W1_r * Math.Sin(W1_Φ * Math.PI / 180) + W1_V,
                P2_H = W2_r * Math.Cos(W2_Φ * Math.PI / 180) + W2_H,
                P2_V = W2_r * Math.Sin(W2_Φ * Math.PI / 180) + W2_V,
            };
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// 部材系分布荷重を全体系分布荷重に変換します。
        /// ※部材系分布荷重のみに使用してください。
        /// </summary>
        /// <param name="angle">部材系の傾き</param>
        /// <returns></returns>
        public Load GetTransformedToGlobalGrid(double angle)
        {
            switch (Type)
            {
                case LoadType.DistributedLoad_MemberGrid:
                    {
                        double W1_H = P1_H * Math.Cos(angle) - P1_V * Math.Sin(angle);
                        double W1_V = P1_H * Math.Sin(angle) + P1_V * Math.Cos(angle);
                        double W2_H = P2_H * Math.Cos(angle) - P2_V * Math.Sin(angle);
                        double W2_V = P2_H * Math.Sin(angle) + P2_V * Math.Cos(angle);
                        return CreateDistributedLoad(true, W1_H, W1_V, W2_H, W2_V);
                    }
                case LoadType.ConcentratedLoad:
                case LoadType.Moment:
                case LoadType.DistributedLoad_GlobalGrid:
                default:
                    throw new InvalidDataException("※部材系分布荷重のみに使用してください。");
            }
        }

        public override string ToString()
        {
            string AAA = "";
            switch (Type)
            {
                case LoadType.Moment:
                    AAA = "荷重種類＝\tモーメント荷重\r\n大きさ＝\t" + M;
                    break;
                case LoadType.ConcentratedLoad:
                    AAA = "荷重種類＝\t集中荷重\r\n水平大きさ＝\t" + P1_H + "\r\n鉛直大きさ＝\t" + P1_V;
                    break;
                case LoadType.DistributedLoad_GlobalGrid:
                    AAA = "荷重種類＝\t全体系分布荷重\r\n１端鉛直大きさ＝\t" + P1_V + "\r\n２端鉛直大きさ＝\t" + P2_V + "\r\n１端水平大きさ＝\t" + P1_H + "\r\n２端水平大きさ＝\t" + P2_H;
                    break;
                case LoadType.DistributedLoad_MemberGrid:
                    AAA = "荷重種類＝\t部材系分布荷重\r\n１端鉛直大きさ＝\t" + P1_V + "\r\n２端鉛直大きさ＝\t" + P2_V + "\r\n１端水平大きさ＝\t" + P1_H + "\r\n２端水平大きさ＝\t" + P2_H;
                    break;
                default:
                    break;
            }
            return AAA;
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
