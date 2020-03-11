namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"activities")]
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Indictments = new List<Indictment>();
            Cases = new List<Case>();
        }

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
        public string FaxNo { get; set; }
        public string MailAddress { get; set; }
        public string PlaceName { get; set; }
        public string ManagerName { get; set; }
        public string OtaName { get; set; }
        public decimal CoordX { get; set; }
        public decimal CoordY { get; set; }

        [ForeignKey(@"OrganizationId")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(@"TaxOfficeId")]
        public virtual TaxOffice TaxOffice { get; set; }
        [ForeignKey(@"RegionalUnityId")]
        public virtual RegionalUnity RegionalUnity { get; set; }
        [ForeignKey(@"WorkCategoryId")]
        public virtual WorkCategory WorkCategory { get; set; }
        [ForeignKey(@"WorkSubcategoryId")]
        public virtual WorkSubcategory WorkSubcategory { get; set; }
        [ForeignKey(@"NaceCodeSectorId")]
        public virtual NaceCodeSector NaceCodeSector { get; set; }
        [ForeignKey(@"NaceCodeId")]
        public virtual NaceCode NaceCode { get; set; }

        public ICollection<Indictment> Indictments { get; set; }
        public ICollection<Case> Cases { get; set; }
    }
}
