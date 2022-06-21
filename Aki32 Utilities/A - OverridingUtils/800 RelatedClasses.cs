using System.Runtime.InteropServices;
using System.Security;

namespace Aki32_Utilities.OverridingUtils;
public static class RelatedClasses
{

    // ★★★★★★★★★★★★★★★ 901 Sort IEnumerable<FileInfo>

    [SuppressUnmanagedCodeSecurity]
    public static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
    public sealed class NaturalFileInfoNameComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo a, FileInfo b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
        }
    }
    public static IEnumerable<FileInfo> Sort(this IEnumerable<FileInfo> targetFiles)
    {
        var targetFilesList = targetFiles.ToList();
        targetFilesList.Sort(new NaturalFileInfoNameComparer());
        return targetFilesList;
    }

    // ★★★★★★★★★★★★★★★ 

}
