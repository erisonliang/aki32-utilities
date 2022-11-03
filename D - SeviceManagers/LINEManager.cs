using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Wordprocessing;

using Newtonsoft.Json;

namespace Aki32_Utilities.SeviceManagers;
internal class LINEManager
{
    /// <summary>
    /// Obtain from https://notify-bot.line.me/my/
    /// </summary>
    public string LineAccessToken { get; set; } = "";

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public LINEManager(string lineAccessToken)
    {
        LineAccessToken = lineAccessToken;
    }

    /// <summary>
    /// send message to LINE
    /// </summary>
    internal async Task<bool> SendMessageAsync(string message)
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

}
