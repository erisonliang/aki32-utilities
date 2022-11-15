using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Zip
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_Zip/{inputFile.Name}</param>
    /// <returns></returns>
    public static FileInfo Zip(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        //// preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.zip");
        if (!outputFile!.Name.EndsWith(".zip"))
            throw new Exception("outputFile name must end with .zip");


        throw new NotImplementedException();

        //// main
        //Directory.zip
        //inputDir.Zip();


        // post process
        return outputFile!;
    }

}
