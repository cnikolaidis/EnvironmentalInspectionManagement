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

    public interface IOrganizationsViewModel : IBaseViewModel { }

    public class OrganizationsViewModel : BaseViewModel, IOrganizationsViewModel
    {
        #region Properties
        private static bool _forUpdateOrganization;
        private static bool _forUpdateActivity;
        private static List<OrganizationDto> _organizationItems = new List<OrganizationDto>();
        private static List<ActivityDto> _activityItems = new List<ActivityDto>();
        private static List<WorkSubcategoryDto> _workSubcategoriesItems = new List<WorkSubcategoryDto>();
        private static List<NaceCodeDto> _naceCodeItems = new List<NaceCodeDto>();
        #endregion

        #region Constructor
        public OrganizationsViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<OrganizationDto> Organizations { get; set; }
        public ObservableCollection<ActivityDto> Activities { get; set; }
        public ObservableCollection<ComboBoxItemDto> RegionalUnitiesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> RegionsOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> LegalEntitiesOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> TaxOfficeOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> WorkCategoryOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> WorkSubcategoryOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> NaceCodeSectorOptions { get; set; }
        public ObservableCollection<ComboBoxItemDto> NaceCodeOptions { get; set; }

        private OrganizationDto _selectedOrganization;
        public OrganizationDto SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                SetProperty(ref _selectedOrganization, value);
                InvokeOperation(InitializeActivitiesForOrganization);
                InvokeOperation(PreviewOrganization);
            }
        }

        private ActivityDto _selectedActivity;
        public ActivityDto SelectedActivity
        {
            get { return _selectedActivity; }
            set
            {
                SetProperty(ref _selectedActivity, value);
                InvokeOperation(PreviewActivity);
            }
        }

        private bool _insertOrganizationTabSelected;
        public bool InsertOrganizationTabSelected
        {
            get { return _insertOrganizationTabSelected; }
            set { SetProperty(ref _insertOrganizationTabSelected, value); }
        }

        private bool _insertActivityTabSelected;
        public bool InsertActivityTabSelected
        {
            get { return _insertActivityTabSelected; }
            set { SetProperty(ref _insertActivityTabSelected, value); }
        }

        private string _organizationSearchTerm;
        public string OrganizationSearchTerm
        {
            get { return _organizationSearchTerm; }
            set
            {
                SetProperty(ref _organizationSearchTerm, value);
                InvokeOperation(SearchOrganization);
            }
        }

        private string _activitySearchTerm;
        public string ActivitySearchTerm
        {
            get { return _activitySearchTerm; }
            set
            {
                SetProperty(ref _activitySearchTerm, value);
                InvokeOperation(SearchActivities);
            }
        }

        private string _organizationName;
        public string OrganizationName
        {
            get { return _organizationName; }
            set { SetProperty(ref _organizationName, value); }
        }

        private string _organizationTaxationNo;
        public string OrganizationTaxationNo
        {
            get { return _organizationTaxationNo; }
            set { SetProperty(ref _organizationTaxationNo, value); }
        }

        private string _organizationAddress;
        public string OrganizationAddress
        {
            get { return _organizationAddress; }
            set { SetProperty(ref _organizationAddress, value); }
        }

        private string _organizationPhoneNo1;
        public string OrganizationPhoneNo1
        {
            get { return _organizationPhoneNo1; }
            set { SetProperty(ref _organizationPhoneNo1, value); }
        }

        private string _organizationPhoneNo2;
        public string OrganizationPhoneNo2
        {
            get { return _organizationPhoneNo2; }
            set { SetProperty(ref _organizationPhoneNo2, value); }
        }

        private string _relatedOrganizationName;
        public string RelatedOrganizationName
        {
            get { return _relatedOrganizationName; }
            set { SetProperty(ref _relatedOrganizationName, value); }
        }

        private string _activityName;
        public string ActivityName
        {
            get { return _activityName; }
            set { SetProperty(ref _activityName, value); }
        }

        private string _activityDescription;
        public string ActivityDescription
        {
            get { return _activityDescription; }
            set { SetProperty(ref _activityDescription, value); }
        }
        
        private string _activityAddress;
        public string ActivityAddress
        {
            get { return _activityAddress; }
            set { SetProperty(ref _activityAddress, value); }
        }

        private string _activityTaxationNumber;
        public string ActivityTaxationNumber
        {
            get { return _activityTaxationNumber; }
            set { SetProperty(ref _activityTaxationNumber, value); }
        }

        private string _activityPlaceName;
        public string ActivityPlaceName
        {
            get { return _activityPlaceName; }
            set { SetProperty(ref _activityPlaceName, value); }
        }

        private string _activityOtaName;
        public string ActivityOtaName
        {
            get { return _activityOtaName; }
            set { SetProperty(ref _activityOtaName, value); }
        }

        private string _activityManagerName;
        public string ActivityManagerName
        {
            get { return _activityManagerName; }
            set { SetProperty(ref _activityManagerName, value); }
        }

        private string _activityFaxNo;
        public string ActivityFaxNo
        {
            get { return _activityFaxNo; }
            set { SetProperty(ref _activityFaxNo, value); }
        }

        private string _activityMailAddress;
        public string ActivityMailAddress
        {
            get { return _activityMailAddress; }
            set { SetProperty(ref _activityMailAddress, value); }
        }

        private string _phoneNo1;
        public string PhoneNo1
        {
            get { return _phoneNo1; }
            set { SetProperty(ref _phoneNo1, value); }
        }

        private string _phoneNo2;
        public string PhoneNo2
        {
            get { return _phoneNo2; }
            set { SetProperty(ref _phoneNo2, value); }
        }

        private string _coordX;
        public string CoordX
        {
            get { return _coordX; }
            set { SetProperty(ref _coordX, value); }
        }

        private string _coordY;
        public string CoordY
        {
            get { return _coordY; }
            set { SetProperty(ref _coordY, value); }
        }

        private string _selectedOrganizationActivitiesCount;
        public string SelectedOrganizationActivitiesCount
        {
            get { return _selectedOrganizationActivitiesCount; }
            set { SetProperty(ref _selectedOrganizationActivitiesCount, value); }
        }

        private string _selectedOrganizationCasesCount;
        public string SelectedOrganizationCasesCount
        {
            get { return _selectedOrganizationCasesCount; }
            set { SetProperty(ref _selectedOrganizationCasesCount, value); }
        }

        private string _selectedActivityCasesCount;
        public string SelectedActivityCasesCount
        {
            get { return _selectedActivityCasesCount; }
            set { SetProperty(ref _selectedActivityCasesCount, value); }
        }

        private string _organizationNamePreview;
        public string OrganizationNamePreview
        {
            get { return _organizationNamePreview; }
            set { SetProperty(ref _organizationNamePreview, value); }
        }

        private string _organizationTaxationNoPreview;
        public string OrganizationTaxationNoPreview
        {
            get { return _organizationTaxationNoPreview; }
            set { SetProperty(ref _organizationTaxationNoPreview, value); }
        }

        private string _organizationAddressPreview;
        public string OrganizationAddressPreview
        {
            get { return _organizationAddressPreview; }
            set { SetProperty(ref _organizationAddressPreview, value); }
        }

        private string _organizationPhoneNo1Preview;
        public string OrganizationPhoneNo1Preview
        {
            get { return _organizationPhoneNo1Preview; }
            set { SetProperty(ref _organizationPhoneNo1Preview, value); }
        }

        private string _organizationPhoneNo2Preview;
        public string OrganizationPhoneNo2Preview
        {
            get { return _organizationPhoneNo2Preview; }
            set { SetProperty(ref _organizationPhoneNo2Preview, value); }
        }

        private string _organizationTaxOfficePreview;
        public string OrganizationTaxOfficePreview
        {
            get { return _organizationTaxOfficePreview; }
            set { SetProperty(ref _organizationTaxOfficePreview, value); }
        }

        private string _organizationRegionPreview;
        public string OrganizationRegionPreview
        {
            get { return _organizationRegionPreview; }
            set { SetProperty(ref _organizationRegionPreview, value); }
        }

        private string _organizationRegionalUnityPreview;
        public string OrganizationRegionalUnityPreview
        {
            get { return _organizationRegionalUnityPreview; }
            set { SetProperty(ref _organizationRegionalUnityPreview, value); }
        }

        private string _organizationLegalEntityCategoryPreview;
        public string OrganizationLegalEntityCategoryPreview
        {
            get { return _organizationLegalEntityCategoryPreview; }
            set { SetProperty(ref _organizationLegalEntityCategoryPreview, value); }
        }

        private string _activityOrganizationPreview;
        public string ActivityOrganizationPreview
        {
            get { return _activityOrganizationPreview; }
            set { SetProperty(ref _activityOrganizationPreview, value); }
        }

        private string _activityRegionalUnityPreview;
        public string ActivityRegionalUnityPreview
        {
            get { return _activityRegionalUnityPreview; }
            set { SetProperty(ref _activityRegionalUnityPreview, value); }
        }

        private string _activityTaxOfficePreview;
        public string ActivityTaxOfficePreview
        {
            get { return _activityTaxOfficePreview; }
            set { SetProperty(ref _activityTaxOfficePreview, value); }
        }

        private string _activityNamePreview;
        public string ActivityNamePreview
        {
            get { return _activityNamePreview; }
            set { SetProperty(ref _activityNamePreview, value); }
        }

        private string _activityWorkCategoryPreview;
        public string ActivityWorkCategoryPreview
        {
            get { return _activityWorkCategoryPreview; }
            set { SetProperty(ref _activityWorkCategoryPreview, value); }
        }

        private string _activityWorkSubcategoryPreview;
        public string ActivityWorkSubcategoryPreview
        {
            get { return _activityWorkSubcategoryPreview; }
            set { SetProperty(ref _activityWorkSubcategoryPreview, value); }
        }

        private string _activityNaceCodeSectorPreview;
        public string ActivityNaceCodeSectorPreview
        {
            get { return _activityNaceCodeSectorPreview; }
            set { SetProperty(ref _activityNaceCodeSectorPreview, value); }
        }

        private string _activityNaceCodePreview;
        public string ActivityNaceCodePreview
        {
            get { return _activityNaceCodePreview; }
            set { SetProperty(ref _activityNaceCodePreview, value); }
        }

        private string _selectedActivityPlaceName;
        public string SelectedActivityPlaceName
        {
            get { return _selectedActivityPlaceName; }
            set { SetProperty(ref _selectedActivityPlaceName, value); }
        }

        private string _selectedActivityFaxNo;
        public string SelectedActivityFaxNo
        {
            get { return _selectedActivityFaxNo; }
            set { SetProperty(ref _selectedActivityFaxNo, value); }
        }

        private string _selectedActivityManagerName;
        public string SelectedActivityManagerName
        {
            get { return _selectedActivityManagerName; }
            set { SetProperty(ref _selectedActivityManagerName, value); }
        }

        private string _selectedActivityMailAddress;
        public string SelectedActivityMailAddress
        {
            get { return _selectedActivityMailAddress; }
            set { SetProperty(ref _selectedActivityMailAddress, value); }
        }

        private string _selectedActivityOtaName;
        public string SelectedActivityOtaName
        {
            get { return _selectedActivityOtaName; }
            set { SetProperty(ref _selectedActivityOtaName, value); }
        }

        private ComboBoxItemDto _organizationRegionSelected;
        public ComboBoxItemDto OrganizationRegionSelected
        {
            get { return _organizationRegionSelected; }
            set { SetProperty(ref _organizationRegionSelected, value); }
        }

        private ComboBoxItemDto _organizationRegionalUnitySelected;
        public ComboBoxItemDto OrganizationRegionalUnitySelected
        {
            get { return _organizationRegionalUnitySelected; }
            set { SetProperty(ref _organizationRegionalUnitySelected, value); }
        }

        private ComboBoxItemDto _activityRegionalUnitySelected;
        public ComboBoxItemDto ActivityRegionalUnitySelected
        {
            get { return _activityRegionalUnitySelected; }
            set { SetProperty(ref _activityRegionalUnitySelected, value); }
        }

        private ComboBoxItemDto _legalEntitySelected;
        public ComboBoxItemDto LegalEntitySelected
        {
            get { return _legalEntitySelected; }
            set { SetProperty(ref _legalEntitySelected, value); }
        }

        private ComboBoxItemDto _organizationTaxOfficeSelected;
        public ComboBoxItemDto OrganizationTaxOfficeSelected
        {
            get { return _organizationTaxOfficeSelected; }
            set { SetProperty(ref _organizationTaxOfficeSelected, value); }
        }

        private ComboBoxItemDto _activityTaxOfficeSelected;
        public ComboBoxItemDto ActivityTaxOfficeSelected
        {
            get { return _activityTaxOfficeSelected; }
            set { SetProperty(ref _activityTaxOfficeSelected, value); }
        }

        private ComboBoxItemDto _workCategorySelected;
        public ComboBoxItemDto WorkCategorySelected
        {
            get { return _workCategorySelected; }
            set
            {
                SetProperty(ref _workCategorySelected, value);
                InvokeOperation(InitializeWorkSubcategoryForCategoryOptions);
            }
        }

        private ComboBoxItemDto _workSubcategorySelected;
        public ComboBoxItemDto WorkSubcategorySelected
        {
            get { return _workSubcategorySelected; }
            set { SetProperty(ref _workSubcategorySelected, value); }
        }

        private ComboBoxItemDto _naceCodeSectorSelected;
        public ComboBoxItemDto NaceCodeSectorSelected
        {
            get { return _naceCodeSectorSelected; }
            set
            {
                SetProperty(ref _naceCodeSectorSelected, value);
                InvokeOperation(InitializeNaceCodesForSectorOptions);
            }
        }

        private ComboBoxItemDto _naceCodeSelected;
        public ComboBoxItemDto NaceCodeSelected
        {
            get { return _naceCodeSelected; }
            set { SetProperty(ref _naceCodeSelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand OrganizationsDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareOrganizationForUpdate);
            });}
        }

        public DelegateCommand DeleteOrganizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteOrganization);
                InvokeOperation(RefreshOrganizationInputs);
            });}
        }

        public DelegateCommand AddOrganizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddOrganization);

                _forUpdateOrganization = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshOrganizationInputs);
            });}
        }

        public DelegateCommand UpdateOrganizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateOrganization);

                _forUpdateOrganization = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshOrganizationInputs);
            });}
        }

        public DelegateCommand ClearOrganizationInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshOrganizationInputs);
            });}
        }

        public DelegateCommand ActivitiesDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareActivityForUpdate);
            });}
        }

        public DelegateCommand DeleteActivityCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteActivity);
                InvokeOperation(RefreshActivityInputs);
            });}
        }

        public DelegateCommand AddActivityCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddActivity);

                _forUpdateActivity = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshActivityInputs);
            });}
        }

        public DelegateCommand UpdateActivityCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateActivity);

                _forUpdateActivity = false;
                if (OperationSuccess)
                    InvokeOperation(RefreshActivityInputs);
            });}
        }

        public DelegateCommand ClearActivityInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshActivityInputs);
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeOrganizations()
        {
            _organizationItems = Svc.OrganizationsService
                    .ListByCriteria(new OrganizationCriteria())
                    .GetDtos().ToList();

            if (Organizations != null)
            {
                Organizations.Clear();
                _organizationItems.ForEach(x => Organizations.Add(x));
            }
            else Organizations = new ObservableCollection<OrganizationDto>(_organizationItems);
        }

        private void InitializeActivities()
        {
            _activityItems = Svc.ActivitiesService
                    .ListByCriteria(new ActivityCriteria())
                    .GetDtos().ToList();

            if (Activities != null)
            {
                Activities.Clear();
                _activityItems.ForEach(x => Activities.Add(x));
            }
            else Activities = new ObservableCollection<ActivityDto>(_activityItems);
        }

        private void InitializeActivitiesForOrganization()
        {
            if (SelectedOrganization == null)
                return;

            var organizationActivities = _activityItems
                .Where(x => x.OrganizationId == SelectedOrganization.Id)
                .ToList();

            if (Activities != null)
            {
                Activities.Clear();
                organizationActivities.ForEach(x => Activities.Add(x));
            }
            else Activities = new ObservableCollection<ActivityDto>(organizationActivities);
        }

        private void InitializeRegionalUnitiesOptions()
        {
            var regionalUnityOptions = Svc.RegionalUnitiesService
                    .ListByCriteria(new RegionalUnityCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (RegionalUnitiesOptions != null)
            {
                RegionalUnitiesOptions.Clear();
                regionalUnityOptions.ToList().ForEach(x => RegionalUnitiesOptions.Add(x));
            }
            else RegionalUnitiesOptions = new ObservableCollection<ComboBoxItemDto>(regionalUnityOptions);

            RegionalUnitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Περιφερειακές Ενότητες -", Value = -1 });
            OrganizationRegionalUnitySelected = ActivityRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault();
        }

        private void InitializeRegionOptions()
        {
            var regionOptions = Svc.RegionsService
                .ListByCriteria(new RegionCriteria())
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                }).OrderBy(x => x.Text);

            if (RegionsOptions != null)
            {
                RegionsOptions.Clear();
                regionOptions.ToList().ForEach(x => RegionsOptions.Add(x));
            }
            else RegionsOptions = new ObservableCollection<ComboBoxItemDto>(regionOptions);

            RegionsOptions.Insert(0, new ComboBoxItemDto { Text = @"- Περιφέρειες -", Value = -1 });
            OrganizationRegionSelected = RegionsOptions.FirstOrDefault();
        }

        private void InitializeLegalEntitiesOptions()
        {
            var legalEntitiesOptions = Svc.LegalEntityCategoriesService
                    .ListByCriteria(new LegalEntityCategoryCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (LegalEntitiesOptions != null)
            {
                LegalEntitiesOptions.Clear();
                legalEntitiesOptions.ToList().ForEach(x => LegalEntitiesOptions.Add(x));
            }
            else LegalEntitiesOptions = new ObservableCollection<ComboBoxItemDto>(legalEntitiesOptions);

            LegalEntitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Νομικό Πρόσωπο -", Value = -1 });
            LegalEntitySelected = LegalEntitiesOptions.FirstOrDefault();
        }

        private void InitializeTaxOfficeOptions()
        {
            var taxOfficeOptions = Svc.TaxOfficesService
                    .ListByCriteria(new TaxOfficeCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (TaxOfficeOptions != null)
            {
                TaxOfficeOptions.Clear();
                taxOfficeOptions.ToList().ForEach(x => TaxOfficeOptions.Add(x));
            }
            else TaxOfficeOptions = new ObservableCollection<ComboBoxItemDto>(taxOfficeOptions);

            TaxOfficeOptions.Insert(0, new ComboBoxItemDto { Text = @"- Δ.Ο.Υ. -", Value = -1 });
            ActivityTaxOfficeSelected = OrganizationTaxOfficeSelected = TaxOfficeOptions.FirstOrDefault();
        }

        private void InitializeWorkCategoryOptions()
        {
            var workCategoryOptions = Svc.WorkCategoriesService
                    .ListByCriteria(new WorkCategoryCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = $"{x.LibraryNumber} | {x.Name}",
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (WorkCategoryOptions != null)
            {
                WorkCategoryOptions.Clear();
                workCategoryOptions.ToList().ForEach(x => WorkCategoryOptions.Add(x));
            }
            else WorkCategoryOptions = new ObservableCollection<ComboBoxItemDto>(workCategoryOptions);

            WorkCategoryOptions.Insert(0, new ComboBoxItemDto { Text = @"- Κατηγορία Έργου -", Value = -1 });
            WorkCategorySelected = WorkCategoryOptions.FirstOrDefault();
        }

        private void InitializeWorkSubcategoryOptions()
        {
            _workSubcategoriesItems = Svc.WorkSubcategoriesService
                    .ListByCriteria(new WorkSubcategoryCriteria())
                    .GetDtos().OrderBy(x => x.Name).ToList();
        }

        private void InitializeWorkSubcategoryForCategoryOptions()
        {
            var workSubcategoriesOptions = _workSubcategoriesItems
                    .Where(x => x.WorkCategoryId == (WorkCategorySelected?.Value.ToInt32() ?? 0))
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (WorkSubcategoryOptions != null)
            {
                WorkSubcategoryOptions.Clear();
                workSubcategoriesOptions.ToList().ForEach(x => WorkSubcategoryOptions.Add(x));
            }
            else WorkSubcategoryOptions = new ObservableCollection<ComboBoxItemDto>(workSubcategoriesOptions);

            WorkSubcategoryOptions.Insert(0, new ComboBoxItemDto { Text = @"- Υποκατηγορία Έργου -", Value = -1 });
            WorkSubcategorySelected = WorkSubcategoryOptions.FirstOrDefault();
        }

        private void InitializeNaceCodeSectorOptions()
        {
            var naceCodeSectorOptions = Svc.NaceCodeSectorsService
                    .ListByCriteria(new NaceCodeSectorCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = $"{x.Sector} | {x.Name}",
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (NaceCodeSectorOptions != null)
            {
                NaceCodeSectorOptions.Clear();
                naceCodeSectorOptions.ToList().ForEach(x => NaceCodeSectorOptions.Add(x));
            }
            else NaceCodeSectorOptions = new ObservableCollection<ComboBoxItemDto>(naceCodeSectorOptions);

            NaceCodeSectorOptions.Insert(0, new ComboBoxItemDto { Text = @"Κατηγορία Κ.Α.Δ.", Value = -1 });
            NaceCodeSectorSelected = NaceCodeSectorOptions.FirstOrDefault();
        }

        private void InitializeNaceCodesOptions()
        {
            _naceCodeItems = Svc.NaceCodesService
                    .ListByCriteria(new NaceCodeCriteria())
                    .GetDtos().OrderBy(x => x.Class).ToList();
        }

        private void InitializeNaceCodesForSectorOptions()
        {
            var naceCodesOptions = _naceCodeItems
                    .Where(x => x.SectorId == (NaceCodeSectorSelected?.Value.ToInt32() ?? 0))
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = $"{x.Class} - {x.Name}",
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (NaceCodeOptions != null)
            {
                NaceCodeOptions.Clear();
                naceCodesOptions.ToList().ForEach(x => NaceCodeOptions.Add(x));
            }
            else NaceCodeOptions = new ObservableCollection<ComboBoxItemDto>(naceCodesOptions);

            NaceCodeOptions.Insert(0, new ComboBoxItemDto { Text = @"Κ.Α.Δ.", Value = -1 });
            NaceCodeSelected = NaceCodeOptions.FirstOrDefault();
        }

        private void PrepareOrganizationForUpdate()
        {
            if (SelectedOrganization == null)
                return;

            _forUpdateOrganization = InsertOrganizationTabSelected = true;

            OrganizationName = SelectedOrganization.Name;
            OrganizationTaxationNo = SelectedOrganization.TaxationNo;
            OrganizationAddress = SelectedOrganization.Address;
            OrganizationPhoneNo1 = SelectedOrganization.PhoneNo1;
            OrganizationPhoneNo2 = SelectedOrganization.PhoneNo2;
            OrganizationTaxOfficeSelected =
                TaxOfficeOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedOrganization.TaxOfficeId) ??
                new ComboBoxItemDto();
            OrganizationRegionSelected =
                RegionsOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedOrganization.RegionId) ??
                new ComboBoxItemDto();
            OrganizationRegionalUnitySelected =
                RegionalUnitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedOrganization.RegionalUnityId) ??
                new ComboBoxItemDto();
            LegalEntitySelected =
                LegalEntitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedOrganization.LegalEntityCategoryId) ??
                new ComboBoxItemDto();
        }

        private void DeleteOrganization()
        {
            var deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

            if (!deletionConfirmed)
                return;
            if (SelectedOrganization.Activities.Any())
                throw new EiViewException(@"Στον Οργανισμό αυτό ανήκουν κάποιες δραστηριότητες και δεν μπορεί να διαγραφεί");

            Svc.OrganizationsService.Delete(SelectedOrganization.Id);
        }

        private void PreviewOrganization()
        {
            if (SelectedOrganization == null)
                return;

            OrganizationRegionalUnityPreview = SelectedOrganization.RegionalUnity;
            OrganizationLegalEntityCategoryPreview = SelectedOrganization.LegalEntityCategory;
            RelatedOrganizationName = OrganizationNamePreview = SelectedOrganization.Name;
            OrganizationTaxationNoPreview = SelectedOrganization.TaxationNo;
            OrganizationAddressPreview = SelectedOrganization.Address;
            OrganizationPhoneNo1Preview = SelectedOrganization.PhoneNo1;
            OrganizationPhoneNo2Preview = SelectedOrganization.PhoneNo2;
            OrganizationTaxOfficePreview = SelectedOrganization.TaxOffice;
            OrganizationRegionPreview = SelectedOrganization.Region;
            SelectedOrganizationActivitiesCount = SelectedOrganization.Activities.Count().ToString();
            SelectedOrganizationCasesCount = Svc.CasesService.ListByCriteria(new CaseCriteria
            {
                ActivityIds = SelectedOrganization.Activities.Select(x => x.Id)
            }).Count().ToString();
        }

        private void AddOrganization()
        {
            var legalEntityCategoryId = LegalEntitySelected?.Value.ToInt32() ?? 0;
            var regionalUnityId = OrganizationRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var regionId = OrganizationRegionSelected?.Value.ToInt32() ?? 0;
            var taxOfficeId = OrganizationTaxOfficeSelected?.Value.ToInt32() ?? 0;

            if (legalEntityCategoryId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη κατηγορία νομικού προσώπου");
            if (regionalUnityId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφερειακή Ενότητα");
            if (regionId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφέρεια");
            if (taxOfficeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Δ.Ο.Υ.");

            Svc.OrganizationsService.Create(new OrganizationDto
            {
                Name = OrganizationName,
                TaxationNo = OrganizationTaxationNo,
                Address = OrganizationAddress,
                PhoneNo1 = OrganizationPhoneNo1,
                PhoneNo2 = OrganizationPhoneNo2,
                TaxOfficeId = taxOfficeId,
                RegionId = regionId,
                LegalEntityCategoryId = legalEntityCategoryId,
                RegionalUnityId = regionalUnityId
            });
        }

        private void UpdateOrganization()
        {
            if (!_forUpdateOrganization)
                return;

            var legalEntityCategoryId = LegalEntitySelected?.Value.ToInt32() ?? 0;
            var regionalUnityId = OrganizationRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var regionId = OrganizationRegionSelected?.Value.ToInt32() ?? 0;
            var taxOfficeId = OrganizationTaxOfficeSelected?.Value.ToInt32() ?? 0;

            if (legalEntityCategoryId == 0 || legalEntityCategoryId == 99)
                throw new EiViewException(@"Επιλέξτε κατάλληλη κατηγορία νομικού προσώπου");
            if (regionalUnityId == 0 || regionalUnityId == 99)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφερειακή Ενότητα");
            if (regionId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφέρεια");
            if (taxOfficeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Δ.Ο.Υ.");

            Svc.OrganizationsService.Update(new OrganizationDto
            {
                Id = SelectedOrganization.Id,
                Name = OrganizationName,
                TaxationNo = OrganizationTaxationNo,
                Address = OrganizationAddress,
                PhoneNo1 = OrganizationPhoneNo1,
                PhoneNo2 = OrganizationPhoneNo2,
                TaxOfficeId = taxOfficeId,
                RegionId = regionId,
                LegalEntityCategoryId = legalEntityCategoryId,
                RegionalUnityId = regionalUnityId
            });
        }

        private void PrepareActivityForUpdate()
        {
            if (SelectedActivity == null)
                return;

            _forUpdateActivity = InsertActivityTabSelected = true;

            RelatedOrganizationName = SelectedActivity.Organization;
            ActivityName = SelectedActivity.Name;
            ActivityAddress = SelectedActivity.Address;
            ActivityDescription = SelectedActivity.Description;
            ActivityTaxationNumber = SelectedActivity.TaxNumber;
            PhoneNo1 = SelectedActivity.PhoneNo1;
            PhoneNo2 = SelectedActivity.PhoneNo2;
            ActivityFaxNo = SelectedActivity.FaxNo;
            ActivityPlaceName = SelectedActivity.PlaceName;
            ActivityMailAddress = SelectedActivity.MailAddress;
            ActivityManagerName = SelectedActivity.ManagerName;
            ActivityOtaName = SelectedActivity.OtaName;
            CoordX = SelectedActivity.CoordX.ToString("N");
            CoordY = SelectedActivity.CoordY.ToString("N");
            ActivityRegionalUnitySelected =
                RegionalUnitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.RegionalUnityId) ??
                new ComboBoxItemDto();
            ActivityTaxOfficeSelected =
                TaxOfficeOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.TaxOfficeId) ??
                new ComboBoxItemDto();
            WorkCategorySelected =
                WorkCategoryOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.WorkCategoryId) ??
                new ComboBoxItemDto();
            WorkSubcategorySelected =
                WorkSubcategoryOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.WorkSubcategoryId) ??
                new ComboBoxItemDto();
            NaceCodeSectorSelected =
                NaceCodeSectorOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.NaceCodeSectorId) ??
                new ComboBoxItemDto();
            NaceCodeSelected =
                NaceCodeOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedActivity.NaceCodeId) ??
                new ComboBoxItemDto();
        }

        private void DeleteActivity()
        {
            var deletionConfirmed =
                       @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

            if (!deletionConfirmed)
                return;

            Svc.ActivitiesService.Delete(SelectedActivity.Id);
        }

        private void PreviewActivity()
        {
            if (SelectedActivity == null)
                return;

            ActivityOrganizationPreview = SelectedActivity.Organization;
            ActivityRegionalUnityPreview = SelectedActivity.RegionalUnity;
            ActivityTaxOfficePreview = SelectedActivity.TaxOffice;
            ActivityNamePreview = SelectedActivity.Name;
            ActivityWorkCategoryPreview = SelectedActivity.WorkCategory;
            ActivityWorkSubcategoryPreview = SelectedActivity.WorkSubcategory;
            ActivityNaceCodeSectorPreview = SelectedActivity.NaceCodeSector;
            ActivityNaceCodePreview = SelectedActivity.NaceCode;
            SelectedActivityPlaceName = SelectedActivity.PlaceName;
            SelectedActivityFaxNo = SelectedActivity.FaxNo;
            SelectedActivityManagerName = SelectedActivity.ManagerName;
            SelectedActivityMailAddress = SelectedActivity.MailAddress;
            SelectedActivityOtaName = SelectedActivity.OtaName;
            SelectedActivityCasesCount = Svc.CasesService.ListByCriteria(new CaseCriteria
            {
                ActivityId = SelectedActivity.Id
            }).Count().ToString();
        }

        private void AddActivity()
        {
            var organizationId = SelectedOrganization?.Id ?? 0;
            var regionalUnityId = ActivityRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var taxOfficeId = ActivityTaxOfficeSelected?.Value.ToInt32() ?? 0;
            var workCategoryId = WorkCategorySelected?.Value.ToInt32() ?? 0;
            var workSubcategoryId = WorkSubcategorySelected?.Value.ToInt32() ?? 0;
            var naceCodeSectorId = NaceCodeSectorSelected?.Value.ToInt32() ?? 0;
            var naceCodeId = NaceCodeSelected?.Value.ToInt32() ?? 0;

            if (organizationId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλο Οργανισμό");
            if (regionalUnityId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφερειακή Ενότητα");
            if (taxOfficeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Δ.Ο.Υ.");
            if (workCategoryId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Κατηγορία Έργου");
            if (workSubcategoryId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Υποκατηγορία Έργου");
            if (naceCodeSectorId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Κατηγορία Κ.Α.Δ.");
            if (naceCodeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλο Κ.Α.Δ.");

            Svc.ActivitiesService.Create(new ActivityDto
            {
                Name = ActivityName,
                Address = ActivityAddress,
                CoordX = CoordX.ToDecimal(),
                CoordY = CoordY.ToDecimal(),
                Description = ActivityDescription,
                TaxNumber = ActivityTaxationNumber,
                PhoneNo1 = PhoneNo1,
                PhoneNo2 = PhoneNo2,
                WorkCategoryId = workCategoryId,
                WorkSubcategoryId = workSubcategoryId,
                RegionalUnityId = regionalUnityId,
                OrganizationId = organizationId,
                NaceCodeId = naceCodeId,
                NaceCodeSectorId = naceCodeSectorId,
                TaxOfficeId = taxOfficeId,
                FaxNo = ActivityFaxNo,
                MailAddress = ActivityMailAddress,
                PlaceName = ActivityPlaceName,
                ManagerName = ActivityManagerName,
                OtaName = ActivityOtaName
            });
        }

        private void UpdateActivity()
        {
            if (!_forUpdateActivity)
                return;

            var organizationId = SelectedOrganization?.Id ?? SelectedActivity?.OrganizationId ?? 0;
            var regionalUnityId = ActivityRegionalUnitySelected?.Value.ToInt32() ?? 0;
            var taxOfficeId = ActivityTaxOfficeSelected?.Value.ToInt32() ?? 0;
            var workCategoryId = WorkCategorySelected?.Value.ToInt32() ?? 0;
            var workSubcategoryId = WorkSubcategorySelected?.Value.ToInt32() ?? 0;
            var naceCodeSectorId = NaceCodeSectorSelected?.Value.ToInt32() ?? 0;
            var naceCodeId = NaceCodeSelected?.Value.ToInt32() ?? 0;

            if (organizationId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλο Οργανισμό");
            if (regionalUnityId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Περιφερειακή Ενότητα");
            if (taxOfficeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Δ.Ο.Υ.");
            if (workCategoryId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Κατηγορία Έργου");
            if (workSubcategoryId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Υποκατηγορία Έργου");
            if (naceCodeSectorId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλη Κατηγορία Κ.Α.Δ.");
            if (naceCodeId < 1)
                throw new EiViewException(@"Επιλέξτε κατάλληλο Κ.Α.Δ.");

            Svc.ActivitiesService.Update(new ActivityDto
            {
                Id = SelectedActivity?.Id ?? 0,
                Name = ActivityName,
                Address = ActivityAddress,
                CoordX = CoordX.ToDecimal(),
                CoordY = CoordY.ToDecimal(),
                Description = ActivityDescription,
                TaxNumber = ActivityTaxationNumber,
                PhoneNo1 = PhoneNo1,
                PhoneNo2 = PhoneNo2,
                WorkCategoryId = workCategoryId,
                WorkSubcategoryId = workSubcategoryId,
                RegionalUnityId = regionalUnityId,
                OrganizationId = organizationId,
                NaceCodeId = naceCodeId,
                NaceCodeSectorId = naceCodeSectorId,
                TaxOfficeId = taxOfficeId,
                FaxNo = ActivityFaxNo,
                MailAddress = ActivityMailAddress,
                PlaceName = ActivityPlaceName,
                ManagerName = ActivityManagerName,
                OtaName = ActivityOtaName
            });
        }

        private void SearchOrganization()
        {
            var searchDtos = _organizationItems
                .Where(x =>
                    x.Name.NoPunctuationGr().ToLower().Contains(OrganizationSearchTerm.NoPunctuationGr().ToLower())
                    || x.RegionalUnity.NoPunctuationGr().ToLower().Contains(OrganizationSearchTerm.NoPunctuationGr().ToLower())).ToList();

            Organizations.Clear();
            searchDtos.ForEach(x => Organizations.Add(x));
        }

        private void SearchActivities()
        {
            var searchDtos = _activityItems
                .Where(x =>
                    x.Name.NoPunctuationGr().ToLower().Contains(ActivitySearchTerm.NoPunctuationGr().ToLower())
                    || x.Description.NoPunctuationGr().ToLower().Contains(ActivitySearchTerm.NoPunctuationGr().ToLower())
                    || x.Address.NoPunctuationGr().ToLower().Contains(ActivitySearchTerm.NoPunctuationGr().ToLower())
                    || x.TaxNumber.Contains(ActivitySearchTerm)).ToList();

            Activities.Clear();
            searchDtos.ForEach(x => Activities.Add(x));
        }

        private void RefreshOrganizationInputs()
        {
            _forUpdateOrganization = false;

            OrganizationName = string.Empty;
            OrganizationAddress = string.Empty;
            OrganizationTaxationNo = string.Empty;
            OrganizationPhoneNo1 = string.Empty;
            OrganizationPhoneNo2 = string.Empty;

            OrganizationRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault();
            OrganizationRegionSelected = RegionsOptions.FirstOrDefault();
            OrganizationTaxOfficeSelected = TaxOfficeOptions.FirstOrDefault();
            LegalEntitySelected = LegalEntitiesOptions.FirstOrDefault();

            InvokeOperation(InitializeOrganizations);
        }

        private void RefreshActivityInputs()
        {
            _forUpdateActivity = false;

            RelatedOrganizationName = string.Empty;
            ActivityName = string.Empty;
            ActivityDescription = string.Empty;
            ActivityAddress = string.Empty;
            ActivityTaxationNumber = string.Empty;
            PhoneNo1 = string.Empty;
            PhoneNo2 = string.Empty;
            ActivityFaxNo = string.Empty;
            ActivityMailAddress = string.Empty;
            ActivityPlaceName = string.Empty;
            ActivityManagerName = string.Empty;
            ActivityOtaName = string.Empty;
            CoordX = string.Empty;
            CoordY = string.Empty;

            ActivityRegionalUnitySelected = RegionalUnitiesOptions.FirstOrDefault();
            ActivityTaxOfficeSelected = TaxOfficeOptions.FirstOrDefault();
            WorkCategorySelected = WorkCategoryOptions.FirstOrDefault();
            WorkSubcategorySelected = WorkSubcategoryOptions.FirstOrDefault();
            NaceCodeSectorSelected = NaceCodeSectorOptions.FirstOrDefault();
            NaceCodeSelected = NaceCodeOptions.FirstOrDefault();

            InvokeOperation(InitializeActivities);
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            OrganizationName = string.Empty;
            RelatedOrganizationName = string.Empty;
            ActivityName = string.Empty;
            ActivityDescription = string.Empty;
            ActivityAddress = string.Empty;
            ActivityTaxationNumber = string.Empty;
            PhoneNo1 = string.Empty;
            PhoneNo2 = string.Empty;
            CoordX = string.Empty;
            CoordY = string.Empty;

            InvokeOperationsParallel(new Action[]
            {
                InitializeOrganizations,
                InitializeActivities,
                InitializeLegalEntitiesOptions,
                InitializeRegionalUnitiesOptions,
                InitializeRegionOptions,
                InitializeTaxOfficeOptions,
                InitializeWorkCategoryOptions,
                InitializeWorkSubcategoryOptions,
                InitializeNaceCodeSectorOptions,
                InitializeNaceCodesOptions
            });
        }
    }
}
