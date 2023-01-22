using System;
using System.IO.Compression;
using System.Text;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ compress

    /// <summary>
    /// compress to .zip
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Compress_Zip(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        //// preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir.Parent!, "output.zip");


        // main
        ZipFile.CreateFromDirectory(inputDir.FullName, outputFile!.FullName);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// compress to .tar
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Compress_Tar(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputDir.Parent!, "output.tar");

        // main
        // TODO: migrate to defaul API https://learn.microsoft.com/ja-jp/dotnet/api/system.formats.tar.tarfile?view=net-7.0
        using var outputStream = new FileStream(outputFile!.FullName, FileMode.Create);
        using var tarArchive = TarArchive.CreateOutputTarArchive(outputStream);
        foreach (var file in inputDir.GetFiles("*", SearchOption.AllDirectories))
        {
            var tarEntry = TarEntry.CreateEntryFromFile(file.FullName);
            tarEntry.Name = Path.GetRelativePath(inputDir.FullName, file.FullName);
            tarArchive.WriteEntry(tarEntry, false);
        }


        // post process
        return outputFile!;
    }

    /// <summary>
    /// compress to .gz
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Compress_Gzip(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        outputFile ??= new FileInfo($"{inputFile.FullName}.gz");
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, "");


        // main
        using var inputStream = new FileStream(inputFile.FullName, FileMode.Open);
        using var outputStream = new FileStream(outputFile!.FullName, FileMode.Create);
        using var gzStream = new GZipStream(outputStream, CompressionMode.Compress);
        inputStream.CopyTo(gzStream);


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ decompress

    /// <summary>
    /// decompress .zip
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Decompress_Zip(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        // preprocess
        outputDir ??= new DirectoryInfo(inputFile.FullName.Replace(Path.GetExtension(inputFile.Name), ""));
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);


        // main
        ZipFile.ExtractToDirectory(inputFile.FullName, outputDir!.FullName, true);


        // post process
        return outputDir!;
    }

    /// <summary>
    /// decompress .tar
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Decompress_Tar(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        // preprocess
        outputDir ??= new DirectoryInfo(inputFile.FullName.Replace(".tar", ""));
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);


        // main
        // TODO: migrate to defaul API https://learn.microsoft.com/ja-jp/dotnet/api/system.formats.tar.tarfile?view=net-7.0
        using var inputStream = new FileStream(inputFile.FullName, FileMode.Open);
        using var tarArchive = TarArchive.CreateInputTarArchive(inputStream, Encoding.UTF8);
        tarArchive.ExtractContents(outputDir!.FullName);


        // post process
        return outputDir!;
    }

    /// <summary>
    /// decompress .gz
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Decompress_Gzip(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        outputFile ??= new FileInfo(inputFile.FullName.Replace(".gz", ""));
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, "");


        // main
        using var inputStream = new FileStream(inputFile.FullName, FileMode.Open);
        using var outputStream = new FileStream(outputFile!.FullName, FileMode.Create);
        using var gzStream = new GZipStream(inputStream, CompressionMode.Decompress);
        gzStream.CopyTo(outputStream);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// decompress .tar.gz
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Decompress_TarGzip(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        // preprocess
        outputDir ??= new DirectoryInfo(inputFile.FullName.Replace(".tar", "").Replace(".gz", ""));
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!);


        // main
        using var inputStream = new FileStream(inputFile.FullName, FileMode.Open);
        using var gzStream = new GZipStream(inputStream, CompressionMode.Decompress);
        using var tarArchive = TarArchive.CreateInputTarArchive(gzStream, Encoding.UTF8);
        tarArchive.ExtractContents(outputDir!.FullName);


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// Unzip_Loop
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Unzip_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
        => inputDir.Loop(outputDir, (inF, _) => inF.Decompress_Zip(null),
            searchRegexen: new string[] { @"^.*\.zip$" });


    // ★★★★★★★★★★★★★★★

}
