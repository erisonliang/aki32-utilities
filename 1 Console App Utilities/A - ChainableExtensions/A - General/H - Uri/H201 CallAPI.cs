using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace Aki32_Utilities.UsefulClasses;
public static partial class ChainableExtensions
{

    /// <summary>
    /// generalized api access
    /// </summary>
    /// <returns></returns>
    internal static async Task<HttpResponseMessage> CallAPIAsync_ForResponse(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);
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


        // post process
        return response;

    }


    /// <summary>
    /// generalized api access
    /// </summary>
    /// <exception cref="HttpRequestException">
    /// response was not OK
    /// </exception>
    internal static async Task<T> CallAPIAsync_ForXmlData<T>(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // access
        using var response = await url.CallAPIAsync_ForResponse(method,
           authBearerToken: authBearerToken,
          jsonStringContent: jsonStringContent,
          httpContent: httpContent
          );


        // response
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);


        // post process
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
    internal static async Task<T> CallAPIAsync_ForJsonData<T>(this Uri url, HttpMethod method,
        string authBearerToken = "",
        string jsonStringContent = null,
        HttpContent httpContent = null
        )
    {
        // preprocess
        General.UtilPreprocessors.PreprocessBasic(true);


        // access
        using var response = await url.CallAPIAsync_ForResponse(method,
           authBearerToken: authBearerToken,
          jsonStringContent: jsonStringContent,
          httpContent: httpContent
          );

        // response
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase);


        // post process
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


}
