using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OpenCvSharp.Internal;

namespace Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

/// <summary>
/// Mainly, syntax sugar of device IO OS commands.
/// </summary>
public class SystemExtension
{
    // ★★★★★★★★★★★★★★★ for use

    /// <summary>
    /// プロセスの環境変数PATHに、指定されたディレクトリを追加する(パスを通す)。
    /// </summary>
    /// <example>
    /// <code>
    /// 
    /// AddEnvPath("PATH", new string[]
    /// {
    ///    pythonPathEnvVar,
    ///    Path.Combine(pythonPathEnvVar, @"DLLs"),
    /// });
    /// 
    /// </code>
    /// </example>
    /// <param name="paths">PATHに追加するディレクトリ。</param>
    public static void AddEnvPath(string pathName, string[] paths, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        var envPaths = Environment.GetEnvironmentVariable(pathName)?.Split(Path.PathSeparator).ToList() ?? new List<string>();
        foreach (var path in paths)
            if (path.Length > 0 && !envPaths!.Contains(path))
                envPaths.Insert(0, path);

        Environment.SetEnvironmentVariable(pathName, string.Join(Path.PathSeparator.ToString(), envPaths!), target);
    }


    // ★★★★★★★★★★★★★★★ 

}
