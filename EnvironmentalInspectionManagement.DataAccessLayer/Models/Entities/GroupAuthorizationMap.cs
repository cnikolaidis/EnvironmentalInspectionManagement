namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, GroupId: {GroupId}, AuthId: {AuthorizationId}")]
    [Table(@"groupauthorizationsmapping")]
    public class GroupAuthorizationMap : BaseEntity
    {
        public int GroupId { get; set; }
        public int AuthorizationId { get; set; }
        public bool C { get; set; }
        public bool R { get; set; }
        public bool U { get; set; }
        public bool D { get; set; }

        [ForeignKey(@"GroupId")]
        public virtual UserGroup UserGroup { get; set; }
        [ForeignKey(@"AuthorizationId")]
        public virtual Authorization Authorization { get; set; }
    }
}
