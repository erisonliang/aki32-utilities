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

        Console.WriteLine("LINEController Instance Created.");
        Console.WriteLine("Data Powered by LINE Notify (https://notify-bot.line.me/ja/)");

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
            var url = new Uri("https://notify-api.line.me/api/notify");
            await url.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
                authBearerToken: LineAccessToken,
                httpContent: content);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    // ★★★★★★★★★★★★★★★ 

}
