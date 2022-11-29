using System.Text;

using Newtonsoft.Json;

namespace Aki32_Utilities.UsefulClasses;
public static class DataFileManager
{

    // ★★★★★★★★★★★★★★★ local

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

    /// <summary>
    /// write data as json local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task<FileInfo> WriteJsonToLocalAsync<T>(FileInfo outputFile, T data, bool formatted = false)
    {
        var json = JsonConvert.SerializeObject(data, formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);

        using var sr = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        await sr.WriteAsync(json);

        return outputFile;
    }


    // ★★★★★★★★★★★★★★★ 

}
