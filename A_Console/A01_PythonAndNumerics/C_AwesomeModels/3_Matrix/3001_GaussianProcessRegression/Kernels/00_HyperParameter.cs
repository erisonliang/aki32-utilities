

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
                if (IsFixed)
                    return;
                _Value = MathExtension.Between(Minimum, value, Maximum);
            }
        }
        public double InitialValue { get; internal set; }
        public bool IsFixed { get; set; } = false;
        public double Minimum { get; internal set; }
        public double Maximum { get; internal set; }


        // ★★★★★★★★★★★★★★★ init

        public HyperParameter(string name, double initialValue, bool fixValue = false, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            Name = name;
            Minimum = minValue;
            Maximum = maxValue;
            InitialValue = Value = initialValue;
            IsFixed = fixValue;

            if (IsFixed)
                Minimum = Maximum = Value;

        }


        // ★★★★★★★★★★★★★★★ operator

        public static implicit operator double(HyperParameter d)
        {
            return d.Value;
        }


        // ★★★★★★★★★★★★★★★

    }
}
