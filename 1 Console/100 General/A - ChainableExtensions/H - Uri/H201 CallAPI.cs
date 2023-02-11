using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// generalized api access
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    /// <returns></returns>
    public static async Task<Stream> CallAPIAsync_ForResponseStream(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);
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
        var response = await client.SendAsync(req,HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        // post process
        return await response.Content.ReadAsStreamAsync();

    }


    /// <summary>
    /// generalized api access
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    public static async Task<T> CallAPIAsync_ForXmlData<T>(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);


        // access
        using var responseStream = await url.CallAPIAsync_ForResponseStream(method,
            authBearerToken: authBearerToken,
            jsonStringContent: jsonStringContent,
            httpContent: httpContent
            );


        // post process
        if (typeof(T) == typeof(object))
        {
            using var responseReadStream = new StreamReader(responseStream);
            return (dynamic)responseReadStream.ReadToEnd();
        }
        else
        {
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
    public static async Task<T> CallAPIAsync_ForJsonData<T>(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);


        // access
        using var responseStream = await url.CallAPIAsync_ForResponseStream(method,
           authBearerToken: authBearerToken,
          jsonStringContent: jsonStringContent,
          httpContent: httpContent
          );


        // post process
        using var responseReadStream = new StreamReader(responseStream);
        var responseString = responseReadStream.ReadToEnd();


        // Avoiding dynamic type recognized as object this way
        try
        {
            return JsonConvert.DeserializeObject<T>(responseString);
        }
        catch (Exception ex)
        {
            if (typeof(T) == typeof(object))
                return (dynamic)responseString;

            throw;
        }

    }

}
