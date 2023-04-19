using System.Numerics;

using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Numerics.IntegralTransforms;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class FFTExecuter
{
    /// <summary>
    /// Fast Fourier Transform
    /// </summary>
    /// <param name="inputData"></param>
    public static async Task<TimeHistory> Execute(double timeStep, double[] inputData)
    {
        if (timeStep == 0)
            throw new InvalidDataException("timeStep must be more than 0.");
        var sampleRate = 1d / timeStep;
        var resultHistory = new TimeHistory();

        await Task.Run(() =>
        {
            // 2の累乗にカットして複素数データに変換
            var cutData = DynamicsUtils.MakePow2Order(inputData, true, out int N);
            var complexInputData = cutData.Select(x => new Complex(x, 0)).ToArray();

            // FFTを実行
            Fourier.Forward(complexInputData, FourierOptions.Default);

            // グラフのデータを変換。
            var freqStep = sampleRate / N;

            for (int i = 0; i < N / 2; i++)
            {
                var step = new TimeHistoryStep();
                var freq = freqStep * i;
                step["Freq"] = freq;
                step["T"] = freq == 0 ? double.PositiveInfinity : 1d / freq;
                step["Amp"] = Math.Pow(complexInputData[i].Magnitude, 2);
                resultHistory.AppendStep(step);
            }

        });

        return resultHistory;
    }
}