namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class WorkCategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public IEnumerable<WorkSubcategoryDto> WorkSubcategories { get; set; }
    }
}
