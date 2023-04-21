using System.Diagnostics;

using MathNet.Numerics.LinearAlgebra.Double;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class _360ImageCropperWrapper
{

    // ★★★★★★★★★★★★★★★ props


    // ★★★★★★★★★★★★★★★ init

    public _360ImageCropperWrapper()
    {
        // re-init
        var stackFrame = new StackFrame(true);
        string currentFilePath = stackFrame.GetFileName()!;
        var additionalPath1 = new FileInfo(currentFilePath).Directory!.GetChildDirectoryInfo("_360ImageCropper").FullName;

        if (!PythonController.AdditionalPath.Contains(additionalPath1))
        {
            PythonController.AdditionalPath.Add(additionalPath1);
            PythonController.Initialize(true);
        }
    }

    // ★★★★★★★★★★★★★★★ methods

    public void CropAndSave(FileInfo inputImageFile, FileInfo outputImageFile,
        int outputZoom, int outputWidth, int outputHeight,
        double px, double py, double pz,
        FileInfo? outputJsonInfoFile = null
        )
    {
        outputImageFile.Directory!.Create();

        var E2P = PythonController.Import("Equirec2Perspec");
        var cv2 = PythonController.Import("cv2");

        var equ = E2P.Equirectangular(inputImageFile.FullName);

        var intrinsic = new double[,]
        {
                { outputZoom, 0,  outputWidth},
                { 0, outputZoom, outputHeight},
                { 0,          0,            1},
        };

        var extrinsic = GetRotationMatrix(px, py, pz);

        var img = equ.GetPerspectiveByMat(
            NumpyWrapper.ToCorrect2DNDArray<double>(intrinsic),
            NumpyWrapper.ToCorrect2DNDArray<double>(extrinsic));
        cv2.imwrite(outputImageFile.FullName, img);

        var data = new Dictionary<string, object>
            {
                { "intrinsic", intrinsic},
                { "extrinsic", extrinsic},
            };

        if (outputJsonInfoFile is not null)
            data.SaveAsJson(outputJsonInfoFile);

    }

    public void AutoCropAndSave(FileInfo inputImageFile, DirectoryInfo outputImageDir,
        int outputZoom, int outputWidth, int outputHeight,
        //double px, double py, double pz,
        bool outputJson = false,
        int divCount = 20
        )
    {
        outputImageDir.Create();

        using var progress = new ProgressManager(divCount);
        progress.StartAutoWrite(100);

        for (int i = 0; i < divCount; i++)
        {
            var outputImageFile = outputImageDir.GetChildFileInfo($"{i}.jpg");
            var outputJsonFile = outputJson ? outputImageDir.GetChildFileInfo($"{i}.json") : null;

            CropAndSave(inputImageFile, outputImageFile,
                outputZoom, outputWidth, outputHeight,
                -Math.PI / 4, 0, -2 * Math.PI * i / divCount,
                outputJsonFile
                );

            progress.CurrentStep = i;
        }

        progress.WriteDone();
    }

    private double[,] GetRotationMatrix(double px, double py, double pz)
    {
        var cos = Math.Cos;
        var sin = Math.Sin;

        var Rx = DenseMatrix.OfArray(new double[,] {
                { 1, 0, 0, 0},
                { 0, cos(px), sin(px), 0 },
                { 0, -sin(px), cos(px), 0 },
                { 0, 0, 0, 1},
            });
        var Ry = DenseMatrix.OfArray(new double[,] {
                { cos(py), 0, -sin(py), 0 },
                { 0, 1, 0, 0},
                { sin(py), 0, cos(py), 0 },
                { 0, 0, 0, 1},
            });
        var Rz = DenseMatrix.OfArray(new double[,] {
                { cos(pz), sin(pz), 0, 0 },
                { -sin(pz), cos(pz), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 },
            });

        var R = Rx * Ry * Rz;

        return R.ToArray();
    }


    // ★★★★★★★★★★★★★★★ 

}
