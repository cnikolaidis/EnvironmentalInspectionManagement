namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, GroupId: {GroupId}, AuthId: {AuthorizationId}")]
    public class GroupAuthorizationMapDto : BaseDto
    {
        public int? GroupId { get; set; }
        public int? AuthorizationId { get; set; }
        public bool? C { get; set; }
        public bool? R { get; set; }
        public bool? U { get; set; }
        public bool? D { get; set; }
        public string Group { get; set; }
        public string Authorization { get; set; }
    }
}
