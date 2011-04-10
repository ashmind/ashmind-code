using System;
using System.Collections.Generic;
using System.Linq;

using AshMind.Code.Analysis;
using AshMind.Extensions;

namespace AshMind.Code.Smells {
    public abstract class LocalSmell<TMemberData> : ISmell
        where TMemberData : IMemberData
    {
        public abstract bool SpreadsFrom(TMemberData member);
        public abstract object Explain(TMemberData member);

        #region ISmell Members

        public HashSet<IMemberData> FindSources(IEnumerable<IAssemblyData> assemblies) {
            return (
                from assembly in assemblies
                from member in assembly.GetAllMembers().OfType<TMemberData>()
                where this.SpreadsFrom(member)
                select (IMemberData)member
            ).ToSet();
        }

        object ISmell.Explain(IMemberData data) {
            return this.Explain((TMemberData)data);
        }

        #endregion
    }
}
