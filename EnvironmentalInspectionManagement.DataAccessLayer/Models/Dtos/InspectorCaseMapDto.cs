namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, CaseId: {CaseId}, InspectorId: {InspectorId}")]
    public class InspectorCaseMapDto : BaseDto
    {
        public int CaseId { get; set; }
        public int InspectorId { get; set; }
    }
}
