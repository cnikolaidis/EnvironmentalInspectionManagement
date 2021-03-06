﻿namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class DocumentCriteria : BaseCriteria
    {
        public int? CaseId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? ActivityId { get; set; }
        public int? DocumentId { get; set; }
        public IEnumerable<int> CaseIds { get; set; }
        public IEnumerable<int> DocumentTypeIds { get; set; }
        public IEnumerable<int> ActivityIds { get; set; }
        public IEnumerable<int> DocumentIds { get; set; }
    }
}
