using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M062_SNAP_2_PyDraw(
        string[] targetModels,
        string[] targetEQs,
        LINEController line,
        DirectoryInfo collectedResultsBaseDir,
        FileInfo buildingInfoExcel
        )
    {
        // init
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★★★★★★★★★★★★★★★ SNAP_2_PyDraw 開始！！！！", ConsoleColor.Blue);
        _ = line.SendMessageAsync(@$"★ SNAP_2_PyDraw 開始！！！！");


        // main
        PythonController.AdditionalPath.Add(@"C:\Users\aki32\Dropbox\Codes\# Projects\研究\修士論文研究\8_Personal\80A_LowCycleFatigueResearch\P_PythonCodes");
        PythonController.Initialize();
        dynamic snap = PythonController.Import("SNAPVisualizer");


        // 全組み合わせ呼び出し
        foreach (var model in targetModels)
            foreach (var eq in targetEQs)
            {
                try
                {
                    var resultExcelPath = collectedResultsBaseDir.GetChildFileInfo(@$"{model}_{eq}.xlsx").FullName;
                    Console.WriteLine(resultExcelPath);
                    var bv = snap.SNAPBeamVisualizer(buildingInfoExcel.FullName, resultExcelPath, grid_size: 3);


                    // ★★★★★ For getting all processed data;
                    bv.VisualizeBeamDamage_CUI(target_story: 4, skip_show: true);
                    bv.VisualizeBeamDamage(target_story: 4, skip_show: true);

                    bv.VisualizeSpecificBeamMuAmpHist_Loop(target_story: 4, skip_show: true);
                    bv.VisualizeBeamMuAmpHist(target_story: 4, skip_show: true);

                    bv.VisualizeBeamMuAmpPeak_CUI(target_story: 4, skip_show: true);
                    bv.VisualizeBeamMuAmpPeak(target_story: 4, skip_show: true);


                    // ★★★★★ ALL

                    //bv.VisualizeFrame(skip_show: true);

                    //bv.VisualizeBeamName_CUI(target_story: 4, skip_show: false);

                    //bv.VisualizeBeamName(target_story: 4, skip_show: true);

                    //bv.GetSpecificBeamDamage_CUI("600_j");
                    //bv.VisualizeBeamDamage_CUI(target_story: 4, skip_show: false);
                    //bv.VisualizeBeamDamage(target_story: 4, skip_show: true);

                    //bv.VisualizeSpecificBeamMuAmpHist("600_j", skip_show: true);
                    //bv.VisualizeSpecificBeamMuAmpHist_Loop(5, skip_show: true);
                    //bv.VisualizeBeamMuAmpHist_Test(skip_show: true);
                    //bv.VisualizeBeamMuAmpHist(target_story: 4, skip_show: true);

                    //bv.VisualizeBeamMuAmpPeak_CUI(target_story: 4, skip_show: false);
                    //bv.VisualizeBeamMuAmpPeak(target_story: 4, skip_show: true);


                    // ★★★★★ 確認
                    // Console.WriteLine(bv.df_beam);

                }
                catch (Exception e)
                {
                }
            }


        // post process
        PythonController.Shutdown();

    }
}
