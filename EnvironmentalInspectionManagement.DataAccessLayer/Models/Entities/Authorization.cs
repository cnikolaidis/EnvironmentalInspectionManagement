namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"authorizations")]
    public class Authorization : BaseEntity
    {
        public Authorization()
        {
            GroupAuthorizations = new List<GroupAuthorizationMap>();
        }

        public string Name { get; set; }

        public ICollection<GroupAuthorizationMap> GroupAuthorizations { get; set; }
    }
}
