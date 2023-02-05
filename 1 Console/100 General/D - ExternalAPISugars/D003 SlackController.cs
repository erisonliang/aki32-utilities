﻿using System.Collections.Specialized;
using System.Net;
using System.Text;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class SlackController
{

    // ★★★★★★★★★★★★★★★ props

    /// <summary>
    /// Obtain from https://risaki-masa.com/how-to-get-api-token-in-slack/
    /// </summary>
    public string SlackAccessToken { get; set; } = "";

    private static readonly Uri TargetUri = new("https://slack.com/api/chat.postMessage");


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="slackAccessToken"></param>
    public SlackController(string slackAccessToken)
    {
        SlackAccessToken = slackAccessToken;

        Console.WriteLine("SlackController Instance Created.");
        Console.WriteLine("Data Powered by slack api (https://api.slack.com/apps)");
        Console.WriteLine();
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// send message to LINE
    /// </summary>
    public async Task<bool> SendMessageAsync(string message, string channelName, bool sendAsAUser = true)
    {
        try
        {
            await Task.Run(async () =>
            {
                // content
                var data = new Dictionary<string, string>
                {
                    { "token", SlackAccessToken },
                    { "channel", channelName },
                    { "as_user", sendAsAUser ? "true" : "false" }, // to send this message as the user who owns the token, false by default
                    { "text", message },
                    //{ "attachments", "[{\"fallback\":\"dummy\", \"text\":\"this is an attachment\"}]" },
                };
                var content = new FormUrlEncodedContent(data);

                // request
                await TargetUri.CallAPIAsync_ForJsonData<object>(HttpMethod.Post,
                    httpContent: new FormUrlEncodedContent(data)
                );

                return true;

                // 元々
                {
                    //var data = new NameValueCollection();
                    //data["token"] = slack_token;
                    //data["channel"] = channel_name;
                    //data["as_user"] = "true"; // to send this message as the user who owns the token, false by default
                    //data["text"] = message;
                    ////data["attachments"] = "[{\"fallback\":\"dummy\", \"text\":\"this is an attachment\"}]";

                    //using (var client = new WebClient())
                    //{
                    //    var response = client.UploadValues("https://slack.com/api/chat.postMessage", "POST", data);
                    //    string responseInString = Encoding.UTF8.GetString(response);
                    //}
                }

            });
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }


    // ★★★★★★★★★★★★★★★ 

}
