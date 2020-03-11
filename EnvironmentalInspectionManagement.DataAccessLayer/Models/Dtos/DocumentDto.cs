namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Type: {DocumentType}")]
    public class DocumentDto : BaseDto
    {
        public int CaseId { get; set; }
        public int DocumentTypeId { get; set; }
        public int ActivityId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string LibraryIdentity { get; set; }
    }
}
