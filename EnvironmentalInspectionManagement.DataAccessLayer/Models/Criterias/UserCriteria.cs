namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    using System;
    #endregion

    public class UserCriteria : BaseCriteria
    {
        public int? GroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? FromBirthDate { get; set; }
        public DateTime? ToBirthDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<int> GroupIds { get; set; }
    }
}
