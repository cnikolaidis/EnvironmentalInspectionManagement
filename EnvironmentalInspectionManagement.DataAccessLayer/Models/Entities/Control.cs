namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {ControlManagerName}, Faculty: {ControlManagerFaculty}")]
    [Table(@"controls")]
    public class Control : BaseEntity
    {
        public int ControlTriggerId { get; set; }
        public int ControlTypeId { get; set; }
        public string ControlManagerName { get; set; }
        public string ControlManagerFaculty { get; set; }
        public DateTime ControlDate { get; set; }

        [ForeignKey(@"ControlTriggerId")]
        public virtual ControlTrigger ControlTrigger { get; set; }
        [ForeignKey(@"ControlTypeId")]
        public virtual ControlType ControlType { get; set; }
    }
}
