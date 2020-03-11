namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, NaturaRegionId: {NaturaRegionId}, ActivityId: {ActivityId}")]
    public class NaturaRegionActivityMapDto : BaseDto
    {
        public int NaturaRegionId { get; set; }
        public int ActivityId { get; set; }
    }
}
