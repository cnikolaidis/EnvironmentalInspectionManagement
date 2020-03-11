namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"cities")]
    public class City : BaseEntity
    {
        public City()
        {
            TaxOffices = new List<TaxOffice>();
        }

        public string Name { get; set; }

        public ICollection<TaxOffice> TaxOffices { get; set; }
    }
}
