namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"regions")]
    public class Region : BaseEntity
    {
        public Region()
        {
            RegionalUnities = new List<RegionalUnity>();
        }

        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public bool IsNorth { get; set; }
        public bool IsSouth { get; set; }

        public ICollection<RegionalUnity> RegionalUnities { get; set; }
    }
}
