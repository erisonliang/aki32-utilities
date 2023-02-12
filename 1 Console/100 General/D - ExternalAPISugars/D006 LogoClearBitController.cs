using System.Collections.Specialized;
using System.Net;
using System.Text;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class LogoClearBitController
{

    // ★★★★★★★★★★★★★★★ props

    public static Uri TargetUri(string targetSite) => new($"https://logo.clearbit.com/{targetSite}");


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// download logo file from https://logo.clearbit.com
    /// </summary>
    public static async Task<bool> DownloadWebSiteLogoAsync(Uri targetSite, FileInfo outputFile)
    {
        try
        {
            // request
            await TargetUri(targetSite.ToString())
                .DownloadFileAsync(outputFile);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }


    // ★★★★★★★★★★★★★★★ 

}
