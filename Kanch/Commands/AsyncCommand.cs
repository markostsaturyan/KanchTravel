using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanch.Commands
{
    class AsyncCommand<TIn,TOut>:ICommand
    {
        private Func<TIn, Task<TOut>> executeMethod;
        private Func<TIn, bool> canExecuteMethod;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        public AsyncCommand(Func<TIn, Task<TOut>> executeMethod, Func<TIn, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object param)
        {
            return this.canExecuteMethod((TIn)param);
        }

        public async Task<TOut> ExecuteAsync(TIn param)
        {
            return await this.executeMethod(param);
        }

        public async virtual void Execute(object param)
        {
            this.ExecuteAsync((TIn)param);
        }

    }
}
