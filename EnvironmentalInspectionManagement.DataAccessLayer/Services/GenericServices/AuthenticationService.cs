namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.GenericServices
{
    #region Usings
    using Models.Criterias;
    using EntityServices;
    using System.Linq;
    using Models.Dtos;
    #endregion

    public interface IAuthenticationService : IBaseGenericService
    {
        UserDto AuthenticateUser(string uname, string pwd);
    }

    public class AuthenticationService : BaseGenericService, IAuthenticationService
    {
        public UserDto AuthenticateUser(string uname, string pwd)
        {
            var userDto = Svc.UsersService.ListByCriteria(new UserCriteria
            {
                Username = uname,
                Password = pwd
            })
            .GetDtos()
            .FirstOrDefault();
            
            return userDto;
        }
    }
}
