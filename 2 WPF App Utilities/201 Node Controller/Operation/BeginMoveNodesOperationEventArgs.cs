using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Operation
{
    public class BeginMoveNodesOperationEventArgs : EventArgs
    {
        public Guid[] NodeGuids { get; } = null;

        public BeginMoveNodesOperationEventArgs(Guid[] nodeGuids)
        {
            NodeGuids = nodeGuids;
        }
    }
}
