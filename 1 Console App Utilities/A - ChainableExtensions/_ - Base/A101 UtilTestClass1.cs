﻿using System.Runtime.CompilerServices;
using Aki32_Utilities.UsefulClasses;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Aki32_Utilities.General;
public class UtilTestClass1
{
    public string StringProp1 { get; set; }
    public string StringProp2 { get; set; }
    public string StringProp3 { get; set; }
    public int IntProp { get; set; }

    public string[] PropArray { get; set; }

    //[JsonIgnore, XmlIgnore]
    //public string PropArrayJsonString
    //{
    //    get
    //    {
    //        return JsonConvert.SerializeObject(PropArray);
    //    }
    //    set
    //    {
    //        PropArray = JsonConvert.DeserializeObject<string[]>(value);
    //    }
    //}

}