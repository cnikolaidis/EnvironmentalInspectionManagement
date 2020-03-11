namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, CaseId: {CaseId}, DocumentId: {DocumentId}")]
    [Table(@"documentscasesmapping")]
    public class DocumentCaseMap : BaseEntity
    {
        public int CaseId { get; set; }
        public int DocumentId { get; set; }

        [ForeignKey(@"CaseId")]
        public virtual Case Case { get; set; }
        [ForeignKey(@"CaseId")]
        public virtual Document Document { get; set; }
    }
}
