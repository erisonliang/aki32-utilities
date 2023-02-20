using System.Net.Http.Json;
using System.Text.Json.Nodes;

using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Drawing.Charts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class OpenAIController
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Obtain from https://openai.com/api/ .
    /// </summary>
    public string APISecretKey { private get; set; } = "";
    public string APIOrganization { private get; set; } = "org-7murWSIykm6CM3QmaHOotTNg"; // for personal use

    private static readonly string BaseUriString = "https://api.openai.com/v1";
    private List<KeyValuePair<string, string>> Header => new() { new("OpenAI-Organization", APIOrganization) };

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
    public async Task<string> GetModelsAsync()
    {
        // content
        var data = new Dictionary<string, string>
        {
        };
        var content = new FormUrlEncodedContent(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/models");
        var response = await targetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Get,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             httpContent: content);

        return response.ToString();
    }

    /// <summary>
    /// edit text
    /// </summary>
    /// <param name="instruction">The instruction that tells the model how to edit the prompt.</param>
    /// <param name="input">The input text to use as a starting point for the edit.</param>
    /// <param name="model">ID of the model to use. You can use the text-davinci-edit-001 or code-davinci-edit-001. model with this endpoint.</param>
    /// <returns></returns>
    public async Task<object> EditTextAsync(string instruction, string input = "", string model = "text-davinci-edit-001")
    {
        // content
        var data = new Dictionary<string, object>
        {
            { "model", "text-davinci-edit-001"},
            { "input", input},
            { "instruction", instruction },
            //{ "n", "1" },
        };
        var jsonContent = JsonConvert.SerializeObject(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/edits");
        object response = await targetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             jsonStringContent: jsonContent
             );

        return response;
    }

    /// <summary>
    /// generate image
    /// </summary>
    /// <param name="prompt">for example, "a white Siamese cat"</param>
    /// <param name="size">256x256, 512x512, or 1024x1024</param>
    /// <returns></returns>
    public async Task<Uri[]> GenerateImageAsync(string prompt, string size = "1024x1024", int n = 1)
    {
        // content
        var data = new Dictionary<string, object>
        {
            { "prompt", prompt},
            { "n", n },
            { "size", size },
        };
        var jsonContent = JsonConvert.SerializeObject(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/images/generations");
        dynamic response = await targetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             jsonStringContent: jsonContent
             );

        return (response["data"] as JArray)?.Select(x => new Uri(x["url"]?.ToString()!))?.ToArray()!;
    }


    // ★★★★★★★★★★★★★★★ 

}
