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

    public interface IWorkSubcategoriesViewModel : IBaseViewModel { }

    public class WorkSubcategoriesViewModel : BaseViewModel, IWorkSubcategoriesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<WorkSubcategoryDto> _workSubcategoriesItems;
        #endregion

        #region Constructor
        public WorkSubcategoriesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<WorkSubcategoryDto> WorkSubcategories { get; set; }
        public ObservableCollection<ComboBoxItemDto> WorkCategoryOptions { get; set; }
        public WorkSubcategoryDto SelectedWorkSubcategory { get; set; }

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

        private ComboBoxItemDto _workCategorySelected;
        public ComboBoxItemDto WorkCategorySelected
        {
            get { return _workCategorySelected; }
            set { SetProperty(ref _workCategorySelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddWorkSubcategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(AddWorkSubcategory);
                });
            }
        }

        public DelegateCommand UpdateWorkSubcategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(UpdateWorkSubcategory);
                });
            }
        }

        public DelegateCommand DeleteWorkSubcategoryCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    InvokeOperation(DeleteWorkSubcategory);
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
        private void InitializeWorkSubcategories()
        {
            _workSubcategoriesItems = Svc.WorkSubcategoriesService
                    .ListByCriteria(new WorkSubcategoryCriteria())
                    .GetDtos();

            if (WorkSubcategories != null)
            {
                WorkSubcategories.Clear();
                _workSubcategoriesItems.ToList().ForEach(x => WorkSubcategories.Add(x));
            }
            else WorkSubcategories = new ObservableCollection<WorkSubcategoryDto>(_workSubcategoriesItems);
        }

        private void InitializeWorkCategoryOptions()
        {
            var workCategoryList = Svc.WorkCategoriesService
                    .ListByCriteria(new WorkCategoryCriteria())
                    .OrderBy(x => x.LibraryNumber)
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = $"{x.LibraryNumber} | {x.Name}",
                        Value = x.Id
                    });

            if (WorkCategoryOptions != null)
            {
                WorkCategoryOptions.Clear();
                workCategoryList.ToList().ForEach(x => WorkCategoryOptions.Add(x));
            }
            else WorkCategoryOptions = new ObservableCollection<ComboBoxItemDto>(workCategoryList);

            WorkCategoryOptions.Insert(0, new ComboBoxItemDto { Text = @"- Κατηγορία Έργου -", Value = -1 });
            WorkCategorySelected = WorkCategoryOptions.FirstOrDefault();
        }

        private void AddWorkSubcategory()
        {
            try
            {
                var isNumber = int.TryParse(LibraryNumber, out _);

                if (WorkCategorySelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Κατηγορία Έργου");
                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.WorkSubcategoriesService.Create(new WorkSubcategoryDto { Name = Name, WorkCategoryId = WorkCategorySelected.Value.ToInt32(), LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateWorkSubcategory()
        {
            try
            {
                if (!_forUpdate)
                    return;

                var isNumber = int.TryParse(LibraryNumber, out _);

                if (WorkCategorySelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Επιλέξτε σωστή Περιφέρεια");
                if (!isNumber)
                    throw new EiViewException(@"Ο Κωδικός πρέπει να είναι ακέραιος αριθμός");

                Svc.WorkSubcategoriesService.Update(new WorkSubcategoryDto { Id = SelectedWorkSubcategory.Id, Name = Name, WorkCategoryId = WorkCategorySelected.Value.ToInt32(), LibraryNumber = LibraryNumber });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteWorkSubcategory()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedWorkSubcategoryId = SelectedWorkSubcategory?.Id ?? 0;
                Svc.WorkSubcategoriesService.Delete(selectedWorkSubcategoryId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedWorkSubcategory != null)
            {
                _forUpdate = true;

                Name = SelectedWorkSubcategory.Name;
                LibraryNumber = SelectedWorkSubcategory.LibraryNumber;
                WorkCategorySelected =
                    WorkCategoryOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedWorkSubcategory.WorkCategoryId) ??
                    new ComboBoxItemDto();
            }
        }

        private void Search()
        {
            var searchDtos = _workSubcategoriesItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr())
                        || x.LibraryNumber.Contains(LibraryNumber))
                    .ToList();

            WorkSubcategories.Clear();
            searchDtos.ForEach(x => WorkSubcategories.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            Name = string.Empty;
            LibraryNumber = string.Empty;
            InvokeOperation(InitializeWorkCategoryOptions);
            InvokeOperation(InitializeWorkSubcategories);
        }
    }
}
