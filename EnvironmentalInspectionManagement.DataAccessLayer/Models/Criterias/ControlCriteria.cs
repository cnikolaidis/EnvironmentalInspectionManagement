namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public class ControlCriteria : BaseCriteria
    {
        public int? ControlTriggerId { get; set; }
        public int? ControlTypeId { get; set; }
        public string ControlManagerName { get; set; }
        public string ControlManagerFaculty { get; set; }
        public DateTime? ControlDate { get; set; }
        public DateTime? ControlDateFrom { get; set; }
        public DateTime? ControlDateTo { get; set; }

        public IEnumerable<int> ControlTriggerIds { get; set; }
        public IEnumerable<int> ControlTypeIds { get; set; }
    }
}
