namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Models.OtherObjects;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using DataAccessLayer.Models;
    using System.Linq;
    using Utilities;
    #endregion

    public interface IOtherDictionariesViewModel : IBaseViewModel { }

    public class OtherDictionariesViewModel : BaseViewModel, IOtherDictionariesViewModel
    {
        #region Properties
        private static int _selectedLibraryId;
        #endregion

        #region Constructors
        public OtherDictionariesViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<ComboBoxItemDto> Libraries { get; set; }

        private ComboBoxItemDto _selectedLibrary;
        public ComboBoxItemDto SelectedLibrary
        {
            get { return _selectedLibrary; }
            set
            {
                SetProperty(ref _selectedLibrary, value);
                _selectedLibraryId = SelectedLibrary?.Value.ToInt32() ?? _selectedLibraryId;
                InvokeOperation(RenderSelectedLibrary);
            }
        }

        private IBaseViewModel _viewModel;
        public IBaseViewModel ViewModel
        {
            get { return _viewModel; }
            set { SetProperty(ref _viewModel, value); }
        }
        #endregion

        #region Operation Methods
        private void InitializeLibraries()
        {
            var namesList = new List<ComboBoxItemDto>
                {
                    new ComboBoxItemDto { Text = @"Κατηγορίες Κ.Α.Δ.", Value = 0 },
                    new ComboBoxItemDto { Text = @"Κ.Α.Δ.", Value = 1 },
                    new ComboBoxItemDto { Text = @"Επιθεωρητές", Value = 2 },
                    new ComboBoxItemDto { Text = @"Περιφερειακές Ενότητες", Value = 3 },
                    new ComboBoxItemDto { Text = @"Δ.Ο.Υ.", Value = 5 },
                    new ComboBoxItemDto { Text = @"Υποκατηγορίες Έργων", Value = 6 },
                    new ComboBoxItemDto { Text = @"Περιφέρειες", Value = 7 },
                    new ComboBoxItemDto { Text = @"Κατηγορίες Έργων", Value = 8 },
                    new ComboBoxItemDto { Text = @"Έντυπα Αυτοψιών", Value = 9 }
                };

            if (Libraries != null)
            {
                Libraries.Clear();
                namesList.ForEach(x => Libraries.Add(x));
            }
            else Libraries = new ObservableCollection<ComboBoxItemDto>(namesList);

            SelectedLibrary = namesList.FirstOrDefault(x => x.Value.ToInt32() == _selectedLibraryId) ?? Libraries.FirstOrDefault();
        }

        private void RenderSelectedLibrary()
        {
            var selectedLibrary = SelectedLibrary?.Value.ToInt32() ?? 0;
            switch (selectedLibrary)
            {
                case (int)DictionaryLibraries.NaceCodeSectors:
                    InvokeOperation(RenderNaceCodeSectorsView);
                    break;
                case (int)DictionaryLibraries.NaceCodes:
                    InvokeOperation(RenderNaceCodesView);
                    break;
                case (int)DictionaryLibraries.Inspectors:
                    InvokeOperation(RenderInspectorsView);
                    break;
                case (int)DictionaryLibraries.RegionalUnities:
                    InvokeOperation(RenderRegionalUnitiesView);
                    break;
                case (int)DictionaryLibraries.TaxOffices:
                    InvokeOperation(RenderTaxOfficesView);
                    break;
                case (int)DictionaryLibraries.WorkSubcategories:
                    InvokeOperation(RenderWorkSubcategoriesView);
                    break;
                case (int)DictionaryLibraries.Regions:
                    InvokeOperation(RenderRegionsView);
                    break;
                case (int)DictionaryLibraries.WorkCategories:
                    InvokeOperation(RenderWorkCategoriesView);
                    break;
                case (int)DictionaryLibraries.AutopsyDocumentCategories:
                    InvokeOperation(RenderAutopsyDocumentCategoriesView);
                    break;
            }
        }

        private void RenderNaceCodeSectorsView()
        {
            ViewModel = Vm.NaceCodeSectorsViewModel;
        }

        private void RenderNaceCodesView()
        {
            ViewModel = Vm.NaceCodesViewModel;
        }

        private void RenderInspectorsView()
        {
            ViewModel = Vm.InspectorsViewModel;
        }

        private void RenderRegionalUnitiesView()
        {
            ViewModel = Vm.RegionalUnitiesViewModel;
        }

        private void RenderTaxOfficesView()
        {
            ViewModel = Vm.TaxOfficesViewModel;
        }

        private void RenderWorkSubcategoriesView()
        {
            ViewModel = Vm.WorkSubcategoriesViewModel;
        }

        private void RenderRegionsView()
        {
            ViewModel = Vm.RegionsViewModel;
        }

        private void RenderWorkCategoriesView()
        {
            ViewModel = Vm.WorkCategoriesViewModel;
        }

        private void RenderAutopsyDocumentCategoriesView()
        {
            ViewModel = Vm.AutopsyDocumentCategoriesViewModel;
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            ViewModel = null;
            InvokeOperation(InitializeLibraries);
        }
    }
}
