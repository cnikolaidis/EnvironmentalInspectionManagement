namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class CourtDecisionAutopsyMapCriteria : BaseCriteria
    {
        public int? AutopsyId { get; set; }
        public int? CourtDecisionId { get; set; }
        public IEnumerable<int> AutopsyIds { get; set; }
        public IEnumerable<int> CourtDecisionIds { get; set; }
    }
}
