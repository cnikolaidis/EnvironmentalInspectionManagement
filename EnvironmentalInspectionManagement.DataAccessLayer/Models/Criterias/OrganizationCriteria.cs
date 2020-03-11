namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class OrganizationCriteria : BaseCriteria
    {
        public string Name { get; set; }
        public string TaxationNo { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public int? TaxOfficeId { get; set; }
        public int? RegionId { get; set; }
        public int? RegionalUnityId { get; set; }
        public int? LegalEntityCategoryId { get; set; }
        public IEnumerable<int> TaxOfficeIds { get; set; }
        public IEnumerable<int> RegionIds { get; set; }
        public IEnumerable<int> RegionalUnityIds { get; set; }
        public IEnumerable<int> LegalEntityCategoryIds { get; set; }
    }
}
