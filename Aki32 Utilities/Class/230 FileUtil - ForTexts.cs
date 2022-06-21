using System.Text;

namespace Aki32_Utilities.Class;

public static partial class FileUtil
{

    // ★★★★★★★★★★★★★★★ 231 MergeAllLines

    /// <summary>
    /// merge all file's lines in one file
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_MergeAllLines/output.txt</param>
    /// <param name="skipRowCount"></param>
    /// <returns></returns>
    public static FileInfo MergeAllLines(this DirectoryInfo inputDir, FileInfo outputFile, int skipRowCount = 0)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** MergeAllLines() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputDir.FullName, "output_MergeAllLines", $"output.txt"));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();

        // main
        var files = inputDir.GetFiles("*", SearchOption.TopDirectoryOnly);
        files = files.Where(x => !x.Name.Contains(outputFile.DirectoryName)).ToArray();

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));
        foreach (var f in files)
        {
            try
            {
                var input = File.ReadLines(f.FullName, Encoding.GetEncoding("SHIFT_JIS")).ToArray();
                for (int i = skipRowCount; i < input.Length; i++)
                    sw.WriteLine(input[i]);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {f.FullName}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {f.FullName}, {ex.Message}");
            }
        }

        return outputFile;
    }

    // ★★★★★★★★★★★★★★★ 

}