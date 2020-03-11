namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class WorkSubcategoryDto : BaseDto
    {
        public string Name { get; set; }
        public int WorkCategoryId { get; set; }
        public string WorkCategory { get; set; }
        public string LibraryNumber { get; set; }
        public string WorkCategoryLibraryNumber { get; set; }
    }
}
