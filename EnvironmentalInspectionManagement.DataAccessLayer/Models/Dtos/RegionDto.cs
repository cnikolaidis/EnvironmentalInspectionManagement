namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class RegionDto : BaseDto
    {
        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public bool IsNorth { get; set; }
        public bool IsSouth { get; set; }
        public IEnumerable<RegionalUnityDto> RegionalUnities { get; set; }
    }
}
