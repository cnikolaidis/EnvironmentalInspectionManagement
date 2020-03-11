namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class NaceCodeDto : BaseDto
    {
        public string Class { get; set; }
        public string Name { get; set; }
        public int SectorId { get; set; }
        public string Sector { get; set; }
    }
}
