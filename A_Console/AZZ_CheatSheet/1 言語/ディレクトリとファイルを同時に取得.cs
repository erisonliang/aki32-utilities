

namespace Aki32Utilities.ConsoleAppUtilities.CheatSheet;
internal class ディレクトリとファイルを同時に取得
{
    internal void all()
    {


        var path = "";
        var fileSystems = new DirectoryInfo(path).EnumerateFileSystemInfos();
        foreach (var item in fileSystems)
        {
            if ((item.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                //ディレクトリ
            }
            else
            {
                //ファイル
            }
        }


    }
}
