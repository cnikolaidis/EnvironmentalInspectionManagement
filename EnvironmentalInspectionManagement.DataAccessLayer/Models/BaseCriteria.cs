namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public interface IBaseCriteria
    {
        int? Id { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? FromDateCreated { get; set; }
        DateTime? ToDateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        DateTime? FromDateUpdated { get; set; }
        DateTime? ToDateUpdated { get; set; }
        IEnumerable<int> Ids { get; set; }
    }

    public class BaseCriteria : IBaseCriteria
    {
        public int? Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? FromDateCreated { get; set; }
        public DateTime? ToDateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? FromDateUpdated { get; set; }
        public DateTime? ToDateUpdated { get; set; }

        public IEnumerable<int> Ids { get; set; }
    }
}
