using System.Text;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{
    // TODO: test

    /// <summary>
    /// MacOS が生成するゴミを削除
    /// </summary>
    /// <param name="inputDir"></param>
    public static DirectoryInfo OrganizeMacOsJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);


        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            if (fi.Name == "_DS_Store" || fi.Name.StartsWith("._"))
            {
                fi.Delete();
                Console.WriteLine($"削除：{fi}");
            }
        }


        // post process
        return inputDir!;
    }

}