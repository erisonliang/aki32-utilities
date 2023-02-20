using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Drawing.Charts;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class OpenAIController
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Obtain from https://openai.com/api/ .
    /// </summary>
    public string APISecretKey { get; set; } = "";

    private static readonly string BaseUriString = "https://api.openai.com/v1";


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="apiSecretKey"></param>
    public OpenAIController(string apiSecretKey)
    {
        APISecretKey = apiSecretKey;

        Console.WriteLine("OpenAIController Instance Created.");
        Console.WriteLine("Data Powered by OpenAI (https://platform.openai.com/docs/api-reference)");
        Console.WriteLine();

        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// generate image
    /// </summary>
    /// <param name="prompt">for example, "a white Siamese cat"</param>
    /// <param name="size">256x256, 512x512, or 1024x1024</param>
    /// <returns></returns>
    public async Task<Uri> GenerateImageAsync(string prompt, string size = "1024x1024")
    {
        // content
        var data = new Dictionary<string, string>
        {
            { "prompt", prompt },
            { "n", "1" },
            { "size", size },
        };
        var content = new FormUrlEncodedContent(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/images/generations");
        dynamic response = await targetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             httpContent: content);

        return response["data"][0]["url"];
    }




    // ★★★★★★★★★★★★★★★ 

}
