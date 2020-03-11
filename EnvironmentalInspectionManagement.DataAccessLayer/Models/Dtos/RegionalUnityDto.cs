namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class RegionalUnityDto : BaseDto
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public string LibraryNumber { get; set; }
        public string Region { get; set; }
        public string RegionLibraryNumber { get; set; }
    }
}
