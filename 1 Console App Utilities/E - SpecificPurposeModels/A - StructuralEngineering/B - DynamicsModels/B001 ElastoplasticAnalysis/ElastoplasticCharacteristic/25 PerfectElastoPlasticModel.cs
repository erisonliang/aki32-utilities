

namespace Aki32_Utilities.SpecificPurposeModels.StructuralEngineering;

/// <summary>
/// Bilinear Model where K2=0
/// </summary>
public class PerfectElastoPlasticModel : BilinearModel
{

    // ★★★★★★★★★★★★★★★ inits

    public PerfectElastoPlasticModel(double K1, double Fy) : base(K1, MIN_K / K1, Fy)
    {
        // K2は，X が発散しないくらいの極小の数値とする。
    }


    // ★★★★★★★★★★★★★★★

}
