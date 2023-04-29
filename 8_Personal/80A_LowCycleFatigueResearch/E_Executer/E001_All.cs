using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public static partial class Executer
{
    public static void All(LINEController line)
    {

        // ★★★★★ 小規模なやつ
        //Module.Small();


        // ★★★★★ 実験
        {
            //// ★ 動画作成
            //Module.M020_CreateVideo();

        }

        //013-CKKMM006

        // ★★★★★ SNAP
        {
            // SNAP前処理
            {
                // ★ 地震応答
                {
                    //Module.M040_EQResponse();
                }

                // ★ KNetのデータの情報整理
                {
                    //PythonController.Initialize();

                    //var decompressing = new FileInfo(@"C:\Users\aki32\Dropbox\PC\Desktop\selecteddata.tar");
                    //Module.M041_KNet_Decompress(decompressing);

                    //// KNet → SNAP
                    //var baseDir = new DirectoryInfo(@"C:\Users\aki32\Dropbox\Apps\建築系\SNAP（任意形状の解析）\SNAP共有データ\SharedWave\ストック\BCJ");
                    //Module.M042_KNet_CreateSNAPWave(baseDir);
                }

                // ★ SNAPのデータの情報整理（複数の地震動のスペクトルとか）
                {

                    //PythonController.Initialize();

                    //var baseDir = new DirectoryInfo(@"C:\Users\aki32\Dropbox\Apps\建築系\SNAP（任意形状の解析）\SNAP共有データ\SharedWave\ストック\KNet\KNMYG004");
                    //Dictionary<string, string>? IdAndDisplayNameRelationalInfo = null;
                    //{
                    //    var metaDataFile = baseDir.GetChildFileInfo(@$"{baseDir.Name}-meta.xlsx");
                    //    var meta = metaDataFile.GetExcelSheet("Target1")!;
                    //    IdAndDisplayNameRelationalInfo = new Dictionary<string, string>();
                    //    foreach (var metaLine in meta.ConvertToJaggedArray().Skip(1))
                    //        IdAndDisplayNameRelationalInfo.Add(metaLine[1], metaLine[0]);
                    //}

                    //// SNAP → （スペクトル，波）
                    //Module.M051_SNAP_CalcWaveAndSpectrum_FromMultiple(baseDir, IdAndDisplayNameRelationalInfo);

                    ////（スペクトル，波） → グラフ
                    //Module.M052_SNAP_DrawGraphs_FromMultiple(baseDir, IdAndDisplayNameRelationalInfo, "Target1");

                }

                // ★ SNAPのデータの情報整理（１個地震動に対して比例倍仮定を使ったやつ）
                {

                    //PythonController.Initialize();

                    //var waveFile = new FileInfo(@"C:\Users\aki32\Dropbox\Apps\建築系\SNAP（任意形状の解析）\SNAP共有データ\SharedWave\ストック\KNet\KNMYG004\SnapWaves\KNMYG004-000829.wv");
                    //var metaDataFile = new FileInfo(@"C:\Users\aki32\Dropbox\Apps\建築系\SNAP（任意形状の解析）\SNAP共有データ\SharedWave\ストック\KNet\KNMYG004\KNMYG004-000829-Amp.xlsm");
                    //List<(int PGV, double Amp)> PGVsAndAmpsList = null;
                    //{
                    //    var meta = metaDataFile.GetExcelSheet("Target1")!;
                    //    PGVsAndAmpsList = new List<(int PGV, double Amp)>();
                    //    foreach (var metaLine in meta.ConvertToJaggedArray().Skip(1))
                    //        PGVsAndAmpsList.Add((int.Parse(metaLine[1]), double.Parse(metaLine[2])));
                    //}


                    //// SNAP → （スペクトル，波）
                    //Module.M053_SNAP_CalcWaveAndSpectrum_FromSingle(waveFile, PGVsAndAmpsList);

                    ////（スペクトル，波） → グラフ
                    //Module.M054_SNAP_54_DrawGraphs_FromSingle(waveFile, PGVsAndAmpsList);

                }
            }

            // SNAP後処理 1
            if (true)
            {
                // ★ 変数（ここを割と編集する。）

                var targetModels = Array.Empty<string>();
                var targetEQs = Array.Empty<string>();
                DirectoryInfo collectedResultsBaseDir = null;

                {

                    targetModels = new string[] { "S-A30-B仕-CKNKMM006--4" };
                    targetEQs = Enumerable.Range(1, 32).Select(x => $"D{x}").ToArray();
                    collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\013-CKKMM006").CreateAndPipe();

                    //targetModels = new string[] { "S-A30-B仕-CKNMYG004--3" };
                    //targetEQs = Enumerable.Range(1, 34).Select(x => $"D{x}").ToArray();
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\011-MYG829 比例倍仮定");

                    //targetModels = new string[] { "S-A30-B仕-CKNMYG004--3" };
                    //targetEQs = new string[] { "D35" };
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\012-MYG829 極稀");

                    //targetModels = new string[] { "S-A30-B仕-CBCJ--2" };
                    //targetEQs = Enumerable.Range(1, 40).Select(x => $"D{x}").ToArray();
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\010-CBCJ");

                    //targetModels = new string[] { "S-A30-B仕-CKNFKS004--2" };
                    //targetEQs = Enumerable.Range(1, 23).Select(x => $"D{x}").ToArray();
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\010-CKNFKS004");

                    //targetModels = new string[] { "S-A30-B仕-CKKMM006--2" };
                    //targetEQs = Enumerable.Range(1, 27).Select(x => $"D{x}").ToArray();
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\010-CKKMM006");

                    //targetModels = new string[] { "S-A30-B仕-CKNMYG004--2" };
                    //targetEQs = Enumerable.Range(1, 30).Select(x => $"D{x}").ToArray();
                    //collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\010-CKNMYG004");

                    //targetModels = new string[] { "S-A30-B仕-COS1" };
                    //targetModels = new string[] { "S-A30-B仕-CCH1", "S-A30-B仕-CSZ1", "S-A30-B仕-CKA1" };
                    //targetModels = new string[] { "S-A30-B仕-CCH1", "S-A30-B仕-CSZ1", "S-A30-B仕-COS1", "S-A30-B仕-CKA1" };
                    //targetModels = new string[] { "S-A30-B仕-CBCJ", "S-A30-B仕-CE4E" };
                    //targetModels = new string[] { "S-A30-B仕-CBCJ" };

                    //targetEQs = new string[] { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11", "D12", "D13", "D14", "D15" };
                    //targetEQs = new string[] { "D1", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11" };
                    //targetEQs = new string[] { "D10", "D11" };
                    //targetEQs = Enumerable.Range(1, 15).Select(x => $"D{x}").ToArray();

                    //var collectedResultsBaseDir = new DirectoryInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\999");

                }


                // ★ ほぼ固定の変数

                //var snapOutputDir = new DirectoryInfo($@"D:\SNAP結果バックアップ").CreateAndPipe();
                var snapOutputDir = new DirectoryInfo($@"C:\Users\aki32\MyLocalData\SNAP - results\修論モデル").CreateAndPipe();

                var collectedResultsBaseDir_ResultCsv = collectedResultsBaseDir.GetChildDirectoryInfo("ResultCsv").CreateAndPipe();
                var buildingInfoExcel = new FileInfo(@$"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\01 model\部材配置情報.xlsx");


                //// ★ SNAPエクセルキラー（10回終わったら次の処理に移行！）
                //Module.M030_KeepClosingExcel();
                //Console.WriteLine("\r\n\r\nExcelキラー終了！\r\n次の処理に移行。\r\n\r\n");
                //_ = line.SendMessageAsync(@"Excelキラー終了");

                //// ★ SNAPからのデータ抽出
                //Module.M061_SNAP_1_CSCollect(targetModels, targetEQs, line, snapOutputDir, collectedResultsBaseDir);

                //// ★ Pythonで描画やら解析やら
                //Module.M062_SNAP_2_PyDraw(targetModels, targetEQs, line, collectedResultsBaseDir, buildingInfoExcel);

                //// ★ SNAPの，ResultCsvの更なる集計！
                //Module.M063_SNAP_3_CSCollect(line, collectedResultsBaseDir_ResultCsv);

            }

            // SNAP後処理 2
            {
                //var collectedResultsBaseDir = new DirectoryInfo(@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\14 SNAP\修論モデル\09 calc\011-MYG829 比例倍仮定");

                //// ★ Pythonで描画やら解析やら
                //Module.M064_SNAP_4_Calc100YearDamage(line, collectedResultsBaseDir, "600_j", "{DValue}_{edgeName}");

            }

        }

    }
}
