namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"safetydegrees")]
    public class SafetyDegree : BaseEntity
    {
        public SafetyDegree()
        {
            Indictments = new List<Indictment>();
        }

        public string Name { get; set; }

        public ICollection<Indictment> Indictments { get; set; }
    }
}
