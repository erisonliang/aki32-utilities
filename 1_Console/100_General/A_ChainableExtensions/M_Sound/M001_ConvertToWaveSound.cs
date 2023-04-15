using MediaToolkit;
using MediaToolkit.Model;

using NAudio.Dmo;
using NAudio.Wave;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// convert sound or video file to wav file.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo ConvertAnySoundToWav(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        static void ConvertSoundToWav(FileInfo inputFile, FileInfo outputFile)
        {
            using var reader = new MediaFoundationReader(inputFile.FullName);
            using var reSampler = new MediaFoundationResampler(reader, new WaveFormat(16000, 1));
            WaveFileWriter.CreateWaveFile(outputFile.FullName, reSampler);
        }

        if (inputFile.Extension.IsMatchAny(GetRegexen_VideoFiles(mp4: true, avi: true)))
        {

            var tempOutputFile = inputFile.GetExtensionChangedFileInfo(".temp.v.wav");

            var inputMediaFile = new MediaFile { Filename = inputFile.FullName };
            var outputMediaFile = new MediaFile { Filename = tempOutputFile.FullName };

            using var engine = new Engine();
            engine.GetMetadata(inputMediaFile);
            engine.Convert(inputMediaFile, outputMediaFile);

            ConvertSoundToWav(tempOutputFile, outputFile!);
            tempOutputFile.Delete();
        }
        else if (inputFile.Extension.IsMatchAny(GetRegexen_SoundFiles(wav: true, mp3: true, m4a: true)))
        {
            ConvertSoundToWav(inputFile, outputFile!);
        }
        else
            throw new Exception("input file extension was out of expectations");


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ 

}
