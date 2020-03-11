namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public class AutopsyCriteria : BaseCriteria
    {
        public DateTime? DateStarted { get; set; }
        public DateTime? FromDateStarted { get; set; }
        public DateTime? ToDateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public DateTime? FromDateEnded { get; set; }
        public DateTime? ToDateEnded { get; set; }
        public string AutopsyElements { get; set; }
        public string WantedElements { get; set; }
        public string SubmittedElements { get; set; }
        public string ProtocolNumber { get; set; }
        public decimal? Fine { get; set; }
        public decimal? FromFine { get; set; }
        public decimal? ToFine { get; set; }
        public int? AutopsyDocumentCategoryId { get; set; }
        public IEnumerable<int> AutopsyDocumentCategoryIds { get; set; }
    }
}
