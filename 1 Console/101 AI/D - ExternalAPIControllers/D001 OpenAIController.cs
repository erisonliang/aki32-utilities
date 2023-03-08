using System.Net.Http.Json;
using System.Text;
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
    public async Task<string> CallGetModelsAsync()
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
    /// EditText
    /// </summary>
    /// <param name="instruction">The instruction that tells the model how to edit the prompt.</param>
    /// <param name="input">The input text to use as a starting point for the edit.</param>
    /// <param name="model">ID of the model to use. You can use the text-davinci-edit-001 or code-davinci-edit-001. model with this endpoint.</param>
    /// <returns></returns>
    public async Task<string[]> CallEditTextAsync(string instruction, string input = "", string model = "text-davinci-edit-001")
    {
        // content
        var data = new Dictionary<string, object>
        {
            { "model", model},
            { "input", input},
            { "instruction", instruction },
            //{ "n", "1" },
        };
        var jsonContent = JsonConvert.SerializeObject(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/edits");
        JObject response = await targetUri.CallAPIAsync_ForJsonData<JObject>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             jsonStringContent: jsonContent
             );

        return response["choices"]!.Select(x => x!["text"]!.ToString()).ToArray();
        //return response["choices"]![0]!["text"]!.ToString();
    }

    /// <summary>
    /// ChatGPT
    /// </summary>
    /// <param name="input">The input text to use as a starting point for the edit.</param>
    /// <param name="model">ID of the model to use. You can use the "gpt-3.5-turbo" or "gpt-3.5-turbo-0301". model with this endpoint.</param>
    /// <returns></returns>
    public async Task<string[]> CallChatAsync((string role, string message)[] input, string model = "gpt-3.5-turbo", int n = 1)
    {
        // content

        var messages = input.Select(x => new Dictionary<string, string> { { "role", x.role }, { "content", x.message }, });

        var data = new Dictionary<string, object>
        {
            { "model", model},
            { "messages", messages},
            { "n", n },
            { "user", "aki32 utils"},
        };
        var jsonContent = JsonConvert.SerializeObject(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/chat/completions");
        JObject response = await targetUri.CallAPIAsync_ForJsonData<JObject>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             jsonStringContent: jsonContent
             );

        return response["choices"]!.Select(x => x!["message"]!["content"]!.ToString()).ToArray();
    }

    public async Task<string[]> CallChatAsync(string input, string model = "gpt-3.5-turbo") => await CallChatAsync(new[] { ("user", input) }, model);

    /// <summary>
    /// generate image
    /// </summary>
    /// <param name="prompt">for example, "a white Siamese cat"</param>
    /// <param name="size">256x256, 512x512, or 1024x1024</param>
    /// <returns></returns>
    public async Task<Uri[]> CallGenerateImageAsync(string prompt, string size = "1024x1024", int n = 1)
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

    /// <summary>
    /// whisper transcriptions
    /// </summary>
    /// <param name="inputAudioFile">The audio file to translate, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.</param>
    /// <param name="model">ID of the model to use. Only whisper-1 is currently available.</param>
    /// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
    /// <returns></returns>
    public async Task<string> CallAudioTranscriptionsAsync(FileInfo inputAudioFile, string model = "whisper-1", string? language = null)
    {
        // content
        //Path.GetFileName(filePath), "multipart/form-data");
        var httpContent = new MultipartFormDataContent();
        {
            var audioFileContent = new StringContent(model);
            httpContent.Add(audioFileContent, "model");
        }
        {
            if (!string.IsNullOrEmpty(language))
            {
                var audioFileContent = new StringContent(language);
                httpContent.Add(audioFileContent, "language");
            }
        }
        {
            var audioFileContent = new ByteArrayContent(inputAudioFile.ReadAllBytes());
            audioFileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data");
            httpContent.Add(audioFileContent, "file", inputAudioFile.Name);
        }

        // request
        var targetUri = new Uri($"{BaseUriString}/audio/transcriptions");
        JObject response = await targetUri.CallAPIAsync_ForJsonData<JObject>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             httpContent: httpContent
             );

        return response["text"]!.ToString();
    }

    /// <summary>
    /// whisper translations
    /// </summary>
    /// <param name="inputAudioFile">The audio file to translate, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.</param>
    /// <param name="model">ID of the model to use. Only whisper-1 is currently available.</param>
    /// <returns></returns>
    public async Task<string> CallAudioTranslationsAsync(FileInfo inputAudioFile, string model = "whisper-1")
    {
        // content
        //Path.GetFileName(filePath), "multipart/form-data");
        var httpContent = new MultipartFormDataContent();
        {
            var audioFileContent = new StringContent(model);
            httpContent.Add(audioFileContent, "model");
        }
        {
            var audioFileContent = new ByteArrayContent(inputAudioFile.ReadAllBytes());
            audioFileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data");
            httpContent.Add(audioFileContent, "file", inputAudioFile.Name);
        }

        // request
        var targetUri = new Uri($"{BaseUriString}/audio/translations");
        JObject response = await targetUri.CallAPIAsync_ForJsonData<JObject>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             httpContent: httpContent
             );

        return response["text"]!.ToString();
    }


    // ★★★★★★★★★★★★★★★ 

}
