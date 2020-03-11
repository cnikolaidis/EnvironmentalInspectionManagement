namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, ControlId: {ControlId}, InspectorId: {InspectorId}")]
    [Table(@"inspectorscontrolsmapping")]
    public class InspectorsControlsMap : BaseEntity
    {
        public int ControlId { get; set; }
        public int InspectorId { get; set; }
    }
}
