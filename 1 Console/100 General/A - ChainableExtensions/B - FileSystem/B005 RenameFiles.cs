using System.Text;
using System.util;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// automatically rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles(this DirectoryInfo targetDir)
    {
        return targetDir.RenameFiles_AppendAndReplace("*", Array.Empty<(string from, string to)>());
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="newFileNameWithoutExtension">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <param name="replaceSets">
    /// replace designated strings in filenames
    /// If 0-length array was given, replaceSet will be automatically decided.
    /// </param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo inputDir, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(true);
        if (!newFileNameWithoutExtension.Contains('*'))
            throw new InvalidOperationException(@"""newFileNameWithoutExtension"" must contain "" * """);
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();


        // main
        var inputFiles = inputDir.GetFiles();

        if (replaceSets.Length == 0)
        {
            var inputFileNames = inputFiles.Select(x => Path.GetFileNameWithoutExtension(x.Name)).ToArray();
            if (inputFileNames.Length == 0)
                return inputDir;

            var matchF = inputFileNames[0];
            var matchB = inputFileNames[0];

            foreach (var inputFileName in inputFileNames)
            {
                var F = 0;
                for (int i = 0; i < Math.Min(inputFileName.Length, matchF.Length); i++)
                {
                    if (inputFileName[i] != matchF[i])
                        break;
                    F++;
                }
                matchF = inputFileName.Take(F).ToString_Extension();

                var B = 0;
                for (int i = 0; i < Math.Min(inputFileName.Length, matchB.Length); i++)
                {
                    if (inputFileName[^(i + 1)] != matchB[^(i + 1)])
                        break;
                    B++;
                }
                matchB = inputFileName.TakeLast(B).ToString_Extension();
            }

            var replaceSetList = new List<(string from, string to)>();
            if (matchF != "") { replaceSetList.Add((matchF, "")); }
            if (matchB != "") { replaceSetList.Add((matchB, "")); }
            replaceSets = replaceSetList.ToArray();
        }


        inputDir.Loop(inputDir, (inF, _) => inF.RenameFile(newFileNameWithoutExtension, replaceSets));


        // post process
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return inputDir;
    }

    /// <summary>
    /// rename all file names in targetDir
    /// </summary>
    /// <param name="targetDir"></param>
    /// <param name="newFileNameWithoutExtension">with "abc*def", "123" will be "abc123def". must include "*".</param>
    /// <param name="deletingStringSet">delete designated strings from filenames</param>
    /// <returns></returns>
    public static DirectoryInfo RenameFiles_AppendAndReplace(this DirectoryInfo targetDir, string newFileNameWithoutExtension, params string[] deletingStringSet)
    {
        return targetDir.RenameFiles_AppendAndReplace(newFileNameWithoutExtension, deletingStringSet.Select(x => (x, "")).ToArray());
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
    /// <param name="newNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo RenameFile(this FileInfo inputFile, string newNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // main
        var outputFile = inputFile.GetRenamedFileInfo(newNameWithoutExtension, replaceSets);
        inputFile.MoveTo(outputFile);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// rename a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static FileInfo GetRenamedFileInfo(this FileInfo inputFile, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // sugar
        return new FileInfo(GetRenamedPath(inputFile.FullName, newFileNameWithoutExtension, replaceSets));
    }

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
        return new FileInfo(GetExtensionChangedPath(inputFile.FullName, newExtension));
    }

}
