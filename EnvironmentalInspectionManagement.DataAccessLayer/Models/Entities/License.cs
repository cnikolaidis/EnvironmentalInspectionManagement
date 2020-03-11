namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    [Table(@"licenses")]
    public class License : BaseEntity
    {
        public string LicenseNo { get; set; }
        public string LicensedBy { get; set; }
        public string Notes { get; set; }
        public DateTime DateLicensed { get; set; }
        public DateTime DateExpiring { get; set; }
    }
}
