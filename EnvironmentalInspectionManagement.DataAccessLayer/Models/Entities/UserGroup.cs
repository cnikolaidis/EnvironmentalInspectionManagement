namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"usergroups")]
    public class UserGroup : BaseEntity
    {
        public UserGroup()
        {
            Users = new List<User>();
            GroupAuthorizations = new List<GroupAuthorizationMap>();
        }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<GroupAuthorizationMap> GroupAuthorizations { get; set; }
    }
}
