namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class InspectorCaseMapCriteria : BaseCriteria
    {
        public int? CaseId { get; set; }
        public int? InspectorId { get; set; }
        public IEnumerable<int> CaseIds { get; set; }
        public IEnumerable<int> InspectorIds { get; set; }
    }
}
