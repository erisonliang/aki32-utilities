

namespace Aki32Utilities.WPFAppUtilities.NodeController.Operation;
public class EndMoveNodesOperationEventArgs : EventArgs
{
    public Guid[] NodeGuids { get; } = null;

    public EndMoveNodesOperationEventArgs(Guid[] nodeGuids)
    {
        NodeGuids = nodeGuids;
    }
}
