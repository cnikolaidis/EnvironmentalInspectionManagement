namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, CaseId: {CaseId}, InspectorId: {InspectorId}")]
    [Table(@"inspectorscasesmapping")]
    public class InspectorCaseMap : BaseEntity
    {
        public int CaseId { get; set; }
        public int InspectorId { get; set; }
    }
}
