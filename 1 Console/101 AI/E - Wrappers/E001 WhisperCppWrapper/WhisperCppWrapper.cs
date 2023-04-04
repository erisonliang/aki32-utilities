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

    public async Task<FileInfo> ExecuteWhisper(FileInfo inputAudioFile,
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
        var wavFile = inputAudioFile.GetExtensionChangedFileInfo(".temp.wav");
        inputAudioFile.ConvertAnySoundToWav(wavFile);

        // Run whisper
        {
            string whisperCommand = mainExecuter.FullName;

            string whisperArguments = "";
            whisperArguments += $"-m \"{modelBinary}\"";
            whisperArguments += $" ";
            whisperArguments += $"-t {usingThreadsCount}";
            whisperArguments += $" ";
            whisperArguments += $"-p {usingProcessorsCount}";
            whisperArguments += $" ";
            whisperArguments += $"-o{outputFormat}";
            whisperArguments += $" ";
            whisperArguments += $"-l {targetLanguage}";
            whisperArguments += $" ";
            whisperArguments += $"\"{wavFile}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = whisperCommand,
                    Arguments = whisperArguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    byte[] bytesUTF8 = System.Text.Encoding.Default.GetBytes(e.Data);
                    string stringSJIS = System.Text.Encoding.UTF8.GetString(bytesUTF8);
                    Console.WriteLine(stringSJIS);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    byte[] bytesUTF8 = System.Text.Encoding.Default.GetBytes(e.Data);
                    string stringSJIS = System.Text.Encoding.UTF8.GetString(bytesUTF8);
                    Console.WriteLine(stringSJIS);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
        }

        //出力テキストをリネーム
        var outputFile = wavFile.GetExtensionChangedFileInfo(outputFormat.ToString());
        if (outputFile.Exists)
        {
            string path1 = Path.GetFileNameWithoutExtension(outputFile.FullName);
            string path2 = Path.GetFileNameWithoutExtension(path1);
            string path3 = Path.GetFileNameWithoutExtension(path2);
            var finalFile = outputFile.Directory!.GetChildFileInfo($"{path3}.{outputFormat}");
            outputFile.MoveTo(finalFile, overwriteExistingFile: true);
            outputFile = finalFile;
        }

        //一時的なwavファイルを全部消す
        wavFile.Delete();


        return outputFile;
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