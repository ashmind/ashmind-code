using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AshMind.Code.Usage.Tests.Of.Strategies.Specific.Web {
    public class GlobalAsaxStub : HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {
        }

        protected void Application_Error(object sender, EventArgs e) {
        }

        protected void Session_Start(object sender, EventArgs e) {
        }
    }
}
