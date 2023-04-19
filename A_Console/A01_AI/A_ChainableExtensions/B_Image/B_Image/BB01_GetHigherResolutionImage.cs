using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public static partial class ChainableExtensions
{

    /// <summary>
    ///  GetHigherResolutionImage by CodeFormer
    /// </summary>
    /// <remarks>
    ///参考： http://cedro3.com/ai/codeformer/
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DirectoryInfo AI_GetHigherResolutionImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, double weight = 0.7, bool reCloneRepo = false)
    {
        // preprocess
        if (inputDir.FullName.ContainsJapanese())
            throw new FormatException("Try not to include non-alphanumeric characters in path");
        if (!PythonController.Activated)
            throw new Exception("Required to call PythonController.Initialize() first");
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir!);

        using var prompt = new CommandPromptController()
        {
            RealTimeConsoleWriteLineOutput = true,
            OmitCurrentDirectoryDisplay = false,
        };


        // main
        // install CodeFormer
        if (reCloneRepo && Directory.Exists(SUB_REPO_NAME))
            Directory.Delete(SUB_REPO_NAME);
        if (!Directory.Exists(SUB_REPO_NAME))
        {
            prompt.WriteLine(@"pwd");
            prompt.WriteLine(@$"mkdir {SUB_REPO_NAME}");
            prompt.WriteLine(@$"cd {SUB_REPO_NAME}");
            prompt.WriteLine(@$"git clone https://github.com/cedro3/CodeFormer.git");
            prompt.WriteLine(@$"cd CodeFormer");
            prompt.WriteLine(@$"pip install -r requirements.txt");
            prompt.WriteLine(@$"python basicsr/setup.py develop");
            prompt.WriteLine(@$"python scripts/download_pretrained_models.py facelib");
            prompt.WriteLine(@$"python scripts/download_pretrained_models.py CodeFormer");
            prompt.WriteLine(@$"");
        }
        else
        {
            prompt.WriteLine(@$"cd {SUB_REPO_NAME}");
            prompt.WriteLine(@$"cd CodeFormer");
            prompt.WriteLine(@$"");
        }

        //        PythonController.RunSimpleString(@$"

        //# import
        //import cv2
        //import os
        //import shutil

        //def imread(img_path):
        //  img = cv2.imread(img_path)
        //  img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        //  return img

        //def reset_folder(path):
        //    if os.path.isdir(path):
        //      shutil.rmtree(path)
        //    os.makedirs(path,exist_ok=True)

        //");

        var query = @$"";
        query += @$"python inference_codeformer.py";
        query += @$" --test_path ""{inputDir.FullName}""";
        query += @$" --w {weight:F1}";
        query += @$" --bg_upsampler realesrgan";
        //query += @$" --face_upsample";
        //query += @$" --has_aligned"; // make it square

        prompt.WriteLine(query);
        prompt.WriteLine(@$"mv ""results/{Path.GetFileNameWithoutExtension(inputDir.Name)}_{weight:F1}"" ""{outputDir!.Name}""");
        prompt.WriteLine(@$"mv ""{outputDir!.Name}"" ""{outputDir!.Parent!.FullName}""");
        prompt.WaitForAllProcessFinished();

        return outputDir;
    }

    /// <summary>
    ///  GetHigherResolutionImage by CodeFormer
    /// </summary>
    /// <remarks>
    ///参考： http://cedro3.com/ai/codeformer/
    /// </remarks>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static FileInfo AI_GetHigherResolutionImage(this FileInfo inputFile, FileInfo? outputFile, double weight = 0.7, bool forceClone = false)
    {
        // preprocess
        if (inputFile.FullName.ContainsJapanese())
            throw new FormatException("try not include Japanese characters in path");
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);
        outputFile = outputFile!.GetExtensionChangedFileInfo(".png");
        UtilConfig.StopTemporary_ConsoleOutput_Preprocess();

        var tempDir = inputFile.Directory!.GetChildDirectoryInfo("_temp");
        tempDir.Create();
        inputFile.CopyTo(tempDir);

        // main
        var tempOutputDir = tempDir.AI_GetHigherResolutionImage_Loop(null, weight, forceClone);
        var tempOutputFile = new FileInfo(Path.Combine(tempOutputDir.FullName, "final_results", inputFile.Name)).GetExtensionChangedFileInfo(".png");
        tempOutputFile.MoveTo(outputFile!);


        // post process
        tempDir.Delete(true);
        UtilConfig.TryRestart_ConsoleOutput_Preprocess();
        return outputFile!;
    }

}