namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels.RenderViewModels
{
    #region Usings
    using DataAccessLayer.Services.EntityServices;
    using DataAccessLayer.Models.OtherObjects;
    using DataAccessLayer.Models.Criterias;
    using System.Collections.ObjectModel;
    using DataAccessLayer.Models.Dtos;
    using System.Collections.Generic;
    using DataAccessLayer.Services;
    using System.Linq;
    using Utilities;
    using System;
    using Core;
    #endregion

    public interface IUserManagementViewModel : IBaseViewModel { }

    public class UserManagementViewModel : BaseViewModel, IUserManagementViewModel
    {
        #region Properties
        private static bool _forUpdateUser;
        private static bool _forUpdateUserGroup;
        private static bool _forUpdateAuthorization;
        private static IEnumerable<UserDto> _usersList;
        private static IEnumerable<GroupAuthorizationMapDto> _allGroupAuths;
        #endregion

        #region Constructor
        public UserManagementViewModel()
        {
            InvokeOperation(RefreshInputs);
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<UserDto> Users { get; set; }
        public ObservableCollection<UserGroupDto> UserGroups { get; set; }
        public ObservableCollection<AuthorizationDto> Authorizations { get; set; }
        public ObservableCollection<ComboBoxItemDto> UserGroupOptions { get; set; }
        public ObservableCollection<GroupAuthorizationMapDto> UserGroupAuthorizations { get; set; }

        public UserDto SelectedUser { get; set; }
        public UserGroupDto SelectedUserGroup { get; set; }
        public AuthorizationDto SelectedAuthorization { get; set; }
        public GroupAuthorizationMapDto SelectedUserGroupAuthorization { get; set; }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                SetProperty(ref _searchTerm, value);
                InvokeOperation(Search);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _passwordConfirm;
        public string PasswordConfirm
        {
            get { return _passwordConfirm; }
            set { SetProperty(ref _passwordConfirm, value); }
        }

        private string _userGroupName;
        public string UserGroupName
        {
            get { return _userGroupName; }
            set { SetProperty(ref _userGroupName, value); }
        }

        private string _authorizationName;
        public string AuthorizationName
        {
            get { return _authorizationName; }
            set { SetProperty(ref _authorizationName, value); }
        }

        private DateTime _birthDateSelected;
        public DateTime BirthDateSelected
        {
            get { return _birthDateSelected; }
            set { SetProperty(ref _birthDateSelected, value); }
        }

        private ComboBoxItemDto _userGroupOptionSelected;
        public ComboBoxItemDto UserGroupOptionSelected
        {
            get { return _userGroupOptionSelected; }
            set { SetProperty(ref _userGroupOptionSelected, value); }
        }

        private ComboBoxItemDto _userGroupAuthOptionSelected;
        public ComboBoxItemDto UserGroupAuthOptionSelected
        {
            get { return _userGroupAuthOptionSelected; }
            set { SetProperty(ref _userGroupAuthOptionSelected, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand AddUserCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddUser);
            });}
        }

        public DelegateCommand UpdateUserCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateUser);
            });}
        }

        public DelegateCommand DeleteUserCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteUser);
            });}
        }

        public DelegateCommand ClearInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshInputs);
            });}
        }

        public DelegateCommand DataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareForUpdate);
            });}
        }

        public DelegateCommand AddUserGroupCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddUserGroup);
            });}
        }

        public DelegateCommand UpdateUserGroupCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateUserGroup);
            });}
        }

        public DelegateCommand DeleteUserGroupCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteUserGroup);
            });}
        }

        public DelegateCommand ClearUserGroupInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshUserGroupInputs);
            });}
        }

        public DelegateCommand UserGroupDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareForUpdateUserGroup);
            });}
        }

        public DelegateCommand AddAuthorizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(AddAuthorization);
            });}
        }

        public DelegateCommand UpdateAuthorizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(UpdateAuthorization);
            });}
        }

        public DelegateCommand DeleteAuthorizationCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(DeleteAuthorization);
            });}
        }

        public DelegateCommand ClearAuthorizationInputsCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(RefreshAuthorizationInputs);
            });}
        }

        public DelegateCommand AuthorizationsDataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(PrepareForUpdateAuthorization);
            });}
        }

        public DelegateCommand UserGroupOptionsSelectionChanged
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(LoadAuthorizationsForUserGroup);
            });}
        }

        public DelegateCommand AuthorizationRightCheckedCommand
        {
            get { return new DelegateCommand(x =>
            {
                InvokeOperation(ChangeRightForAuth, x.ToString());
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeUsers()
        {
            _usersList = Svc.UsersService.ListByCriteria(new UserCriteria()).GetDtos();

            if (Users != null)
            {
                Users.Clear();
                _usersList.ToList().ForEach(x => Users.Add(x));
            }
            else Users = new ObservableCollection<UserDto>(_usersList);
        }

        private void InitializeUserGroups()
        {
            var userGroupsList = Svc.UserGroupsService.ListByCriteria(new UserGroupCriteria()).GetDtos();

            if (UserGroups != null)
            {
                UserGroups.Clear();
                userGroupsList.ToList().ForEach(x => UserGroups.Add(x));
            }
            else UserGroups = new ObservableCollection<UserGroupDto>(userGroupsList);
        }

        private void InitializeAuthorizations()
        {
            var authorizationsList = Svc.AuthorizationService.ListByCriteria(new AuthorizationCriteria()).GetDtos();

            if (Authorizations != null)
            {
                Authorizations.Clear();
                authorizationsList.ToList().ForEach(x => Authorizations.Add(x));
            }
            else Authorizations = new ObservableCollection<AuthorizationDto>(authorizationsList);
        }

        private void InitializeUserGroupAuthorizationMappings()
        {
            _allGroupAuths = Svc.GroupAuthorizationsMapsService
                    .ListByCriteria(new GroupAuthorizationMapCriteria())
                    .GetDtos();

            if (UserGroupAuthorizations != null)
            {
                UserGroupAuthorizations.Clear();
                _allGroupAuths.ToList().ForEach(x => UserGroupAuthorizations.Add(x));
            }
            else UserGroupAuthorizations = new ObservableCollection<GroupAuthorizationMapDto>(_allGroupAuths);
        }

        private void InitializeUserGroupOptions()
        {
            var userGroupOptionsList = Svc.UserGroupsService
                    .ListByCriteria(new UserGroupCriteria())
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Name,
                        Value = x.Id
                    });

            if (UserGroupOptions != null)
            {
                UserGroupOptions.Clear();
                userGroupOptionsList.ToList().ForEach(x => UserGroupOptions.Add(x));
            }
            else UserGroupOptions = new ObservableCollection<ComboBoxItemDto>(userGroupOptionsList);

            UserGroupOptions.Insert(0, new ComboBoxItemDto { Text = @"- Ομάδα -", Value = -1 });
            UserGroupAuthOptionSelected = UserGroupOptionSelected = UserGroupOptions.FirstOrDefault();
        }

        private void AddUser()
        {
            try
            {
                if (!Password.Equals(PasswordConfirm))
                    throw new EiViewException(@"Passwords must match");
                if (UserGroupOptionSelected.Value.ToInt32() < 1)
                    throw new EiViewException(@"Select a valid UserGroup first");

                var userDto = new UserDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Address = Address,
                    Username = Username,
                    Password = Password,
                    GroupId = (int) UserGroupOptionSelected?.Value.ToInt32(),
                    BirthDate = BirthDateSelected
                };
                Svc.UsersService.Create(userDto);
            }
            finally
            {
                _forUpdateUser = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void UpdateUser()
        {
            try
            {
                if (_forUpdateUser)
                {
                    if (!Password.Equals(PasswordConfirm))
                        throw new EiViewException(@"Passwords must match");

                    var userDto = new UserDto
                    {
                        Id = SelectedUser.Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Address = Address,
                        Username = Username,
                        Password = Password,
                        GroupId = UserGroupOptionSelected?.Value.ToInt32() ?? 0,
                        BirthDate = BirthDateSelected
                    };
                    Svc.UsersService.Update(userDto);
                }
            }
            finally
            {
                _forUpdateUser = false;
                InvokeOperation(RefreshInputs);
            }
        }

        private void DeleteUser()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί ο συγκεκριμένος χρήστης;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                Svc.UsersService.Delete(SelectedUser.Id);
            }
            finally
            {
                InvokeOperation(RefreshInputs);
            }
        }

        private void PrepareForUpdate()
        {
            if (SelectedUser != null)
            {
                _forUpdateUser = true;

                FirstName = SelectedUser.FirstName;
                LastName = SelectedUser.LastName;
                BirthDateSelected = SelectedUser.BirthDate;
                Address = SelectedUser.Address;
                Username = SelectedUser.Username;
                UserGroupOptionSelected =
                    UserGroupOptions.FirstOrDefault(x => x.Value.ToInt32() == SelectedUser.GroupId) ??
                    new ComboBoxItemDto();
            }
        }

        private void Search()
        {
            var searchDtos = _usersList
                    .Where(x => x.FirstName.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                    .Where(x => x.LastName.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                    .Where(x => x.Username.ToLower().NoPunctuationGr().Contains(SearchTerm.ToLower().NoPunctuationGr()))
                    .ToList();

            Users.Clear();
            searchDtos.ForEach(x => Users.Add(x));
        }

        private void AddUserGroup()
        {
            try
            {
                Svc.UserGroupsService.Create(new UserGroupDto { Name = UserGroupName });
            }
            finally
            {
                _forUpdateUserGroup = false;
                InvokeOperation(RefreshUserGroupInputs);
            }
        }

        private void UpdateUserGroup()
        {
            try
            {
                if (_forUpdateUserGroup)
                    Svc.UserGroupsService.Update(new UserGroupDto { Id = SelectedUserGroup.Id, Name = UserGroupName });
            }
            finally
            {
                _forUpdateUserGroup = false;
                InvokeOperation(RefreshUserGroupInputs);
            }
        }

        private void DeleteUserGroup()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί η συγκεκριμένη ομάδα χρηστών;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                Svc.UserGroupsService.Delete(SelectedUserGroup.Id);
            }
            finally
            {
                InvokeOperation(RefreshUserGroupInputs);
            }
        }

        private void RefreshUserGroupInputs()
        {
            _forUpdateUserGroup = false;

            UserGroupName = string.Empty;
            InvokeOperation(InitializeUserGroupOptions);
            InvokeOperation(InitializeUserGroups);
            InvokeOperation(InitializeUserGroupAuthorizationMappings);
        }

        private void PrepareForUpdateUserGroup()
        {
            if (SelectedUserGroup != null)
            {
                _forUpdateUserGroup = true;
                UserGroupName = SelectedUserGroup.Name;
            }
        }

        private void AddAuthorization()
        {
            try
            {
                Svc.AuthorizationService.Create(new AuthorizationDto { Name = AuthorizationName });
            }
            finally
            {
                _forUpdateAuthorization = false;
                InvokeOperation(RefreshAuthorizationInputs);
            }
        }

        private void UpdateAuthorization()
        {
            try
            {
                if (_forUpdateAuthorization)
                    Svc.AuthorizationService.Update(new AuthorizationDto { Id = SelectedAuthorization.Id, Name = AuthorizationName });
            }
            finally
            {
                _forUpdateAuthorization = false;
                InvokeOperation(RefreshAuthorizationInputs);
            }
        }

        private void DeleteAuthorization()
        {
            try
            {
                bool deletionConfirmed =
                    @"Θέλετε να διαγραφεί το συγκεκριμένο δικαίωμα;".SimpleQMsg(@"Επιβεβαίωση Διαγραφής");

                if (!deletionConfirmed)
                    return;

                Svc.AuthorizationService.Delete(SelectedAuthorization.Id);
            }
            finally
            {
                InvokeOperation(RefreshAuthorizationInputs);
            }
        }

        private void RefreshAuthorizationInputs()
        {
            _forUpdateAuthorization = false;

            AuthorizationName = string.Empty;
            InvokeOperation(InitializeAuthorizations);
            InvokeOperation(InitializeUserGroupAuthorizationMappings);
            InvokeOperation(LoadAuthorizationsForUserGroup);
        }

        private void PrepareForUpdateAuthorization()
        {
            if (SelectedAuthorization != null)
            {
                _forUpdateAuthorization = true;
                AuthorizationName = SelectedAuthorization.Name;
            }
        }

        private void LoadAuthorizationsForUserGroup()
        {
            var selectedUserGroupId = UserGroupAuthOptionSelected?.Value.ToInt32() ?? 0;
            var userGroupAuthorizations = _allGroupAuths
                .Where(x => x.GroupId == selectedUserGroupId)
                .ToList();

            if (UserGroupAuthorizations != null)
            {
                UserGroupAuthorizations.Clear();
                userGroupAuthorizations.ForEach(x => UserGroupAuthorizations.Add(x));
            }
            else UserGroupAuthorizations = new ObservableCollection<GroupAuthorizationMapDto>(userGroupAuthorizations);
        }

        private void ChangeRightForAuth(string right)
        {
            try
            {
                if (right.Equals(@"C"))
                    SelectedUserGroupAuthorization.C = !SelectedUserGroupAuthorization.C;
                if (right.Equals(@"R"))
                    SelectedUserGroupAuthorization.R = !SelectedUserGroupAuthorization.R;
                if (right.Equals(@"U"))
                    SelectedUserGroupAuthorization.U = !SelectedUserGroupAuthorization.U;
                if (right.Equals(@"D"))
                    SelectedUserGroupAuthorization.D = !SelectedUserGroupAuthorization.D;

                Svc.GroupAuthorizationsMapsService.Update(SelectedUserGroupAuthorization);
            }
            finally
            {
                InvokeOperation(RefreshAuthorizationInputs);
            }
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            _forUpdateUser = false;

            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            PasswordConfirm = string.Empty;
            BirthDateSelected = DateTime.Now;
            UserGroupOptionSelected = null;
            InvokeOperation(InitializeUsers);
            InvokeOperation(InitializeUserGroups);
            InvokeOperation(InitializeUserGroupAuthorizationMappings);
            InvokeOperation(InitializeAuthorizations);
            InvokeOperation(InitializeUserGroupOptions);
            InvokeOperation(LoadAuthorizationsForUserGroup);
        }
    }
}
