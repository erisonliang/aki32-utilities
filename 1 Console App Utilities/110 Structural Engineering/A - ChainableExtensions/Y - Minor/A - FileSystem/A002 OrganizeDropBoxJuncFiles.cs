

namespace Aki32Utilities.ConsoleAppUtilities.Minor;
public static partial class ChainableExtensions
{
    /// <summary>
    /// delete prefix of Dropbox updated files. (e.g.: Photo, Video,...)
    /// </summary>
    /// <param name="inputDir"></param>
    public static DirectoryInfo OrganizeDropBoxJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            string oldName = fi.Name;
            string newName = fi.Name;

            newName = newName
                .Replace("スクリーンショット ", "")
                .Replace("Photo ", "")
                .Replace("Video ", "");

            if (oldName != newName)
            {
                var oldFullName = fi.FullName;
                var newFullName = oldFullName.Replace(oldName, newName);

                try
                {
                    fi.MoveTo(newFullName);
                    Console.WriteLine($"updated from: {oldFullName}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"error: {newFullName}");
                }
            }
        }


        // post process
        return inputDir;
    }

}