namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, CaseId: {CaseId}, DocumentId: {DocumentId}")]
    public class DocumentCaseMapDto : BaseDto
    {
        public int CaseId { get; set; }
        public int DocumentId { get; set; }
    }
}
