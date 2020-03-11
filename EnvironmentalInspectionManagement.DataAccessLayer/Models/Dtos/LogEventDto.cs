namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Type}, Message: {Message}")]
    public class LogEventDto
    {
        public string Color { get; set; }
        public string Application { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string RawMessage { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
