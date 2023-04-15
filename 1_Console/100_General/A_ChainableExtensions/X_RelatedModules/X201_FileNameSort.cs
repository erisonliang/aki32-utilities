using System.Runtime.InteropServices;
using System.Security;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    public static IEnumerable<FileInfo> Sort(this IEnumerable<FileInfo> targetFiles)
    {
        var targetFilesList = targetFiles.ToList();
        targetFilesList.Sort(new NaturalFileInfoNameComparer());
        return targetFilesList;
    }


    // ★★★★★★★★★★★★★★★ helper

    [SuppressUnmanagedCodeSecurity]
    private static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
    private sealed class NaturalFileInfoNameComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo? a, FileInfo? b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a!.Name, b!.Name);
        }
    }


    // ★★★★★★★★★★★★★★★ 

}
