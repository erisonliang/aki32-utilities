using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class LINEController
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Obtain from https://notify-bot.line.me/my/
    /// </summary>
    public string LineAccessToken { get; set; } = "";

    private static readonly Uri TargetUri = new("https://notify-api.line.me/api/notify");


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
        Console.WriteLine();

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
            var data = new Dictionary<string, string>
            {
                { "message", message }
            };
            var content = new FormUrlEncodedContent(data);

            // request
            await TargetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
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
