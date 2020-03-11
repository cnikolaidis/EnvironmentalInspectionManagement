namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class NaceCodeCriteria : BaseCriteria
    {
        public string Class { get; set; }
        public string Name { get; set; }
        public int? SectorId { get; set; }
        public IEnumerable<int> SectorIds { get; set; }
    }
}
