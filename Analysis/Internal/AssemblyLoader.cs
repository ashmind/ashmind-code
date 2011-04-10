using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AshMind.Code.Analysis.Internal {
    internal class AssemblyLoader {
        public IEnumerable<Assembly> LoadReferences(Assembly assembly) {
            return from reference in assembly.GetReferencedAssemblies()
                   select this.LoadReference(reference, assembly);
        }

        private Assembly LoadReference(AssemblyName name, Assembly referencing) {
            var referencingDirectory = Path.GetDirectoryName(referencing.Location);
            var supposedAssemblyPath = Path.Combine(referencingDirectory, name.Name + ".dll");

            var assembly = File.Exists(supposedAssemblyPath)
                               ? Assembly.LoadFrom(supposedAssemblyPath)
                               : Assembly.Load(name.FullName);

            return assembly;
        }
    }
}