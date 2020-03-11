namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class NaturaRegionActivityMapCriteria : BaseCriteria
    {
        public int? NaturaRegionId { get; set; }
        public int? ActivityId { get; set; }
        public IEnumerable<int> ActivityIds { get; set; }
        public IEnumerable<int> NaturaRegionIds { get; set; }
    }
}
