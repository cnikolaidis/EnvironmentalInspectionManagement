namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Models.OtherObjects;
    using DataAccessLayer.Models.Criterias;
    using System.Collections.ObjectModel;
    using DataAccessLayer.Models.Dtos;
    using System.Collections.Generic;
    using DataAccessLayer.Services;
    using DataAccessLayer.Models;
    using System.Linq;
    using Utilities;
    using Core;
    #endregion

    public interface IDictionaryViewModel : IBaseViewModel { }

    public class DictionaryViewModel : BaseViewModel, IDictionaryViewModel
    {
        #region Properties
        private static int _selectedLibraryId;
        private static bool _forUpdate;
        private static IEnumerable<NamedItemDto> _namedItems;
        #endregion

        #region Constructor
        public DictionaryViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<ComboBoxItemDto> Libraries { get; set; }
        public ObservableCollection<NamedItemDto> NamedItems { get; set; }

        private ComboBoxItemDto _selectedLibrary;
        public ComboBoxItemDto SelectedLibrary
        {
            get { return _selectedLibrary; }
            set
            {
                SetProperty(ref _selectedLibrary, value);
                _selectedLibraryId = SelectedLibrary?.Value.ToInt32() ?? _selectedLibraryId;
                InvokeOperation(InitializeNamedItems);
            }
        }
        
        private NamedItemDto _selectedNamedItem;
        public NamedItemDto SelectedNamedItem
        {
            get { return _selectedNamedItem; }
            set { SetProperty(ref _selectedNamedItem, value); }
        }
        
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
        #endregion

        #region Commands
        public DelegateCommand AddNamedItemCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddNamedItem);
            });}
        }

        public DelegateCommand UpdateNamedItemCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateNamedItem);
            });}
        }

        public DelegateCommand DeleteNamedItemCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteNamedItem);
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
        private void InitializeNames()
        {
            var namesList = new List<ComboBoxItemDto>
            {
                new ComboBoxItemDto {Text = @"Τύποι Εγγράφων", Value = 0},
                new ComboBoxItemDto {Text = @"Πρόσθετες Ενέργειες", Value = 1},
                new ComboBoxItemDto {Text = @"Αποφάσεις Δικαστηρίου", Value = 2},
                new ComboBoxItemDto {Text = @"Εναύσματα Ελέγχου", Value = 3},
                new ComboBoxItemDto {Text = @"Πορείες Ελέγχου", Value = 4},
                new ComboBoxItemDto {Text = @"Πόλεις", Value = 5},
                new ComboBoxItemDto {Text = @"Κατηγορίες Νομικών Προσώπων", Value = 7},
                new ComboBoxItemDto {Text = @"Περιοχές Περιβάλλοντος", Value = 8},
                new ComboBoxItemDto {Text = @"Υπαγωγές Εγκατάστασης", Value = 9},
                new ComboBoxItemDto {Text = @"Τύποι Ελέγχων", Value = 12},
                new ComboBoxItemDto {Text = @"Βαθμοί Ασφαλείας Καταγγελιών", Value = 13},
                new ComboBoxItemDto {Text = @"Βαθμοί Προτεραιότητας Καταγγελιών", Value = 14}
            }.OrderBy(x => x.Text);

            if (Libraries != null)
            {
                Libraries.Clear();
                namesList.ToList().ForEach(x => Libraries.Add(x));
            }
            else Libraries = new ObservableCollection<ComboBoxItemDto>(namesList);

            SelectedLibrary = namesList.FirstOrDefault(x => x.Value.ToInt32() == _selectedLibraryId) ?? Libraries.FirstOrDefault();
        }

        private void InitializeNamedItems()
        {
            var selectedName = _selectedLibraryId;
            switch (selectedName)
            {
                case (int)NamedTables.DocumentTypes:
                    _namedItems = Svc.DocumentTypesService.ListByCriteria(new DocumentTypeCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.AdditionalActions:
                    _namedItems = Svc.AdditionalActionsService.ListByCriteria(new AdditionalActionCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.CourtDecisions:
                    _namedItems = Svc.CourtDecisionsService.ListByCriteria(new CourtDecisionCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.ControlTriggers:
                    _namedItems = Svc.ControlTriggersService.ListByCriteria(new ControlTriggerCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.ControlProgresses:
                    _namedItems = Svc.ControlProgressesService.ListByCriteria(new ControlProgressCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.Cities:
                    _namedItems = Svc.CitiesService.ListByCriteria(new CityCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.LegalEntityCategories:
                    _namedItems = Svc.LegalEntityCategoriesService.ListByCriteria(new LegalEntityCategoryCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.NaturaRegions:
                    _namedItems = Svc.NaturaRegionsService.ListByCriteria(new NaturaRegionCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.Subordinations:
                    _namedItems = Svc.SubordinationsService.ListByCriteria(new SubordinationCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.ControlTypes:
                    _namedItems = Svc.ControlTypesService.ListByCriteria(new ControlTypeCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.SafetyDegrees:
                    _namedItems = Svc.SafetyDegreesService.ListByCriteria(new SafetyDegreeCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
                case (int)NamedTables.PriorityDegrees:
                    _namedItems = Svc.PriorityDegreesService.ListByCriteria(new PriorityDegreeCriteria()).Select(x => new NamedItemDto { Id = x.Id, Name = x.Name });
                    break;
            }

            if (NamedItems != null)
            {
                NamedItems.Clear();
                _namedItems.ToList().ForEach(x => NamedItems.Add(x));
            }
            else NamedItems = new ObservableCollection<NamedItemDto>(_namedItems);
        }

        private void AddNamedItem()
        {
            try
            {
                var selectedName = SelectedLibrary?.Value.ToInt32() ?? 0;
                switch (selectedName)
                {
                    case (int) NamedTables.DocumentTypes:
                        Svc.DocumentTypesService.Create(new DocumentTypeDto {Name = Name});
                        break;
                    case (int) NamedTables.AdditionalActions:
                        Svc.AdditionalActionsService.Create(new AdditionalActionDto {Name = Name});
                        break;
                    case (int) NamedTables.CourtDecisions:
                        Svc.CourtDecisionsService.Create(new CourtDecisionDto {Name = Name});
                        break;
                    case (int) NamedTables.ControlTriggers:
                        Svc.ControlTriggersService.Create(new ControlTriggerDto {Name = Name});
                        break;
                    case (int) NamedTables.ControlProgresses:
                        Svc.ControlProgressesService.Create(new ControlProgressDto {Name = Name});
                        break;
                    case (int) NamedTables.Cities:
                        Svc.CitiesService.Create(new CityDto {Name = Name});
                        break;
                    case (int) NamedTables.LegalEntityCategories:
                        Svc.LegalEntityCategoriesService.Create(new LegalEntityCategoryDto {Name = Name});
                        break;
                    case (int) NamedTables.NaturaRegions:
                        Svc.NaturaRegionsService.Create(new NaturaRegionDto {Name = Name});
                        break;
                    case (int) NamedTables.Subordinations:
                        Svc.SubordinationsService.Create(new SubordinationDto {Name = Name});
                        break;
                    case (int)NamedTables.ControlTypes:
                        Svc.ControlTypesService.Create(new ControlTypeDto { Name = Name });
                        break;
                    case (int)NamedTables.SafetyDegrees:
                        Svc.SafetyDegreesService.Create(new SafetyDegreeDto { Name = Name });
                        break;
                    case (int)NamedTables.PriorityDegrees:
                        Svc.PriorityDegreesService.Create(new PriorityDegreeDto { Name = Name });
                        break;
                }
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateNamedItem()
        {
            try
            {
                if (_forUpdate)
                {
                    var selectedName = SelectedLibrary?.Value.ToInt32() ?? 0;
                    switch (selectedName)
                    {
                        case (int)NamedTables.DocumentTypes:
                            Svc.DocumentTypesService.Update(new DocumentTypeDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.AdditionalActions:
                            Svc.AdditionalActionsService.Update(new AdditionalActionDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.CourtDecisions:
                            Svc.CourtDecisionsService.Update(new CourtDecisionDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.ControlTriggers:
                            Svc.ControlTriggersService.Update(new ControlTriggerDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.ControlProgresses:
                            Svc.ControlProgressesService.Update(new ControlProgressDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.Cities:
                            Svc.CitiesService.Update(new CityDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.LegalEntityCategories:
                            Svc.LegalEntityCategoriesService.Update(new LegalEntityCategoryDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.NaturaRegions:
                            Svc.NaturaRegionsService.Update(new NaturaRegionDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.Subordinations:
                            Svc.SubordinationsService.Update(new SubordinationDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.ControlTypes:
                            Svc.ControlTypesService.Update(new ControlTypeDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.SafetyDegrees:
                            Svc.SafetyDegreesService.Update(new SafetyDegreeDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                        case (int)NamedTables.PriorityDegrees:
                            Svc.PriorityDegreesService.Update(new PriorityDegreeDto { Id = SelectedNamedItem.Id, Name = Name });
                            break;
                    }
                }
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteNamedItem()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedName = SelectedLibrary?.Value.ToInt32() ?? 0;
                var selectedNamedItem = SelectedNamedItem?.Id ?? 0;
                switch (selectedName)
                {
                    case (int) NamedTables.DocumentTypes:
                        Svc.DocumentTypesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.AdditionalActions:
                        Svc.AdditionalActionsService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.CourtDecisions:
                        Svc.CourtDecisionsService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.ControlTriggers:
                        Svc.ControlTriggersService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.ControlProgresses:
                        Svc.ControlProgressesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.Cities:
                        Svc.CitiesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.LegalEntityCategories:
                        Svc.LegalEntityCategoriesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.NaturaRegions:
                        Svc.NaturaRegionsService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.Subordinations:
                        Svc.SubordinationsService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.ControlTypes:
                        Svc.ControlTypesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.SafetyDegrees:
                        Svc.SafetyDegreesService.Delete(selectedNamedItem);
                        break;
                    case (int) NamedTables.PriorityDegrees:
                        Svc.PriorityDegreesService.Delete(selectedNamedItem);
                        break;
                }
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedNamedItem == null)
                return;

            _forUpdate = true;
            Name = SelectedNamedItem.Name;
        }

        private void Search()
        {
            var searchDtos = _namedItems
                .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                .ToList();

            NamedItems.Clear();
            searchDtos.ForEach(x => NamedItems.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            InvokeOperation(InitializeNames);
            InvokeOperation(InitializeNamedItems);
        }
    }
}
