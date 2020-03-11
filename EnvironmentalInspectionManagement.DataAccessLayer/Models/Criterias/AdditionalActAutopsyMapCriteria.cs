namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class AdditionalActAutopsyMapCriteria : BaseCriteria
    {
        public int? AutopsyId { get; set; }
        public int? AdditionalActionId { get; set; }
        public IEnumerable<int> AutopsyIds { get; set; }
        public IEnumerable<int> AdditionalActionIds { get; set; }
    }
}
