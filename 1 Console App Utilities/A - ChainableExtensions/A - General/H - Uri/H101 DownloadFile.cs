using System.Net;

namespace Aki32Utilities.ConsoleAppUtilities.UsefulClasses;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Download item from server to local
    /// </summary>
    /// <param name="url">where download a file from</param>
    /// <param name="outputFile">not nullable</param>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    public static async Task<FileInfo> DownloadFileAsync(this Uri url, FileInfo outputFile,
        bool ignoreWhenExists = false
        )
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // ignore
        if (ignoreWhenExists && outputFile.Exists)
            return outputFile!;


        // access and save
        using var responseStream = await url.CallAPIAsync_ForResponseStream(HttpMethod.Get);
        using var localStream = new StreamWriter(outputFile.FullName, false).BaseStream;
        responseStream.CopyTo(localStream);


        // post process
        return outputFile!;

    }

}
