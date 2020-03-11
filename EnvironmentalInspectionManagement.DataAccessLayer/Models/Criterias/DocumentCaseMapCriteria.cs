namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class DocumentCaseMapCriteria : BaseCriteria
    {
        public int? CaseId { get; set; }
        public int? DocumentId { get; set; }
        public IEnumerable<int> CaseIds { get; set; }
        public IEnumerable<int> DocumentIds { get; set; }
    }
}
