﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Newtonsoft.Json;

using OpenCvSharp.LineDescriptor;

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
    public static FileInfo SaveAsJson<T>(this T data, FileInfo outputFile, bool formatted = false)
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
    public static FileInfo SaveAsXml<T>(this T data, FileInfo outputFile)
    {
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);

        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(sw, data);

        return outputFile;
    }

    /// <summary>
    /// write data as csv local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static FileInfo SaveAsCsv<T>(this IEnumerable<T> dataList, FileInfo outputFile, bool withHeader = true)
    {
        var csvGrid = new List<string[]>();
        var props = typeof(T).GetProperties();

        // header
        if (withHeader)
        {
            var csvLine = new List<string>();
            foreach (var prop in props)
                csvLine.Add(prop.Name);

            csvGrid.Add(csvLine.ToArray());
        }

        // contents
        foreach (var dataLine in dataList)
        {
            var csvLine = new List<string>();
            foreach (var prop in props)
            {
                var addingValue = "";

                try
                {
                    var value = prop.GetValue(dataLine);

                    if (value is not string && value is IEnumerable enumProp)
                        addingValue = JsonConvert.SerializeObject(enumProp);
                    else
                        addingValue = value?.ToString() ?? "";

                }
                catch (Exception)
                {
                }
                finally
                {
                    csvLine.Add(addingValue);
                }
            }

            csvGrid.Add(csvLine.ToArray());
        }

        csvGrid.ToArray().SaveCsv_Rows(outputFile);

        return outputFile;
    }


}
