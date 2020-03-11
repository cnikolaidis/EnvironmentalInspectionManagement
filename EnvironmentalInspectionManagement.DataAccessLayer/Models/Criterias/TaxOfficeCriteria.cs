namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class TaxOfficeCriteria : BaseCriteria
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? PostalCode { get; set; }
        public int? CityId { get; set; }
        public string PhoneNo { get; set; }
        public IEnumerable<int> CityIds { get; set; }
    }
}
