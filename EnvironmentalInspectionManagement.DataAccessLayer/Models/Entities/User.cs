namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Username}")]
    [Table(@"users")]
    public class User : BaseEntity
    {
        public int GroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [ForeignKey(@"GroupId")]
        public virtual UserGroup UserGroup { get; set; }
    }
}
