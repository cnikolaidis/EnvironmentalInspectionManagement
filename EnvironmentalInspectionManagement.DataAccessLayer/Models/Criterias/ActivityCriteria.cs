namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class ActivityCriteria : BaseCriteria
    {
        public int? OrganizationId { get; set; }
        public int? TaxOfficeId { get; set; }
        public int? RegionalUnityId { get; set; }
        public int? WorkCategoryId { get; set; }
        public int? WorkSubcategoryId { get; set; }
        public int? NaceCodeSectorId { get; set; }
        public int? NaceCodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public string FaxNo { get; set; }
        public string MailAddress { get; set; }
        public string PlaceName { get; set; }
        public string ManagerName { get; set; }
        public string OtaName { get; set; }
        public decimal? CoordX { get; set; }
        public decimal? CoordY { get; set; }
        public IEnumerable<int> OrganizationIds { get; set; }
        public IEnumerable<int> TaxOfficeIds { get; set; }
        public IEnumerable<int> RegionalUnityIds { get; set; }
        public IEnumerable<int> WorkCategoryIds { get; set; }
        public IEnumerable<int> WorkSubcategoryIds { get; set; }
        public IEnumerable<int> NaceCodeSectorIds { get; set; }
        public IEnumerable<int> NaceCodeIds { get; set; }
    }
}
