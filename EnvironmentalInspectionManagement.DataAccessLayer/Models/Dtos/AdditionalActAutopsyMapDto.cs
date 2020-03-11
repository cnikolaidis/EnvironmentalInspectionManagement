namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, AutopsyId: {AutopsyId}, AdditionalActionId: {AdditionalActionId}")]
    public class AdditionalActAutopsyMapDto : BaseDto
    {
        public int AutopsyId { get; set; }
        public int AdditionalActionId { get; set; }
    }
}
