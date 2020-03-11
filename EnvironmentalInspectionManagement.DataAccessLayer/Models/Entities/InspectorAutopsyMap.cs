namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, AutopsyId: {AutopsyId}, InspectorId: {InspectorId}")]
    [Table(@"inspectorsautopsiesmapping")]
    public class InspectorAutopsyMap : BaseEntity
    {
        public int AutopsyId { get; set; }
        public int InspectorId { get; set; }

        [ForeignKey(@"AutopsyId")]
        public virtual Autopsy Autopsy { get; set; }
        [ForeignKey(@"InspectorId")]
        public virtual Inspector Inspector { get; set; }
    }
}
