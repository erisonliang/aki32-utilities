using System.Text;

namespace StructuralEngineering_Utilities.SoftwareHelpers;
public static partial class StructuralEngineering_Utilities_Extensions
{

    // ★★★★★★★★★★★★★★★ main

    public static FileInfo CreateAccFromCsv_For_DynamicPro(this FileInfo inputFile)
    {
        // TODO update

        var inputFilePath = inputFile.FullName;
        const int STEP = 8;
        try
        {
            if (!inputFilePath.EndsWith(".csv"))
                throw new Exception("受け付けられるのはCSVファイルのみです。");
            var outputFilePath = inputFilePath.Replace(".csv", ".acc");


            var input = File.ReadLines(inputFilePath, Encoding.UTF8).ToArray();

            var input2 = input // 全加速度データ
                .Select(x => x.Split(new string[] { "," }, StringSplitOptions.None)[1])
                .Skip(1)
                .Select(x => float.Parse(x))
                .ToList();

            var input3 = input // 最初の2データの時間
                .Skip(1)
                .Take(2)
                .Select(x => x.Split(new string[] { "," }, StringSplitOptions.None)[0])
                .Select(x => float.Parse(x))
                .ToArray();

            var resultStr = new List<string>();
            resultStr.Add($"{input2.Count},{input3[1] - input3[0]}"); // データ個数，時間間隔推測

            var resultStrBuffer = "";
            for (int i = 0; i < input2.Count; i++)
            {
                // 8つ分キープして，押し出す。
                if (i % STEP == 0 && resultStrBuffer != "")
                {
                    resultStr.Add(resultStrBuffer);
                    resultStrBuffer = "";
                }

                resultStrBuffer += string.Format("{0,10:F3}", input2[i]); // 10桁右寄せ 小数点以下3桁
            }

            // 最後にバッファを押し出す。
            if (resultStrBuffer != "")
            {
                resultStr.Add(resultStrBuffer);
                resultStrBuffer = "";
            }

            File.WriteAllLines(outputFilePath, resultStr.ToArray(), Encoding.ASCII);

            Console.WriteLine($"O 成功 : {outputFilePath}");

            return new FileInfo(outputFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"X 失敗 : {inputFilePath} - {e.Message}");
            return null;
        }
    }


    // ★★★★★★★★★★★★★★★ sugar

    public static FileInfo CreateAccFromCsvConversationally_For_DynamicPro()
    {
        Console.WriteLine("2行目からA列に時間・B列に加速度を配置した.csvファイルのパスを入力することで処理を実行できます。");

        while (true)
        {
            Console.WriteLine("================================================");
            Console.WriteLine("処理したいデータのパスを入力：");

            var input = Console.ReadLine();
            input = input.Trim('\"');
            try
            {
                var inputFile = new FileInfo(input);
                inputFile.CreateAccFromCsv_For_DynamicPro();
            }
            catch (Exception)
            {
                Console.WriteLine("失敗：入力が正しいか確認してください。");
            }
        }
    }


    // ★★★★★★★★★★★★★★★

}
