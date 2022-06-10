using System.Text;

namespace Aki32_Utilities.Class;

internal static partial class FileUtil
{
    // TODO: was lazy to test 291 and 292. do it later.


    // ★★★★★★★★★★★★★★★ 291 OrganizeDropBoxJuncFiles - wait for test

    /// <summary>
    /// MacOS が生成するゴミを削除
    /// </summary>
    /// <param name="inputDir"></param>
    internal static void OrganizeMacOsJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** OrganizeMacOsJuncFiles() Called");

        // main
        foreach (var fi in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            if (fi.Name == "_DS_Store" || fi.Name.StartsWith("._"))
            {
                fi.Delete();
                Console.WriteLine($"削除：{fi}");
            }
        }
    }

    // ★★★★★★★★★★★★★★★ 292 OrganizeDropBoxJuncFiles - wait for test

    /// <summary>
    /// 選んだフォルダ内のデータのPhotoとVideoの接頭辞を削除
    /// </summary>
    /// <param name="inputDir"></param>
    internal static void OrganizeDropBoxJuncFiles(this DirectoryInfo inputDir)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** OrganizeDropBoxJuncFiles() Called");

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
                    Console.WriteLine($"変更");
                    Console.WriteLine($"　前：{oldFullName}");
                    Console.WriteLine($"　後：{newFullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー：{newFullName}");
                }
            }
        }
    }

    // ★★★★★★★★★★★★★★★ 

}