namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, SubordinationId: {SubordinationId}, ActivityId: {ActivityId}")]
    public class SubordinationActivityMapDto : BaseDto
    {
        public int SubordinationId { get; set; }
        public int ActivityId { get; set; }
    }
}
