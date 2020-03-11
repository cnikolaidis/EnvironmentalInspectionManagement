namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    public interface IPreferencesViewModel : IBaseViewModel { }

    public class PreferencesViewModel : BaseViewModel, IPreferencesViewModel
    {
        public sealed override void RefreshInputs() { }
    }
}
