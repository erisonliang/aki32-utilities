﻿

namespace Aki32Utilities.WPFAppUtilities.NodeController.OperationEventArgs;
public class EndMoveNodesOperationEventArgs : EventArgs
{
    public Guid[] NodeGuids { get; } = null;

    public EndMoveNodesOperationEventArgs(Guid[] nodeGuids)
    {
        NodeGuids = nodeGuids;
    }
}
