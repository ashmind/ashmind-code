using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

using AshMind.Code.Analysis;

namespace AshMind.Code.Smells.Sniffer.Model.Nodes {
    [DebuggerDisplay("Data = {Data.Name}, ChildNodes.Loaded = {ChildNodes.Loaded}")]
    public class AnalysisDataNode : CodeTreeNode, IComparable<AnalysisDataNode> {
        private static readonly IDictionary<Type, int> typeSortOrder = new Dictionary<Type, int> {
            { typeof(TypeData), 0 },
            { typeof(EventData), 1 },
            { typeof(PropertyData), 2 },
            { typeof(MethodData), 3 }
        };

        private string sourceCode;

        public AnalysisDataNode(IAnalysisData data, IEnumerable<AnalysisDataNode> childNodes)
            : base(GetText(data), GetIcon(data), childNodes.Cast<CodeTreeNode>())
        {
            this.Data = data;
        }

        public string SourceCode {
            get {
                if (this.sourceCode == null)
                    sourceCode = this.GetSourceCode();

                return sourceCode;
            }
        }

        private string GetSourceCode() {
            var member = this.Data as IMemberData;
            if (member == null)
                return null;

            var document = member.GetSourceDocument();
            if (document == null)
                return null;

            return document.Content;
        }

        private static ImageSource GetIcon(IAnalysisData data) {
            return NodeIcon.SelectFor(data);
        }

        private static string GetText(IAnalysisData data) {
            var method = data as MethodData;
            if (method == null)
                return data.Name;

            var property = method.DeclaringMember as PropertyData;
            if (property != null) {
                if (method == property.Getter)
                    return "get";

                if (method == property.Setter)
                    return "set";
            }

            return data.Name;
        }

        public IAnalysisData Data { get; private set; }

        #region IComparable<AnalysisDataNode> Members

        public int CompareTo(AnalysisDataNode other) {
            var type = this.Data.GetType();
            var otherType = other.Data.GetType();

            if (type == otherType)
                return this.Data.Name.CompareTo(other.Data.Name);

            var sortOrder = this.GetSortOrder(type) ?? int.MaxValue;
            var otherSortOrder = this.GetSortOrder(otherType) ?? int.MaxValue;

            return sortOrder.CompareTo(otherSortOrder);
        }

        private int? GetSortOrder(Type type) {
            int ordinal;
            var succeded = typeSortOrder.TryGetValue(type, out ordinal);

            return succeded ? (int?)ordinal : null;
        }

        #endregion
    }
}