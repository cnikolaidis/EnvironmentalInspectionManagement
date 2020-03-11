namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"controltriggers")]
    public class ControlTrigger : BaseEntity
    {
        public ControlTrigger()
        {
            Cases = new List<Case>();
        }

        public string Name { get; set; }

        public ICollection<Case> Cases { get; set; }
    }
}
