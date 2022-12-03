using System;

namespace Aki32Utilities.WPFAppUtilities.NodeController.OperationEventArgs;

public class BeginMoveNodesOperationEventArgs : EventArgs
{
    public Guid[] NodeGuids { get; } = null;

    public BeginMoveNodesOperationEventArgs(Guid[] nodeGuids)
    {
        NodeGuids = nodeGuids;
    }
}
