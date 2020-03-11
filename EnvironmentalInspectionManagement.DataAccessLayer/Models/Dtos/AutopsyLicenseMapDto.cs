namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    public class AutopsyLicenseMapDto : BaseDto
    {
        public int? AutopsyId { get; set; }
        public int? LicenseId { get; set; }
        public string LicenseNo { get; set; }
        public string LicensedBy { get; set; }
        public string LicenseNotes { get; set; }
        public DateTime? DateLicensed { get; set; }
        public DateTime? DateExpiring { get; set; }
    }
}
