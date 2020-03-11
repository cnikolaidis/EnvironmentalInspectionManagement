namespace EnvironmentalInspectionManagement.BusinessLogic.Core
{
    #region Usings
    using System.Windows.Input;
    using System;
    #endregion

    public class DelegateCommand : ICommand
    {
        #region Properties
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;
        #endregion

        #region Constructors
        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Implementations
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
