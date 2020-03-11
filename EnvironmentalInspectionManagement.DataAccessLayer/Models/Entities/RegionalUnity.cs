namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"regionalunities")]
    public class RegionalUnity : BaseEntity
    {
        public string Name { get; set; }
        public int RegionId { get; set; }
        public string LibraryNumber { get; set; }

        [ForeignKey(@"RegionId")]
        public virtual Region Region { get; set; }
    }
}
