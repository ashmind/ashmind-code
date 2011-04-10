using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using AshMind.Code.Analysis;
using AshMind.Code.Smells.Sniffer.Model.Nodes;

namespace AshMind.Code.Smells.Sniffer.Controllers.Commands {
    public class ExplainCommand : CommandBase<AnalysisDataNode> {
        public override bool CanExecute(AnalysisDataNode node) {
            return node.Annotation<ISmell>() != null;
        }

        public override void Execute(AnalysisDataNode node) {
            var explanation = node.Annotation<ISmell>().Explain((IMemberData)node.Data);
            MessageBox.Show(explanation.ToString());
        }
    }
}
