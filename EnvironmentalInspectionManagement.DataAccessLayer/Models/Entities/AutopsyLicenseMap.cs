namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    [Table(@"autopsylicensesmapping")]
    public class AutopsyLicenseMap : BaseEntity
    {
        public int AutopsyId { get; set; }
        public int LicenseId { get; set; }

        [ForeignKey(@"AutopsyId")]
        public virtual Autopsy Autopsy { get; set; }
        [ForeignKey(@"LicenseId")]
        public virtual License License { get; set; }
    }
}
