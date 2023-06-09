﻿

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// move the entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static DirectoryInfo MoveTo(this DirectoryInfo inputDir, DirectoryInfo outputDir, bool overwriteExistingDir = true)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));
        var outputParentDir = outputDir!.Parent; // outputDir will be created later 
        UtilPreprocessors.PreprocessOutDir(ref outputParentDir, null!);


        // main
        if (inputDir.FullName[0..3] == outputDir.FullName[0..3])
        {
            // use default MoveTo().
            if (!outputDir.Exists)
            {
                inputDir.MoveTo(outputDir.FullName);
            }
            else if (overwriteExistingDir)
            {
                if (outputDir.Exists)
                    outputDir.Delete(true);
                inputDir.MoveTo(outputDir.FullName);
            }
            else
            {
                // move inside files one by one to integrate
                //foreach (var inputFile in inputDir.GetFiles("*", SearchOption.AllDirectories))
                //{
                //    var outputFile = new FileInfo(inputFile.FullName.Replace(inputDir.FullName, outputDir.FullName));
                //    outputFile.Directory!.Create();
                //    inputFile.MoveTo(outputFile);
                //}

                Loop(inputDir, outputDir, (inF, _) =>
                {
                    var outputFile = new FileInfo(inF.FullName.Replace(inputDir.FullName, outputDir.FullName));
                    outputFile.Directory!.Create();
                    inF.MoveTo(outputFile);
                },
                targetFilesOption: SearchOption.AllDirectories
                );

                inputDir.Delete(true);
            }
        }
        else
        {
            // For different drive, we can't use default Move().
            inputDir.CopyTo(outputDir, overwriteExistingDir);
            inputDir.Delete(true);
        }


        // postprocess
        return outputDir!;
    }

    /// <summary>
    /// move a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo MoveTo(this FileInfo inputFile, FileInfo outputFile, bool overwriteExistingFile = true)
    {
        // preprocess
        if (outputFile is null)
            throw new ArgumentNullException(nameof(outputFile));
        UtilPreprocessors.PreprocessOutFile(ref outputFile!, null!, null!,
            deletingExistingOutputFile: false);


        // main
        if (inputFile.FullName[0..3] == outputFile.FullName[0..3])
        {
            // use default. overwrite
            File.Move(inputFile.FullName, outputFile.FullName, overwriteExistingFile);
        }
        else
        {
            // For different drive, we can't use default Move().
            File.Copy(inputFile.FullName, outputFile.FullName, overwriteExistingFile);
            File.Delete(inputFile.FullName);
        }


        return outputFile!;
    }

    /// <summary>
    /// move a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static FileInfo MoveTo(this FileInfo inputFile, DirectoryInfo outputDir)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));


        // main
        var outputFile = outputDir.GetChildFileInfo(inputFile.Name);
        inputFile.MoveTo(outputFile);


        return outputFile!;
    }

}
