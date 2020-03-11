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

    public interface ITaxOfficesViewModel : IBaseViewModel { }

    public class TaxOfficesViewModel : BaseViewModel, ITaxOfficesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<TaxOfficeDto> _taxOfficeItems;
        #endregion

        #region Constructor
        public TaxOfficesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<TaxOfficeDto> TaxOffices { get; set; }
        public ObservableCollection<ComboBoxItemDto> CitiesOptions { get; set; }
        public TaxOfficeDto SelectedTaxOffice { get; set; }

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

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set { SetProperty(ref _postalCode, value); }
        }

        private string _phoneNo;
        public string PhoneNo
        {
            get { return _phoneNo; }
            set { SetProperty(ref _phoneNo, value); }
        }

        private ComboBoxItemDto _citySelected;
        public ComboBoxItemDto CitySelected
        {
            get { return _citySelected; }
            set { SetProperty(ref _citySelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddTaxOfficeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddTaxOffice);
                });
            }
        }

        public DelegateCommand UpdateTaxOfficeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateTaxOffice);
                });
            }
        }

        public DelegateCommand DeleteTaxOfficeCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteTaxOffice);
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
        private void InitializeTaxOffices()
        {
            _taxOfficeItems = Svc.TaxOfficesService
                     .ListByCriteria(new TaxOfficeCriteria())
                     .GetDtos();

            if (TaxOffices != null)
            {
                TaxOffices.Clear();
                _taxOfficeItems.ToList().ForEach(x => TaxOffices.Add(x));
            }
            else TaxOffices = new ObservableCollection<TaxOfficeDto>(_taxOfficeItems);
        }

        private void InitializeCitiesOptions()
        {
            var citiesList = Svc.CitiesService
                    .ListByCriteria(new CityCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    }).OrderBy(x => x.Text);

            if (CitiesOptions != null)
            {
                CitiesOptions.Clear();
                citiesList.ToList().ForEach(x => CitiesOptions.Add(x));
            }
            else CitiesOptions = new ObservableCollection<ComboBoxItemDto>(citiesList);

            CitiesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Πόλη -", Value = -1 });
            CitySelected = CitiesOptions.FirstOrDefault();
        }

        private void AddTaxOffice()
        {
            try
            {
                if (CitySelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Πόλη");

                Svc.TaxOfficesService.Create(new TaxOfficeDto
                {
                    Name = Name,
                    Description = Description,
                    Address = Address,
                    PostalCode = PostalCode.ToInt32(),
                    PhoneNo = PhoneNo,
                    CityId = CitySelected.Value.ToInt32()
                });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateTaxOffice()
        {
            try
            {
                if (!_forUpdate)
                    return;

                if (CitySelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Πόλη");

                Svc.TaxOfficesService.Update(new TaxOfficeDto
                {
                    Id = SelectedTaxOffice.Id,
                    Name = Name,
                    Description = Description,
                    Address = Address,
                    PostalCode = PostalCode.ToInt32(),
                    PhoneNo = PhoneNo,
                    CityId = CitySelected.Value.ToInt32()
                });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteTaxOffice()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedTaxOfficeId = SelectedTaxOffice?.Id ?? 0;
                Svc.TaxOfficesService.Delete(selectedTaxOfficeId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedTaxOffice != null)
            {
                _forUpdate = true;

                Name = SelectedTaxOffice.Name;
                Description = SelectedTaxOffice.Description;
                Address = SelectedTaxOffice.Address;
                PostalCode = SelectedTaxOffice.PostalCode.ToString();
                PhoneNo = SelectedTaxOffice.PhoneNo;
                CitySelected = CitiesOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedTaxOffice.CityId) ??
                               new ComboBoxItemDto();
            }
        }

        private void Search()
        {
            var searchDtos = _taxOfficeItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                    .ToList();

            TaxOffices.Clear();
            searchDtos.ForEach(x => TaxOffices.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            Description = string.Empty;
            Address = string.Empty;
            PostalCode = string.Empty;
            PhoneNo = string.Empty;
            InvokeOperation(InitializeCitiesOptions);
            InvokeOperation(InitializeTaxOffices);
        }
    }
}
