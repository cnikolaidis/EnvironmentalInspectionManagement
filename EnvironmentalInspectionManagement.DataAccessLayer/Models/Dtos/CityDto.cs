namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class CityDto : BaseDto
    {
        public string Name { get; set; }
        public IEnumerable<TaxOfficeDto> TaxOffices { get; set; }
    }
}
