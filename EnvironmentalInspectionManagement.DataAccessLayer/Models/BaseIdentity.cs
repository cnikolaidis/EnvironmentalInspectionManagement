namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    #region Usings
    using System.Security.Principal;
    using Dtos;
    #endregion

    public class BaseIdentity : IIdentity
    {
        public BaseIdentity(string name, UserDto user)
        {
            Name = name;
            User = user;
        }

        public string Name { get; }
        public UserDto User { get; }
        public string AuthenticationType => string.Empty;
        public bool IsAuthenticated => User != null;
    }
}
