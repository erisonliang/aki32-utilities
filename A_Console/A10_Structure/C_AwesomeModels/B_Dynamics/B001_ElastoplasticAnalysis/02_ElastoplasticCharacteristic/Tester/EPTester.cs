﻿using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public class EPTester
{

    // ★★★★★★★★★★★★★★★ props

    public ElastoplasticCharacteristicBase ep { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public EPTester(ElastoplasticCharacteristicBase ep)
    {
        this.ep = ep;
    }

    public TimeHistory Calc(TestWave waveType)
    {
        TimeHistory wave = waveType switch
        {
            TestWave.TestWave1 => GetTestWave1(),
            _ => throw new NotImplementedException("This wave type is not available"),
        };
        var resultHistory = wave.Clone();
        resultHistory.Name = $"result_{wave.Name}_{ep.GetType().Name}_{GetType().Name}";

        for (int i = 0; i < wave.x.Length; i++)
        {
            resultHistory.f[i] = ep.TryCalcNextF(wave.x[i]);
            ep.AdoptNextPoint();
        }
        return resultHistory;
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// get new test wave
    /// </summary>
    /// <remarks>
    /// recommended setting
    /// K1 = 2 
    /// beta = 0.1
    /// Fy = 80
    /// </remarks>
    /// <returns></returns>
    private static TimeHistory GetTestWave1()
    {
        var th = new TimeHistory() { Name = $"TestWave1" };

        th.x = new double[]
        {
            0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,
            100,98,96,94,92,90,88,86,84,82,80,78,76,74,72,70,68,66,64,62,60,58,56,54,52,50,48,46,44,42,40,38,36,34,32,30,28,26,24,22,20,18,16,14,12,10,8,6,4,2,0,-2,-4,-6,-8,-10,-12,-14,-16,-18,-20,-22,-24,-26,-28,-30,-32,-34,-36,-38,-40,-42,-44,-46,-48,-50,-52,-54,-56,-58,-60,-62,-64,-66,-68,-70,-72,-74,-76,-78,-80,-82,-84,-86,-88,-90,-92,-94,-96,-98,-100,
            -100,-97.5,-95,-92.5,-90,-87.5,-85,-82.5,-80,-77.5,-75,-72.5,-70,-67.5,-65,-62.5,-60,-57.5,-55,-52.5,-50,-47.5,-45,-42.5,-40,-37.5,-35,-32.5,-30,-27.5,-25,-22.5,-20,-17.5,-15,-12.5,-10,-7.5,-5,-2.5,0,2.5,5,7.5,10,12.5,15,17.5,20,22.5,25,27.5,30,32.5,35,37.5,40,42.5,45,47.5,50,52.5,55,57.5,60,62.5,65,67.5,70,72.5,75,77.5,80,82.5,85,87.5,90,92.5,95,97.5,100,102.5,105,107.5,110,112.5,115,117.5,120,122.5,125,127.5,130,132.5,135,137.5,140,142.5,145,147.5,150,
            150,147.5,145,142.5,140,137.5,135,132.5,130,127.5,125,122.5,120,117.5,115,112.5,110,107.5,105,102.5,100,97.5,95,92.5,90,87.5,85,82.5,80,77.5,75,72.5,70,67.5,65,62.5,60,57.5,55,52.5,50,47.5,45,42.5,40,37.5,35,32.5,30,27.5,25,22.5,20,17.5,15,12.5,10,7.5,5,2.5,0,-2.5,-5,-7.5,-10,-12.5,-15,-17.5,-20,-22.5,-25,-27.5,-30,-32.5,-35,-37.5,-40,-42.5,-45,-47.5,-50,-52.5,-55,-57.5,-60,-62.5,-65,-67.5,-70,-72.5,-75,-77.5,-80,-82.5,-85,-87.5,-90,-92.5,-95,-97.5,-100,
            -100,-98,-96,-94,-92,-90,-88,-86,-84,-82,-80,-78,-76,-74,-72,-70,-68,-66,-64,-62,-60,-58,-56,-54,-52,-50,-48,-46,-44,-42,-40,-38,-36,-34,-32,-30,-28,-26,-24,-22,-20,-18,-16,-14,-12,-10,-8,-6,-4,-2,0,2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,74,76,78,80,82,84,86,88,90,92,94,96,98,100,
            100,99.9,99.8,99.7,99.6,99.5,99.4,99.3,99.2,99.1,99,98.9,98.8,98.7,98.6,98.5,98.4,98.3,98.2,98.1,98,97.9,97.8,97.7,97.6,97.5,97.4,97.3,97.2,97.1,97,96.9,96.8,96.7,96.6,96.5,96.4,96.3,96.2,96.1,96,95.9,95.8,95.7,95.6,95.5,95.4,95.3,95.2,95.1,95,94.9,94.8,94.7,94.6,94.5,94.4,94.3,94.2,94.1,94,93.9,93.8,93.7,93.6,93.5,93.4,93.3,93.2,93.1,93,92.9,92.8,92.7,92.6,92.5,92.4,92.3,92.2,92.1,92,91.9,91.8,91.7,91.6,91.5,91.4,91.3,91.2,91.1,91,90.9,90.8,90.7,90.6,90.5,90.4,90.3,90.2,90.1,90,
            90,91.1,92.2,93.3,94.4,95.5,96.6,97.7,98.8,99.9,101,102.1,103.2,104.3,105.4,106.5,107.6,108.7,109.8,110.9,112,113.1,114.2,115.3,116.4,117.5,118.6,119.7,120.8,121.9,123,124.1,125.2,126.3,127.4,128.5,129.6,130.7,131.8,132.9,134,135.1,136.2,137.3,138.4,139.5,140.6,141.7,142.8,143.9,145,146.1,147.2,148.3,149.4,150.5,151.6,152.7,153.8,154.9,156,157.1,158.2,159.3,160.4,161.5,162.6,163.7,164.8,165.9,167,168.1,169.2,170.3,171.4,172.5,173.6,174.7,175.8,176.9,178,179.1,180.2,181.3,182.4,183.5,184.6,185.7,186.8,187.9,189,190.1,191.2,192.3,193.4,194.5,195.6,196.7,197.8,198.9,200,
        };
        return th;
    }

    // ★★★★★★★★★★★★★★★ enums

    public enum TestWave
    {
        TestWave1,
    }


    // ★★★★★★★★★★★★★★★

}
