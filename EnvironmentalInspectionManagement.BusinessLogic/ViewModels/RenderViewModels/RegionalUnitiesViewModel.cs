namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Services.EntityServices;
    using DataAccessLayer.Models.OtherObjects;
    using DataAccessLayer.Models.Criterias;
    using System.Collections.ObjectModel;
    using DataAccessLayer.Models.Dtos;
    using System.Collections.Generic;
    using DataAccessLayer.Services;
    using System.Linq;
    using Utilities;
    using Core;
    #endregion

    public interface IRegionalUnitiesViewModel : IBaseViewModel { }

    public class RegionalUnitiesViewModel : BaseViewModel, IRegionalUnitiesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<RegionalUnityDto> _regionalUnityItems;
        #endregion

        #region Constructor
        public RegionalUnitiesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<RegionalUnityDto> RegionalUnities { get; set; }
        public ObservableCollection<ComboBoxItemDto> RegionOptions { get; set; }
        public RegionalUnityDto SelectedRegionalUnity { get; set; }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                SetProperty(ref _searchTerm, value);
                InvokeOperation(Search);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _libraryNumber;
        public string LibraryNumber
        {
            get { return _libraryNumber; }
            set { SetProperty(ref _libraryNumber, value); }
        }

        private ComboBoxItemDto _regionSelected;
        public ComboBoxItemDto RegionSelected
        {
            get { return _regionSelected; }
            set { SetProperty(ref _regionSelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddRegionalUnityCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddRegionalUnity);
                });
            }
        }

        public DelegateCommand UpdateRegionalUnityCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateRegionalUnity);
                });
            }
        }

        public DelegateCommand DeleteRegionalUnityCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteRegionalUnity);
                });
            }
        }

        public DelegateCommand ClearInputsCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(RefreshInputs);
                });
            }
        }

        public DelegateCommand DataGridRowDoubleClick
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(PrepareForUpdate);
                });
            }
        }
        #endregion

        #region Operation Methods
        private void InitializeRegionOptions()
        {
            var regions = Svc.RegionsService
                    .ListByCriteria(new RegionCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    });

            if (RegionOptions != null)
            {
                RegionOptions.Clear();
                regions.ToList().ForEach(x => RegionOptions.Add(x));
            }
            else RegionOptions = new ObservableCollection<ComboBoxItemDto>(regions);

            RegionOptions.Insert(0, new ComboBoxItemDto { Text = @"- Περιφέρεια -", Value = -1 });
            RegionSelected = RegionOptions.FirstOrDefault();
        }

        private void InitializeRegionalUnities()
        {
            _regionalUnityItems = Svc.RegionalUnitiesService
                    .ListByCriteria(new RegionalUnityCriteria())
                    .GetDtos();

            if (RegionalUnities != null)
            {
                RegionalUnities.Clear();
                _regionalUnityItems.ToList().ForEach(x => RegionalUnities.Add(x));
            }
            else RegionalUnities = new ObservableCollection<RegionalUnityDto>(_regionalUnityItems);
        }

        private void AddRegionalUnity()
        {
            try
            {
                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);

                if (RegionSelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Περιφέρεια");
                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.RegionalUnitiesService.Create(new RegionalUnityDto { Name = Name, RegionId = RegionSelected.Value.ToInt32(), LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateRegionalUnity()
        {
            try
            {
                if (!_forUpdate)
                    return;

                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);

                if (RegionSelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Περιφέρεια");
                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.RegionalUnitiesService.Update(new RegionalUnityDto { Id = SelectedRegionalUnity.Id, Name = Name, RegionId = RegionSelected.Value.ToInt32(), LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteRegionalUnity()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedRegionalUnityId = SelectedRegionalUnity?.Id ?? 0;
                Svc.RegionalUnitiesService.Delete(selectedRegionalUnityId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedRegionalUnity != null)
            {
                _forUpdate = true;
                Name = SelectedRegionalUnity.Name;
                LibraryNumber = SelectedRegionalUnity.LibraryNumber;
                RegionSelected =
                    RegionOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedRegionalUnity.RegionId) ?? new ComboBoxItemDto();
            }
        }

        private void Search()
        {
            var searchDtos = _regionalUnityItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr())
                        || x.LibraryNumber.Contains(LibraryNumber))
                    .ToList();

            RegionalUnities.Clear();
            searchDtos.ForEach(x => RegionalUnities.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            LibraryNumber = string.Empty;
            InvokeOperation(InitializeRegionOptions);
            InvokeOperation(InitializeRegionalUnities);
        }
    }
}
