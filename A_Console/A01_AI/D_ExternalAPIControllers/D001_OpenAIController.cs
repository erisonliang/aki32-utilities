using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
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

    const double yenPerDollar = 130;
    public double lastPriceInYen = -1;


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
    public async Task<string?> CallGetModelsAsync()
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

        lastPriceInYen = -1;

        return response?.ToString();
    }

    /// <summary>
    /// EditText
    /// </summary>
    /// <param name="instruction">The instruction that tells the model how to edit the prompt.</param>
    /// <param name="input">The input text to use as a starting point for the edit.</param>
    /// <param name="model">ID of the model to use. You can use the text-davinci-edit-001 or code-davinci-edit-001. model with this endpoint.</param>
    /// <returns></returns>
    public async Task<string[]> CallEditTextAsync(string instruction,
        string input = "",
        EditTextModel model = EditTextModel.DavinciEdit001)
    {
        // model
        var modelString = model switch
        {
            EditTextModel.DavinciEdit001 => "text-davinci-edit-001",
            _ => throw new NotImplementedException("Selected model is not implemented."),
        };

        // content
        var data = new Dictionary<string, object>
        {
            { "model", modelString},
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

        lastPriceInYen = -1;

        return response["choices"]!.Select(x => x!["text"]!.ToString()).ToArray();
        //return response["choices"]![0]!["text"]!.ToString();
    }
    public enum EditTextModel
    {
        DavinciEdit001,
    }

    /// <summary>
    /// ChatGPT
    /// </summary>
    /// <param name="input">The input text to use as a starting point for the edit.</param>
    /// <param name="model">ID of the model to use. You can use the "gpt-3.5-turbo" or "gpt-3.5-turbo-0301". model with this endpoint.</param>
    /// <returns></returns>
    public async Task<string[]> CallChatAsync((string role, string message)[] input,
        ChatModel model = ChatModel.GPT35Turbo,
        int n = 1)
    {
        // model
        var modelString = model switch
        {
            ChatModel.GPT35Turbo => "gpt-3.5-turbo",
            _ => throw new NotImplementedException("Selected model is not implemented."),
        };

        // content
        var messages = input.Select(x => new Dictionary<string, string> { { "role", x.role }, { "content", x.message }, });

        var data = new Dictionary<string, object>
        {
            { "model", modelString},
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

        try
        {
            lastPriceInYen = -1;
            var consumedToken = int.Parse(response["usage"]!["total_tokens"]!.ToString());
            var dollarPerToken = 0.002 / 1000;
            lastPriceInYen = consumedToken * dollarPerToken * yenPerDollar;
        }
        catch (Exception)
        {
        }

        return response["choices"]!.Select(x => x!["message"]!["content"]!.ToString()).ToArray();
    }
    public async Task<string[]> CallChatAsync(string input, ChatModel model = ChatModel.GPT35Turbo) => await CallChatAsync(new[] { ("user", input) }, model);
    public enum ChatModel
    {
        GPT35Turbo,
    }

    /// <summary>
    /// generate image
    /// </summary>
    /// <param name="prompt">for example, "a white Siamese cat"</param>
    /// <param name="size">256x256, 512x512, or 1024x1024</param>
    /// <returns></returns>
    public async Task<Uri[]> CallGenerateImageAsync(string prompt, ImageSize size = ImageSize.W1024xH1024, int n = 1)
    {
        // size
        var sizeString = size switch
        {
            ImageSize.W1024xH1024 => "1024x1024",
            ImageSize.W512xH512 => "512x512",
            ImageSize.W256xH256 => "256x256",
            _ => throw new NotImplementedException("Selected size is not implemented."),
        };

        // content
        var data = new Dictionary<string, object>
        {
            { "prompt", prompt},
            { "n", n },
            { "size", sizeString },
        };
        var jsonContent = JsonConvert.SerializeObject(data);

        // request
        var targetUri = new Uri($"{BaseUriString}/images/generations");
        dynamic response = await targetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
             authBearerToken: APISecretKey,
             additionalHeaders: Header,
             jsonStringContent: jsonContent
             );

        lastPriceInYen = -1;

        return (response["data"] as JArray)?.Select(x => new Uri(x["url"]?.ToString()!))?.ToArray()!;
    }
    public enum ImageSize
    {
        W1024xH1024,
        W512xH512,
        W256xH256,
    }

    /// <summary>
    /// whisper transcriptions
    /// </summary>
    /// <param name="inputAudioFile">The audio file to translate, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.</param>
    /// <param name="model">ID of the model to use. Only whisper-1 is currently available.</param>
    /// <param name="language">The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.</param>
    /// <returns></returns>
    public async Task<string> CallAudioTranscriptionsAsync(FileInfo inputAudioFile,
        AudioModel model = AudioModel.Whisper1,
        string? language = null)
    {
        // model
        var modelString = model switch
        {
            AudioModel.Whisper1 => "whisper-1",
            _ => throw new NotImplementedException("Selected model is not implemented."),
        };

        // content
        //Path.GetFileName(filePath), "multipart/form-data");
        var httpContent = new MultipartFormDataContent();
        {
            var audioFileContent = new StringContent(modelString);
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

        lastPriceInYen = -1;

        return response["text"]!.ToString();
    }

    /// <summary>
    /// whisper translations
    /// </summary>
    /// <param name="inputAudioFile">The audio file to translate, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.</param>
    /// <param name="model">ID of the model to use. Only whisper-1 is currently available.</param>
    /// <returns></returns>
    public async Task<string> CallAudioTranslationsAsync(FileInfo inputAudioFile, AudioModel model = AudioModel.Whisper1)
    {
        // model
        var modelString = model switch
        {
            AudioModel.Whisper1 => "whisper-1",
            _ => throw new NotImplementedException("Selected model is not implemented."),
        };

        // content
        //Path.GetFileName(filePath), "multipart/form-data");
        var httpContent = new MultipartFormDataContent();
        {
            var audioFileContent = new StringContent(modelString);
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

        lastPriceInYen = -1;

        return response["text"]!.ToString();
    }
    public enum AudioModel
    {
        Whisper1,
    }


    // ★★★★★★★★★★★★★★★ 

}
