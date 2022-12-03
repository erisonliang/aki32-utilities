﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeGraph.NET6.Operation
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
