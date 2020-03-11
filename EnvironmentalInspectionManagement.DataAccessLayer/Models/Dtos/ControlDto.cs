namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, ControlType: {ControlType}")]
    public class ControlDto : BaseDto
    {
        public int ControlTriggerId { get; set; }
        public string ControlTrigger { get; set; }
        public int ControlTypeId { get; set; }
        public string ControlType { get; set; }
        public string ControlManagerName { get; set; }
        public string ControlManagerFaculty { get; set; }
        public DateTime ControlDate { get; set; }
    }
}
