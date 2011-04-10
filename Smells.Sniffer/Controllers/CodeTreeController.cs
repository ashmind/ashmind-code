using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

using AshMind.Code.Analysis;
using AshMind.Code.Smells.Sniffer.Internal;
using AshMind.Code.Smells.Sniffer.Model.Nodes;
using AshMind.Extensions;

namespace AshMind.Code.Smells.Sniffer.Controllers {
    public class CodeTreeController {
        private readonly AnalysisDataResolver analyzer = new AnalysisDataResolver();

        private readonly CollectionPair<CodeTreeNode> rootNodes = new CollectionPair<CodeTreeNode>();

        private ISmell filter;

        private IEnumerable<AnalysisDataNode> GetRootAssemblyNodes() {
            return this.rootNodes.Inner.OfType<AnalysisDataNode>();
        }

        public void SetFilter(ISmell smell) {
            this.filter = smell;
            this.ApplyFilter(this.GetRootAssemblyNodes());
        }

        private void ApplyFilter(IEnumerable<AnalysisDataNode> nodes) {
            var data = this.GetRootAssemblyNodes().Select(node => node.Data).Cast<IAssemblyData>();
            var filtered = this.filter.FindSources(data);

            this.ApplyFilter(nodes, filtered);
        }

        private bool ApplyFilter(IEnumerable<AnalysisDataNode> nodes, HashSet<IMemberData> filtered) {
            var anyNodePasses = false;
            foreach (var node in nodes) {
                anyNodePasses = this.ApplyFilter(node, filtered) || anyNodePasses;
            }

            return anyNodePasses;
        }

        private bool ApplyFilter(AnalysisDataNode node, HashSet<IMemberData> filtered) {
            var passes = this.PassesFilter(node.Data, filtered);
            if (passes)
                node.Annotate(this.filter);
            else
                node.Deannotate<ISmell>();

            if (node.ChildNodes.Loaded) {
                bool childPasses = this.ApplyFilter(node.ChildNodes.Cast<AnalysisDataNode>(), filtered);
                node.Visible = passes || childPasses;
            }
            else {
                node.ChildNodes.LazyForEach(n => this.ApplyFilter((AnalysisDataNode)n, filtered));
                node.Visible = passes || AnyDescendantPassesFilter(node.Data, filtered);
            }

            node.VisibleOnlyBecauseOfChildren = node.Visible && !passes;

            return node.Visible;
        }

        private bool AnyDescendantPassesFilter(IAnalysisData data, HashSet<IMemberData> filtered) {
            return data.Members.Any(member => PassesFilter(member, filtered) || AnyDescendantPassesFilter(member, filtered));
        }

        private bool PassesFilter(IAnalysisData data, HashSet<IMemberData> filtered) {
            var member = data as IMemberData;
            if (member == null)
                return false;

            if (this.filter == null)
                return true;

            return filtered.Contains(member);
        }

        private static AnalysisDataNode ToNode(IAnalysisData data) {
            return new AnalysisDataNode(
                data,
                data.Members.Select(member => ToNode(member))
                            .OrderBy(node => node)
            );
        }

        public ReadOnlyObservableCollection<CodeTreeNode> RootNodes {
            get { return this.rootNodes.ReadOnly; }
        }

        public void LoadAssemblies(string[] paths) {
            var loaded = paths.Select(path => TryLoad(path));
            this.AddAssemblies(loaded.ToArray());
        }

        private CodeTreeNode TryLoad(string path) {
            Assembly assembly;
            try {
                assembly = Assembly.LoadFrom(path);
            }
            catch(BadImageFormatException ex) {
                var fileName = Path.GetFileNameWithoutExtension(path);
                return new FailedLoadingNode(fileName, ex);
            }

            var data = this.analyzer.Resolve(assembly);
            return ToNode(data);
        }

        private void AddAssemblies(params CodeTreeNode[] newNodes) {
            this.rootNodes.Inner.AddRange(newNodes);

            if (this.filter == null)
                return;
            
            this.ApplyFilter(newNodes.OfType<AnalysisDataNode>());
        }
    }
}
