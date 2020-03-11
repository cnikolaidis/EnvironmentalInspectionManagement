namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class OrganizationDto : BaseDto
    {
        public string Name { get; set; }
        public string TaxationNo { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public int TaxOfficeId { get; set; }
        public int RegionId { get; set; }
        public int RegionalUnityId { get; set; }
        public int LegalEntityCategoryId { get; set; }
        public string Region { get; set; }
        public string TaxOffice { get; set; }
        public string RegionalUnity { get; set; }
        public string LegalEntityCategory { get; set; }
        public IEnumerable<ActivityDto> Activities { get; set; }
    }
}
