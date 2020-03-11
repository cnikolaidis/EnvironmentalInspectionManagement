namespace EnvironmentalInspectionManagement.DataAccessLayer.Services
{
    #region Usings
    using DbReaderServices;
    using GenericServices;
    using EntityServices;
    using System.Linq;
    using System;
    #endregion

    public static class Svc
    {
        #region GenericServices Registry
        public static IAppSettingsService AppSettingsService => Get<IAppSettingsService>();
        public static ILoggerService LoggerService => Get<ILoggerService>();
        public static IAuthenticationService AuthenticationService => Get<IAuthenticationService>();
        #endregion

        #region DbReaderServices Registry
        public static IExcelDatabaseService ExcelDbReaderService => Get<IExcelDatabaseService>();
        #endregion

        #region EntityServices Registry
        public static IActivityService ActivitiesService => Get<IActivityService>();
        public static IAdditionalActAutopsyMapService AdditionalActAutopsyMapsService => Get<IAdditionalActAutopsyMapService>();
        public static IAdditionalActionsService AdditionalActionsService => Get<IAdditionalActionsService>();
        public static IAuthorizationsService AuthorizationService => Get<IAuthorizationsService>();
        public static IAutopsyDocumentCategoriesService AutopsyDocumentCategoriesService => Get<IAutopsyDocumentCategoriesService>();
        public static IAutopsyService AutopsiesService => Get<IAutopsyService>();
        public static ICasesService CasesService => Get<ICasesService>();
        public static ICitiesService CitiesService => Get<ICitiesService>();
        public static IControlProgressesService ControlProgressesService => Get<IControlProgressesService>();
        public static IControlTriggersService ControlTriggersService => Get<IControlTriggersService>();
        public static ICourtDecisionsService CourtDecisionsService => Get<ICourtDecisionsService>();
        public static ICourtDecisionAutopsyMapsService CourtDecisionAutopsyMapsService => Get<ICourtDecisionAutopsyMapsService>();
        public static IDocumentTypesService DocumentTypesService => Get<IDocumentTypesService>();
        public static IDocumentCaseMapsService DocumentCaseMapsService => Get<IDocumentCaseMapsService>();
        public static IDocumentsService DocumentsService => Get<IDocumentsService>();
        public static IGroupAuthorizationsMapService GroupAuthorizationsMapsService => Get<IGroupAuthorizationsMapService>();
        public static IInspectorAutopsyMapsService InspectorAutopsyMapsService => Get<IInspectorAutopsyMapsService>();
        public static IInspectorCaseMapsService InspectorCaseMapsService => Get<IInspectorCaseMapsService>();
        public static IInspectorsService InspectorsService => Get<IInspectorsService>();
        public static ILegalEntityCategoriesService LegalEntityCategoriesService => Get<ILegalEntityCategoriesService>();
        public static INaceCodeSectorsService NaceCodeSectorsService => Get<INaceCodeSectorsService>();
        public static INaceCodesService NaceCodesService => Get<INaceCodesService>();
        public static INaturaRegionsService NaturaRegionsService => Get<INaturaRegionsService>();
        public static INaturaRegionActivityMapsService NaturaRegionActivityMapsService => Get<INaturaRegionActivityMapsService>();
        public static IOrganizationsService OrganizationsService => Get<IOrganizationsService>();
        public static IRegionalUnitiesService RegionalUnitiesService => Get<IRegionalUnitiesService>();
        public static IRegionsService RegionsService => Get<IRegionsService>();
        public static ISubordinationsService SubordinationsService => Get<ISubordinationsService>();
        public static ISubordinationActivityMapsService SubordinationActivityMapsService => Get<ISubordinationActivityMapsService>();
        public static ITaxOfficesService TaxOfficesService => Get<ITaxOfficesService>();
        public static IUserGroupsService UserGroupsService => Get<IUserGroupsService>();
        public static IUsersService UsersService => Get<IUsersService>();
        public static IWorkCategoriesService WorkCategoriesService => Get<IWorkCategoriesService>();
        public static IWorkSubcategoriesService WorkSubcategoriesService => Get<IWorkSubcategoriesService>();
        public static IIndictmentsService IndictmentsService => Get<IIndictmentsService>();
        public static IControlsService ControlsService => Get<IControlsService>();
        public static IInspectorsControlsMapService InspectorsControlsMapService => Get<IInspectorsControlsMapService>();
        public static IFindingsService FindingsService => Get<IFindingsService>();
        public static IGeneralDocumentsService GeneralDocumentsService => Get<IGeneralDocumentsService>();
        public static IControlTypesService ControlTypesService => Get<IControlTypesService>();
        public static ISafetyDegreesService SafetyDegreesService => Get<ISafetyDegreesService>();
        public static IPriorityDegreesService PriorityDegreesService => Get<IPriorityDegreesService>();
        public static ILicensesService LicensesService => Get<ILicensesService>();
        public static IAutopsyLicenseMapsService AutopsyLicenseMapsService => Get<IAutopsyLicenseMapsService>();
        #endregion

        private static T Get<T>() where T : class
        {
            object svcInstance = null;

            var svcClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Where(x => !x.IsInterface)
                .FirstOrDefault(x => !x.IsAbstract);

            if (svcClass != null)
                svcInstance = Activator.CreateInstance(svcClass);

            return svcInstance as T;
        }
    }
}
