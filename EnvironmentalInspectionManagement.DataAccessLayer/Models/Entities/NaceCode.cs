namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"nacecodes")]
    public class NaceCode : BaseEntity
    {
        public string Class { get; set; }
        public string Name { get; set; }
        public int SectorId { get; set; }

        [ForeignKey(@"SectorId")]
        public virtual NaceCodeSector NaceCodeSector { get; set; }
    }
}
