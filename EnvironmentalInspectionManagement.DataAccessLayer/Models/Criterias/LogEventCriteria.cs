namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System;
    #endregion

    public class LogEventCriteria
    {
        public string Color { get; set; }
        public string Application { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string RawMessage { get; set; }
        public string User { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }
    }
}
