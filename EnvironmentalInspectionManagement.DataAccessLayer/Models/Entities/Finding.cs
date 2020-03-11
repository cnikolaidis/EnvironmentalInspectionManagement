namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {DocumentId}, Name: {Description}")]
    [Table(@"findings")]
    public class Finding : BaseEntity
    {
        public int DocumentId { get; set; }
        public string Description { get; set; }
    }
}
