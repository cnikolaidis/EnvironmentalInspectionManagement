namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class ActivityDto : BaseDto
    {
        public int OrganizationId { get; set; }
        public int TaxOfficeId { get; set; }
        public int RegionalUnityId { get; set; }
        public int WorkCategoryId { get; set; }
        public int WorkSubcategoryId { get; set; }
        public int NaceCodeSectorId { get; set; }
        public int NaceCodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public decimal CoordX { get; set; }
        public decimal CoordY { get; set; }
        public string Organization { get; set; }
        public string TaxOffice { get; set; }
        public string RegionalUnity { get; set; }
        public string WorkCategory { get; set; }
        public string WorkSubcategory { get; set; }
        public string NaceCodeSector { get; set; }
        public string NaceCode { get; set; }
        public string FaxNo { get; set; }
        public string MailAddress { get; set; }
        public string PlaceName { get; set; }
        public string ManagerName { get; set; }
        public string OtaName { get; set; }
    }
}
