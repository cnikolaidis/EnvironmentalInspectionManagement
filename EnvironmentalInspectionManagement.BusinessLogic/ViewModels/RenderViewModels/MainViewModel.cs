namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Models;
    using System.Windows;
    using System;
    using Core;
    #endregion

    public interface IMainViewModel : IBaseViewModel { }

    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        #region Constructor
        public MainViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        private IBaseViewModel _viewModel;
        public IBaseViewModel ViewModel
        {
            get { return _viewModel; }
            set { SetProperty(ref _viewModel, value); }
        }

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set { SetProperty(ref _welcomeMessage, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand ExitCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(ExitOperation);
            });}
        }

        public DelegateCommand StatisticsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderStatisticsView);
            });}
        }

        public DelegateCommand OrganizationsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderOrganizationsView);
            });}
        }

        public DelegateCommand DocumentsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderDocumentsView);
            });}
        }

        public DelegateCommand PreferencesCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderPreferencesView);
            });}
        }

        public DelegateCommand DictionariesCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderDictionariesView);
            });}
        }

        public DelegateCommand OtherDictionariesCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderOtherDictionariesView);
            });}
        }

        public DelegateCommand UserManagementCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderUserManagementView);
            });}
        }

        public DelegateCommand RestartCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RestartOperation);
            });}
        }

        public DelegateCommand LogViewCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RenderLogMonitoringView);
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeWelcomeMessage()
        {
            var currentPrincipal = System.Threading.Thread.CurrentPrincipal as BasePrincipal;
            if (currentPrincipal == null)
                throw new EiViewException(@"There is an issue processing Application Principal");

            WelcomeMessage = $"Hello {currentPrincipal.Identity.User.FirstName} {currentPrincipal.Identity.User.LastName}! " +
                $"[{currentPrincipal.Identity.User.Username}]";
        }

        private void RenderStatisticsView()
        {
            ViewModel = Vm.StatisticsViewModel;
        }

        private void RenderOrganizationsView()
        {
            ViewModel = Vm.OrganizationsViewModel;
        }

        private void RenderDocumentsView()
        {
            ViewModel = Vm.CasesViewModel;
        }

        private void RenderPreferencesView()
        {
            ViewModel = Vm.PreferencesViewModel;
        }
        
        private void RenderDictionariesView()
        {
            ViewModel = Vm.DictionaryViewModel;
        }

        private void RenderOtherDictionariesView()
        {
            ViewModel = Vm.OtherDictionariesViewModel;
        }

        private void RenderUserManagementView()
        {
            ViewModel = Vm.UserManagementViewModel;
        }

        private void RenderLogMonitoringView()
        {
            ViewModel = Vm.LogMonitoringViewModel;
        }

        private void RestartOperation()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            InvokeOperation(ExitOperation);
        }

        private void ExitOperation()
        {
            Application.Current.Resources.Clear();
            Application.Current.Exit += (s, e) => { GC.Collect(); };
            Application.Current.Shutdown();
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            ViewModel = null;
            InvokeOperation(InitializeWelcomeMessage);
        }
    }
}
