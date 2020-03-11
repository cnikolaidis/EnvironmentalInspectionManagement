namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class TaxOfficeDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? PostalCode { get; set; }
        public int CityId { get; set; }
        public string PhoneNo { get; set; }
        public string City { get; set; }
    }
}
