using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AshMind.Code.Smells.Sniffer.Controllers.Commands {
    public abstract class CommandBase<TParameter> : ICommand
        where TParameter : class
    {
        public event EventHandler CanExecuteChanged;

        protected CommandBase() {
            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        void CommandManager_RequerySuggested(object sender, EventArgs e) {
            var handler = this.CanExecuteChanged;
            if (handler != null)
                handler(this, e);
        }

        public abstract bool CanExecute(TParameter parameter);
        public abstract void Execute(TParameter parameter);

        #region ICommand Members

        bool ICommand.CanExecute(object parameter) {
            var cast = parameter as TParameter;
            if (cast == null)
                return false;

            return this.CanExecute(cast);
        }

        void ICommand.Execute(object parameter) {
            this.Execute((TParameter)parameter);
        }

        #endregion
    }
}
