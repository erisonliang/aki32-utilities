

namespace Aki32_Utilities.Console_App_Utilities.Minor;
public static partial class ChainableExtensions
{

    /// <summary>
    /// delete junc files that MacOS creates 
    /// </summary>
    /// <param name="inputDir"></param>
    public static DirectoryInfo OrganizeMacOsJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            if (fi.Name == "_DS_Store" || fi.Name.StartsWith("._"))
            {
                fi.Delete();
                Console.WriteLine($"deleted: {fi}");
            }
        }


        // post process
        return inputDir!;
    }

}