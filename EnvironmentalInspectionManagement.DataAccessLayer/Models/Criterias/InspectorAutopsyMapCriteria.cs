namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class InspectorAutopsyMapCriteria : BaseCriteria
    {
        public int? AutopsyId { get; set; }
        public int? InspectorId { get; set; }
        public IEnumerable<int> AutopsyIds { get; set; }
        public IEnumerable<int> InspectorIds { get; set; }
    }
}
