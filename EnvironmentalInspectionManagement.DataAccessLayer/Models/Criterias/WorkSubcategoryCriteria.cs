namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class WorkSubcategoryCriteria : BaseCriteria
    {
        public string Name { get; set; }
        public int? WorkCategoryId { get; set; }
        public string LibraryNumber { get; set; }
        public IEnumerable<int> WorkCategoryIds { get; set; }
        public IEnumerable<string> LibraryNumbers { get; set; }
    }
}
