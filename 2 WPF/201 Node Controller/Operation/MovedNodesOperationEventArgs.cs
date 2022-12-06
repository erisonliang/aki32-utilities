using System;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Operation;

public class NodesMovedOperationEventArgs : EventArgs
{
    public Guid[] NodeGuids { get; } = null;

    public NodesMovedOperationEventArgs(Guid[] nodeGuids)
    {
        NodeGuids = nodeGuids;
    }
}
