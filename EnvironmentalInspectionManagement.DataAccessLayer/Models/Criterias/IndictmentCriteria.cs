namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public class IndictmentCriteria : BaseCriteria
    {
        public string ProtocolNo { get; set; }
        public string IndictorName { get; set; }
        public string IndicteeName { get; set; }
        public string IndictmentSubject { get; set; }
        public string IndictmentContent { get; set; }
        public string ViolationRegionDescription { get; set; }
        public string IndictmentRegionDescription { get; set; }
        public int? IndicteeActivityId { get; set; }
        public int? ViolationRegionalUnityId { get; set; }
        public int? ViolationRegionId { get; set; }
        public int? ViolationCityId { get; set; }
        public int? IndictmentRegionalUnityId { get; set; }
        public int? IndictmentRegionId { get; set; }
        public int? IndictmentCityId { get; set; }
        public int? SafetyDegreeId { get; set; }
        public int? PriorityDegreeId { get; set; }
        public DateTime? IndictmentDate { get; set; }
        public DateTime? IndictmentDateFrom { get; set; }
        public DateTime? IndictmentDateTo { get; set; }

        public IEnumerable<int> IndicteeActivityIds { get; set; }
        public IEnumerable<int> ViolationRegionalUnityIds { get; set; }
        public IEnumerable<int> ViolationRegionIds { get; set; }
        public IEnumerable<int> ViolationCityIds { get; set; }
        public IEnumerable<int> IndictmentRegionalUnityIds { get; set; }
        public IEnumerable<int> IndictmentRegionIds { get; set; }
        public IEnumerable<int> IndictmentCityIds { get; set; }
        public IEnumerable<int> PriorityDegreeIds { get; set; }
        public IEnumerable<int> SafetyDegreeIds { get; set; }
    }
}
