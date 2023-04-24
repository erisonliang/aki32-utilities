﻿

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class GaussianProcessRegressionExecuter
{
    /// <summary>
    /// Multiple kernel combination
    /// </summary>
    public abstract class OperationKernelBase : KernelBase
    {

        // ★★★★★★★★★★★★★★★ props

        public KernelBase LeftChild { get; set; }
        public KernelBase RightChild { get; set; }


        // ★★★★★★★★★★★★★★★

    }
}