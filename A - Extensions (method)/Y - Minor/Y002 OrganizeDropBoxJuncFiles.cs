using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{
    // TODO: test

    /// <summary>
    /// 選んだフォルダ内のデータのPhotoとVideoの接頭辞を削除
    /// </summary>
    /// <param name="inputDir"></param>
    public static void OrganizeDropBoxJuncFiles(this DirectoryInfo inputDir)
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

}