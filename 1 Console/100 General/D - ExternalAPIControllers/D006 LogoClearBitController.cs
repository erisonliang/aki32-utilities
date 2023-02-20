

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class LogoClearBitController
{

    // ★★★★★★★★★★★★★★★ props

    public static Uri TargetUri(string targetSite) => new($"https://logo.clearbit.com/{targetSite}");


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// download logo file from https://logo.clearbit.com
    /// </summary>
    public static async Task<bool> DownloadWebSiteLogoAsync(string targetSiteString, FileInfo outputFile)
    {
        try
        {
            // request
            await TargetUri(targetSiteString)
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
