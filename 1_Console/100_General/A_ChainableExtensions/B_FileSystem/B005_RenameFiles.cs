

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// automatically rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles(this DirectoryInfo targetDir, bool recursive = false)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", Array.Empty<(string from, string to)>(), recursive);
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="newFileNameWithoutExtension">Define base file name. "*" represent old name. Must include "*".</param>
    /// <param name="replaceSets">Replace designated strings in FileNames<br/>If nothing was given, replaceSet will be automatically decided.</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo inputDir, string newFileNameWithoutExtension, (string from, string to)[] replaceSets, bool recursive = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);
        if (!newFileNameWithoutExtension.Contains('*'))
            throw new InvalidOperationException(@"""newFileNameWithoutExtension"" must contain "" * """);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();

        using var progress = new ProgressManager(1);
        if (recursive)
            progress.MaxStep = inputDir.GetFiles("*", SearchOption.AllDirectories).Length;
        else
            progress.MaxStep = inputDir.GetFiles("*", SearchOption.TopDirectoryOnly).Length;
        progress.StartAutoWrite(100);


        // main
        void ProcessOne(DirectoryInfo parentDirs)
        {
            if (recursive)
                foreach (var targetDir in parentDirs.GetDirectories())
                    ProcessOne(targetDir);

            var targetFiles = parentDirs.GetFiles();

            if (replaceSets.Length == 0)
            {
                var targetFileNames = targetFiles.Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToArray();
                if (targetFileNames.Length == 0)
                    return;

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
                if (matchF != "")
                { replaceSetList.Add((matchF, "")); }
                if (matchB != "")
                { replaceSetList.Add((matchB, "")); }
                replaceSets = replaceSetList.ToArray();
            }

            foreach (var targetFile in targetFiles)
            {
                try
                {
                    targetFile.RenameFile(newFileNameWithoutExtension, replaceSets);
                }
                catch (Exception ex)
                {
                    progress.AddErrorMessage($"{targetFile.FullName}, {ex.Message}");
                }
                finally
                {
                    progress.CurrentStep++;
                }
            }

        }

        ProcessOne(inputDir);


        // post process
        progress.WriteDone();
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return inputDir;
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="newFileNameWithoutExtension">First, define base file name. "*" represent old name. Must include "*".</param>
    /// <param name="deletingStringSet">delete designated strings from filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo targetDir, string newFileNameWithoutExtension, string[] deletingStringSet, bool recursive = false)
    {
        return targetDir.RenameFiles_AppendAndReplace(newFileNameWithoutExtension, deletingStringSet.Select(x => (x, "")).ToArray(), recursive);
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="replaceSet">replace designated strings in filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Replace(this DirectoryInfo targetDir, (string from, string to)[] replaceSet, bool recursive = false)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", replaceSet, recursive);
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="deletingStringSet">delete designated strings from filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Replace(this DirectoryInfo targetDir, string[] deletingStringSet, bool recursive = false)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", deletingStringSet.Select(x => (x, "")).ToArray(), recursive);
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="pattern">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_Append(this DirectoryInfo targetDir, string pattern, bool recursive = false)
    {
        return targetDir.RenameFiles_AppendAndReplace(pattern, Array.Empty<(string from, string to)>(), recursive);
    }


    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// rename a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="newFileNameWithoutExtension">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo RenameFile(this FileInfo inputFile, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // main
        var outputFile = inputFile.GetRenamedFileInfo(newFileNameWithoutExtension, replaceSets);
        if (inputFile.FullName != outputFile.FullName)
            inputFile.MoveTo(outputFile);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo RenameFile(this FileInfo inputFile, (string from, string to)[] replaceSets)
        => RenameFile(inputFile, "*", replaceSets);

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="newFileNameWithoutExtension">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo GetRenamedFileInfo(this FileInfo inputFile, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
        => new(GetRenamedFilePath(inputFile.FullName, newFileNameWithoutExtension, replaceSets));

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo GetRenamedFileInfo(this FileInfo inputFile, params (string from, string to)[] replaceSets)
        => GetRenamedFileInfo(inputFile, "*", replaceSets);

    /// <summary>
    /// rename a extension
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo GetExtensionChangedFileInfo(this FileInfo inputFile, string newExtension)
    {
        // sugar
        return new FileInfo(GetExtensionChangedFilePath(inputFile.FullName, newExtension));
    }


    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="newFileNameWithoutExtension">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedFilePath(this string inputFilePath, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // main
        var ext = Path.GetExtension(inputFilePath);

        newFileNameWithoutExtension = newFileNameWithoutExtension
            .Replace("*", Path.GetFileNameWithoutExtension(inputFilePath));

        foreach (var (from, to) in replaceSets)
            newFileNameWithoutExtension = newFileNameWithoutExtension.Replace(from, to);

        if (newFileNameWithoutExtension == "")
            newFileNameWithoutExtension = "output";

        var outputFileName = $"{newFileNameWithoutExtension}{ext}";
        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath)!, outputFileName);


        // post process
        return outputFilePath;
    }

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedFilePath(this string inputFilePath, params (string from, string to)[] replaceSets) => GetRenamedFilePath(inputFilePath, "*", replaceSets);

    /// <summary>
    /// rename a file path
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static string GetExtensionChangedFilePath(this string inputFilePath, string newExtension)
    {
        // main
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);
        newExtension = newExtension.TrimStart('.');

        var outputFileName = $"{fileNameWithoutExtension}.{newExtension}";
        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath)!, outputFileName);


        // post process
        return outputFilePath;
    }


    // ★★★★★★★★★★★★★★★ 

}
