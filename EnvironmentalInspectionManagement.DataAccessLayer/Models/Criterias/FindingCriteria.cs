namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class FindingCriteria : BaseCriteria
    {
        public int? DocumentId { get; set; }
        public string Description { get; set; }

        public IEnumerable<int> DocumentIds { get; set; }
    }
}
