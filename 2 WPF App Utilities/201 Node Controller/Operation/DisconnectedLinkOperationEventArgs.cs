﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Operation
{
    public class DisconnectedLinkOperationEventArgs : EventArgs
    {
        public Guid NodeLinkGuid { get; } = Guid.Empty;
        public Guid OutputConnectorGuid { get; } = Guid.Empty;
        public Guid OutputConnectorNodeGuid { get; } = Guid.Empty;
        public Guid InputConnectorGuid { get; } = Guid.Empty;
        public Guid InputConnectorNodeGuid { get; } = Guid.Empty;

        public DisconnectedLinkOperationEventArgs(
            Guid nodeLinkGuid,
            Guid outputConnectorGuid,
            Guid outputConnectorNodeGuid,
            Guid inputConnectorGuid,
            Guid inputConnectorNodeGuid)
        {
            NodeLinkGuid = nodeLinkGuid;
            InputConnectorNodeGuid = inputConnectorNodeGuid;
            InputConnectorGuid = inputConnectorGuid;
            OutputConnectorNodeGuid = outputConnectorNodeGuid;
            OutputConnectorGuid = outputConnectorGuid;
        }
    }
}
