namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    #region Usings
    using System.ComponentModel.DataAnnotations;
    using System;
    #endregion

    public interface IBaseEntity
    {
        [Key]
        int Id { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }

    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
