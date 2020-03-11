namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    [Table(@"autopsies")]
    public class Autopsy : BaseEntity
    {
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public string ProtocolNumber { get; set; }
        public string AutopsyElements { get; set; }
        public string WantedElements { get; set; }
        public string SubmittedElements { get; set; }
        public int AutopsyDocumentCategoryId { get; set; }
        public decimal? Fine { get; set; }

        [ForeignKey(@"AutopsyDocumentCategoryId")]
        public virtual AutopsyDocumentCategory AutopsyDocumentCategory { get; set; }
    }
}
