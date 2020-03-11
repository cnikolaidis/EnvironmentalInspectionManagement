namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using System.Security.Principal;
    using DataAccessLayer.Services;
    using DataAccessLayer.Models;
    using System.Windows;
    using Utilities;
    using System;
    using Core;
    #endregion

    public interface ILogViewModel : IBaseViewModel { }

    public class LogViewModel : BaseViewModel, ILogViewModel
    {
        #region Properties
        private readonly Window _authWindow;
        #endregion

        #region Constructor
        public LogViewModel(Window window)
        {
            _authWindow = window;
            AppDomain.CurrentDomain.SetThreadPrincipal(new BasePrincipal());
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        }
        #endregion

        #region ViewModel Properties
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _passWord;
        public string PassWord
        {
            get { return _passWord; }
            set { SetProperty(ref _passWord, value); }
        }
        #endregion
        
        #region Commands
        public DelegateCommand LogInCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(LogInOperation);
            });}
        }

        public DelegateCommand ExitCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(ExitOperation);
            });}
        }
        #endregion

        #region Operation Methods
        private void LogInOperation()
        {
            try
            {
                if (UserName.IsEmpty() || PassWord.IsEmpty())
                    throw new EiViewException(@"Υπάρχει άδειο πεδίο");

                var user = Svc.AuthenticationService.AuthenticateUser(UserName, PassWord);
                if (user == null)
                    throw new EiViewException(@"Δεν βρέθηκε ο χρήστης");

                var threadPrincipal = System.Threading.Thread.CurrentPrincipal as BasePrincipal;
                if (threadPrincipal == null)
                    throw new EiViewException(@"Principal was not set");

                threadPrincipal.Identity = new BaseIdentity(user.Username, user);
                _authWindow.Close();
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void ExitOperation()
        {
            _authWindow.Close();
            Application.Current.Resources.Clear();
            Application.Current.Exit += (s, e) => { GC.Collect(); };
            Application.Current.Shutdown();
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            UserName = string.Empty;
            PassWord = string.Empty;
        }
    }
}
