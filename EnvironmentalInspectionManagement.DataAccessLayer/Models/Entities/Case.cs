namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, ActivityId: {ActivityId}")]
    [Table(@"cases")]
    public class Case : BaseEntity
    {
        public Case()
        {
            DocumentCaseMaps = new List<DocumentCaseMap>();
        }

        public int ActivityId { get; set; }
        public int? ControlTriggerId { get; set; }
        public int? ControlProgressId { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }

        [ForeignKey(@"ActivityId")]
        public virtual Activity Activity { get; set; }
        [ForeignKey(@"ControlTriggerId")]
        public virtual ControlTrigger ControlTrigger { get; set; }
        [ForeignKey(@"ControlProgressId")]
        public virtual ControlProgress ControlProgress { get; set; }

        public ICollection<DocumentCaseMap> DocumentCaseMaps { get; set; }
    }
}
