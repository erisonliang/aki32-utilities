

using System.Xml.Linq;
using System;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices;
using XPlot.Plotly;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Xml;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.ML.AutoML;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class JStage_DOI_ArticleAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://www.jstage.jst.go.jp/search/global/_search/searchdoi/-char/ja?fromPageDoi=&sryCd=&doiJoi=";

    internal Uri builtUri = null;

    public string DOI { get; set; } = "";


    // ★★★★★★★★★★★★★★★ inits

    public JStage_DOI_ArticleAPIAccessor()
    {
    }



    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri()
    {
        // preprocess
        if (string.IsNullOrEmpty(DOI))
            throw new InvalidDataException("DOI property is required to be filled first");


        // post process
        var builtUriString = $"{BASE_URL}{DOI}";
        return builtUri = new Uri(builtUriString);
    }

    /// <summary>
    /// fetch articles
    /// </summary>
    /// <returns></returns>
    public async Task<List<ResearchArticle>> FetchArticles()
    {
        return await Task.Run(() => _FetchArticles().ToList());
    }
    private IEnumerable<ResearchArticle> _FetchArticles()
    {
        // get xml
        var uri = BuildUri();
        using var htmlStream = uri.CallAPIAsync_ForResponseStream(HttpMethod.Get).Result;
        using var htmlStreamReader = new StreamReader(htmlStream);
        var htmlSting = htmlStreamReader.ReadToEnd();
        var html = new HtmlDocument();
        html.LoadHtml(htmlSting);

        // analyse 
        {

            var article = new ResearchArticle();
            {
                article.DataFrom_JStage = true;

                // メタ情報にアクセス！
                var metas = html.DocumentNode.SelectNodes(@"//meta").ToList();
                string? GetMetaValue(string targetName) => metas
                        ?.FirstOrDefault(m => m.Attributes.Any(a => a.Name == "name" && a.Value == targetName))
                        ?.Attributes
                        ?.FirstOrDefault(a => a.Name == "content")
                        ?.Value
                        ;
                string[]? GetMetaValues(string targetName) => metas
                        ?.FirstOrDefault(m => m.Attributes.Any(a => a.Name == "name" && a.Value == targetName))
                        ?.Attributes
                        ?.Where(a => a.Name == "content")
                        ?.Select(a => a.Value)
                        ?.ToArray()
                        ;

                // タイトルはいつもここ
                article.JStage_ArticleTitle_Japanese = GetMetaValue("title");
                article.JStage_ArticleTitle_English = null;

                article.JStage_Authors_Japanese = GetMetaValues("authors");
                article.JStage_Authors_English = null;

                article.JStage_Link_Japanese = uri.AbsoluteUri;
                article.JStage_Link_English = null;

                article.JStage_JournalCode = entity.Element(ExpandAtom("cdjournal"))?.Value;

                article.JStage_MaterialTitle_English = entity.Element(ExpandAtom("material_title"))?.Element(ExpandAtom("en"))?.Value;
                article.JStage_MaterialTitle_Japanese = entity.Element(ExpandAtom("material_title"))?.Element(ExpandAtom("ja"))?.Value;

                article.PrintISSN = GetMetaValue("print_issn");
                article.OnlineISSN = GetMetaValue("online_issn");

                article.JStage_MaterialVolume = GetMetaValue("volume");
                article.JStage_MaterialSubVolume = GetMetaValue("issue");

                article.JStage_Number = GetMetaValue("■■■■■■");
                article.JStage_StartingPage = GetMetaValue("firstpage");
                article.JStage_EndingPage = GetMetaValue("lastpage");

                article.JStage_PublishedYear = entity.Element(ExpandAtom("pubyear"))?.Value;

                article.JStage_JOI = entity.Element(ExpandAtom("joi"))?.Value;
                article.DOI = entity.Element(ExpandPrism("doi"))?.Value;

                article.JStage_SystemCode = entity.Element(ExpandAtom("systemcode"))?.Value;
                article.JStage_SystemName = entity.Element(ExpandAtom("systemname"))?.Value;

                //article.JStage_ArticleTitle = entity.Element(ExpandXml("title"))?.Value;

                //article.JStage_Link = entity.Element(ExpandXml("link"))?.Value;
                article.JStage_Id = entity.Element(ExpandAtom("id"))?.Value;
                article.JStage_UpdatedOn = entity.Element(ExpandAtom("updated"))?.Value;

            }

            yield return article;

        }
    }


    // ★★★★★★★★★★★★★★★ method helper

    private static string ExpandAtom(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;

    private static string ReplaceCharEntities(Match m)
    {
        string s = m.ToString();
        s = s.Substring(1, s.Length - 2);
        if (s[0] == '#')
        {
            if (s[1] == 'x')
            {
                return Char.ConvertFromUtf32(Int32.Parse(s.Substring(2), System.Globalization.NumberStyles.HexNumber));
            }
            return Char.ConvertFromUtf32(Int32.Parse(s.Substring(1)));
        }
        switch (s)
        {
            case "nbsp":
                return "\u00a0";
            case "iexcl":
                return "\u00a1";
            case "cent":
                return "\u00a2";
            case "pound":
                return "\u00a3";
            case "curren":
                return "\u00a4";
            case "yen":
                return "\u00a5";
            case "brvbar":
                return "\u00a6";
            case "sect":
                return "\u00a7";
            case "uml":
                return "\u00a8";
            case "copy":
                return "\u00a9";
            case "ordf":
                return "\u00aa";
            case "laquo":
                return "\u00ab";
            case "not":
                return "\u00ac";
            case "shy":
                return "\u00ad";
            case "reg":
                return "\u00ae";
            case "macr":
                return "\u00af";
            case "deg":
                return "\u00b0";
            case "plusmn":
                return "\u00b1";
            case "sup2":
                return "\u00b2";
            case "sup3":
                return "\u00b3";
            case "acute":
                return "\u00b4";
            case "micro":
                return "\u00b5";
            case "para":
                return "\u00b6";
            case "middot":
                return "\u00b7";
            case "cedil":
                return "\u00b8";
            case "sup1":
                return "\u00b9";
            case "ordm":
                return "\u00ba";
            case "raquo":
                return "\u00bb";
            case "frac14":
                return "\u00bc";
            case "frac12":
                return "\u00bd";
            case "frac34":
                return "\u00be";
            case "iquest":
                return "\u00bf";
            case "Agrave":
                return "\u00c0";
            case "Aacute":
                return "\u00c1";
            case "Acirc":
                return "\u00c2";
            case "Atilde":
                return "\u00c3";
            case "Auml":
                return "\u00c4";
            case "Aring":
                return "\u00c5";
            case "AElig":
                return "\u00c6";
            case "Ccedil":
                return "\u00c7";
            case "Egrave":
                return "\u00c8";
            case "Eacute":
                return "\u00c9";
            case "Ecirc":
                return "\u00ca";
            case "Euml":
                return "\u00cb";
            case "Igrave":
                return "\u00cc";
            case "Iacute":
                return "\u00cd";
            case "Icirc":
                return "\u00ce";
            case "Iuml":
                return "\u00cf";
            case "ETH":
                return "\u00d0";
            case "Ntilde":
                return "\u00d1";
            case "Ograve":
                return "\u00d2";
            case "Oacute":
                return "\u00d3";
            case "Ocirc":
                return "\u00d4";
            case "Otilde":
                return "\u00d5";
            case "Ouml":
                return "\u00d6";
            case "times":
                return "\u00d7";
            case "Oslash":
                return "\u00d8";
            case "Ugrave":
                return "\u00d9";
            case "Uacute":
                return "\u00da";
            case "Ucirc":
                return "\u00db";
            case "Uuml":
                return "\u00dc";
            case "Yacute":
                return "\u00dd";
            case "THORN":
                return "\u00de";
            case "szlig":
                return "\u00df";
            case "agrave":
                return "\u00e0";
            case "aacute":
                return "\u00e1";
            case "acirc":
                return "\u00e2";
            case "atilde":
                return "\u00e3";
            case "auml":
                return "\u00e4";
            case "aring":
                return "\u00e5";
            case "aelig":
                return "\u00e6";
            case "ccedil":
                return "\u00e7";
            case "egrave":
                return "\u00e8";
            case "eacute":
                return "\u00e9";
            case "ecirc":
                return "\u00ea";
            case "euml":
                return "\u00eb";
            case "igrave":
                return "\u00ec";
            case "iacute":
                return "\u00ed";
            case "icirc":
                return "\u00ee";
            case "iuml":
                return "\u00ef";
            case "eth":
                return "\u00f0";
            case "ntilde":
                return "\u00f1";
            case "ograve":
                return "\u00f2";
            case "oacute":
                return "\u00f3";
            case "ocirc":
                return "\u00f4";
            case "otilde":
                return "\u00f5";
            case "ouml":
                return "\u00f6";
            case "divide":
                return "\u00f7";
            case "oslash":
                return "\u00f8";
            case "ugrave":
                return "\u00f9";
            case "uacute":
                return "\u00fa";
            case "ucirc":
                return "\u00fb";
            case "uuml":
                return "\u00fc";
            case "yacute":
                return "\u00fd";
            case "thorn":
                return "\u00fe";
            case "yuml":
                return "\u00ff";
            case "Alpha":
                return "\u0391";
            case "Beta":
                return "\u0392";
            case "Gamma":
                return "\u0393";
            case "Delta":
                return "\u0394";
            case "Epsilon":
                return "\u0395";
            case "Zeta":
                return "\u0396";
            case "Eta":
                return "\u0397";
            case "Theta":
                return "\u0398";
            case "Iota":
                return "\u0399";
            case "Kappa":
                return "\u039a";
            case "Lambda":
                return "\u039b";
            case "Mu":
                return "\u039c";
            case "Nu":
                return "\u039d";
            case "Xi":
                return "\u039e";
            case "Omicron":
                return "\u039f";
            case "Pi":
                return "\u03a0";
            case "Rho":
                return "\u03a1";
            case "Sigma":
                return "\u03a3";
            case "Tau":
                return "\u03a4";
            case "Upsilon":
                return "\u03a5";
            case "Phi":
                return "\u03a6";
            case "Chi":
                return "\u03a7";
            case "Psi":
                return "\u03a8";
            case "Omega":
                return "\u03a9";
            case "alpha":
                return "\u03b1";
            case "beta":
                return "\u03b2";
            case "gamma":
                return "\u03b3";
            case "delta":
                return "\u03b4";
            case "epsilon":
                return "\u03b5";
            case "zeta":
                return "\u03b6";
            case "eta":
                return "\u03b7";
            case "theta":
                return "\u03b8";
            case "iota":
                return "\u03b9";
            case "kappa":
                return "\u03ba";
            case "lambda":
                return "\u03bb";
            case "mu":
                return "\u03bc";
            case "nu":
                return "\u03bd";
            case "xi":
                return "\u03be";
            case "omicron":
                return "\u03bf";
            case "pi":
                return "\u03c0";
            case "rho":
                return "\u03c1";
            case "sigmaf":
                return "\u03c2";
            case "sigma":
                return "\u03c3";
            case "tau":
                return "\u03c4";
            case "upsilon":
                return "\u03c5";
            case "phi":
                return "\u03c6";
            case "chi":
                return "\u03c7";
            case "psi":
                return "\u03c8";
            case "omega":
                return "\u03c9";
            case "&thetasym;":
                return "\u03d1";
            case "upsih":
                return "\u03d2";
            case "piv":
                return "\u03d6";
            case "bull":
                return "\u2022";
            case "hellip":
                return "\u2026";
            case "prime":
                return "\u2032";
            case "Prime":
                return "\u2033";
            case "oline":
                return "\u203e";
            case "frasl":
                return "\u2044";
            case "weierp":
                return "\u2118";
            case "image":
                return "\u2111";
            case "real":
                return "\u211c";
            case "trade":
                return "\u2122";
            case "alefsym":
                return "\u2135";
            case "larr":
                return "\u2190";
            case "uarr":
                return "\u2191";
            case "rarr":
                return "\u2192";
            case "darr":
                return "\u2193";
            case "harr":
                return "\u2194";
            case "crarr":
                return "\u21b5";
            case "lArr":
                return "\u21d0";
            case "uArr":
                return "\u21d1";
            case "rArr":
                return "\u21d2";
            case "dArr":
                return "\u21d3";
            case "hArr":
                return "\u21d4";
            case "forall":
                return "\u2200";
            case "part":
                return "\u2202";
            case "exist":
                return "\u2203";
            case "empty":
                return "\u2205";
            case "nabla":
                return "\u2207";
            case "isin":
                return "\u2208";
            case "notin":
                return "\u2209";
            case "ni":
                return "\u220b";
            case "prod":
                return "\u220f";
            case "sum":
                return "\u2211";
            case "minus":
                return "\u2212";
            case "lowast":
                return "\u2217";
            case "radic":
                return "\u221a";
            case "prop":
                return "\u221d";
            case "infin":
                return "\u221e";
            case "ang":
                return "\u2220";
            case "and":
                return "\u2227";
            case "or":
                return "\u2228";
            case "cap":
                return "\u2229";
            case "cup":
                return "\u222a";
            case "int":
                return "\u222b";
            case "there4":
                return "\u2234";
            case "sim":
                return "\u223c";
            case "cong":
                return "\u2245";
            case "asymp":
                return "\u2248";
            case "ne":
                return "\u2260";
            case "equiv":
                return "\u2261";
            case "le":
                return "\u2264";
            case "ge":
                return "\u2265";
            case "sub":
                return "\u2282";
            case "sup":
                return "\u2283";
            case "nsub":
                return "\u2284";
            case "sube":
                return "\u2286";
            case "supe":
                return "\u2287";
            case "oplus":
                return "\u2295";
            case "otimes":
                return "\u2297";
            case "perp":
                return "\u22a5";
            case "sdot":
                return "\u22c5";
            case "lceil":
                return "\u2308";
            case "rceil":
                return "\u2309";
            case "lfloor":
                return "\u230a";
            case "rfloor":
                return "\u230b";
            case "lang":
                return "\u2329";
            case "rang":
                return "\u232a";
            case "loz":
                return "\u25ca";
            case "spades":
                return "\u2660";
            case "clubs":
                return "\u2663";
            case "hearts":
                return "\u2665";
            case "diams":
                return "\u2666";
            case "OElig":
                return "\u0152";
            case "oelig":
                return "\u0153";
            case "Scaron":
                return "\u0160";
            case "scaron":
                return "\u0161";
            case "Yuml":
                return "\u0178";
            case "circ":
                return "\u02c6";
            case "tilde":
                return "\u02dc";
            case "ensp":
                return "\u2002";
            case "emsp":
                return "\u2003";
            case "thinsp":
                return "\u2009";
            case "zwnj":
                return "\u200c";
            case "zwj":
                return "\u200d";
            case "lrm":
                return "\u200e";
            case "rlm":
                return "\u200f";
            case "ndash":
                return "\u2013";
            case "mdash":
                return "\u2014";
            case "lsquo":
                return "\u2018";
            case "rsquo":
                return "\u2019";
            case "sbquo":
                return "\u201a";
            case "ldquo":
                return "\u201c";
            case "rdquo":
                return "\u201d";
            case "bdquo":
                return "\u201e";
            case "dagger":
                return "\u2020";
            case "Dagger":
                return "\u2021";
            case "permil":
                return "\u2030";
            case "lsaquo":
                return "\u2039";
            case "rsaquo":
                return "\u203a";
            case "euro":
                return "\u20ac";
            default:
                return s;
        }
    }

    // ★★★★★★★★★★★★★★★ 

}
