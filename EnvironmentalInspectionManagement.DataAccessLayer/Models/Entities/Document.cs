namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}")]
    [Table(@"documents")]
    public class Document : BaseEntity
    {
        public Document()
        {
            DocumentCaseMaps = new List<DocumentCaseMap>();
        }

        public int CaseId { get; set; }
        public int DocumentTypeId { get; set; }
        public int ActivityId { get; set; }
        public int DocumentId { get; set; }

        [ForeignKey(@"CaseId")]
        public virtual Case Case { get; set; }
        [ForeignKey(@"DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }
        [ForeignKey(@"ActivityId")]
        public virtual Activity Activity { get; set; }
        [ForeignKey(@"DocumentId")]
        public virtual Document CaseDocument { get; set; }

        public ICollection<DocumentCaseMap> DocumentCaseMaps { get; set; }
    }
}
