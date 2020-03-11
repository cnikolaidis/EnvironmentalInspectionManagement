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

    public interface IAutopsyDocumentCategoriesViewModel : IBaseViewModel { }

    public class AutopsyDocumentCategoriesViewModel : BaseViewModel, IAutopsyDocumentCategoriesViewModel
    {
        #region Properties
        private static bool _forUpdate;
        private static IEnumerable<AutopsyDocumentCategoryDto> _autopsyDocumentCategoriesItems;
        #endregion

        #region Constructor
        public AutopsyDocumentCategoriesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<AutopsyDocumentCategoryDto> AutopsyDocumentCategories { get; set; }
        public AutopsyDocumentCategoryDto SelectedAutopsyDocumentCategory { get; set; }

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

        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddAutopsyDocumentCategoryCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddAutopsyDocumentCategory);
            });}
        }

        public DelegateCommand UpdateAutopsyDocumentCategoryCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateAutopsyDocumentCategory);
            });}
        }

        public DelegateCommand ClearInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshInputs);
            });}
        }

        public DelegateCommand DeleteAutopsyDocumentCategoryCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteAutopsyDocumentCategory);
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
        private void InitializeAutopsyDocumentCategories()
        {
            _autopsyDocumentCategoriesItems = Svc.AutopsyDocumentCategoriesService
                .ListByCriteria(new AutopsyDocumentCategoryCriteria())
                .GetDtos();

            if (AutopsyDocumentCategories != null)
            {
                AutopsyDocumentCategories.Clear();
                _autopsyDocumentCategoriesItems.ToList().ForEach(x => AutopsyDocumentCategories.Add(x));
            }
            else AutopsyDocumentCategories = new ObservableCollection<AutopsyDocumentCategoryDto>(_autopsyDocumentCategoriesItems);
        }

        private void AddAutopsyDocumentCategory()
        {
            try
            {
                Svc.AutopsyDocumentCategoriesService.Create(new AutopsyDocumentCategoryDto { Code = Code, Name = Name, Version = Version });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateAutopsyDocumentCategory()
        {
            try
            {
                if (_forUpdate)
                    Svc.AutopsyDocumentCategoriesService.Update(new AutopsyDocumentCategoryDto { Id = SelectedAutopsyDocumentCategory?.Id ?? 0, Code = Code, Name = Name, Version = Version });
            }
            finally
            {
                _forUpdate = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteAutopsyDocumentCategory()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο στοιχείο;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                var selectedAutopsyDocumentCategoryId = SelectedAutopsyDocumentCategory?.Id ?? 0;
                Svc.AutopsyDocumentCategoriesService.Delete(selectedAutopsyDocumentCategoryId);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedAutopsyDocumentCategory != null)
            {
                _forUpdate = true;
                Name = SelectedAutopsyDocumentCategory.Name;
                Code = SelectedAutopsyDocumentCategory.Code;
                Version = SelectedAutopsyDocumentCategory.Version;
            }
        }

        private void Search()
        {
            var searchDtos = _autopsyDocumentCategoriesItems
                    .Where(x => x.Name.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                    .ToList();

            AutopsyDocumentCategories.Clear();
            searchDtos.ForEach(x => AutopsyDocumentCategories.Add(x));
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdate = false;

            Code = string.Empty;
            Name = string.Empty;
            Version = string.Empty;
            InvokeOperation(InitializeAutopsyDocumentCategories);
        }
    }
}
