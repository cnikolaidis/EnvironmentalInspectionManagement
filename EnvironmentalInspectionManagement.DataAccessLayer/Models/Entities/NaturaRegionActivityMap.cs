namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, NaturaRegionId: {NaturaRegionId}, ActivityId: {ActivityId}")]
    [Table(@"naturaregionsactivitiesmapping")]
    public class NaturaRegionActivityMap : BaseEntity
    {
        public int NaturaRegionId { get; set; }
        public int ActivityId { get; set; }
    }
}
