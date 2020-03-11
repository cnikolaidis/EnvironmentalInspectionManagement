namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Models.OtherObjects;
    using DataAccessLayer.Models.Criterias;
    using System.Collections.ObjectModel;
    using DataAccessLayer.Services;
    using System.Linq;
    #endregion

    public interface IStatisticsViewModel : IBaseViewModel { }

    public class StatisticsViewModel : BaseViewModel, IStatisticsViewModel
    {
        #region Constructor
        public StatisticsViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<ComboBoxItemDto> Years { get; set; }

        private ComboBoxItemDto _yearSelected;
        public ComboBoxItemDto YearSelected
        {
            get { return _yearSelected; }
            set { SetProperty(ref _yearSelected, value); }
        }
        #endregion

        #region Operation Methods
        private void InitializeYears()
        {
            var yearList = Svc.DocumentsService
                .ListByCriteria(new DocumentCriteria())
                .Where(x => x.DateCreated != null)
                .Select(x => x.DateCreated.Value.Year)
                .OrderBy(x => x)
                .Select(x => new ComboBoxItemDto
                {
                    Text = $"{x:D}",
                    Value = x
                })
                .ToList();

            if (Years != null)
            {
                Years.Clear();
                yearList.ForEach(x => Years.Add(x));
            }
            else Years = new ObservableCollection<ComboBoxItemDto>(yearList);
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            InvokeOperation(InitializeYears);
        }
    }
}
