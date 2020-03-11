namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, SubordinationId: {SubordinationId}, ActivityId: {ActivityId}")]
    [Table(@"subordinationsactivitiesmapping")]
    public class SubordinationActivityMap : BaseEntity
    {
        public int SubordinationId { get; set; }
        public int ActivityId { get; set; }
    }
}
