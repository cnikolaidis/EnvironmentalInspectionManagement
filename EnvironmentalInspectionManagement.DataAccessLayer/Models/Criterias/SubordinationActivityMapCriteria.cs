namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class SubordinationActivityMapCriteria : BaseCriteria
    {
        public int? SubordinationId { get; set; }
        public int? ActivityId { get; set; }
        public IEnumerable<int> ActivityIds { get; set; }
        public IEnumerable<int> SubordinationIds { get; set; }
    }
}
