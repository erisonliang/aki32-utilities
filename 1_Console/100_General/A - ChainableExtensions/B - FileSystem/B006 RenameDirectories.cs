

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="newDirName">Define base DirName. "*" represent old name. Must include "*".</param>
    /// <param name="replaceSets">Replace designated strings in DirNames<br/>If nothing was given, replaceSet will be automatically decided.</param>
    /// <returns></returns>
    public static DirectoryInfo RenameDirs(this DirectoryInfo inputDir, string newDirName, (string from, string to)[] replaceSets, bool recursive = false)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();

        using var progress = new ProgressManager(1);
        if (recursive)
            progress.MaxStep = inputDir.GetFiles("*", SearchOption.AllDirectories).Length;
        else
            progress.MaxStep = inputDir.GetFiles("*", SearchOption.TopDirectoryOnly).Length;
        progress.StartAutoWrite(100);


        // main
        void ProcessOne(DirectoryInfo parentDir)
        {
            var targetDirs = parentDir.GetDirectories();
            if (recursive)
                foreach (var targetDir in targetDirs)
                    ProcessOne(targetDir);

            targetDirs = parentDir.GetDirectories();

            //guess
            if (replaceSets.Length == 0)
            {
                var targetDirNames = targetDirs.Select(x => x.Name).ToArray();
                if (targetDirNames.Length == 0)
                    return;

                var matchF = targetDirNames[0];
                var matchB = targetDirNames[0];

                foreach (var targetDirName in targetDirNames)
                {
                    var F = 0;
                    for (int i = 0; i < Math.Min(targetDirName.Length, matchF.Length); i++)
                    {
                        if (targetDirName[i] != matchF[i])
                            break;
                        F++;
                    }
                    matchF = targetDirName.Take(F).ToString_Extension();

                    var B = 0;
                    for (int i = 0; i < Math.Min(targetDirName.Length, matchB.Length); i++)
                    {
                        if (targetDirName[^(i + 1)] != matchB[^(i + 1)])
                            break;
                        B++;
                    }
                    matchB = targetDirName.TakeLast(B).ToString_Extension();
                }

                var replaceSetList = new List<(string from, string to)>();
                if (matchF != "")
                { replaceSetList.Add((matchF, "")); }
                if (matchB != "")
                { replaceSetList.Add((matchB, "")); }
                replaceSets = replaceSetList.ToArray();
            }

            foreach (var targetDir in targetDirs)
            {
                try
                {
                    targetDir.RenameDir(newDirName, replaceSets);
                }
                catch (Exception ex)
                {
                    progress.AddErrorMessage($"{targetDir.FullName}, {ex.Message}");
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


    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// rename a dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="newDirName">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static DirectoryInfo RenameDir(this DirectoryInfo inputDir, string newDirName, (string from, string to)[] replaceSets)
    {
        // main
        var outputDir = inputDir.GetRenamedDirInfo(newDirName, replaceSets);
        inputDir.MoveTo(outputDir);


        // post process
        return outputDir!;
    }

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static DirectoryInfo RenameDir(this DirectoryInfo inputDir, (string from, string to)[] replaceSets)
        => RenameDir(inputDir, "*", replaceSets);

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="newDirName">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static DirectoryInfo GetRenamedDirInfo(this DirectoryInfo inputDir, string newDirName, params (string from, string to)[] replaceSets)
        => new(GetRenamedDirPath(inputDir.FullName, newDirName, replaceSets));

    /// <summary>
    /// rename a file (sugar)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static DirectoryInfo GetRenamedDirInfo(this DirectoryInfo inputDir, params (string from, string to)[] replaceSets)
        => GetRenamedDirInfo(inputDir, "*", replaceSets);


    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputDirPath"></param>
    /// <param name="newDirName">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedDirPath(this string inputDirPath, string newDirName, params (string from, string to)[] replaceSets)
    {
        // main
        newDirName = newDirName.Replace("*", Path.GetFileName(inputDirPath));

        foreach (var (from, to) in replaceSets)
            newDirName = newDirName.Replace(from, to);

        if (newDirName == "")
            newDirName = "output";

        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputDirPath)!, newDirName);


        // post process
        return outputFilePath;
    }

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedDirPath(this string inputPath, params (string from, string to)[] replaceSets)
        => GetRenamedDirPath(inputPath, "*", replaceSets);


    // ★★★★★★★★★★★★★★★

}
