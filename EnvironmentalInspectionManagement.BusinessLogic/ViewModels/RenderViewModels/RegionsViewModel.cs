namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Services.EntityServices;
    using DataAccessLayer.Models.Criterias;
    using System.Collections.ObjectModel;
    using DataAccessLayer.Models.Dtos;
    using System.Collections.Generic;
    using DataAccessLayer.Services;
    using System.Linq;
    using Utilities;
    using Core;
    #endregion

    public interface IRegionsViewModel : IBaseViewModel { }

    public class RegionsViewModel : BaseViewModel, IRegionsViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<RegionDto> _regionItems;
        #endregion

        #region Constructor
        public RegionsViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<RegionDto> Regions { get; set; }
        public RegionDto SelectedRegion { get; set; }

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

        private bool _isNorth;
        public bool IsNorth
        {
            get { return _isNorth; }
            set { SetProperty(ref _isNorth, value); }
        }

        private bool _isSouth;
        public bool IsSouth
        {
            get { return _isSouth; }
            set { SetProperty(ref _isSouth, value); }
        }

        private string _libraryNumber;
        public string LibraryNumber
        {
            get { return _libraryNumber; }
            set { SetProperty(ref _libraryNumber, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddRegionCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddRegion);
                });
            }
        }

        public DelegateCommand UpdateRegionCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateRegion);
                });
            }
        }

        public DelegateCommand DeleteRegionCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteRegion);
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
        private void InitializeRegions()
        {
            _regionItems = Svc.RegionsService
                .ListByCriteria(new RegionCriteria())
                .GetDtos();

            if (Regions != null)
            {
                Regions.Clear();
                _regionItems.ToList().ForEach(x => Regions.Add(x));
            }
            else Regions = new ObservableCollection<RegionDto>(_regionItems);
        }

        private void AddRegion()
        {
            try
            {
                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);
                
                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.RegionsService.Create(new RegionDto
                {
                    Name = Name,
                    LibraryNumber = LibraryNumber,
                    IsNorth = IsNorth,
                    IsSouth = IsSouth
                });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateRegion()
        {
            try
            {
                if (!_forUpdate)
                    return;

                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);

                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.RegionsService.Update(new RegionDto
                {
                    Id = SelectedRegion.Id,
                    Name = Name,
                    LibraryNumber = LibraryNumber,
                    IsNorth = IsNorth,
                    IsSouth = IsSouth
                });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteRegion()
        {
            try
            {
                var deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedRegionId = SelectedRegion?.Id ?? 0;
                Svc.RegionsService.Delete(selectedRegionId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedRegion == null)
                return;

            _forUpdate = true;
            Name = SelectedRegion.Name;
            LibraryNumber = SelectedRegion.LibraryNumber;
            IsSouth = SelectedRegion.IsSouth;
            IsNorth = SelectedRegion.IsNorth;
        }

        private void Search()
        {
            var searchDtos = _regionItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr())
                        || x.LibraryNumber.Contains(LibraryNumber))
                    .ToList();

            Regions.Clear();
            searchDtos.ForEach(x => Regions.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            LibraryNumber = string.Empty;
            IsNorth = false;
            IsSouth = false;
            InvokeOperation(InitializeRegions);
        }
    }
}
