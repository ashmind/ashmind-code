using System;
using System.Linq;
using System.Reflection;

using ClrTest.Reflection;

namespace AshMind.Code.Analysis.Internal.MethodSupport {
    internal class MethodIL {
        private readonly MethodBase method;
        private ILInstruction[] instructions;

        public MethodIL(MethodBase method) {
            this.method = method;
        }

        private void EnsureLoaded() {
            if (this.instructions != null)
                return;

            this.Load();
        }

        private void Load() {
            this.instructions = new ILReader(this.method).ToArray();
        }

        public ILInstruction[] Instructions {
            get {
                this.EnsureLoaded();
                return this.instructions;
            }
        }
    }
}