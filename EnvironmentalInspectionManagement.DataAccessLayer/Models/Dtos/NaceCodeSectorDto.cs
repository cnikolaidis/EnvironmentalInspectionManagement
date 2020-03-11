namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Sector}, Name: {Name}")]
    public class NaceCodeSectorDto : BaseDto
    {
        public string Sector { get; set; }
        public string Name { get; set; }
        public IEnumerable<NaceCodeDto> NaceCodes { get; set; }
    }
}
