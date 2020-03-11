namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, IndictorName: {IndictorName}, IndicteeName: {IndicteeName}")]
    public class IndictmentDto : BaseDto
    {
        public string ProtocolNo { get; set; }
        public string IndictorName { get; set; }
        public string IndicteeName { get; set; }
        public string ViolationRegionDescription { get; set; }
        public string IndictmentRegionDescription { get; set; }
        public int IndicteeActivityId { get; set; }
        public string IndicteeActivityName { get; set; }
        public int ViolationRegionalUnityId { get; set; }
        public string ViolationRegionalUnity { get; set; }
        public int ViolationRegionId { get; set; }
        public string ViolationRegion { get; set; }
        public int ViolationCityId { get; set; }
        public string ViolationCity { get; set; }
        public int IndictmentRegionalUnityId { get; set; }
        public string IndictmentRegionalUnity { get; set; }
        public int IndictmentRegionId { get; set; }
        public string IndictmentRegion { get; set; }
        public int IndictmentCityId { get; set; }
        public string IndictmentCity { get; set; }
        public string IndictmentSubject { get; set; }
        public string IndictmentContent { get; set; }
        public string SafetyDegree { get; set; }
        public int SafetyDegreeId { get; set; }
        public string PriorityDegree { get; set; }
        public int PriorityDegreeId { get; set; }
        public DateTime? IndictmentDate { get; set; }
    }
}
