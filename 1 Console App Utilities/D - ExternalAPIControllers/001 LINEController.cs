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
            using (var client = new HttpClient())
            {
                // 通知するメッセージ
                var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "message", message } });

                // ヘッダーにアクセストークンを追加
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LineAccessToken);

                // 実行
                var result = await client.PostAsync("https://notify-api.line.me/api/notify", content);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;

        //string ConvertEncoding(string src, Encoding destEnc)
        //{
        //    byte[] src_temp = Encoding.GetEncoding("Shift_JIS").GetBytes(src);
        //    byte[] dest_temp = Encoding.Convert(Encoding.ASCII, destEnc, src_temp);
        //    string ret = destEnc.GetString(dest_temp);
        //    return ret;
        //}
    }


    // ★★★★★★★★★★★★★★★ 

}
