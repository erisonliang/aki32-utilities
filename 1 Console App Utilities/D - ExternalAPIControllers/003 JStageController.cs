using System.Net.Http.Headers;

namespace Aki32_Utilities.ExternalAPIControllers;
public class JStageController
{

    // ★★★★★★★★★★★★★★★ consts

    public string LocalPath { get; set; } = "";

    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do?";

    public const string url_base1 = $@"{BASE_URL}?service=3&pubyearfrom=2010&";
    public const string url_base2 = $@"{BASE_URL}?service=3&pubyearfrom=2010&start=1001&count=1000&";
    public const string url_base3 = $@"{BASE_URL}?service=3&pubyearfrom=2010&start=2001&count=1000&";

    public const string url_structure = $@"issn=1881-8153";
    public const string url_environment = $@"issn=1881-817X";
    public const string url_plan = $@"issn=1881-8161";

    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public JStageController(string localPath)
    {
        LocalPath = localPath;
    }

    // ★★★★★★★★★★★★★★★ methods

    public async Task<string> GetData()
    {

        var url = $@"http://api.jstage.jst.go.jp/searchapi/do?service=3&pubyearfrom=2010&issn=1881-8153";

        try
        {
            using (var client = new HttpClient())
            {
                // 実行
                var result = await client.GetAsync(url);

                return result.Content.ToString();
            }
        }
        catch (Exception ex)
        {
        }

        return null;
    }



    // ★★★★★★★★★★★★★★★

}
