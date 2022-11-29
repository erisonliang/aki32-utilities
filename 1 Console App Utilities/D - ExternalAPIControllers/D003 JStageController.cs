﻿using System.Collections.Generic;
using System.Net.Http.Headers;

using Aki32_Utilities.UsefulClasses;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Aki32_Utilities.ExternalAPIControllers;

/// <summary>
/// </summary>
/// <remarks>
/// 
/// 参考：
/// https://ooooooha.hatenablog.com/entry/2017/05/03/023516
/// 
/// </remarks>
public partial class JStageController
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }

    public DirectoryInfo DatabaseDirectory => new DirectoryInfo($@"{LocalDirectory.FullName}\DB");




    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do";

    public const string TEST_URL = "http://api.jstage.jst.go.jp/searchapi/do?service=3&pubyearfrom=2022&issn=1881-8153&count=5";


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public JStageController(DirectoryInfo baseDir)
    {
        LocalDirectory = baseDir;

        Console.WriteLine("JStageController Instance Created.");
        Console.WriteLine("Data Powered by J-STAGE (https://www.jstage.jst.go.jp/browse/-char/ja)");

    }


    // ★★★★★★★★★★★★★★★ methods

    public async Task DownloadData()
    {
        var uri = new Uri(TEST_URL);

        var newFileName = $"{uri.Query.Replace("?", "").Replace("&", "_")}.xml";

        var outputFile = new FileInfo($@"{LocalDirectory.FullName}\{newFileName}");

        await uri.DownloadFileAsync(outputFile);

    }

    public string BuildUri()
    {
        var queryList = new Dictionary<string, string>();


        // 巻号取得一覧
        if (true)
        {
            //1   service 必須  利用する機能を指定 巻号一覧取得は2、論文検索結果取得は3を指定
            queryList.Add("service", "3");

            //3   pubyearfrom 任意  発行年を指定(from)    西暦４桁
            queryList.Add("pubyearfrom", "2010");

            //4   pubyearto 任意  発行年を指定(to)  西暦４桁
            queryList.Add("pubyearto", "9999");

            //5   material 任意  資料名の検索語句を指定 完全一致検索
            queryList.Add("material", "XXXXXXXXXXXXXXXXｘ");

            //6   issn 任意  onlineISSN またはPrintISSNを指定  完全一致検索 xxxx-xxxx形式
            queryList.Add("issn", "1881-8161");

            //7   cdjournal 任意  資料コードを指定 J-STAGEで付与される資料を識別するコード
            queryList.Add("cdjournal", "XXXXXXXXXXXXXXXXｘ");

            //8   volorder 任意  巻の並び順を指定    1:昇順, 2:降順, 未指定時は1
            queryList.Add("volorder", "1");

            queryList.Add("start", "2001");
            queryList.Add("count", "1000");

        }

        // 論文検索結果取得
        else
        {
            //1   service 必須  利用する機能を指定 巻号一覧取得は2、論文検索結果取得は3を指定
            queryList.Add("service", "3");

            //3   pubyearfrom 任意  発行年を指定(from)    西暦４桁
            queryList.Add("pubyearfrom", "2010");

            //4   pubyearto 任意  発行年を指定(to)  西暦４桁
            queryList.Add("pubyearto", "9999");

            //5   material 任意  資料名の検索語句を指定 完全一致検索
            queryList.Add("material", "XXXXXXXXXXXXXXXXｘ");

            //6   issn 任意  onlineISSN またはPrintISSNを指定  完全一致検索 xxxx-xxxx形式
            queryList.Add("issn", "1881-8161");

            //7   cdjournal 任意  資料コードを指定 J-STAGEで付与される資料を識別するコード
            queryList.Add("cdjournal", "XXXXXXXXXXXXXXXXｘ");

            //8   volorder 任意  巻の並び順を指定    1:昇順, 2:降順, 未指定時は1
            queryList.Add("volorder", "1");

            queryList.Add("start", "2001");
            queryList.Add("count", "1000");

        }











        return $"{BASE_URL}?{string.Join("&", queryList.Select(x => $"{x.Key}={x.Value}"))}";
    }



    // ★★★★★★★★★★★★★★★

}
