namespace EnvironmentalInspectionManagement.Presentation.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = BusinessLogic.ViewModels.Vm.MainViewModel;
        }
    }
}
