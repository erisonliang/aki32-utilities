using System;
using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static class KNetHelper
{

    // ★★★★★★★★★★★★★★★ AccData

    /// <summary>
    /// Row data is from 
    /// <br/>
    /// - https://www.kyoshin.bosai.go.jp/kyoshin/search/
    /// <br/>
    /// - https://www.kyoshin.bosai.go.jp/kyoshin/data/
    /// </summary>
    public class KNetAccData
    {
        public int[] Acc { get; set; }

        public DateTime OriginTime { get; set; }

        public double EQLatitude { get; set; }
        public double EQLongitude { get; set; }
        public int EQDepth_km { get; set; }
        public double EQMagnitude { get; set; }

        public string StationCode { get; set; }
        public double StationLatitude { get; set; }
        public double StationLongitude { get; set; }
        public double StationHeight { get; set; }

        public DateTime RecordTime { get; set; }
        public int SamplingFreq { get; set; }
        public int DurationTime { get; set; }
        public string SampleDirection { get; set; }
        public int Scale_Gal { get; set; }
        public int Scale_Num { get; set; }
        public double MaxAcc { get; set; }
        public DateTime LastCorrection { get; set; }


        public KNetAccData(FileInfo inputFile)
        {
            using var sr = new StreamReader(inputFile.FullName);

            var meta = new List<string>();
            for (int i = 0; i < 17; i++)
                meta.Add(sr.ReadLine()![18..]);

            // metas
            OriginTime = DateTime.Parse(meta[0]);

            EQLatitude = double.Parse(meta[1]);
            EQLongitude = double.Parse(meta[2]);
            EQDepth_km = int.Parse(meta[3]);
            EQMagnitude = double.Parse(meta[4]);

            StationCode = meta[5];
            StationLatitude = double.Parse(meta[6]);
            StationLongitude = double.Parse(meta[7]);
            StationHeight = double.Parse(meta[8]);

            RecordTime = DateTime.Parse(meta[9]);
            SamplingFreq = int.Parse(meta[10].Replace("Hz", ""));
            DurationTime = int.Parse(meta[11]);
            SampleDirection = meta[12];
            var scale = meta[13].Split("(gal)/");
            Scale_Gal = int.Parse(scale[0]);
            Scale_Num = int.Parse(scale[1]);
            MaxAcc = double.Parse(meta[14]);

            LastCorrection = DateTime.Parse(meta[15]);


            // accs
            var acc = new List<int>();
            while (!sr.EndOfStream)
            {
                var newAccs = sr
                    .ReadLine()!
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => int.Parse(a));

                acc.AddRange(newAccs);
            }
            Acc = acc.ToArray();
        
        }

    }



    // ★★★★★★★★★★★★★★★ 

}

