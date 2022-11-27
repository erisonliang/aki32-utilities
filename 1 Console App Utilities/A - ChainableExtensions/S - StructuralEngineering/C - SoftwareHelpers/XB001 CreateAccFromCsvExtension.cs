using System.Text;

using Aki32_Utilities.ChainableExtensions;

namespace StructuralEngineering_Utilities.SoftwareHelpers;
public static partial class StructuralEngineering_Utilities_Extensions
{

    // ★★★★★★★★★★★★★★★ main

    public static FileInfo CreateAccFromCsv_For_DynamicPro(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main
        return inputFile.ReadCsv_Rows(skipColumnCount, skipRowCount).SaveCsv_Columns(outputFile!, header);

        CreateAccFromCsv.CreateAccFromCsv_For_DynamicPro(inputFile,);
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
