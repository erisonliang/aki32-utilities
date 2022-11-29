using System.Net.Http.Headers;

using Aki32_Utilities.UsefulClasses;

namespace Aki32_Utilities.ExternalAPIControllers;
public class LINEController
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Obtain from https://notify-bot.line.me/my/
    /// </summary>
    public string LineAccessToken { get; set; } = "";


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public LINEController(string lineAccessToken)
    {
        LineAccessToken = lineAccessToken;
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// send message to LINE
    /// </summary>
    public async Task<bool> SendMessageAsync(string message)
    {
        try
        {
            // content
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "message", message } });

            // request
            await DataFileManager.AccessAPIAsync_ForJsonData<object>(HttpMethod.Post, new Uri("https://notify-api.line.me/api/notify"),
                authBearerToken: LineAccessToken, httpContent: content);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    // ★★★★★★★★★★★★★★★ 

}
