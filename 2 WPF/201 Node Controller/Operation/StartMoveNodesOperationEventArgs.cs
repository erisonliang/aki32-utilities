

namespace Aki32Utilities.WPFAppUtilities.NodeController.Operation;
public class StartMoveNodesOperationEventArgs : EventArgs
{
    public Guid[] NodeGuids { get; } = null;

    public StartMoveNodesOperationEventArgs(Guid[] nodeGuids)
    {
        NodeGuids = nodeGuids;
    }
}
