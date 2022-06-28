using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// automatically rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles(this DirectoryInfo targetDir)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", new (string from, string to)[] { });
    }
  
    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="pattern">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <param name="replaceSet">
    /// replace designated strings in filenames
    /// If 0-length array was given, replaceSet will be automatically decided.
    /// </param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo targetDir, string pattern, params (string from, string to)[] replaceSet)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** RenameFiles() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (!pattern.Contains("*"))
            throw new InvalidOperationException("\"pattern\" must contain \"*\"");


        // main
        var targetFiles = targetDir.GetFiles();

        if (replaceSet.Length == 0)
        {
            var targetFileNames = targetFiles.Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToArray();
            var matchF = targetFileNames[0];
            var matchB = targetFileNames[0];

            foreach (var targetFileName in targetFileNames)
            {
                var F = 0;
                for (int i = 0; i < Math.Min(targetFileName.Length, matchF.Length); i++)
                {
                    if (targetFileName[i] != matchF[i])
                        break;
                    F++;
                }
                matchF = targetFileName.Take(F).ToString_Extension();

                var B = 0;
                for (int i = 0; i < Math.Min(targetFileName.Length, matchB.Length); i++)
                {
                    if (targetFileName[^(i + 1)] != matchB[^(i + 1)])
                        break;
                    B++;
                }
                matchB = targetFileName.TakeLast(B).ToString_Extension();
            }

            var replaceSetList = new List<(string from, string to)>();
            if (matchF != "") { replaceSetList.Add((matchF, "")); }
            if (matchB != "") { replaceSetList.Add((matchB, "")); }
            replaceSet = replaceSetList.ToArray();
        }

        foreach (var targetFile in targetFiles)
        {
            var newFileName = Path.GetFileNameWithoutExtension(targetFile.Name);
            foreach (var item in replaceSet)
                newFileName = newFileName.Replace(item.from, item.to);
            newFileName = pattern.Replace("*", newFileName);
            var newFilePath = Path.Combine(targetFile.DirectoryName, newFileName + Path.GetExtension(targetFile.Name));

            try
            {
                targetFile.MoveTo(newFilePath);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {targetFile.FullName}, {ex.Message}");
            }
        }


        // postprocess
        return targetDir;
    }
    
    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="pattern">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <param name="deletingStringSet">delete designated strings from filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo targetDir, string pattern, params string[] deletingStringSet)
    {
        return targetDir.RenameFiles_AppendAndReplace(pattern, deletingStringSet.Select(x => (x, "")).ToArray());
    }
    
    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="replaceSet">replace designated strings in filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Replace(this DirectoryInfo targetDir, params (string from, string to)[] replaceSet)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", replaceSet);
    }
    
    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="deletingStringSet">delete designated strings from filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Replace(this DirectoryInfo targetDir, params string[] deletingStringSet)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", deletingStringSet.Select(x => (x, "")).ToArray());
    }
    
    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="pattern">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Append(this DirectoryInfo targetDir, string pattern)
    {
        return targetDir.RenameFiles_AppendAndReplace(pattern, new (string from, string to)[] { (" ", " ") });
    }
    
    /// <summary>
    /// rename a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo RenameFile(this FileInfo inputFile, string newNameWithoutExtension)
    {
        // main
        var ext = Path.GetExtension(inputFile.Name);
        var outputFileName = $"{newNameWithoutExtension}{ext}";
        var outputFilePath = Path.Combine(inputFile.DirectoryName, outputFileName);
        var outputFile = new FileInfo(outputFilePath);
        inputFile.MoveTo(outputFile);


        // post process
        return outputFile;
    }

}
