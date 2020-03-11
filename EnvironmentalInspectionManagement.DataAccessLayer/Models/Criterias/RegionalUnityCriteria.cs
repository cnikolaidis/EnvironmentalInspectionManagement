namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class RegionalUnityCriteria : BaseCriteria
    {
        public int? RegionId { get; set; }
        public string LibraryNumber { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> RegionIds { get; set; }
        public IEnumerable<string> LibraryNumbers { get; set; }
    }
}
