using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M063_SNAP_3_CSCollect(
        LINEController line,
        DirectoryInfo ResultCsvDir
        )
    {
        // init
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★★★★★★★★★★★★★★★ SNAP_3_CSCollect 開始！！！！", ConsoleColor.Blue);
        _ = line.SendMessageAsync(@$"★ SNAP_3_CSCollect 開始！！！！");


        // main

        // SNAPのPythonからの出力，エクセルに集計（損傷と最大塑性振幅のやつ！！）
        var BeamDamageDir = ResultCsvDir;
        var BeamDamageDirCollectedFile = ResultCsvDir.GetChildFileInfo("# CollectedBeamDamageAndMuAmpPeak.xlsx");
        BeamDamageDir
            .RenameFiles_Replace(new (string from, string to)[] { ("BeamDamage", "d-"), ("BeamMuAmpPeak", "mu-") })
            .RenameFiles()
            .Csvs2ExcelSheets(BeamDamageDirCollectedFile);


        // SNAPのPythonからの出力，エクセルに集計（SpecificBeamMuAmpHistのヒストグラム！）
        var SpecificBeamMuAmpHistDir = ResultCsvDir.GetChildDirectoryInfo("SpecificBeamMuAmpHist");
        var SpecificBeamMuAmpHistCollectedFile = ResultCsvDir.GetChildFileInfo("# CollectedSpecificBeamMuAmpHist.csv");
        SpecificBeamMuAmpHistDir
            .TransposeCsv_Loop(null)
            .RenameFiles()
            .CollectCsvColumns(SpecificBeamMuAmpHistCollectedFile, 1, initialColumn: 2, skipRowCount: 1);


    }
}
