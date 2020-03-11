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

    public interface IWorkCategoriesViewModel : IBaseViewModel { }

    public class WorkCategoriesViewModel : BaseViewModel, IWorkCategoriesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<WorkCategoryDto> _workCategoryItems;
        #endregion

        #region Constructor
        public WorkCategoriesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<ComboBoxItemDto> AutopsyDocumentCategoriesOptions { get; set; }
        public ObservableCollection<AutopsyDocumentCategoryDto> AutopsyDocumentCategoriesForWorkCategory { get; set; }
        public ObservableCollection<WorkCategoryDto> WorkCategories { get; set; }
        public AutopsyDocumentCategoryDto SelectedAutopsyDocumentCategoryForWorkCategory { get; set; }

        private WorkCategoryDto _selectedWorkCategory;
        public WorkCategoryDto SelectedWorkCategory
        {
            get { return _selectedWorkCategory; }
            set { SetProperty(ref _selectedWorkCategory, value); }
        }

        private ComboBoxItemDto _autopsyDocumentCategorySelected;
        public ComboBoxItemDto AutopsyDocumentCategorySelected
        {
            get { return _autopsyDocumentCategorySelected; }
            set { SetProperty(ref _autopsyDocumentCategorySelected, value); }
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

        private string _libraryNumber;
        public string LibraryNumber
        {
            get { return _libraryNumber; }
            set { SetProperty(ref _libraryNumber, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddWorkCategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddWorkCategory);
                });
            }
        }

        public DelegateCommand UpdateWorkCategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateWorkCategory);
                });
            }
        }

        public DelegateCommand DeleteWorkCategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteWorkCategory);
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
        private void InitializeWorkCategories()
        {
            _workCategoryItems = Svc.WorkCategoriesService
                .ListByCriteria(new WorkCategoryCriteria())
                .GetDtos();

            if (WorkCategories != null)
            {
                WorkCategories.Clear();
                _workCategoryItems.ToList().ForEach(x => WorkCategories.Add(x));
            }
            else WorkCategories = new ObservableCollection<WorkCategoryDto>(_workCategoryItems);
        }

        private void InitializeAutopsyDocumentCategoriesOptions()
        {
            var autopsyDocCategoriesOptions = Svc.AutopsyDocumentCategoriesService
                .ListByCriteria(new AutopsyDocumentCategoryCriteria())
                .Select(x => new ComboBoxItemDto
                {
                    Text = x.Name,
                    Value = x.Id
                });

            if (AutopsyDocumentCategoriesOptions != null)
            {
                AutopsyDocumentCategoriesOptions.Clear();
                autopsyDocCategoriesOptions.ToList().ForEach(x => AutopsyDocumentCategoriesOptions.Add(x));
            }
            else AutopsyDocumentCategoriesOptions = new ObservableCollection<ComboBoxItemDto>(autopsyDocCategoriesOptions);

            AutopsyDocumentCategoriesOptions.Insert(0, new ComboBoxItemDto { Text = @"- Κατηγορία Εντύπων Αυτοψίας -", Value = -1 });
            AutopsyDocumentCategorySelected = AutopsyDocumentCategoriesOptions.FirstOrDefault();
        }

        private void AddWorkCategory()
        {
            try
            {
                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);

                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.WorkCategoriesService.Create(new WorkCategoryDto { Name = Name, LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateWorkCategory()
        {
            try
            {
                if (!_forUpdate)
                    return;

                int outInt;
                var isNumber = int.TryParse(LibraryNumber, out outInt);

                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.WorkCategoriesService.Update(new WorkCategoryDto { Id = SelectedWorkCategory.Id, Name = Name, LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteWorkCategory()
        {
            try
            {
                var deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedWorkCategoryId = SelectedWorkCategory?.Id ?? 0;
                Svc.WorkCategoriesService.Delete(selectedWorkCategoryId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedWorkCategory == null)
                return;

            _forUpdate = true;
            Name = SelectedWorkCategory.Name;
            LibraryNumber = SelectedWorkCategory.LibraryNumber;
        }

        private void Search()
        {
            var searchDtos = _workCategoryItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr())
                        || x.LibraryNumber.Contains(LibraryNumber))
                    .ToList();

            WorkCategories.Clear();
            searchDtos.ForEach(x => WorkCategories.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Name = string.Empty;
            LibraryNumber = string.Empty;
            InvokeOperation(InitializeWorkCategories);
            InvokeOperation(InitializeAutopsyDocumentCategoriesOptions);
        }
    }
}
