namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class AuthorizationDto : BaseDto
    {
        public string Name { get; set; }
        public IEnumerable<GroupAuthorizationMapDto> GroupAuthorizations { get; set; }
    }
}
