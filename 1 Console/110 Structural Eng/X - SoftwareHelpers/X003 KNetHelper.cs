using System;
using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public static class KNetHelper
{

    // ★★★★★★★★★★★★★★★ AccData

    public class KNetAccData
    {
        public List<int> Acc { get; set; }

        public DateTime OriginTime { get; set; }
        public double DurationTime { get; set; }

        public KNetAccData(FileInfo inputFile)
        {
            using var sr = new StreamReader(inputFile.FullName);

            var meta = new List<string>();
            for (int i = 0; i < 17; i++)
                meta.Add(sr.ReadLine()![18..]);

            // TODO: read more metas
            OriginTime = DateTime.Parse(meta[0]);
            DurationTime = double.Parse(meta[11]);

            Acc = new List<int>();
            while (!sr.EndOfStream)
            {
                var newAccs = sr
                    .ReadLine()!
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => int.Parse(a));
                
                Acc.AddRange(newAccs);
            }

        }

    }



    // ★★★★★★★★★★★★★★★ 

}

