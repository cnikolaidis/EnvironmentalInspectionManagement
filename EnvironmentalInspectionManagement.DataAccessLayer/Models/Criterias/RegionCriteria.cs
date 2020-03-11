namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class RegionCriteria : BaseCriteria
    {
        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public bool? IsNorth { get; set; }
        public bool? IsSouth { get; set; }

        public IEnumerable<string> LibraryNumbers { get; set; }
    }
}
