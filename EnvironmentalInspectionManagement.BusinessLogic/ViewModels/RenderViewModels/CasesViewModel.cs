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
    using System;
    using Core;
    #endregion

    public interface ICasesViewModel : IBaseViewModel { }

    public class CasesViewModel : BaseViewModel, ICasesViewModel
    {
        #region Properties
        private static List<CaseDto> _caseItems;
        private static bool _forUpdateGeneralDocument;
        private static bool _forUpdateIndictment;
        private static bool _forUpdateAutopsy;
        private static bool _forUpdateAutopsyLicense;
        #endregion

        #region Constructor
        public CasesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<ComboBoxItemDto> ActivitiesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> RegionsOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> RegionalUnitiesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> CitiesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> ControlTriggersOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> ControlTypesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> IndictmentPriorityDegreesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> IndictmentSafetyDegreesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> InspectorsForAutopsyOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> AutopsyDocumentTypesOptions { get; set; }
        public ObservableCollection<CaseDto> Cases { get; set; }
        public ObservableCollection<DocumentDto> CaseDocuments { get; set; }
        public ObservableCollection<InspectorAutopsyMapDto> InspectorsForAutopsy { get; set; }
        public ObservableCollection<CourtDecisionAutopsyMapDto> TrialsForAutopsy { get; set; }
        public ObservableCollection<AutopsyLicenseMapDto> LicensesForAutopsy { get; set; }

        private InspectorAutopsyMapDto _selectedInspectorForAutopsy;
        public InspectorAutopsyMapDto SelectedInspectorForAutopsy
        {
            get { return _selectedInspectorForAutopsy; }
            set { SetProperty(ref _selectedInspectorForAutopsy, value); }
        }

        private CourtDecisionAutopsyMapDto _selectedTrialForAutopsy;
        public CourtDecisionAutopsyMapDto SelectedTrialForAutopsy
        {
            get { return _selectedTrialForAutopsy; }
            set { SetProperty(ref _selectedTrialForAutopsy, value); }
        }

        private AutopsyLicenseMapDto _selectedLicenseForAutopsy;
        public AutopsyLicenseMapDto SelectedLicenseForAutopsy
        {
            get { return _selectedLicenseForAutopsy; }
            set { SetProperty(ref _selectedLicenseForAutopsy, value); }
        }

        private CaseDto _selectedCase;
        public CaseDto SelectedCase
        {
            get { return _selectedCase; }
            set
            {
                SetProperty(ref _selectedCase, value);
                if (SelectedCase == null)
                    return;

                CaseActivitySelected = ActivitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedCase.ActivityId);
                InvokeOperation(InitializeDocumentsForCase);
            }
        }

        private DocumentDto _selectedCaseDocument;
        public DocumentDto SelectedCaseDocument
        {
            get { return _selectedCaseDocument; }
            set { SetProperty(ref _selectedCaseDocument, value); }
        }

        private ComboBoxItemDto _caseActivitySelected;
        public ComboBoxItemDto CaseActivitySelected
        {
            get { return _caseActivitySelected; }
            set { SetProperty(ref _caseActivitySelected, value); }
        }

        private ComboBoxItemDto _autopsyDocumentTypeSelected;
        public ComboBoxItemDto AutopsyDocumentTypeSelected
        {
            get { return _autopsyDocumentTypeSelected; }
            set { SetProperty(ref _autopsyDocumentTypeSelected, value); }
        }

        private ComboBoxItemDto _inspectorsForAutopsyOptionsSelected;
        public ComboBoxItemDto InspectorsForAutopsyOptionsSelected
        {
            get { return _inspectorsForAutopsyOptionsSelected; }
            set { SetProperty(ref _inspectorsForAutopsyOptionsSelected, value); }
        }

        private ComboBoxItemDto _violationRegionSelected;
        public ComboBoxItemDto ViolationRegionSelected
        {
            get { return _violationRegionSelected; }
            set { SetProperty(ref _violationRegionSelected, value); }
        }

        private ComboBoxItemDto _violationRegionalUnitySelected;
        public ComboBoxItemDto ViolationRegionalUnitySelected
        {
            get { return _violationRegionalUnitySelected; }
            set { SetProperty(ref _violationRegionalUnitySelected, value); }
        }

        private ComboBoxItemDto _violationCitySelected;
        public ComboBoxItemDto ViolationCitySelected
        {
            get { return _violationCitySelected; }
            set { SetProperty(ref _violationCitySelected, value); }
        }

        private ComboBoxItemDto _indictmentRegionSelected;
        public ComboBoxItemDto IndictmentRegionSelected
        {
            get { return _indictmentRegionSelected; }
            set { SetProperty(ref _indictmentRegionSelected, value); }
        }

        private ComboBoxItemDto _indictmentRegionalUnitySelected;
        public ComboBoxItemDto IndictmentRegionalUnitySelected
        {
            get { return _indictmentRegionalUnitySelected; }
            set { SetProperty(ref _indictmentRegionalUnitySelected, value); }
        }

        private ComboBoxItemDto _indictmentCitySelected;
        public ComboBoxItemDto IndictmentCitySelected
        {
            get { return _indictmentCitySelected; }
            set { SetProperty(ref _indictmentCitySelected, value); }
        }

        private ComboBoxItemDto _controlTriggerSelected;
        public ComboBoxItemDto ControlTriggerSelected
        {
            get { return _controlTriggerSelected; }
            set { SetProperty(ref _controlTriggerSelected, value); }
        }

        private ComboBoxItemDto _controlTypeSelected;
        public ComboBoxItemDto ControlTypeSelected
        {
            get { return _controlTypeSelected; }
            set { SetProperty(ref _controlTypeSelected, value); }
        }

        private ComboBoxItemDto _indictmentPriorityDegreeSelected;
        public ComboBoxItemDto IndictmentPriorityDegreeSelected
        {
            get { return _indictmentPriorityDegreeSelected; }
            set { SetProperty(ref _indictmentPriorityDegreeSelected, value); }
        }

        private ComboBoxItemDto _indictmentSafetyDegreeSelected;
        public ComboBoxItemDto IndictmentSafetyDegreeSelected
        {
            get { return _indictmentSafetyDegreeSelected; }
            set { SetProperty(ref _indictmentSafetyDegreeSelected, value); }
        }

        private DateTime? _indictmentDate;
        public DateTime? IndictmentDate
        {
            get { return _indictmentDate; }
            set { SetProperty(ref _indictmentDate, value); }
        }

        private DateTime? _autopsyStartDate;
        public DateTime? AutopsyStartDate
        {
            get { return _autopsyStartDate; }
            set { SetProperty(ref _autopsyStartDate, value); }
        }

        private DateTime? _autopsyEndDate;
        public DateTime? AutopsyEndDate
        {
            get { return _autopsyEndDate; }
            set { SetProperty(ref _autopsyEndDate, value); }
        }

        private DateTime? _licenseForAutopsyDateLicensed;
        public DateTime? LicenseForAutopsyDateLicensed
        {
            get { return _licenseForAutopsyDateLicensed; }
            set { SetProperty(ref _licenseForAutopsyDateLicensed, value); }
        }

        private DateTime? _licenseForAutopsyDateExpiring;
        public DateTime? LicenseForAutopsyDateExpiring
        {
            get { return _licenseForAutopsyDateExpiring; }
            set { SetProperty(ref _licenseForAutopsyDateExpiring, value); }
        }

        private bool _generalDocumentTabSelected;
        public bool GeneralDocumentTabSelected
        {
            get { return _generalDocumentTabSelected; }
            set { SetProperty(ref _generalDocumentTabSelected, value); }
        }

        private bool _autopsyTabSelected;
        public bool AutopsyTabSelected
        {
            get { return _autopsyTabSelected; }
            set { SetProperty(ref _autopsyTabSelected, value); }
        }

        private bool _indictmentTabSelected;
        public bool IndictmentTabSelected
        {
            get { return _indictmentTabSelected; }
            set { SetProperty(ref _indictmentTabSelected, value); }
        }

        private string _autopsyProtocolNo;
        public string AutopsyProtocolNo
        {
            get { return _autopsyProtocolNo; }
            set { SetProperty(ref _autopsyProtocolNo, value); }
        }

        private string _licenseForAutopsyLicenseNo;
        public string LicenseForAutopsyLicenseNo
        {
            get { return _licenseForAutopsyLicenseNo; }
            set { SetProperty(ref _licenseForAutopsyLicenseNo, value); }
        }

        private string _licenseForAutopsyLicensedBy;
        public string LicenseForAutopsyLicensedBy
        {
            get { return _licenseForAutopsyLicensedBy; }
            set { SetProperty(ref _licenseForAutopsyLicensedBy, value); }
        }

        private string _licenseForAutopsyNotes;
        public string LicenseForAutopsyNotes
        {
            get { return _licenseForAutopsyNotes; }
            set { SetProperty(ref _licenseForAutopsyNotes, value); }
        }

        private string _indictmentProtocolNo;
        public string IndictmentProtocolNo
        {
            get { return _indictmentProtocolNo; }
            set { SetProperty(ref _indictmentProtocolNo, value); }
        }

        private string _autopsyFine;
        public string AutopsyFine
        {
            get { return _autopsyFine; }
            set { SetProperty(ref _autopsyFine, value); }
        }

        private string _generalDocumentDescription;
        public string GeneralDocumentDescription
        {
            get { return _generalDocumentDescription; }
            set { SetProperty(ref _generalDocumentDescription, value); }
        }

        private string _generalDocumentProtocolNo;
        public string GeneralDocumentProtocolNo
        {
            get { return _generalDocumentProtocolNo; }
            set { SetProperty(ref _generalDocumentProtocolNo, value); }
        }

        private string _generalDocumentName;
        public string GeneralDocumentName
        {
            get { return _generalDocumentName; }
            set { SetProperty(ref _generalDocumentName, value); }
        }

        private string _violationRegionDescription;
        public string ViolationRegionDescription
        {
            get { return _violationRegionDescription; }
            set { SetProperty(ref _violationRegionDescription, value); }
        }

        private string _indictmentRegionDescription;
        public string IndictmentRegionDescription
        {
            get { return _indictmentRegionDescription; }
            set { SetProperty(ref _indictmentRegionDescription, value); }
        }

        private string _indictorName;
        public string IndictorName
        {
            get { return _indictorName; }
            set { SetProperty(ref _indictorName, value); }
        }

        private string _indicteeName;
        public string IndicteeName
        {
            get { return _indicteeName; }
            set { SetProperty(ref _indicteeName, value); }
        }

        private string _indictmentSubject;
        public string IndictmentSubject
        {
            get { return _indictmentSubject; }
            set { SetProperty(ref _indictmentSubject, value); }
        }

        private string _indictmentContent;
        public string IndictmentContent
        {
            get { return _indictmentContent; }
            set { SetProperty(ref _indictmentContent, value); }
        }

        private string _controlManagerName;
        public string ControlManagerName
        {
            get { return _controlManagerName; }
            set { SetProperty(ref _controlManagerName, value); }
        }

        private string _controlManagerFaculty;
        public string ControlManagerFaculty
        {
            get { return _controlManagerFaculty; }
            set { SetProperty(ref _controlManagerFaculty, value); }
        }

        private string _autopsyElements;
        public string AutopsyElements
        {
            get { return _autopsyElements; }
            set { SetProperty(ref _autopsyElements, value); }
        }

        private string _autopsyWantedElements;
        public string AutopsyWantedElements
        {
            get { return _autopsyWantedElements; }
            set { SetProperty(ref _autopsyWantedElements, value); }
        }

        private string _autopsySubmittedElements;
        public string AutopsySubmittedElements
        {
            get { return _autopsySubmittedElements; }
            set { SetProperty(ref _autopsySubmittedElements, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddCaseCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddCase);
                InvokeOperation(RefreshCaseInputs);
            });}
        }

        public DelegateCommand DeleteCaseCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteCase);
                InvokeOperation(RefreshCaseInputs);
            });}
        }

        public DelegateCommand DeleteCaseDocumentCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteCaseDocument);
                InvokeOperation(RefreshInputs);
            });}
        }

        public DelegateCommand AddGeneralDocumentCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddGeneralDocument);

                _forUpdateGeneralDocument = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshGeneralDocumentInputs);
            });}
        }

        public DelegateCommand UpdateGeneralDocumentCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateGeneralDocument);

                _forUpdateGeneralDocument = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshGeneralDocumentInputs);
            });}
        }

        public DelegateCommand ClearGeneralDocumentInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshGeneralDocumentInputs);
            });}
        }

        public DelegateCommand AddIndictmentCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddIndictment);

                _forUpdateIndictment = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshIndictmentInputs);
            });}
        }

        public DelegateCommand UpdateIndictmentCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateIndictment);

                _forUpdateIndictment = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshIndictmentInputs);
            });}
        }

        public DelegateCommand ClearIndictmentInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshIndictmentInputs);
            });}
        }

        public DelegateCommand AddAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddAutopsy);

                _forUpdateAutopsy = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshAutopsyInputs);
            });}
        }

        public DelegateCommand UpdateAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateAutopsy);

                _forUpdateAutopsy = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshAutopsyInputs);
            });}
        }

        public DelegateCommand ClearAutopsyInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshAutopsyInputs);
            });}
        }

        public DelegateCommand AddInspectorForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddInspectorForAutopsy);
            });}
        }

        public DelegateCommand DeleteInspectorForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteInspectorForAutopsy);
            });}
        }

        public DelegateCommand TrialsForAutopsyDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                
            });}
        }

        public DelegateCommand AddTrialForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                
            });}
        }

        public DelegateCommand UpdateTrialForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                
            });}
        }

        public DelegateCommand DeleteTrialForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                
            });}
        }

        public DelegateCommand ClearTrialForAutopsyInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                
            });}
        }

        public DelegateCommand LicensesForAutopsyDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareLicenseForAutopsyForUpdate);
            });}
        }

        public DelegateCommand AddLicenseForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddLicenseForAutopsy);
            });}
        }

        public DelegateCommand DeleteLicenseForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteLicenseForAutopsy);
            });}
        }

        public DelegateCommand UpdateLicenseForAutopsyCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateLicenseForAutopsy);

                _forUpdateAutopsyLicense = false;
                InvokeOperation(RefreshLicenseForAutopsyInputs);
            });}
        }

        public DelegateCommand ClearLicenseForAutopsyInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshLicenseForAutopsyInputs);
            });}
        }

        public DelegateCommand CaseDocumentDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareCaseDocumentForUpdate);
            });}
        }

        public DelegateCommand ClearCaseInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshCaseInputs);
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeCases()
        {
            GeneralDocumentTabSelected = true;
            _caseItems = Svc.CasesService.ListByCriteria(new CaseCriteria())
                .GetDtos()
                .ToList();

            if (Cases != null)
            {
                Cases.Clear();
                _caseItems.ForEach(x => Cases.Add(x));
            }
            else Cases = new ObservableCollection<CaseDto>(_caseItems);
        }

        private void InitializeDocumentsForCase()
        {
            if (SelectedCase == null)
            {
                CaseDocuments = new ObservableCollection<DocumentDto>(new List<DocumentDto>());
                return;
            }

            var caseDocuments = Svc.DocumentsService
                .ListByCriteria(new DocumentCriteria {CaseId = SelectedCase.Id})
                .GetDtos()
                .ToList();

            if (CaseDocuments != null)
            {
                CaseDocuments.Clear();
                caseDocuments.ForEach(x => CaseDocuments.Add(x));
            }
            else CaseDocuments = new ObservableCollection<DocumentDto>(caseDocuments);
        }

        private void InitializeActivitiesOptions()
        {
            var activitiesOptions = Svc.ActivitiesService
                .ListByCriteria(new ActivityCriteria())
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (ActivitiesOptions != null)
            {
                ActivitiesOptions.Clear();
                activitiesOptions.ToList().ForEach(x => ActivitiesOptions.Add(x));
            }
            else ActivitiesOptions = new ObservableCollection<ComboBoxItemDto>(activitiesOptions);

            ActivitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Δραστηριότητες -", Value = -1 });
            CaseActivitySelected = ActivitiesOptions.FirstOrDefault();
        }

        private void InitializeRegionsOptions()
        {
            var regionOptions = Svc.RegionsService
                .ListByCriteria(new RegionCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (RegionsOptions != null)
            {
                RegionsOptions.Clear();
                regionOptions.ToList().ForEach(x => RegionsOptions.Add(x));
            }
            else RegionsOptions = new ObservableCollection<ComboBoxItemDto>(regionOptions);

            RegionsOptions.Insert(0, new ComboBoxItemDto { Text = @"- Περιφέρειες -", Value = -1 });
            IndictmentRegionSelected = ViolationRegionSelected = RegionsOptions.FirstOrDefault();
        }

        private void InitializeRegionalUnitiesOptions()
        {
            var regionalUnitiesOptions = Svc.RegionalUnitiesService
                .ListByCriteria(new RegionalUnityCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (RegionalUnitiesOptions != null)
            {
                RegionalUnitiesOptions.Clear();
                regionalUnitiesOptions.ToList().ForEach(x => RegionalUnitiesOptions.Add(x));
            }
            else RegionalUnitiesOptions = new ObservableCollection<ComboBoxItemDto>(regionalUnitiesOptions);

            RegionalUnitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Περιφερειακές Ενότητες -", Value = -1 });
            IndictmentRegionalUnitySelected = ViolationRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault();
        }

        private void InitializeCitiesOptions()
        {
            var citiesOptions = Svc.CitiesService
                .ListByCriteria(new CityCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (CitiesOptions != null)
            {
                CitiesOptions.Clear();
                citiesOptions.ToList().ForEach(x => CitiesOptions.Add(x));
            }
            else CitiesOptions = new ObservableCollection<ComboBoxItemDto>(citiesOptions);

            CitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Πόλεις -", Value = -1 });
            IndictmentCitySelected = ViolationCitySelected = CitiesOptions.FirstOrDefault();
        }

        private void InitializeControlTriggersOptions()
        {
            var controlTriggerOptions = Svc.ControlTriggersService
                .ListByCriteria(new ControlTriggerCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (ControlTriggersOptions != null)
            {
                ControlTriggersOptions.Clear();
                controlTriggerOptions.ToList().ForEach(x => ControlTriggersOptions.Add(x));
            }
            else ControlTriggersOptions = new ObservableCollection<ComboBoxItemDto>(controlTriggerOptions);

            ControlTriggersOptions.Insert(0, new ComboBoxItemDto { Text = @"- Εναύσματα Ελέγχου -", Value = -1 });
            ControlTriggerSelected = ControlTriggersOptions.FirstOrDefault();
        }

        private void InitializeControlTypesOptions()
        {
            var controlTypesOptions = Svc.ControlTypesService
                .ListByCriteria(new ControlTypeCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (ControlTypesOptions != null)
            {
                ControlTypesOptions.Clear();
                controlTypesOptions.ToList().ForEach(x => ControlTypesOptions.Add(x));
            }
            else ControlTypesOptions = new ObservableCollection<ComboBoxItemDto>(controlTypesOptions);

            ControlTypesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Τύπος Ελέγχου -", Value = -1 });
            ControlTypeSelected = ControlTypesOptions.FirstOrDefault();
        }

        private void InitializeIndictmentSafetyDegreesOptions()
        {
            var safetyDegreesOptions = Svc.SafetyDegreesService
                .ListByCriteria(new SafetyDegreeCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (IndictmentSafetyDegreesOptions != null)
            {
                IndictmentSafetyDegreesOptions.Clear();
                safetyDegreesOptions.ToList().ForEach(x => IndictmentSafetyDegreesOptions.Add(x));
            }
            else IndictmentSafetyDegreesOptions = new ObservableCollection<ComboBoxItemDto>(safetyDegreesOptions);

            IndictmentSafetyDegreesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Βαθμός Ασφαλείας -", Value = -1 });
            IndictmentSafetyDegreeSelected = IndictmentSafetyDegreesOptions.FirstOrDefault();
        }

        private void InitializeIndictmentPriorityDegreesOptions()
        {
            var priorityDegreesOptions = Svc.PriorityDegreesService
                .ListByCriteria(new PriorityDegreeCriteria())
                .OrderBy(x => x.Name)
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (IndictmentPriorityDegreesOptions != null)
            {
                IndictmentPriorityDegreesOptions.Clear();
                priorityDegreesOptions.ToList().ForEach(x => IndictmentPriorityDegreesOptions.Add(x));
            }
            else IndictmentPriorityDegreesOptions = new ObservableCollection<ComboBoxItemDto>(priorityDegreesOptions);

            IndictmentPriorityDegreesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Βαθμός Προτεραιότητας -", Value = -1 });
            IndictmentPriorityDegreeSelected = IndictmentPriorityDegreesOptions.FirstOrDefault();
        }

        private void InitializeAutopsyDocumentTypesOptions()
        {
            var autopsyDocTypes = Svc.AutopsyDocumentCategoriesService
                .ListByCriteria(new AutopsyDocumentCategoryCriteria())
                .Select(x => new ComboBoxItemDto
                {
                    Text = $"[{x.Code}] {x.Name}",
                    Value = x.Id
                }).ToList();

            if (AutopsyDocumentTypesOptions != null)
            {
                AutopsyDocumentTypesOptions.Clear();
                autopsyDocTypes.ForEach(x => AutopsyDocumentTypesOptions.Add(x));
            }
            else AutopsyDocumentTypesOptions = new ObservableCollection<ComboBoxItemDto>(autopsyDocTypes);

            AutopsyDocumentTypesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Τύποι Αυτοψιών -", Value = -1 });
            AutopsyDocumentTypeSelected = AutopsyDocumentTypesOptions.FirstOrDefault();
        }

        private void InitializeInspectorsForAutopsyOptions()
        {
            var inspectors = Svc.InspectorsService
                .ListByCriteria(new InspectorCriteria())
                .Select(x => new ComboBoxItemDto
                {
                    Text = $"{x.FirstName} {x.LastName}",
                    Value = x.Id
                }).ToList();

            if (InspectorsForAutopsyOptions != null)
            {
                InspectorsForAutopsyOptions.Clear();
                inspectors.ForEach(x => InspectorsForAutopsyOptions.Add(x));
            }
            else InspectorsForAutopsyOptions = new ObservableCollection<ComboBoxItemDto>(inspectors);

            InspectorsForAutopsyOptions.Insert(0, new ComboBoxItemDto { Text = @"- Ελεγκτές -", Value = -1 });
            InspectorsForAutopsyOptionsSelected = InspectorsForAutopsyOptions.FirstOrDefault();
        }

        private void AddCase()
        {
            Svc.CasesService.Create(new CaseDto { ActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0 });
        }

        private void DeleteCase()
        {
            var deletionConfirmed =
                       @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

            if (!deletionConfirmed)
                return;

            Svc.CasesService.Delete(SelectedCase.Id);
        }

        private void DeleteCaseDocument()
        {
            var deletionConfirmed =
                       @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

            if (!deletionConfirmed)
                return;

            if (SelectedCaseDocument == null)
                return;

            var document = Svc.DocumentsService
            .ListByCriteria(new DocumentCriteria
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = SelectedCaseDocument.DocumentId,
                ActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                DocumentTypeId = (int)DataAccessLayer.Models.CaseDocuments.GeneralDocument
            })
            .FirstOrDefault();

            if (document == null)
                throw new EiViewException(@"Δεν βρέθηκε έγγραφο");

            switch (SelectedCaseDocument.DocumentTypeId)
            {
                case (int)DataAccessLayer.Models.CaseDocuments.Indictment:
                    Svc.IndictmentsService.Delete(document.Id);
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.Autopsy:
                    Svc.AutopsiesService.Delete(document.Id);
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.Control:
                    Svc.ControlsService.Delete(document.Id);
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.GeneralDocument:
                    Svc.GeneralDocumentsService.Delete(document.Id);
                    break;
            }
        }

        private void AddGeneralDocument()
        {
            var gdCreatedId = Svc.GeneralDocumentsService.Create(new GeneralDocumentDto
            {
                Name = GeneralDocumentName,
                Description = GeneralDocumentDescription,
                ProtocolNo = GeneralDocumentProtocolNo
            });
            var docCreatedId = Svc.DocumentsService.Create(new DocumentDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = gdCreatedId,
                ActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                DocumentTypeId = (int)DataAccessLayer.Models.CaseDocuments.GeneralDocument
            });
            Svc.DocumentCaseMapsService.Create(new DocumentCaseMapDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = docCreatedId
            });
        }

        private void UpdateGeneralDocument()
        {
            if (!_forUpdateGeneralDocument)
                return;

            var generalDocument = Svc.GeneralDocumentsService
                .ListByCriteria(new GeneralDocumentCriteria { Id = SelectedCaseDocument.DocumentId })
                .FirstOrDefault();

            if (generalDocument == null)
                throw new EiViewException(@"Δεν βρέθηκε Γενικό Έγγραφο");

            Svc.GeneralDocumentsService.Update(new GeneralDocumentDto { Id = generalDocument.Id, Name = GeneralDocumentName, Description = GeneralDocumentDescription, ProtocolNo = GeneralDocumentProtocolNo });
        }

        private void AddIndictment()
        {
            var vRegionId = ViolationRegionSelected?.Value.ToInt32() ?? 0;
            var vRegionalUnityId = ViolationRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var vCityId = ViolationCitySelected?.Value.ToInt32() ?? 0;
            var iRegionId = IndictmentRegionSelected?.Value.ToInt32() ?? 0;
            var iRegionalUnityId = IndictmentRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var iCityId = IndictmentCitySelected?.Value.ToInt32() ?? 0;
            var priorityDegreeId = IndictmentPriorityDegreeSelected?.Value.ToInt32() ?? 0;
            var safetyDegreeId = IndictmentSafetyDegreeSelected?.Value.ToInt32() ?? 0;

            var iCreatedId = Svc.IndictmentsService.Create(new IndictmentDto
            {
                IndicteeName = IndicteeName,
                IndictorName = IndictorName,
                IndictmentSubject = IndictmentSubject,
                IndictmentContent = IndictmentContent,
                ProtocolNo = IndictmentProtocolNo,
                ViolationRegionDescription = ViolationRegionDescription,
                IndictmentRegionDescription = IndictmentRegionDescription,
                IndictmentDate = IndictmentDate,
                ViolationRegionId = vRegionId,
                ViolationRegionalUnityId = vRegionalUnityId,
                ViolationCityId = vCityId,
                IndictmentRegionId = iRegionId,
                IndictmentRegionalUnityId = iRegionalUnityId,
                IndictmentCityId = iCityId,
                IndicteeActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                PriorityDegreeId = priorityDegreeId,
                SafetyDegreeId = safetyDegreeId
            });
            var docCreatedId = Svc.DocumentsService.Create(new DocumentDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = iCreatedId,
                ActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                DocumentTypeId = (int)DataAccessLayer.Models.CaseDocuments.Indictment
            });
            Svc.DocumentCaseMapsService.Create(new DocumentCaseMapDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = docCreatedId
            });
        }

        private void UpdateIndictment()
        {
            if (!_forUpdateIndictment)
                return;

            var indictment = Svc.IndictmentsService
                .ListByCriteria(new IndictmentCriteria { Id = SelectedCaseDocument.DocumentId })
                .FirstOrDefault();

            if (indictment == null)
                throw new EiViewException(@"Δεν βρέθηκε Καταγγελία");

            var indictmentId = indictment.Id;
            var vRegionId = ViolationRegionSelected?.Value.ToInt32() ?? 0;
            var vRegionalUnityId = ViolationRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var vCityId = ViolationCitySelected?.Value.ToInt32() ?? 0;
            var iRegionId = IndictmentRegionSelected?.Value.ToInt32() ?? 0;
            var iRegionalUnityId = IndictmentRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var iCityId = IndictmentCitySelected?.Value.ToInt32() ?? 0;
            var priorityDegreeId = IndictmentPriorityDegreeSelected?.Value.ToInt32() ?? 0;
            var safetyDegreeId = IndictmentSafetyDegreeSelected?.Value.ToInt32() ?? 0;

            Svc.IndictmentsService.Update(new IndictmentDto
            {
                Id = indictmentId,
                IndicteeName = IndicteeName,
                IndictorName = IndictorName,
                IndictmentSubject = IndictmentSubject,
                IndictmentContent = IndictmentContent,
                ProtocolNo = IndictmentProtocolNo,
                ViolationRegionDescription = ViolationRegionDescription,
                IndictmentRegionDescription = IndictmentRegionDescription,
                IndictmentDate = IndictmentDate,
                ViolationRegionId = vRegionId,
                ViolationRegionalUnityId = vRegionalUnityId,
                ViolationCityId = vCityId,
                IndictmentRegionId = iRegionId,
                IndictmentRegionalUnityId = iRegionalUnityId,
                IndictmentCityId = iCityId,
                IndicteeActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                PriorityDegreeId = priorityDegreeId,
                SafetyDegreeId = safetyDegreeId
            });
        }

        private void AddAutopsy()
        {
            var autopsyTypeId = AutopsyDocumentTypeSelected?.Value.ToInt32() ?? 0;
            var autopsy = new AutopsyDto
            {
                AutopsyDocumentCategoryId = autopsyTypeId,
                AutopsyElements = AutopsyElements,
                SubmittedElements = AutopsySubmittedElements,
                WantedElements = AutopsyWantedElements,
                ProtocolNumber = AutopsyProtocolNo,
                DateStarted = AutopsyStartDate,
                DateEnded = AutopsyEndDate,
                Fine = AutopsyFine.ToDecimal()
            };
            var autopsyCreatedId = Svc.AutopsiesService.Create(autopsy);
            if (autopsyCreatedId < 1)
                throw new EiViewException(@"Πρόβλημα δημιουργίας αυτοψίας");

            var docCreatedId = Svc.DocumentsService.Create(new DocumentDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = autopsyCreatedId,
                ActivityId = CaseActivitySelected?.Value.ToInt32() ?? 0,
                DocumentTypeId = (int)DataAccessLayer.Models.CaseDocuments.Autopsy
            });
            Svc.DocumentCaseMapsService.Create(new DocumentCaseMapDto
            {
                CaseId = SelectedCase?.Id ?? 0,
                DocumentId = docCreatedId
            });

            if (InspectorsForAutopsy != null && InspectorsForAutopsy.Any())
            {
                InspectorsForAutopsy.ToList().ForEach(x =>
                {
                    Svc.InspectorAutopsyMapsService.Create(new InspectorAutopsyMapDto
                    {
                        AutopsyId = autopsyCreatedId,
                        InspectorId = x.InspectorId
                    });
                });
            }

            if (LicensesForAutopsy != null && LicensesForAutopsy.Any())
            {
                LicensesForAutopsy.ToList().ForEach(x =>
                {
                    var licenseCreatedId = Svc.LicensesService.Create(new LicenseDto
                    {
                        DateExpiring = x.DateExpiring,
                        DateLicensed = x.DateLicensed,
                        LicenseNo = x.LicenseNo,
                        LicensedBy = x.LicensedBy,
                        Notes = x.LicenseNotes
                    });
                    Svc.AutopsyLicenseMapsService.Create(new AutopsyLicenseMapDto
                    {
                        AutopsyId = autopsyCreatedId,
                        LicenseId = licenseCreatedId
                    });
                });
            }
        }

        private void UpdateAutopsy()
        {
            if (!_forUpdateAutopsy)
                return;

            var autopsyId = SelectedCaseDocument?.DocumentId ?? 0;
            var autopsyTypeId = AutopsyDocumentTypeSelected?.Value.ToInt32() ?? 0;
            var autopsy = new AutopsyDto
            {
                Id = autopsyId,
                AutopsyDocumentCategoryId = autopsyTypeId,
                AutopsyElements = AutopsyElements,
                SubmittedElements = AutopsySubmittedElements,
                WantedElements = AutopsyWantedElements,
                ProtocolNumber = AutopsyProtocolNo,
                DateStarted = AutopsyStartDate,
                DateEnded = AutopsyEndDate,
                Fine = AutopsyFine.ToDecimal()
            };
            Svc.AutopsiesService.Update(autopsy);

            var inspectorsAutopsiesIds = Svc.InspectorAutopsyMapsService
                .ListByCriteria(new InspectorAutopsyMapCriteria { AutopsyId = autopsyId })
                .Select(x => x.Id)
                .ToList();
            inspectorsAutopsiesIds.ForEach(x => Svc.InspectorAutopsyMapsService.Delete(x));
            InspectorsForAutopsy.ToList().ForEach(x =>
            {
                Svc.InspectorAutopsyMapsService.Create(new InspectorAutopsyMapDto
                {
                    AutopsyId = autopsyId,
                    InspectorId = x.InspectorId
                });
            });

            LicensesForAutopsy.ToList().ForEach(x =>
            {
                var existingLicense = Svc.LicensesService.ListByCriteria(new LicenseCriteria
                {
                    DateExpiring = x.DateExpiring,
                    DateLicensed = x.DateLicensed,
                    LicenseNo = x.LicenseNo,
                    LicensedBy = x.LicensedBy,
                    Notes = x.LicenseNotes
                }).FirstOrDefault();

                if (existingLicense == null)
                {
                    var licenseCreatedId = Svc.LicensesService.Create(new LicenseDto
                    {
                        DateExpiring = x.DateExpiring,
                        DateLicensed = x.DateLicensed,
                        LicenseNo = x.LicenseNo,
                        LicensedBy = x.LicensedBy,
                        Notes = x.LicenseNotes
                    });
                    Svc.AutopsyLicenseMapsService.Create(new AutopsyLicenseMapDto
                    {
                        AutopsyId = autopsyId,
                        LicenseId = licenseCreatedId
                    });
                }
                else
                {
                    Svc.LicensesService.Update(new LicenseDto
                    {
                        Id = existingLicense.Id,
                        DateExpiring = x.DateExpiring,
                        DateLicensed = x.DateLicensed,
                        LicenseNo = x.LicenseNo,
                        LicensedBy = x.LicensedBy,
                        Notes = x.LicenseNotes
                    });
                }
            });
        }

        private void AddInspectorForAutopsy()
        {
            if (SelectedCaseDocument == null)
                throw new EiViewException(@"Επιλέξτε αυτοψία");
            if (InspectorsForAutopsyOptionsSelected == null)
                throw new EiViewException(@"Επιλέξτε επιθεωρητή");
            
            var inspector = Svc.InspectorsService
                .ListByCriteria(new InspectorCriteria { Id = InspectorsForAutopsyOptionsSelected.Value.ToInt32() })
                .FirstOrDefault();

            if (inspector == null)
                return;

            InspectorsForAutopsy.Add(new InspectorAutopsyMapDto
            {
                InspectorId = inspector.Id,
                FirstName = inspector.FirstName ?? string.Empty,
                LastName = inspector.LastName ?? string.Empty,
                Specialty = string.IsNullOrEmpty(inspector.Specialty) ? string.Empty : inspector.Specialty,
                AutopsyId = SelectedCaseDocument.DocumentId
            });
        }

        private void DeleteInspectorForAutopsy()
        {
            if (SelectedInspectorForAutopsy == null)
                return;

            InspectorsForAutopsy.Remove(SelectedInspectorForAutopsy);
        }

        private void PrepareLicenseForAutopsyForUpdate()
        {
            if (SelectedLicenseForAutopsy == null)
                return;

            _forUpdateAutopsyLicense = true;

            LicenseForAutopsyLicenseNo = SelectedLicenseForAutopsy.LicenseNo;
            LicenseForAutopsyLicensedBy = SelectedLicenseForAutopsy.LicensedBy;
            LicenseForAutopsyNotes = SelectedLicenseForAutopsy.LicenseNotes;
            LicenseForAutopsyDateExpiring = SelectedLicenseForAutopsy.DateExpiring;
            LicenseForAutopsyDateLicensed = SelectedLicenseForAutopsy.DateLicensed;
        }

        private void AddLicenseForAutopsy()
        {
            if (string.IsNullOrEmpty(LicenseForAutopsyLicenseNo) || string.IsNullOrEmpty(LicenseForAutopsyLicensedBy) || LicenseForAutopsyDateLicensed == null || LicenseForAutopsyDateExpiring == null)
                throw new EiViewException(@"Συμπληρώστε όλα τα πεδία της άδειας");

            LicensesForAutopsy.Add(new AutopsyLicenseMapDto
            {
                LicenseNo = LicenseForAutopsyLicenseNo,
                DateExpiring = LicenseForAutopsyDateExpiring,
                DateLicensed = LicenseForAutopsyDateLicensed,
                LicensedBy = LicenseForAutopsyLicensedBy,
                LicenseNotes = LicenseForAutopsyNotes
            });
        }

        private void DeleteLicenseForAutopsy()
        {
            if (SelectedLicenseForAutopsy == null)
                return;

            LicensesForAutopsy.Remove(SelectedLicenseForAutopsy);
        }

        private void UpdateLicenseForAutopsy()
        {
            if (!_forUpdateAutopsyLicense)
                return;

            Svc.LicensesService.Update(new LicenseDto
            {
                Id = SelectedLicenseForAutopsy.LicenseId ?? 0,
                DateExpiring = LicenseForAutopsyDateExpiring,
                DateLicensed = LicenseForAutopsyDateLicensed,
                LicenseNo = LicenseForAutopsyLicenseNo,
                LicensedBy = LicenseForAutopsyLicensedBy,
                Notes = LicenseForAutopsyNotes
            });
        }

        private void PrepareCaseDocumentForUpdate()
        {
            if (SelectedCaseDocument == null)
                return;

            switch (SelectedCaseDocument.DocumentTypeId)
            {
                case (int)DataAccessLayer.Models.CaseDocuments.Indictment:
                    _forUpdateIndictment = IndictmentTabSelected = true;
                    InvokeOperation(PrepareIndictmentForUpdate);
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.Autopsy:
                    _forUpdateAutopsy = AutopsyTabSelected = true;
                    InvokeOperation(PrepareAutopsyForUpdate);
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.Control:
                    break;
                case (int)DataAccessLayer.Models.CaseDocuments.GeneralDocument:
                    _forUpdateGeneralDocument = GeneralDocumentTabSelected = true;
                    InvokeOperation(PrepareGeneralDocumentForUpdate);
                    break;
            }
        }

        private void PrepareIndictmentForUpdate()
        {
            var indictment = Svc.IndictmentsService
                .ListByCriteria(new IndictmentCriteria {Id = SelectedCaseDocument.DocumentId })
                .FirstOrDefault();

            if (indictment == null)
                throw new EiViewException(@"Δεν βρέθηκε Καταγγελία");

            ViolationRegionSelected = RegionsOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.ViolationRegionId) ?? RegionsOptions.FirstOrDefault();
            ViolationRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.ViolationRegionalUnityId) ?? RegionalUnitiesOptions.FirstOrDefault();
            ViolationCitySelected = CitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.ViolationCityId) ?? CitiesOptions.FirstOrDefault();
            ViolationRegionDescription = indictment.ViolationRegionDescription;
            IndictmentRegionSelected = RegionsOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.IndictmentRegionId) ?? RegionsOptions.FirstOrDefault();
            IndictmentRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.IndictmentRegionalUnityId) ?? RegionalUnitiesOptions.FirstOrDefault();
            IndictmentCitySelected = CitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.IndictmentCityId) ?? CitiesOptions.FirstOrDefault();
            IndictmentRegionDescription = indictment.IndictmentRegionDescription;
            IndicteeName = indictment.IndicteeName;
            IndictorName = indictment.IndictorName;
            IndictmentSafetyDegreeSelected = IndictmentSafetyDegreesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.SafetyDegreeId) ?? IndictmentSafetyDegreesOptions.FirstOrDefault();
            IndictmentPriorityDegreeSelected = IndictmentPriorityDegreesOptions.FirstOrDefault(x => x.Value.ToInt32() == indictment.PriorityDegreeId) ?? IndictmentPriorityDegreesOptions.FirstOrDefault();
            IndictmentDate = indictment.IndictmentDate;
            IndictmentProtocolNo = indictment.ProtocolNo;
            IndictmentSubject = indictment.IndictmentSubject;
            IndictmentContent = indictment.IndictmentContent;
        }

        private void PrepareGeneralDocumentForUpdate()
        {
            var generalDocument = Svc.GeneralDocumentsService
                .ListByCriteria(new GeneralDocumentCriteria {Id = SelectedCaseDocument.DocumentId })
                .FirstOrDefault();

            if (generalDocument == null)
                throw new EiViewException(@"Δεν βρέθηκε Γενικό Έγγραφο");

            GeneralDocumentName = generalDocument.Name;
            GeneralDocumentDescription = generalDocument.Description;
            GeneralDocumentProtocolNo = generalDocument.ProtocolNo;
        }

        private void PrepareAutopsyForUpdate()
        {
            var autopsyDoc = Svc.AutopsiesService
                .ListByCriteria(new AutopsyCriteria {Id = SelectedCaseDocument.DocumentId})
                .FirstOrDefault();

            if (autopsyDoc == null)
                throw new EiViewException(@"Δεν βρέθηκε Αυτοψία");

            AutopsyProtocolNo = autopsyDoc.ProtocolNumber;
            AutopsyStartDate = autopsyDoc.DateStarted;
            AutopsyEndDate = autopsyDoc.DateEnded;
            AutopsyFine = $"{autopsyDoc.Fine:N2}";
            AutopsyElements = autopsyDoc.AutopsyElements;
            AutopsyWantedElements = autopsyDoc.WantedElements;
            AutopsySubmittedElements = autopsyDoc.SubmittedElements;
            AutopsyDocumentTypeSelected = AutopsyDocumentTypesOptions.FirstOrDefault(x => x.Value.ToInt32() == autopsyDoc.AutopsyDocumentCategoryId) ?? AutopsyDocumentTypesOptions.FirstOrDefault();

            var autopsyInspectors = Svc.InspectorAutopsyMapsService
                .ListByCriteria(new InspectorAutopsyMapCriteria {AutopsyId = autopsyDoc.Id})
                .GetDtos()
                .ToList();

            if (InspectorsForAutopsy != null)
            {
                InspectorsForAutopsy.Clear();
                autopsyInspectors.ForEach(x => InspectorsForAutopsy.Add(x));
            }
            else InspectorsForAutopsy = new ObservableCollection<InspectorAutopsyMapDto>(autopsyInspectors);

            var autopsyTrials = Svc.CourtDecisionAutopsyMapsService
                .ListByCriteria(new CourtDecisionAutopsyMapCriteria {AutopsyId = autopsyDoc.Id})
                .GetDtos()
                .ToList();

            if (TrialsForAutopsy != null)
            {
                TrialsForAutopsy.Clear();
                autopsyTrials.ForEach(x => TrialsForAutopsy.Add(x));
            }
            else TrialsForAutopsy = new ObservableCollection<CourtDecisionAutopsyMapDto>(autopsyTrials);

            var autopsyLicenses = Svc.AutopsyLicenseMapsService
                .ListByCriteria(new AutopsyLicenseMapCriteria {AutopsyId = autopsyDoc.Id})
                .GetDtos()
                .ToList();

            if (LicensesForAutopsy != null)
            {
                LicensesForAutopsy.Clear();
                autopsyLicenses.ForEach(x => LicensesForAutopsy.Add(x));
            }
            else LicensesForAutopsy = new ObservableCollection<AutopsyLicenseMapDto>(autopsyLicenses);
        }

        private void RefreshCaseInputs()
        {
            CaseActivitySelected = ActivitiesOptions.FirstOrDefault();
            InvokeOperation(InitializeCases);
        }

        private void RefreshGeneralDocumentInputs()
        {
            GeneralDocumentName = GeneralDocumentProtocolNo = GeneralDocumentDescription = string.Empty;
            InvokeOperation(InitializeDocumentsForCase);
        }

        private void RefreshIndictmentInputs()
        {
            ViolationRegionSelected = IndictmentRegionSelected = RegionsOptions.FirstOrDefault();
            ViolationRegionalUnitySelected = IndictmentRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault();
            ViolationCitySelected = IndictmentCitySelected = CitiesOptions.FirstOrDefault();
            ViolationRegionDescription = IndictmentRegionDescription = IndicteeName = IndictorName = IndictmentProtocolNo = IndictmentSubject = IndictmentContent = string.Empty;
            IndictmentSafetyDegreeSelected = IndictmentSafetyDegreesOptions.FirstOrDefault();
            IndictmentPriorityDegreeSelected = IndictmentPriorityDegreesOptions.FirstOrDefault();
            IndictmentDate = null;
            InvokeOperation(InitializeDocumentsForCase);
        }

        private void RefreshAutopsyInputs()
        {
            if (InspectorsForAutopsy == null) InspectorsForAutopsy = new ObservableCollection<InspectorAutopsyMapDto>(new List<InspectorAutopsyMapDto>());
            else InspectorsForAutopsy.Clear();

            if (TrialsForAutopsy == null) TrialsForAutopsy = new ObservableCollection<CourtDecisionAutopsyMapDto>(new List<CourtDecisionAutopsyMapDto>());
            else TrialsForAutopsy.Clear();

            if (LicensesForAutopsy == null) LicensesForAutopsy = new ObservableCollection<AutopsyLicenseMapDto>(new List<AutopsyLicenseMapDto>());
            else LicensesForAutopsy.Clear();

            AutopsyProtocolNo = AutopsyFine = AutopsyElements = AutopsyWantedElements = AutopsySubmittedElements = LicenseForAutopsyLicenseNo = LicenseForAutopsyLicensedBy = LicenseForAutopsyNotes = string.Empty;
            AutopsyStartDate = AutopsyEndDate = LicenseForAutopsyDateExpiring = LicenseForAutopsyDateLicensed = null;
            AutopsyDocumentTypeSelected = AutopsyDocumentTypesOptions.FirstOrDefault();
            InspectorsForAutopsyOptionsSelected = InspectorsForAutopsyOptions.FirstOrDefault();
            InvokeOperation(InitializeDocumentsForCase);
        }

        private void RefreshLicenseForAutopsyInputs()
        {
            LicenseForAutopsyLicenseNo = LicenseForAutopsyLicensedBy = LicenseForAutopsyNotes = string.Empty;
            LicenseForAutopsyDateExpiring = LicenseForAutopsyDateLicensed = null;
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            if (Cases == null) Cases = new ObservableCollection<CaseDto>();
            else Cases.Clear();

            if (CaseDocuments == null) CaseDocuments = new ObservableCollection<DocumentDto>();
            else CaseDocuments.Clear();
            
            if (InspectorsForAutopsy == null) InspectorsForAutopsy = new ObservableCollection<InspectorAutopsyMapDto>();
            else InspectorsForAutopsy.Clear();

            if (TrialsForAutopsy == null) TrialsForAutopsy = new ObservableCollection<CourtDecisionAutopsyMapDto>();
            else TrialsForAutopsy.Clear();
            
            if (LicensesForAutopsy == null) LicensesForAutopsy = new ObservableCollection<AutopsyLicenseMapDto>();
            else LicensesForAutopsy.Clear();

            InvokeOperationsParallel(new Action[]
            {
                InitializeActivitiesOptions,
                InitializeCases,
                InitializeDocumentsForCase,
                InitializeRegionsOptions,
                InitializeRegionalUnitiesOptions,
                InitializeCitiesOptions,
                InitializeControlTriggersOptions,
                InitializeControlTypesOptions,
                InitializeIndictmentSafetyDegreesOptions,
                InitializeIndictmentPriorityDegreesOptions,
                InitializeAutopsyDocumentTypesOptions,
                InitializeInspectorsForAutopsyOptions
            });
        }
    }
}
