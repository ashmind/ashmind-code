using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AshMind.Code.Analysis.Collections;

namespace AshMind.Code.Analysis {
    public class AssemblyData : AnalysisData<TypeData>, IAssemblyData {
        private IAssemblyData[] references;

        internal AssemblyData(Assembly assembly, AnalysisContext context) : base(context) {
            this.Assembly = assembly;
            this.File = new FileInfo(this.Assembly.Location);
        }

        protected override IEnumerable<TypeData> GetMembers() {
            return (
                from type in this.Assembly.GetTypes()
                where !ShouldSkipMember(type)
                let data = this.Context.Resolver.Resolve(type)
                orderby data.Name
                select data
            );
        }

        private bool ShouldSkipMember(Type type) {
            return type.IsNested;
        }

        public ReadOnlyCollection<TypeData> Types {
            get { return this.MembersCore; }
        }
        
        public IAssemblyData[] References {
            get {
                if (this.references == null)
                    this.references = this.GetReferences().ToArray();

                return this.references;
            }
        }

        private IEnumerable<IAssemblyData> GetReferences() {
            return this.Context.AssemblyLoader
                               .LoadReferences(this.Assembly)
                               .Select(a => this.Context.Resolver.Resolve(a));
        }

        public Assembly Assembly { get; private set; }
        public FileInfo File { get; private set; }

        protected override string BuildName() {
            return this.Assembly.GetName().Name;
        }

        public override int GetHashCode() {
            return this.Assembly.GetHashCode();
        }

        public override bool Equals(object obj) {
            var assemblyData = obj as AssemblyData;
            if (assemblyData == null)
                return false;

            return this.Assembly.Equals(assemblyData.Assembly);
        }
    }
}