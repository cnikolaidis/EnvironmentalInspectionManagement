namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System;
    #endregion

    public class LicenseCriteria : BaseCriteria
    {
        public string LicenseNo { get; set; }
        public string LicensedBy { get; set; }
        public string Notes { get; set; }
        public DateTime? DateLicensed { get; set; }
        public DateTime? DateLicensedFrom { get; set; }
        public DateTime? DateLicensedTo { get; set; }
        public DateTime? DateExpiring { get; set; }
        public DateTime? DateExpiringFrom { get; set; }
        public DateTime? DateExpiringTo { get; set; }
    }
}
