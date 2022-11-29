using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.UsefulClasses;
public static partial class ChainableExtensions
{

    /// <summary>
    /// DownloadFile
    /// </summary>
    /// <param name="outputFile">not nullable</param>
    /// <param name="url">where download a file from</param>
    /// <returns></returns>
    public static FileInfo DownloadFileSync(this FileInfo outputFile, Uri url)
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true, takesTimeFlag: true);


        // main
        //using var httpClient = new HttpClient();
        //using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //using var response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
        //if (response.StatusCode != HttpStatusCode.OK)
        //    throw new HttpRequestException($"Http request failed: {response.RequestMessage}");

        //using var content = response.Content;
        //using var serverStream = content.ReadAsStreamAsync().Result;
        //using var localFileStream = new FileStream(outputFile.FullName, FileMode.CreateNew);
        //serverStream.CopyTo(localFileStream);

        DataFileManager.DownloadFileFromServerAsync(outputFile, url).Wait();

        // post process
        return outputFile!;
    }

}
