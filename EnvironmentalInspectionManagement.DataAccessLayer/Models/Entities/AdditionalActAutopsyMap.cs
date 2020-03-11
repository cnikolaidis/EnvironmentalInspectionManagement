namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, AutopsyId: {AutopsyId}, AdditionalActionId: {AdditionalActionId}")]
    [Table(@"additionalactsautopsiesmapping")]
    public class AdditionalActAutopsyMap : BaseEntity
    {
        public int AutopsyId { get; set; }
        public int AdditionalActionId { get; set; }
    }
}
