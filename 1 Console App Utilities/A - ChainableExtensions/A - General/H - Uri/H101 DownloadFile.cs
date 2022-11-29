using System.Net;

namespace Aki32_Utilities.UsefulClasses;
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
    public static async Task<FileInfo> DownloadFileAsync(this Uri url, FileInfo outputFile)
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // main
        using var response = await url.CallAPIAsync_ForResponse(HttpMethod.Get);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(response.ReasonPhrase);

        using var content = response.Content;
        using var receivedStream = await content.ReadAsStreamAsync();
        using var localStream = new StreamWriter(outputFile.FullName, false).BaseStream;

        receivedStream.CopyTo(localStream);


        // post process
        return outputFile!;

    }


}
