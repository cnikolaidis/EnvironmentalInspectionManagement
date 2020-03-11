namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, AutopsyId: {AutopsyId}, CourtDecisionId: {CourtDecisionId}")]
    [Table(@"courtdecisionsautopsiesmapping")]
    public class CourtDecisionAutopsyMap : BaseEntity
    {
        public int AutopsyId { get; set; }
        public int CourtDecisionId { get; set; }

        [ForeignKey(@"CourtDecisionId")]
        public virtual CourtDecision CourtDecision { get; set; }
    }
}
