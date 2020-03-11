namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"workcategories")]
    public class WorkCategory : BaseEntity
    {
        public WorkCategory()
        {
            WorkSubcategories = new List<WorkSubcategory>();
        }

        public string Name { get; set; }
        public string LibraryNumber { get; set; }

        public ICollection<WorkSubcategory> WorkSubcategories { get; set; }
    }
}
