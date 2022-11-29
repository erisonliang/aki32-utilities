using System.Net.Http.Headers;

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
            using var client = new HttpClient();

            // auth
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LineAccessToken);

            // content
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "message", message } });

            // request
            var result = await client.PostAsync("https://notify-api.line.me/api/notify", content);

        }
        catch (Exception)
        {
            return false;
        }

        return true;

    }


    // ★★★★★★★★★★★★★★★ 

}
