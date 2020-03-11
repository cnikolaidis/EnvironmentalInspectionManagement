namespace EnvironmentalInspectionManagement.DataAccessLayer.Core
{
    #region Usings
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity;
    using Models.Entities;
    #endregion

    public class EiDbContext : DbContext
    {
        #region Constructor
        public EiDbContext(string connStr = null)
        {
            Database.SetInitializer<EiDbContext>(null);

            if (!string.IsNullOrEmpty(connStr))
                Database.Connection.ConnectionString =
                    System.Configuration.ConfigurationManager.ConnectionStrings[connStr].ConnectionString;

            Configuration.LazyLoadingEnabled = false;
        }
        #endregion

        #region DbSets
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupAuthorizationMap> GroupAuthorizationMaps { get; set; }
        public DbSet<AdditionalAction> AdditionalActions { get; set; }
        public DbSet<CourtDecision> CourtDecisions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<AdditionalActAutopsyMap> AdditionalActsAutopsyMaps { get; set; }
        public DbSet<AutopsyDocumentCategory> AutopsyDocumentCategories { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ControlProgress> ControlProgresses { get; set; }
        public DbSet<ControlTrigger> ControlTriggers { get; set; }
        public DbSet<CourtDecisionAutopsyMap> CourtDecisionAutopsyMaps { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCaseMap> DocumentCaseMaps { get; set; }
        public DbSet<Inspector> Inspectors { get; set; }
        public DbSet<InspectorAutopsyMap> InspectorAutopsyMaps { get; set; }
        public DbSet<InspectorCaseMap> InspectorCaseMaps { get; set; }
        public DbSet<LegalEntityCategory> LegalEntityCategories { get; set; }
        public DbSet<NaceCode> NaceCodes { get; set; }
        public DbSet<NaceCodeSector> NaceCodeSectors { get; set; }
        public DbSet<NaturaRegion> NaturaRegions { get; set; }
        public DbSet<NaturaRegionActivityMap> NaturaRegionActivityMaps { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionalUnity> RegionalUnities { get; set; }
        public DbSet<Subordination> Subordinations { get; set; }
        public DbSet<SubordinationActivityMap> SubordinationActivityMaps { get; set; }
        public DbSet<TaxOffice> TaxOffices { get; set; }
        public DbSet<WorkCategory> WorkCategories { get; set; }
        public DbSet<WorkSubcategory> WorkSubcategories { get; set; }
        public DbSet<Autopsy> Autopsies { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<Indictment> Indictments { get; set; }
        public DbSet<InspectorsControlsMap> InspectorsControlsMaps { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<GeneralDocument> GeneralDocuments { get; set; }
        public DbSet<ControlType> ControlTypes { get; set; }
        public DbSet<PriorityDegree> PriorityDegrees { get; set; }
        public DbSet<SafetyDegree> SafetyDegrees { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<AutopsyLicenseMap> AutopsyLicenseMaps { get; set; }
        #endregion

        #region DbContext Events
        protected override void OnModelCreating(DbModelBuilder modelBuilder) => modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        #endregion
    }
}
