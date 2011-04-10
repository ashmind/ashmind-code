using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Switch = AshMind.Constructs.Switch;

using AshMind.Code.Analysis;
using AshMind.Code.Smells.Sniffer.Model;

namespace AshMind.Code.Smells.Sniffer.Controllers.Commands {
    public class GoToReflectorCommand : CommandBase<AnalysisDataNode> {
        public override bool CanExecute(AnalysisDataNode parameter) {
            return true; 
        }

        public override void Execute(AnalysisDataNode parameter) {
            var url = this.BuildUrl(parameter.Data);
            Process.Start(url);
        }

        private string BuildUrl(IAnalysisData data) {
            var path = this.GetUrlPath(data);
            var encoded = Uri.EscapeUriString(path);

            return string.Format("code://{0}", encoded);
        }

        private string GetUrlPath(IAnalysisData data) {
            return Switch.Type(data).To<string>()
                         .Case<AssemblyData>(a => a.Assembly.FullName)
                         .Case<TypeData>(t => t.Inner.AssemblyQualifiedName)
                         .Case<IMemberData>(m => m.Inner.ToString())
                         .Result;
        }
    }
}
