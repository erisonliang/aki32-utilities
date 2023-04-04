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

    private DirectoryInfo modelDir;
    private FileInfo mainExecuter;
    private FileInfo whisperDll;
    private FileInfo modelBinary;


    // ★★★★★★★★★★★★★★★ init

    public WhisperCppWrapper()
    {
        // re-init
        var stackFrame = new StackFrame(true);
        string currentFilePath = stackFrame.GetFileName()!;
        modelDir = new FileInfo(currentFilePath).Directory!.GetChildDirectoryInfo("Models");

        // read whisper.cpp
        mainExecuter = modelDir.GetChildFileInfo("main.exe");
        whisperDll = modelDir.GetChildFileInfo("whisper.dll");
        if (!mainExecuter.Exists || !whisperDll.Exists)
            throw new Exception("※ You need to build this(https://github.com/ggerganov/whisper.cpp) and move \"main.exe\" and \"whisper.dll\" to \"Models\" directory.");

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
        modelBinary = usingModel switch
        {
            ModelType.Large => modelDir.GetChildFileInfo("ggml-large.bin"),
            ModelType.Medium => modelDir.GetChildFileInfo("ggml-medium.bin"),
            ModelType.Small => modelDir.GetChildFileInfo("ggml-small.bin"),
            ModelType.Base => modelDir.GetChildFileInfo("ggml-base.bin"),
            ModelType.Tiny => modelDir.GetChildFileInfo("ggml-tiny.bin"),
            _ => throw new Exception("!"),
        };
        if (!modelBinary.Exists)
            throw new Exception("You need to download whisper model from here (https://huggingface.co/datasets/ggerganov/whisper.cpp/tree/main) and move to \"Models\" directory.");


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
            command += $"\"{mainExecuter.FullName}\"";
            command += $" ";
            command += $"-m \"{modelBinary}\"";
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