

using System.Net;

namespace Aki32Utilities.ConsoleAppUtilities.General;
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
    public static FileInfo DownloadFile(this Uri url, FileInfo outputFile,
        bool ignoreWhenExists = false
        )
    {
        // preprocess
        outputFile.Directory!.Create();
        UtilPreprocessors.PreprocessBasic(true);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();


        // main
        url.DownloadFileAsync(outputFile, ignoreWhenExists).Wait();


        // post process
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return outputFile!;
    }

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
        UtilPreprocessors.PreprocessBasic(true);


        // ignore
        if (ignoreWhenExists && outputFile.Exists)
            return outputFile!;


        // access and save
        using var responseStream = await url.CallAPIAsync_ForResponseStream(HttpMethod.Get);
        using var localStream = new StreamWriter(outputFile.FullName, false).BaseStream;
        await responseStream.CopyToAsync(localStream);


        // post process
        return outputFile!;

    }

}
