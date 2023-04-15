using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.Properties;

using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;

using NAudio.Wave;

using System.Diagnostics;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
/// <summary>
/// wrapper of https://github.com/ggerganov/whisper.cpp
/// </summary>
public class WhisperCppWrapper
{
    // ★★★★★★★★★★★★★★★ props

    private readonly DirectoryInfo modelDir;
    private readonly FileInfo mainExecuterFile;
    private readonly FileInfo whisperDllFile;


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="Exception"></exception>
    public WhisperCppWrapper(
        FileInfo designatedMainExecuterFile = null,
        FileInfo designatedWhisperDllFile = null,
        DirectoryInfo designatedModelDir = null
        )
    {
        // re-init
        var stackFrame = new StackFrame(true);
        string currentFilePath = stackFrame.GetFileName()!;
        modelDir = designatedModelDir ?? new FileInfo(currentFilePath).Directory!.GetChildDirectoryInfo("WhisperCpp");

        // read whisper.cpp
        var whisperCppDir = new FileInfo(currentFilePath).Directory!.GetChildDirectoryInfo("WhisperCpp");
        mainExecuterFile = designatedMainExecuterFile ?? whisperCppDir.GetChildFileInfo("main.exe");
        if (!mainExecuterFile.Exists)
            throw new Exception($"※ You need to build this(https://github.com/ggerganov/whisper.cpp) and move \"main.exe\" to \"{mainExecuterFile}\".");

        whisperDllFile = designatedWhisperDllFile ?? whisperCppDir.GetChildFileInfo("whisper.dll");
        if (!whisperDllFile.Exists)
            throw new Exception($"※ You need to build this(https://github.com/ggerganov/whisper.cpp) and move \"whisper.dll\" to \"{whisperDllFile}\".");

    }


    // ★★★★★★★★★★★★★★★ methods

    public FileInfo ExecuteWhisper(FileInfo inputAudioFile,
        FileInfo? outputTextFile = null,
        ModelType usingModel = ModelType.Medium,
        int usingThreadsCount = 4,
        int usingProcessorsCount = 1,
        string targetLanguage = "ja",
        OutputFormat outputFormat = OutputFormat.txt
        )
    {
        UtilPreprocessors.PreprocessBasic();


        // read whisper model
        var modelBinaryFile = usingModel switch
        {
            ModelType.Large => modelDir.GetChildFileInfo("ggml-large.bin"),
            ModelType.Medium => modelDir.GetChildFileInfo("ggml-medium.bin"),
            ModelType.Small => modelDir.GetChildFileInfo("ggml-small.bin"),
            ModelType.Base => modelDir.GetChildFileInfo("ggml-base.bin"),
            ModelType.Tiny => modelDir.GetChildFileInfo("ggml-tiny.bin"),
            _ => throw new Exception("!"),
        };
        if (!modelBinaryFile.Exists)
            throw new Exception($"You need to download whisper models from here (https://huggingface.co/datasets/ggerganov/whisper.cpp/tree/main) and move to \"{modelDir}\".");


        // Convert to WAV format
        var tempWavFile = inputAudioFile.GetExtensionChangedFileInfo(".temp.wav");
        inputAudioFile.ConvertAnySoundToWav(tempWavFile);

        // Run whisper
        {
            using var prompt = new CommandPromptController()
            {
                RealTimeConsoleWriteLineOutput = true,
                OmitCurrentDirectoryDisplay = false,
                ErrorTextForeground = ConsoleColor.Gray,
            };

            string command = "";
            command += $"\"{mainExecuterFile.FullName}\"";
            command += $" ";
            command += $"-m \"{modelBinaryFile}\"";
            command += $" ";
            command += $"-t {usingThreadsCount}";
            command += $" ";
            command += $"-p {usingProcessorsCount}";
            command += $" ";
            command += $"-o{outputFormat}";
            command += $" ";
            command += $"-l {targetLanguage}";
            command += $" ";
            command += $"\"{tempWavFile}\"";

            prompt.WriteLine(command);
            prompt.WaitForAllProcessFinished();
        }

        //出力テキストをリネーム
        var autoOutputFile = new FileInfo($"{tempWavFile}.{outputFormat}");
        if (autoOutputFile.Exists)
        {
            if (outputTextFile is not null)
            {
                autoOutputFile.MoveTo(outputTextFile, overwriteExistingFile: true);
            }
            else
            {
                string path1 = Path.GetFileNameWithoutExtension(autoOutputFile.FullName);
                string path2 = Path.GetFileNameWithoutExtension(path1);
                string path3 = Path.GetFileNameWithoutExtension(path2);
                var finalFile = autoOutputFile.Directory!.GetChildFileInfo($"{path3}{autoOutputFile.Extension}");
                autoOutputFile.MoveTo(finalFile, overwriteExistingFile: true);

                // 最終的な出力で上書き
                outputTextFile = finalFile;
            }
        }

        //一時的なwavファイルを全部消す
        tempWavFile.Delete();

        return outputTextFile!;
    }


    // ★★★★★★★★★★★★★★★ enums

    public enum ModelType
    {
        Large,
        Medium,
        Small,
        Base,
        Tiny,
    }

    public enum OutputFormat
    {
        txt,
        vtt,
        srt,
        wts,
    }


    // ★★★★★★★★★★★★★★★ 

}