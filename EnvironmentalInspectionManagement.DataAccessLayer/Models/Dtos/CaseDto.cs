namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {ActivityName}")]
    public class CaseDto : BaseDto
    {
        public int ActivityId { get; set; }
        public int DocumentsCount { get; set; }
        public int? ControlTriggerId { get; set; }
        public int? ControlProgressId { get; set; }
        public string ActivityName { get; set; }
        public string CaseProgress { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
    }
}
