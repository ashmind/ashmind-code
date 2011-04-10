using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Extensions;
using AshMind.Extensions;

namespace AshMind.Code.Usage.Assemblies {
    internal class AssemblySetInspector {
        public PartitionedAssemblySet Partition(IEnumerable<IAssemblyData> assemblies) {
            var roots = this.SelectRoots(assemblies).ToArray();
            return new PartitionedAssemblySet(roots, assemblies.Except(roots).ToArray());
        }

        private IEnumerable<IAssemblyData> SelectRoots(IEnumerable<IAssemblyData> assemblies) {
            var referenced = assemblies.SelectDescendants(assembly => assembly.References);
            return assemblies.Except(referenced);
        }

        public IEnumerable<IAssemblyData> Filter(IEnumerable<IAssemblyData> assemblies) {
            // I want to exclude *.XmlSerializers.dll -- it has some weird dependencies 
            // that can not be loaded with my default approach. I could have searched for
            // assembly:XmlSerializerVersionAttribute to find out if it is a Serialization
            // assembly.
            //
            // But I can not inspect this assembly with reflection due to dependency problem.
            // So here goes a hack.

            return assemblies.Where(
                a => !a.Name.EndsWith(".XmlSerializers")
            );

            //return from assembly in assemblies
            //       let attributes = CustomAttributeData.GetCustomAttributes(assembly)
            //       where attributes.Any(a => a.Constructor.DeclaringType.AssemblyQualifiedName == XmlSerializerVersionAttributeName)
            //       select assembly;
        }

        public void PreloadReferences(IEnumerable<IAssemblyData> assemblies) {
            //this.PreloadReferences(assemblies, new Stack<IAssemblyData>());
        }

        private void PreloadReferences(IEnumerable<IAssemblyData> assemblies, Stack<IAssemblyData> stack) {
            foreach (var assembly in assemblies) {
                stack.Push(assembly);
                this.PreloadReferences(assembly.References.Except(stack), stack);
                stack.Pop();
            }
        }
    }
}