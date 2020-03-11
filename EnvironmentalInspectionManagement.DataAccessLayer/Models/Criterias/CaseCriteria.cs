namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public class CaseCriteria : BaseCriteria
    {
        public int? ActivityId { get; set; }
        public int? ControlTriggerId { get; set; }
        public int? ControlProgressId { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? FromDateStarted { get; set; }
        public DateTime? ToDateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public DateTime? FromDateEnded { get; set; }
        public DateTime? ToDateEnded { get; set; }
        public IEnumerable<int> ActivityIds { get; set; }
        public IEnumerable<int?> ControlTriggerIds { get; set; }
        public IEnumerable<int?> ControlProgressIds { get; set; }
    }
}
