using System.Drawing;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M020_CreateVideo()
    {
        UtilConfig.IncludeGuidToNewOutputDirName = false;

        // test
        {

            //Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 定義\r\n");

            //var targetDirectory = new DirectoryInfo(@"C:\Users\aki32\Dropbox\Codes\# Projects\研究\修士論文研究\1 CSharp\#TestModel\Images2Video\RawImages");

            //var marginSizes = new float[] { 0.15f, 0.25f, 0.15f, 0.25f };
            //var targetPoints = new PointF[]
            //{
            //    new PointF(marginSizes[0], marginSizes[1]),
            //    new PointF(1 - marginSizes[2], marginSizes[1]),
            //    new PointF(marginSizes[0], 1 - marginSizes[3])
            //};
            //targetPoints = null;


            //Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 画像の歪み補正\r\n");

            //targetDirectory.ProcessVideoSourceImages(targetDirectory, targetPoints, Brushes.White);


            //Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 映像化\r\n");

            //targetDirectory.Images2Video(null, 3);


            //Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

        }

        // 本番
        {

            Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 定義\r\n");

            //var baseDir = new DirectoryInfo(@"C:\Users\aki32\MyLocalData\小振幅実験 - 動画作成\D★ 受信原本1_小振幅実験（千葉大）20220825_新試験体・３体目・μ075peak1_LScE\00001-01335");
            //var baseDir = new DirectoryInfo(@"C:\Users\aki32\MyLocalData\小振幅実験 - 動画作成\D★ 受信原本1_小振幅実験（千葉大）20220825_新試験体・３体目・μ075peak1_LScE\01251-01335");
            var baseDir = new DirectoryInfo(@"C:\Users\aki32\MyLocalData\小振幅実験 - 動画作成\D★ 受信原本1_小振幅実験（千葉大）20220825_新試験体・３体目・μ075peak3_UScE\01028-01335");
            //var baseDir = new DirectoryInfo(@"C:\Users\aki32\MyLocalData\小振幅実験 - 動画作成\D★ 受信原本1_小振幅実験（千葉大）20220825_新試験体・３体目・μ075peak3_UScE\00001-01335");


            var pickingPointCount = 3;
            var imgFrameRate = 15;
            var videoFrameRate = 60;


            var rawDir = new DirectoryInfo($@"{baseDir}\Raw");
            var rawDivDir = new DirectoryInfo($@"{baseDir}\RawDiv");
            var distortedDir = new DirectoryInfo($@"{baseDir}\Distorted p={pickingPointCount}");

            var readyForVideoDir = rawDir;
            //var readyForVideoDir = distortedDir;

            Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 画像の歪み補正\r\n");

            //rawDivDir.ProcessVideoSourceImages(distortedDir,
            //    presetTargetPointRatios: null,
            //    fill: Color.White,
            //    pickingPointCount: pickingPointCount);


            Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 画像の歪み補正やり直し\r\n");

            //new DirectoryInfo(Path.Combine(rawDivDir.FullName, "4"))
            //    .DistortImage_Loop_Conversationally_Func(distortedDir,
            //        presetTargetPointRatios: null,
            //        fill: Color.White,
            //        pickingPointCount: pickingPointCount)();


            Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 文字列の追加・リサイズ・映像化\r\n");

            var nameCandidate = $"{baseDir.Name}   i={imgFrameRate,3}Hz   v={videoFrameRate,3}Hz   p={pickingPointCount}   .mp4";
            var output = new FileInfo(Path.Combine(baseDir.Parent!.FullName, nameCandidate));

            readyForVideoDir
                .ResizeImageProportionally_Loop(null, new SizeF(0.3f, 0.3f))
                .AddTextToImageProportionally_Loop(null, "%FN", new PointF(0.98f, 0.93f), fontSizeRatio: 0.03, alignRight: true)
                .Images2Video(output, imgFrameRate, videoFrameRate)
                ;


            Console.WriteLine("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

        }

    }
}
