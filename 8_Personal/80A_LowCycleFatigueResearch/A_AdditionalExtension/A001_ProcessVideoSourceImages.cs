using System.Drawing;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public static partial class AdditionalExtension
{

    /// <summary>
    /// 画像の歪み補正。
    /// 各視点ごとに画像をまとめたフォルダたちを用意して，その各フォルダに対して座標を指定。
    /// </summary>
    /// <returns></returns>
    public static DirectoryInfo ProcessVideoSourceImages(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        PointF[] presetTargetPointRatios = null,
        Color? fill = null,
        int pickingPointCount = 4
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir!, inputDir!);

        var funcs = new Queue<Func<DirectoryInfo>>();
        var currentTask = Task.Run(() => null);

        // 移動先ターゲットが null なら手動指定！
        Point[] presetFramePoints = null;
        if (presetTargetPointRatios == null)
        {
            Console.WriteLine("★★★★★ 移動先ターゲットを指定。");
            presetFramePoints = ChainableExtensions.GetFramePointsConversationally_For_DistortImage();
            presetTargetPointRatios = ChainableExtensions.GetTargetPointRatiosConversationally_For_DistortImage(
                presetFramePoints: presetFramePoints,
                pickingPointCount: pickingPointCount);
        }

        // main
        UtilConfig.ConsoleOutput_Contents = false;
        foreach (var targetDir in inputDir.GetDirectories())
        {
            if (targetDir.FullName == outputDir.FullName) continue;
            funcs.Enqueue(targetDir.DistortImageConversationally_Loop_Func(outputDir, fill, presetTargetPointRatios,
                presetFramePoints: presetFramePoints,
                pickingPointCount: pickingPointCount));

            if (currentTask.IsCompleted)
                currentTask = Task.Run(funcs.Dequeue());
        }


        Console.WriteLine("\r\n★★★★★ 境界条件設定終了。画像歪み除去処理の終了を待機中…\r\n");
        UtilConfig.ConsoleOutput_Contents = true;

        currentTask.Wait();
        while (funcs.Count > 0)
            funcs.Dequeue()();


        Console.WriteLine("\r\n★★★★★ 画像歪み除去処理終了。\r\n");


        // post process
        return outputDir!;
    }

}
