using System.IO.Compression;
using System.Text;

using Newtonsoft.Json;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// write data as json local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task<FileInfo> WriteJsonToLocalAsync<T>(this T data, FileInfo outputFile, bool formatted = false)
    {
        var json = JsonConvert.SerializeObject(data, formatted ? Formatting.Indented : Formatting.None);

        using var sr = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        await sr.WriteAsync(json);

        return outputFile;
    }

}
