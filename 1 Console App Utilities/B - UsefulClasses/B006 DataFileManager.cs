using System.Net.Http.Headers;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Xml;
using Newtonsoft.Json.Converters;
using OpenCvSharp;
using System.Xml.Serialization;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Aki32_Utilities.UsefulClasses;
public static class DataFileManager
{

    // ★★★★★★★★★★★★★★★ server

    /// <summary>
    /// generalized api access
    /// </summary>
    /// <returns></returns>
    internal static async Task<HttpResponseMessage> AccessAPIAsync_ForResponse(HttpMethod method, Uri url,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        using var client = new HttpClient();

        // auth
        if (!string.IsNullOrEmpty(authBearerToken))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authBearerToken);

        // content
        using var req = new HttpRequestMessage(method, url);

        if (!string.IsNullOrEmpty(jsonStringContent))
            req.Content = new StringContent(jsonStringContent, Encoding.UTF8, "application/json");
        else if (httpContent != null)
            req.Content = httpContent;

        // access
        var response = await client.SendAsync(req);
        return response;

    }

    /// <summary>
    /// generalized api access
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    internal static async Task<T> AccessAPIAsync_ForXmlData<T>(HttpMethod method, Uri url,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // access
        using var response = await AccessAPIAsync_ForResponse(method, url,
           authBearerToken: authBearerToken,
          jsonStringContent: jsonStringContent,
          httpContent: httpContent
          );

        // response
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        if (typeof(T) == typeof(object))
        {
            string responseString = await response.Content.ReadAsStringAsync();
            return (dynamic)responseString;
        }
        else
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(responseStream)!;
        }
    }

    /// <summary>
    /// generalized api access
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    internal static async Task<T> AccessAPIAsync_ForJsonData<T>(HttpMethod method, Uri url,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // access
        using var response = await AccessAPIAsync_ForResponse(method, url,
           authBearerToken: authBearerToken,
          jsonStringContent: jsonStringContent,
          httpContent: httpContent
          );

        // response
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);

        // return
        if (typeof(T) == typeof(object))
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return (dynamic)responseString;
        }
        else
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }

    /// <summary>
    /// Download item from server to local
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    public static async Task<FileInfo> DownloadFileFromServerAsync(FileInfo outputFile, Uri url)
    {
        using var response = await AccessAPIAsync_ForResponse(HttpMethod.Get, url);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(response.ReasonPhrase);

        using var content = response.Content;
        using var receivedStream = await content.ReadAsStreamAsync();
        using var localStream = new StreamWriter(outputFile.FullName, false).BaseStream;

        receivedStream.CopyTo(localStream);

        return outputFile;
    }


    // ★★★★★★★★★★★★★★★ local

    /// <summary>
    /// read json from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static async Task<T> ReadJsonFromLocalAsync<T>(FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);
        var json = await sr.ReadToEndAsync();

        var data = JsonConvert.DeserializeObject<T>(json);

        return data;
    }

    /// <summary>
    /// write data as json local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static async Task<FileInfo> WriteJsonToLocalAsync<T>(FileInfo outputFile, T data, bool formatted = false)
    {
        var json = JsonConvert.SerializeObject(data, formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);

        using var sr = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        await sr.WriteAsync(json);

        return outputFile;
    }


    // ★★★★★★★★★★★★★★★ 

}
