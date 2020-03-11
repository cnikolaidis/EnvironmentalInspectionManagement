namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels
{
    #region Usings
    using RenderViewModels;
    using System.Linq;
    using System;
    #endregion

    public static class Vm
    {
        #region ViewModel Registry
        public static ILogViewModel LogViewModel => Get<ILogViewModel>();
        public static IMainViewModel MainViewModel => Get<IMainViewModel>();
        public static IStatisticsViewModel StatisticsViewModel => Get<IStatisticsViewModel>();
        public static IDictionaryViewModel DictionaryViewModel => Get<IDictionaryViewModel>();
        public static IOrganizationsViewModel OrganizationsViewModel => Get<IOrganizationsViewModel>();
        public static ICasesViewModel CasesViewModel => Get<ICasesViewModel>();
        public static IUserManagementViewModel UserManagementViewModel => Get<IUserManagementViewModel>();
        public static IPreferencesViewModel PreferencesViewModel => Get<IPreferencesViewModel>();
        public static IOtherDictionariesViewModel OtherDictionariesViewModel => Get<IOtherDictionariesViewModel>();
        public static INaceCodeSectorsViewModel NaceCodeSectorsViewModel => Get<INaceCodeSectorsViewModel>();
        public static INaceCodesViewModel NaceCodesViewModel => Get<INaceCodesViewModel>();
        public static IInspectorsViewModel InspectorsViewModel => Get<IInspectorsViewModel>();
        public static IRegionalUnitiesViewModel RegionalUnitiesViewModel => Get<IRegionalUnitiesViewModel>();
        public static ITaxOfficesViewModel TaxOfficesViewModel => Get<ITaxOfficesViewModel>();
        public static IWorkSubcategoriesViewModel WorkSubcategoriesViewModel => Get<IWorkSubcategoriesViewModel>();
        public static ILogMonitoringViewModel LogMonitoringViewModel => Get<ILogMonitoringViewModel>();
        public static IRegionsViewModel RegionsViewModel => Get<IRegionsViewModel>();
        public static IWorkCategoriesViewModel WorkCategoriesViewModel => Get<IWorkCategoriesViewModel>();
        public static IAutopsyDocumentCategoriesViewModel AutopsyDocumentCategoriesViewModel => Get<IAutopsyDocumentCategoriesViewModel>();
        #endregion

        private static T Get<T>() where T : class
        {
            object vmInstance = null;

            var vmClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => !x.IsInterface)
                .FirstOrDefault(x => !x.IsAbstract);

            if (vmClass != null)
                vmInstance = Activator.CreateInstance(vmClass);

            return vmInstance as T;
        }
    }
}
