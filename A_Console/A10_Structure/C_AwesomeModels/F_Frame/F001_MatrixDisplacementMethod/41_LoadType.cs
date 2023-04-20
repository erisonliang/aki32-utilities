

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 荷重種類
    /// </summary>
    public enum LoadType
    {
        ConcentratedLoad,
        Moment,
        DistributedLoad_GlobalGrid,
        DistributedLoad_MemberGrid,
    }
}
