namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    public enum NamedTables
    {
        DocumentTypes = 0,
        AdditionalActions = 1,
        CourtDecisions = 2,
        ControlTriggers = 3,
        ControlProgresses = 4,
        Cities = 5,
        LegalEntityCategories = 7,
        NaturaRegions = 8,
        Subordinations = 9,
        ControlTypes = 12,
        SafetyDegrees = 13,
        PriorityDegrees = 14
    }

    public enum DictionaryLibraries
    {
        NaceCodeSectors = 0,
        NaceCodes = 1,
        Inspectors = 2,
        RegionalUnities = 3,
        TaxOffices = 5,
        WorkSubcategories = 6,
        Regions = 7,
        WorkCategories = 8,
        AutopsyDocumentCategories = 9
    }

    public enum CaseDocuments
    {
        GeneralDocument = 1,
        Indictment = 2,
        Control = 3,
        Autopsy = 4
    }
}
