namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    public class LicenseDto : BaseDto
    {
        public string LicenseNo { get; set; }
        public string LicensedBy { get; set; }
        public string Notes { get; set; }
        public DateTime? DateLicensed { get; set; }
        public DateTime? DateExpiring { get; set; }
    }
}
