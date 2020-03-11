namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"taxoffices")]
    public class TaxOffice : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? PostalCode { get; set; }
        public int CityId { get; set; }
        public string PhoneNo { get; set; }

        [ForeignKey(@"CityId")]
        public virtual City City { get; set; }
    }
}
