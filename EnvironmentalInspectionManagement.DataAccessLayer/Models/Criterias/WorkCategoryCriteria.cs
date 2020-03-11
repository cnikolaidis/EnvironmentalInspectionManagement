namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class WorkCategoryCriteria : BaseCriteria
    {
        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public IEnumerable<string> LibraryNumbers { get; set; }
    }
}
