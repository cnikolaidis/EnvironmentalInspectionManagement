namespace EnvironmentalInspectionManagement.Presentation
{
    public partial class LogWindow
    {
        public LogWindow()
        {
            InitializeComponent();
            DataContext = new BusinessLogic.ViewModels.RenderViewModels.LogViewModel(this);
        }

        #region Event Methods
        protected void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
                new Views.MainView().Show();
        }
        #endregion
    }
}
