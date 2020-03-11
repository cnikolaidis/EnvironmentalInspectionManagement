namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("ControlId: {ControlId}, InspectorId: {InspectorId}")]
    public class InspectorsControlsMapDto : BaseDto
    {
        public int ControlId { get; set; }
        public int InspectorId { get; set; }
    }
}
