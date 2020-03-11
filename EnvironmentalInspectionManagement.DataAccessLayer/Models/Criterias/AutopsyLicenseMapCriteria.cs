namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class AutopsyLicenseMapCriteria : BaseCriteria
    {
        public int? AutopsyId { get; set; }
        public int? LicenseId { get; set; }
        public IEnumerable<int> AutopsyIds { get; set; }
        public IEnumerable<int> LicenseIds { get; set; }
    }
}
