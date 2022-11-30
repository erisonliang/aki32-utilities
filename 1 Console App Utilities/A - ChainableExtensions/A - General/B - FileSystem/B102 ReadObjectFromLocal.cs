using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// read json from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadJsonFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);
        var json = sr.ReadToEnd();

        var data = JsonConvert.DeserializeObject<T>(json);

        return data;
    }

    /// <summary>
    /// read xml from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadXmlFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);

        var serializer = new XmlSerializer(typeof(T));
        var data = (T)serializer.Deserialize(sr)!;

        return data;
    }

    /// <summary>
    /// read csv from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadCsvFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        throw new NotImplementedException();

        using var sr = new StreamReader(inputFile.FullName);

        var serializer = new XmlSerializer(typeof(T));
        var data = (T)serializer.Deserialize(sr)!;

        return data;
    }

}
