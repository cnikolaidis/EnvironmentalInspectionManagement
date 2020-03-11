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

    public interface INaceCodesViewModel : IBaseViewModel { }

    public class NaceCodesViewModel : BaseViewModel, INaceCodesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<NaceCodeDto> _naceCodeItems;
        #endregion

        #region Constructor
        public NaceCodesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<NaceCodeDto> NaceCodes { get; set; }
        public ObservableCollection<ComboBoxItemDto> NaceCodeSectors { get; set; }
        public NaceCodeDto SelectedNaceCode { get; set; }

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

        private string _class;
        public string Class
        {
            get { return _class; }
            set { SetProperty(ref _class, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private ComboBoxItemDto _naceCodeSectorSelected;
        public ComboBoxItemDto NaceCodeSectorSelected
        {
            get { return _naceCodeSectorSelected; }
            set { SetProperty(ref _naceCodeSectorSelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddNaceCodeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddNaceCode);
                });
            }
        }

        public DelegateCommand UpdateNaceCodeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateNaceCode);
                });
            }
        }

        public DelegateCommand DeleteNaceCodeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteNaceCode);
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
        private void InitializeNaceCodeSectorOptions()
        {
            var naceCodeSectors = Svc.NaceCodeSectorsService
                    .ListByCriteria(new NaceCodeSectorCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = $"{x.Sector} - {x.Name}",
                        Value = x.Id
                    });

            if (NaceCodeSectors != null)
            {
                NaceCodeSectors.Clear();
                naceCodeSectors.ToList().ForEach(x => NaceCodeSectors.Add(x));
            }
            else NaceCodeSectors = new ObservableCollection<ComboBoxItemDto>(naceCodeSectors);

            NaceCodeSectors.Insert(0, new ComboBoxItemDto { Text = @"- Τομέας -", Value = -1 });
            NaceCodeSectorSelected = NaceCodeSectors.FirstOrDefault();
        }

        private void InitializeNaceCodes()
        {
            _naceCodeItems = Svc.NaceCodesService
                    .ListByCriteria(new NaceCodeCriteria())
                    .GetDtos();

            if (NaceCodes != null)
            {
                NaceCodes.Clear();
                _naceCodeItems.ToList().ForEach(x => NaceCodes.Add(x));
            }
            else NaceCodes = new ObservableCollection<NaceCodeDto>(_naceCodeItems);
        }

        private void AddNaceCode()
        {
            try
            {
                if (NaceCodeSectorSelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστό τομέα Κ.Α.Δ.");

                Svc.NaceCodesService.Create(new NaceCodeDto { Class = Class, Name = Name, SectorId = NaceCodeSectorSelected.Value.ToInt32() });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateNaceCode()
        {
            try
            {
                if (_forUpdate)
                    Svc.NaceCodesService.Update(new NaceCodeDto { Id = SelectedNaceCode.Id, Name = Name, Class = Class, SectorId = NaceCodeSectorSelected.Value.ToInt32() });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteNaceCode()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedNaceCodeId = SelectedNaceCode?.Id ?? 0;
                Svc.NaceCodesService.Delete(selectedNaceCodeId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedNaceCode != null)
            {
                _forUpdate = true;
                Name = SelectedNaceCode.Name;
                Class = SelectedNaceCode.Class;
                NaceCodeSectorSelected =
                    NaceCodeSectors.FirstOrDefault(x => x.Value.ToInt32() == SelectedNaceCode.SectorId) ??
                    new ComboBoxItemDto();
            }
        }

        private void Search()
        {
            var searchDtos = _naceCodeItems
                    .Where(x =>
                        x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr())
                        ||
                        x.Class.Contains(SearchTerm))
                    .ToList();

            NaceCodes.Clear();
            searchDtos.ForEach(x => NaceCodes.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            Class = string.Empty;
            InvokeOperation(InitializeNaceCodeSectorOptions);
            InvokeOperation(InitializeNaceCodes);
        }
    }
}
