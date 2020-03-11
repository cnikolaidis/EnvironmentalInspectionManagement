namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, Indictor: {IndictorName}, Indictee: {IndicteeName}")]
    [Table(@"indictments")]
    public class Indictment : BaseEntity
    {
        public string ProtocolNo { get; set; }
        public string IndictorName { get; set; }
        public string IndicteeName { get; set; }
        public string ViolationRegionDescription { get; set; }
        public string IndictmentRegionDescription { get; set; }
        public int IndicteeActivityId { get; set; }
        public int ViolationRegionalUnityId { get; set; }
        public int ViolationRegionId { get; set; }
        public int ViolationCityId { get; set; }
        public int IndictmentRegionalUnityId { get; set; }
        public int IndictmentRegionId { get; set; }
        public int IndictmentCityId { get; set; }
        public int SafetyDegreeId { get; set; }
        public int PriorityDegreeId { get; set; }
        public string IndictmentSubject { get; set; }
        public string IndictmentContent { get; set; }
        public DateTime IndictmentDate { get; set; }

        [ForeignKey(@"IndicteeActivityId")]
        public virtual Activity IndicteeActivity { get; set; }
        [ForeignKey(@"ViolationRegionalUnityId")]
        public virtual RegionalUnity ViolationRegionalUnity { get; set; }
        [ForeignKey(@"IndictmentRegionalUnityId")]
        public virtual RegionalUnity IndictmentRegionalUnity { get; set; }
        [ForeignKey(@"ViolationRegionId")]
        public virtual Region ViolationRegion { get; set; }
        [ForeignKey(@"IndictmentRegionId")]
        public virtual Region IndictmentRegion { get; set; }
        [ForeignKey(@"ViolationCityId")]
        public virtual City ViolationCity { get; set; }
        [ForeignKey(@"IndictmentCityId")]
        public virtual City IndictmentCity { get; set; }
        [ForeignKey(@"SafetyDegreeId")]
        public virtual SafetyDegree SafetyDegree { get; set; }
        [ForeignKey(@"PriorityDegreeId")]
        public virtual PriorityDegree PriorityDegree { get; set; }
    }
}
