using Aki32_Utilities.General;
using Aki32_Utilities.StructuralEngineering;

namespace Aki32_Utilities.StructuralEngineering;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    public static FileInfo CreateAccFromCsv_For_DynamicPro(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        DynamicProHelper.CreateAccFromCsv(inputFile, outputFile!);


        // post process
        return outputFile!;
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
                inputFile.CreateAccFromCsv_For_DynamicPro(null);
            }
            catch (Exception)
            {
                Console.WriteLine("失敗：入力が正しいか確認してください。");
            }
        }
    }


    // ★★★★★★★★★★★★★★★

}
