namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    #region Usings
    using System;
    #endregion

    public interface IBaseDto
    {
        int Id { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }

    public class BaseDto : IBaseDto
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
