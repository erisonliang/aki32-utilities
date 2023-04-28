using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M041_KNet_Decompress(FileInfo input)
    {
        // ★★★★★ defs

        input
            .Decompress_Tar(null)
            .Decompress_TarGzip_Loop(null);

        // ★★★★★ main

    }
}
