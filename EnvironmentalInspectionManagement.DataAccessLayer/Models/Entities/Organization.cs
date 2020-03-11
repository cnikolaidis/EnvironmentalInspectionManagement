namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"organizations")]
    public class Organization : BaseEntity
    {
        public Organization()
        {
            Activities = new List<Activity>();
        }

        public string Name { get; set; }
        public string TaxationNo { get; set; }
        public string Address { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public int TaxOfficeId { get; set; }
        public int RegionId { get; set; }
        public int RegionalUnityId { get; set; }
        public int LegalEntityCategoryId { get; set; }

        [ForeignKey(@"TaxOfficeId")]
        public virtual TaxOffice TaxOffice { get; set; }
        [ForeignKey(@"RegionId")]
        public virtual Region Region { get; set; }
        [ForeignKey(@"RegionalUnityId")]
        public virtual RegionalUnity RegionalUnity { get; set; }
        [ForeignKey(@"LegalEntityCategoryId")]
        public virtual LegalEntityCategory LegalEntityCategory { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}
