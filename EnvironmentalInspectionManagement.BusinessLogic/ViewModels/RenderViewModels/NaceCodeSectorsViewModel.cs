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

    public interface INaceCodeSectorsViewModel : IBaseViewModel { }

    public class NaceCodeSectorsViewModel : BaseViewModel, INaceCodeSectorsViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<NaceCodeSectorDto> _naceCodeSectorItems;
        #endregion

        #region Constructor
        public NaceCodeSectorsViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<NaceCodeSectorDto> NaceCodeSectors { get; set; }
        public NaceCodeSectorDto SelectedNaceCodeSector { get; set; }
        
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

        private string _sector;
        public string Sector
        {
            get { return _sector; }
            set { SetProperty(ref _sector, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddNaceCodeSectorCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddNaceCodeSector);
            });}
        }

        public DelegateCommand UpdateNaceCodeSectorCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateNaceCodeSector);
            });}
        }

        public DelegateCommand DeleteNaceCodeSectorCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteNaceCodeSector);
            });}
        }

        public DelegateCommand ClearInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshInputs);
            });}
        }

        public DelegateCommand DataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareForUpdate);
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeNaceCodeSectors()
        {
            _naceCodeSectorItems = Svc.NaceCodeSectorsService
                    .ListByCriteria(new NaceCodeSectorCriteria())
                    .GetDtos();

            if (NaceCodeSectors != null)
            {
                NaceCodeSectors.Clear();
                _naceCodeSectorItems.ToList().ForEach(x => NaceCodeSectors.Add(x));
            }
            else NaceCodeSectors = new ObservableCollection<NaceCodeSectorDto>(_naceCodeSectorItems);
        }

        private void AddNaceCodeSector()
        {
            try
            {
                Svc.NaceCodeSectorsService.Create(new NaceCodeSectorDto {Sector = Sector, Name = Name});
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateNaceCodeSector()
        {
            try
            {
                if (_forUpdate)
                    Svc.NaceCodeSectorsService.Update(new NaceCodeSectorDto { Id = SelectedNaceCodeSector?.Id ?? 0, Name = Name, Sector = Sector });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteNaceCodeSector()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedNaceCodeSectorId = SelectedNaceCodeSector?.Id ?? 0;
                Svc.NaceCodeSectorsService.Delete(selectedNaceCodeSectorId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedNaceCodeSector != null)
            {
                _forUpdate = true;
                Name = SelectedNaceCodeSector.Name;
                Sector = SelectedNaceCodeSector.Sector;
            }
        }

        private void Search()
        {
            var searchDtos = _naceCodeSectorItems
                .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                .ToList();

            NaceCodeSectors.Clear();
            searchDtos.ForEach(x => NaceCodeSectors.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            Sector = string.Empty;
            InvokeOperation(InitializeNaceCodeSectors);
        }
    }
}
