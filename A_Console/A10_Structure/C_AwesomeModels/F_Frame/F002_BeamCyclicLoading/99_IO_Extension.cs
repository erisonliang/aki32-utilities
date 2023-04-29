using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    public static class IO_Extension
    {

        /// <summary>
        /// 1次元のデータリストを読み込みます。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static double[] ReadDataList1D(string path)
        {
            try
            {
                using var sr = new StreamReader(path, Encoding.GetEncoding("UTF-8"));
                var result = sr.ReadToEnd()
                                .Replace("\r", "")
                                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => double.Parse(x))
                                .ToArray();

                return result;
            }
            catch (IOException e)
            {
                Console.WriteLine("※ファイルを読み込めませんでした");
                Console.WriteLine(e.Message);
                return new double[0];
            }
        }

        /// <summary>
        /// 2次元のデータリストを読み込みます。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static double[][] ReadDataList2D(string path)
        {
            try
            {
                using var sr = new StreamReader(path, Encoding.GetEncoding("UTF-8"));
                var data = sr.ReadToEnd()
                                .Replace("\r", "")
                                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                .ToList()
                                .Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                              .ToList()
                                              .Select(y => double.Parse(y))
                                              .ToArray())
                                .ToArray();

                return data;
            }
            catch (IOException e)
            {
                Console.WriteLine("※ファイルを読み込めませんでした。もしくは，データが空です。");
                Console.WriteLine(e.Message);
                return new double[0][];
            }
        }

    }
}
