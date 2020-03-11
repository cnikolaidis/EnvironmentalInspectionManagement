namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, Fine: {Fine}")]
    public class AutopsyDto : BaseDto
    {
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public string AutopsyElements { get; set; }
        public string WantedElements { get; set; }
        public string SubmittedElements { get; set; }
        public string ProtocolNumber { get; set; }
        public string AutopsyDocumentCategory { get; set; }
        public int AutopsyDocumentCategoryId { get; set; }
        public decimal? Fine { get; set; }
    }
}
