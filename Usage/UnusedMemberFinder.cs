using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using AshMind.Extensions;

using AshMind.Code.Analysis;
using AshMind.Code.Usage.Assemblies;
using AshMind.Code.Usage.Extensions;
using AshMind.Code.Usage.Strategies;

namespace AshMind.Code.Usage {
    using IMethodSelectionStrategy = ISelectionStrategy<MethodBase>;
        
    internal class UnusedMemberFinder {
        #region SelectionContext Class

        private class SelectionContext : ISelectionContext {
            public SelectionContext(IEnumerable<TypeData> types) {
                this.Types = new HashSet<TypeData>(types);
            }

            public HashSet<TypeData> Types { get; private set; }
        }

        #endregion

        private readonly UnusedMemberFinderConfiguration configuration;

        public UnusedMemberFinder(UnusedMemberFinderConfiguration configuration) {
            this.configuration = configuration;
        }

        [Conditional("DEBUG")]
        private void Trace(string message) {
            Debug.WriteLine(string.Format("[{0:mm:ss.fff}] {1}", DateTime.Now, message));
        }

        public UnusedMemberFinderResult FindIn(PartitionedAssemblySet assemblies) {
            // cries for refactoring)

            this.Trace("root: types");
            var types = assemblies.Roots.SelectMany(a => this.LoadTypes(a)).ToArray();

            var context = new SelectionContext(types);

            this.Trace("root: all members");
            var allMembers = this.LoadMembers(context.Types, context);

            this.Trace("root: used members");
            var usedMembers = this.FindUsedMembers(allMembers, context).ToSet();

            this.Trace("other: types");
            var otherTypes = assemblies.Other.SelectMany(a => this.LoadTypes(a)).ToSet();

            this.Trace("all: types += other types");
            context.Types.UnionWith(otherTypes);

            this.Trace("other: all members");
            var otherMembers = this.LoadMembers(otherTypes, context);

            this.Trace("all: members += other members");
            allMembers.UnionWith(otherMembers);

            this.Trace("other: used members");
            var otherUsedMembers = this.FindUsedMembers(usedMembers, otherMembers, context);

            this.Trace("all: ignored members");
            var ignoredMembers = this.FindIgnored(allMembers, context).ToSet();

            this.Trace("all: unused members");
            // this obviously messes up allMembers variable,
            // but it is so much faster to do it this way
            var unusedMembers = allMembers; 
            unusedMembers.ExceptWith(ignoredMembers);
            unusedMembers.ExceptWith(usedMembers);
            unusedMembers.ExceptWith(otherUsedMembers);

            return new UnusedMemberFinderResult(
                allMembers, unusedMembers, ignoredMembers
            );
        }

        private IEnumerable<IMemberData> FindIgnored(IEnumerable<IMemberData> members, ISelectionContext context) {
            return configuration.Ignored.SelectFrom(members, context);
        }

        internal HashSet<IMemberData> FindUsedMembers(IEnumerable<IMemberData> members, ISelectionContext context) {
            var entryMembers = configuration.Entry.SelectFrom(members, context);
            entryMembers = entryMembers.Union(
                configuration.DefinitelyUsed.SelectFrom(members, context)
            );

            return this.FindUsedMembers(entryMembers, members, context);
        }

        internal HashSet<IMemberData> FindUsedMembers(IEnumerable<IMemberData> rootMembers, IEnumerable<IMemberData> members, ISelectionContext context) {
            var memberSet = members.ToSet();
            var assemblySet = memberSet.Select(m => m.Assembly)
                                       .Distinct()
                                       .ToSet();

            var methodsByBases = members.OfType<MethodData>().SelectMany(
                method => from baseMethod in method.GetOverridesAndImplements()
                          where context.Types.Contains(baseMethod.DeclaringType)
                          select new { 
                              BaseMethod = baseMethod,
                              Method = method 
                          }
            ).ToLookup(
                x => x.BaseMethod,
                x => x.Method
            );

            var usedMembers = rootMembers.Union(
                configuration.DefinitelyUsed.SelectFrom(members, context)
            ).ToArray();

            var collector = new UsedMemberCollector(
                usedMembers, member => assemblySet.Contains(this.GetDeclaringAssembly(member)), methodsByBases
            );
            collector.CollectAll();

            var result = collector.Collected;
            result.IntersectWith(memberSet);

            return result;
        }

        private IAssemblyData GetDeclaringAssembly(IMemberData member) {
            var declaringType = member.DeclaringType;
            if (declaringType == null)
                return member.Assembly;

            return declaringType.Assembly;
        }
        
        internal HashSet<IMemberData> LoadMembers(HashSet<TypeData> types, ISelectionContext context) {
            var actualTypes = types.Select(type => type.Inner).ToSet();
            var members = new HashSet<IMemberData>(
                from type in types
                from member in type.GetAllMembers(false)
                where !(member is TypeData) && actualTypes.Contains(member.Inner.DeclaringType)
                select member
            );

            return members;
        }

        internal IEnumerable<TypeData> LoadTypes(IAssemblyData assembly) {
            return assembly.Types.SelectDescendantsAndSelves(
                type => type.Members.OfType<TypeData>()
            );
        }
    }
}
