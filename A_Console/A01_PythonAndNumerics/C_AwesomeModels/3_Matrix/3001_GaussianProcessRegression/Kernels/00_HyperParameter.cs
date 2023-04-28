

using static Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics.GaussianProcessRegressionExecuter;

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
        public Guid HyperParameterID { get; set; }
        public Guid ParentKernelID { get; set; }

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

        public HyperParameter(string name, double initialValue, Guid parentKernelID, bool fixValue = false, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            HyperParameterID = Guid.NewGuid();
            ParentKernelID = parentKernelID;

            Name = name;
            Minimum = minValue;
            Maximum = maxValue;
            InitialValue = Value = initialValue;
            IsFixed = fixValue;

            if (IsFixed)
                Minimum = Maximum = Value;

        }


        // ★★★★★★★★★★★★★★★ methods

        public override string ToString()
        {
            return $"{Name} ({HyperParameterID.ToString()[^6..]}), {Value:F4}";
        }

        public string ToInitialStateString()
        {
            return $"{Name} ({HyperParameterID.ToString()[^6..]}), {Value:F4}";
        }


        // ★★★★★★★★★★★★★★★ operator

        public static implicit operator double(HyperParameter d)
        {
            return d.Value;
        }


        // ★★★★★★★★★★★★★★★

    }
}
