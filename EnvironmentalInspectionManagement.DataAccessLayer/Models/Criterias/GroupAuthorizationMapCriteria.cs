namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Criterias
{
    #region Usings
    using System.Collections.Generic;
    #endregion

    public class GroupAuthorizationMapCriteria : BaseCriteria
    {
        public int? GroupId { get; set; }
        public int? AuthorizationId { get; set; }
        public bool? C { get; set; }
        public bool? R { get; set; }
        public bool? U { get; set; }
        public bool? D { get; set; }
        public IEnumerable<int> GroupIds { get; set; }
        public IEnumerable<int> AuthorizationIds { get; set; }
    }
}
