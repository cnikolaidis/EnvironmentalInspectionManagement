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

    public interface IInspectorsViewModel : IBaseViewModel { }

    public class InspectorsViewModel : BaseViewModel, IInspectorsViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<InspectorDto> _inspectorItems;
        #endregion

        #region Constructor
        public InspectorsViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<InspectorDto> Inspectors { get; set; }
        public ObservableCollection<ComboBoxItemDto> InspectorSectors { get; set; }
        public InspectorDto SelectedInspector { get; set; }

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

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _specialty;
        public string Specialty
        {
            get { return _specialty; }
            set { SetProperty(ref _specialty, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddInspectorCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddInspector);
                });
            }
        }

        public DelegateCommand UpdateInspectorCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateInspector);
                });
            }
        }

        public DelegateCommand DeleteInspectorCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteInspector);
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
        private void InitializeInspectors()
        {
            _inspectorItems = Svc.InspectorsService
                    .ListByCriteria(new InspectorCriteria())
                    .GetDtos();

            if (Inspectors != null)
            {
                Inspectors.Clear();
                _inspectorItems.ToList().ForEach(x => Inspectors.Add(x));
            }
            else Inspectors = new ObservableCollection<InspectorDto>(_inspectorItems);
        }

        private void AddInspector()
        {
            try
            {
                Svc.InspectorsService.Create(new InspectorDto { FirstName = FirstName, LastName = LastName, Specialty = Specialty });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateInspector()
        {
            try
            {
                if (_forUpdate)
                    Svc.InspectorsService.Update(new InspectorDto { Id = SelectedInspector.Id, FirstName = FirstName, LastName = LastName, Specialty = Specialty });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteInspector()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedInspectorId = SelectedInspector?.Id ?? 0;
                Svc.InspectorsService.Delete(selectedInspectorId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedInspector != null)
            {
                _forUpdate = true;
                FirstName = SelectedInspector.FirstName;
                LastName = SelectedInspector.LastName;
                Specialty = SelectedInspector.Specialty;
            }
        }

        private void Search()
        {
            var searchDtos = _inspectorItems
                    .Where(x =>
                        x.FirstName.NoPunctuationGr().ToLower().Contains(SearchTerm.NoPunctuationGr().ToLower())
                        || x.LastName.NoPunctuationGr().ToLower().Contains(SearchTerm.NoPunctuationGr().ToLower()))
                    .ToList();

            Inspectors.Clear();
            searchDtos.ForEach(x => Inspectors.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            FirstName = string.Empty;
            LastName = string.Empty;
            Specialty = string.Empty;
            InvokeOperation(InitializeInspectors);
        }
    }
}
