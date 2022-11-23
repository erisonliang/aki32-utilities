using MathNet.Numerics.IntegralTransforms;

using System.Numerics;

namespace Aki32_Utilities.OwesomeModels;
public static partial class TimeHistoryExensions
{

    public static async Task<TimeHistory> FFT(this TimeHistory inputHistory, string targetIndex)
    {
        var inputHistoryClone = inputHistory.Clone();
        var resultHistory = inputHistory.Clone();
        resultHistory.Name += "_FFT";
        resultHistory.data = new Dictionary<string, double[]>();

        if (inputHistoryClone.TimeStep == 0)
            throw new InvalidDataException("\"t\" index is required in inputHistory");
        var sampleRate = 1d / inputHistoryClone.TimeStep;

        await Task.Run(() =>
        {
            // 2の累乗にカットして複素数データに変換
            double len = inputHistoryClone.DataRowCount;
            int Np = 0;
            while (len > 1)
            {
                Np++;
                len /= 2;
            }
            Np--;

            if (Np == 0) return;
            int N = (int)Math.Pow(2, Np); // サンプル数

            var complexVoice = inputHistoryClone[targetIndex]
                .Take(N)
                .Select(x => new Complex(x, 0))
                .ToArray();

            // FFTを実行
            Fourier.Forward(complexVoice, FourierOptions.Default);

            // グラフのデータを変換。
            var freqStep = sampleRate / N;

            for (int i = 0; i < N / 2; i++)
            {
                var step = new TimeHistoryStep();
                var freq = freqStep * i;
                step["Freq"] = freq;
                step["T"] = freq == 0 ? double.PositiveInfinity : 1d / freq;
                step["Amp"] = Math.Pow(complexVoice[i].Magnitude, 2);
                resultHistory.AppendStep(step);
            }
        });

        return resultHistory;
    }

}
