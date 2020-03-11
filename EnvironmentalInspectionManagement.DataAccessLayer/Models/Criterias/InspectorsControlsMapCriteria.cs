namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class InspectorsControlsMapCriteria : BaseCriteria
    {
        public int? ControlId { get; set; }
        public int? InspectorId { get; set; }

        public IEnumerable<int> ControlIds { get; set; }
        public IEnumerable<int> InspectorIds { get; set; }
    }
}
