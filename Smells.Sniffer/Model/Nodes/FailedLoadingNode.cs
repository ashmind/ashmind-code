using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Smells.Sniffer.Model.Nodes {
    public class FailedLoadingNode : CodeTreeNode {
        public FailedLoadingNode(string fileName, Exception ex)
            : base(fileName, NodeIcon.Failed, new[] {new FailedLoadingNode(ex)}) 
        {
        }

        private FailedLoadingNode(Exception ex)
            : base(ex.Message, NodeIcon.Failed, Enumerable.Empty<CodeTreeNode>()) {
        }
    }
}
