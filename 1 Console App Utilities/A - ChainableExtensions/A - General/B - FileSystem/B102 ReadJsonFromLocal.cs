using System.IO.Compression;

using Newtonsoft.Json;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// read json from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> ReadJsonFromLocalAsync<T>(FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);
        var json = await sr.ReadToEndAsync();

        var data = JsonConvert.DeserializeObject<T>(json);

        return data;
    }

}
