using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.MachineLearning;
public static partial class ChainableExtensions
{

    //public static Image CodeFormer()
    //{
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DirectoryInfo ML_GetHigherResolutionImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, double weight = 0.7, bool forceClone = false)
    {
        // preprocess
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
        if (!Directory.Exists(SUB_REPO_NAME) || forceClone)
        {
            prompt.WriteLine(@"pwd");
            prompt.WriteLine(@$"mkdir {SUB_REPO_NAME}");
            prompt.WriteLine(@$"cd {SUB_REPO_NAME}");
            prompt.WriteLine(@"git clone https://github.com/cedro3/CodeFormer.git");
            prompt.WriteLine(@"cd CodeFormer");
            prompt.WriteLine(@"pip install -r requirements.txt");
            prompt.WriteLine(@"python basicsr/setup.py develop");
            prompt.WriteLine(@"python scripts/download_pretrained_models.py facelib");
            prompt.WriteLine(@"python scripts/download_pretrained_models.py CodeFormer");
            prompt.WriteLine(@"");
        }
        else
        {
            prompt.WriteLine(@$"cd {SUB_REPO_NAME}");
            prompt.WriteLine(@"cd CodeFormer");
            prompt.WriteLine(@"");
        }

        PythonController.RunSimpleString(@$"

# import
import cv2
import matplotlib.pyplot as plt
from IPython.display import clear_output
import os
import glob
import shutil


def imread(img_path):
  img = cv2.imread(img_path)
  img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
  return img
 
# display result
def display_result(input_folder, result_folder):
  input_list = sorted(glob.glob(os.path.join(input_folder, '*')))
  for input_path in input_list:
    img_input = imread(input_path)
    basename = os.path.splitext(os.path.basename(input_path))[0]
    output_path = os.path.join(result_folder, basename+'.png')
    img_output = imread(output_path) 
 
def reset_folder(path):
    if os.path.isdir(path):
      shutil.rmtree(path)
    os.makedirs(path,exist_ok=True)

");

        var query = @$"python inference_codeformer.py";
        query += @$" --test_path ""{inputDir.FullName}""";
        query += @$" --w {weight}";
        //query += @$" --bg_upsampler realesrgan";
        //query += @$" --face_upsample";
        //query += @$" --has_aligned"; // make it square
        
        prompt.WriteLine(query);
        prompt.WriteLine(@$"mv ""results/{Path.GetFileNameWithoutExtension(inputDir.Name)}_{weight}"" resultTemp");
        prompt.WriteLine(@$"mv resultTemp ""{outputDir!.FullName}""");
        prompt.WriteLine(@"");

        return outputDir;
    }

}