namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"controlprogresses")]
    public class ControlProgress : BaseEntity
    {
        public ControlProgress()
        {
            Cases = new List<Case>();
        }

        public string Name { get; set; }

        public ICollection<Case> Cases { get; set; }
    }
}
