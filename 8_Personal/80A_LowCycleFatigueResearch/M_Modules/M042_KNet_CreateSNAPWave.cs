using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M042_KNet_CreateSNAPWave(DirectoryInfo targetDir)
    {
        // ★★★★★ defs
        var inputs = targetDir
            .GetFilesWithRegexen(SearchOption.AllDirectories, @$".*\.EW$", @$".*\.NS$")
            .ToArray();

        var outputDir_SnapWave = targetDir.GetChildDirectoryInfo(@"SnapWaves");
        outputDir_SnapWave.Create();

        var outputFile_InfoTable = targetDir.GetChildFileInfo(@"InfoTable.csv");


        // ★★★★★ main

        var knetList = new List<KNetHelper.KNetAccData>();

        using var progress = new ProgressManager(inputs.Length);
        progress.StartAutoWrite(100);

        for (int i = 0; i < inputs.Length; i++)
        {
            var input = inputs[i];
            var knet = new KNetHelper.KNetAccData(input);

            var id = i + 1;
            var waveId = $"KN-{knet.StationCode}-{id:D6}";
            knet.Memo = waveId;

            knet.CalcMaxVel();

            var outputFile = outputDir_SnapWave.GetChildFileInfo($"{waveId}.wv");
            SNAPHelper.SNAPWaveData.CreateSNAPWaveFile(outputFile, knet.Accs, waveId, knet.MaxAcc, knet.MaxVel);

            knet.Accs = null;
            knetList.Add(knet);

            progress.CurrentStep = i;
        }

        progress.WriteDone();

        knetList
            .SaveAsCsv(outputFile_InfoTable)
            .Csv2ExcelSheet(null);
    }
}
