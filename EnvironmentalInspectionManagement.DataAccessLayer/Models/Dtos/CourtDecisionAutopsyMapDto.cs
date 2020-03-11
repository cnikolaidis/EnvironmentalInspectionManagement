namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, AutopsyId: {AutopsyId}, CourtDecisionId: {CourtDecisionId}")]
    public class CourtDecisionAutopsyMapDto : BaseDto
    {
        public int AutopsyId { get; set; }
        public int CourtDecisionId { get; set; }
        public string CourtDecision { get; set; }
    }
}
