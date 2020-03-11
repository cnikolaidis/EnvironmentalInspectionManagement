namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"worksubcategories")]
    public class WorkSubcategory : BaseEntity
    {
        public string Name { get; set; }
        public int WorkCategoryId { get; set; }
        public string LibraryNumber { get; set; }

        [ForeignKey(@"WorkCategoryId")]
        public virtual WorkCategory WorkCategory { get; set; }
    }
}
