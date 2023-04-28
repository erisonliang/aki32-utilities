

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// hyper parameter
    /// </summary>
    public class HyperParameter
    {

        // ★★★★★★★★★★★★★★★ props

        public string Name { get; set; }

        private double _Value;
        public double Value
        {
            get => _Value;
            set
            {
                if (FixValue)
                    return;
                _Value = MathExtension.Between(MinValue, value, MaxValue);
            }
        }
        public double InitialValue { get; internal set; }
        public bool FixValue { get; set; } = false;
        public double MinValue { get; internal set; }
        public double MaxValue { get; internal set; }


        // ★★★★★★★★★★★★★★★ init

        public HyperParameter(string name, double initialValue, bool fixValue = false, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            InitialValue = Value = initialValue;
            FixValue = fixValue;

            if (FixValue)
                MinValue = MaxValue = Value;

        }


        // ★★★★★★★★★★★★★★★ operator

        public static implicit operator double(HyperParameter d)
        {
            return d.Value;
        }


        // ★★★★★★★★★★★★★★★

    }
}
