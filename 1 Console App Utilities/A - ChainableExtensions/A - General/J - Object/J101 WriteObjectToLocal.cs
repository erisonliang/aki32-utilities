using System.Text;
using System.Xml.Serialization;

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
    public static FileInfo WriteJsonToLocal<T>(this T data, FileInfo outputFile, bool formatted = false)
    {
        var json = JsonConvert.SerializeObject(data, formatted ? Formatting.Indented : Formatting.None);

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        sw.Write(json);

        return outputFile;
    }

    /// <summary>
    /// write data as xml local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FileInfo WriteXmlToLocal<T>(this T data, FileInfo outputFile, bool formatted = false)
    {
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);

        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(sw, data);

        return outputFile;
    }

}
