namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Sector}, Name: {Name}")]
    [Table(@"nacecodesectors")]
    public class NaceCodeSector : BaseEntity
    {
        public NaceCodeSector()
        {
            NaceCodes = new List<NaceCode>();
        }

        public string Sector { get; set; }
        public string Name { get; set; }

        public ICollection<NaceCode> NaceCodes { get; set; }
    }
}
