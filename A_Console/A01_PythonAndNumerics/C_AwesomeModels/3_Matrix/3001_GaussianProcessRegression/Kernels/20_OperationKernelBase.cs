

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination
    /// </summary>
    public abstract class OperationKernelBase : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        internal KernelBase LeftChild { get; set; }
        internal KernelBase RightChild { get; set; }


        // ★★★★★★★★★★★★★★★

    }
}
