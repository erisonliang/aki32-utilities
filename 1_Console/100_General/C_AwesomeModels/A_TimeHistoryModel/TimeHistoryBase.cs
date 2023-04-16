using System.ComponentModel.DataAnnotations;

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Time history data table with its columns and rows dynamically and automatically expands
/// </summary>
/// <remarks>
/// decided not to use IClonable
/// </remarks>
public class TimeHistoryBase
{

    // ★★★★★★★★★★★★★★★ field

    public class PresetIndex
    {
        public const string t = "t";

        public const string x = "x";
        public const string xt = "xt";
        public const string xtt = "xtt";
        public const string y = "y";
        public const string yt = "yt";
        public const string ytt = "ytt";
        public const string xtt_plus_ytt = "xtt+ytt";

        public const string f = "f";

        public const string memo = "memo";
        public const string a = "a";
        public const string v = "v";
        public const string mu = "mu";

        public const string Sd = "Sd";
        public const string Sv = "Sv";
        public const string Sa = "Sa";

    }


    // ★★★★★★★★★★★★★★★ 

}
