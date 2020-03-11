namespace EnvironmentalInspectionManagement.DataAccessLayer.Models
{
    #region Usings
    using System.Security.Principal;
    using IdentityModels;
    using System.Linq;
    #endregion

    public class BasePrincipal : IPrincipal
    {
        private BaseIdentity _identity;
        public BaseIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        IIdentity IPrincipal.Identity => Identity;

        public bool IsInRole(string role) => _identity.User.Roles.Contains(role);
    }
}
