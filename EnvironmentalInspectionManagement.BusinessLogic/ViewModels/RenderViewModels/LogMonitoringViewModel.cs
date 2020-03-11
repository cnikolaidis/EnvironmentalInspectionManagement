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

    public interface ILogMonitoringViewModel : IBaseViewModel { }

    public class LogMonitoringViewModel : BaseViewModel, ILogMonitoringViewModel
    {
        #region Properties
        private static IEnumerable<LogEventDto> _logEventItems;
        #endregion

        #region Constructor
        public LogMonitoringViewModel()
        {
            RefreshInputs();
        }
        #endregion

        #region ViewModel Properties
        public ObservableCollection<KeyValuePair<string, int>> LogEventTypes { get; set; }
        public ObservableCollection<KeyValuePair<string, int>> LogEventsPerUser { get; set; }
        public ObservableCollection<KeyValuePair<string, int>> LogEventsInTime { get; set; }
        public ObservableCollection<ComboBoxItemDto> LogTypes { get; set; }
        public ObservableCollection<ComboBoxItemDto> UsersOptions { get; set; }
        public ObservableCollection<LogEventDto> LogEvents { get; set; }
        public LogEventDto SelectedLogEvent { get; set; }

        private ComboBoxItemDto _selectedLogType;
        public ComboBoxItemDto SelectedLogType
        {
            get { return _selectedLogType; }
            set
            {
                SetProperty(ref _selectedLogType, value);
                SearchByFilters();
            }
        }

        private ComboBoxItemDto _selectedUser;
        public ComboBoxItemDto SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                SetProperty(ref _selectedUser, value);
                SearchByFilters();
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                SetProperty(ref _searchTerm, value);
                Search();
            }
        }

        private string _selectedLogEventColor;
        public string SelectedLogEventColor
        {
            get { return _selectedLogEventColor; }
            set { SetProperty(ref _selectedLogEventColor, value); }
        }

        private string _selectedLogEventApplication;
        public string SelectedLogEventApplication
        {
            get { return _selectedLogEventApplication; }
            set { SetProperty(ref _selectedLogEventApplication, value); }
        }

        private string _selectedLogEventType;
        public string SelectedLogEventType
        {
            get { return _selectedLogEventType; }
            set { SetProperty(ref _selectedLogEventType, value); }
        }
        
        private string _selectedLogEventSource;
        public string SelectedLogEventSource
        {
            get { return _selectedLogEventSource; }
            set { SetProperty(ref _selectedLogEventSource, value); }
        }

        private string _selectedLogEventMessage;
        public string SelectedLogEventMessage
        {
            get { return _selectedLogEventMessage; }
            set { SetProperty(ref _selectedLogEventMessage, value); }
        }

        private string _selectedLogEventRawMessage;
        public string SelectedLogEventRawMessage
        {
            get { return _selectedLogEventRawMessage; }
            set { SetProperty(ref _selectedLogEventRawMessage, value); }
        }

        private string _selectedLogEventUser;
        public string SelectedLogEventUser
        {
            get { return _selectedLogEventUser; }
            set { SetProperty(ref _selectedLogEventUser, value); }
        }

        private string _selectedLogEventDateCreated;
        public string SelectedLogEventDateCreated
        {
            get { return _selectedLogEventDateCreated; }
            set { SetProperty(ref _selectedLogEventDateCreated, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand DataGridRowDoubleClick
        {
            get { return new DelegateCommand(x =>
            {
                PreviewLogEvent();
            });}
        }
        #endregion

        #region Operation Methods
        private void InitializeLogEventsAndUserOptions()
        {
            try
            {
                var users = Svc.UsersService
                    .ListByCriteria(new UserCriteria())
                    .ToList();
                var usersOptions = users
                    .Select(x => new ComboBoxItemDto
                    {
                        Text = x.Username,
                        Value = x.Id
                    }).ToList();
                _logEventItems = Svc.LoggerService
                    .ListByCriteria(new LogEventCriteria())
                    .Select(x => new LogEventDto
                    {
                        Application = x.Application,
                        Color = x.Color,
                        DateCreated = x.DateCreated,
                        Message = x.Message,
                        RawMessage = x.RawMessage,
                        Source = x.Source,
                        Type = x.Type,
                        User = users.FirstOrDefault(y => y.Id == x.User.ToInt32())?.Username ?? @"Χωρίς Χρήστη",
                        UserId = users.FirstOrDefault(y => y.Id == x.User.ToInt32())?.Id ?? 0
                    })
                    .ToList();

                if (LogEvents != null)
                {
                    LogEvents.Clear();
                    _logEventItems.ToList().ForEach(x => LogEvents.Add(x));
                }
                else LogEvents = new ObservableCollection<LogEventDto>(_logEventItems);

                if (UsersOptions != null)
                {
                    UsersOptions.Clear();
                    usersOptions.ToList().ForEach(x => UsersOptions.Add(x));
                }
                else UsersOptions = new ObservableCollection<ComboBoxItemDto>(usersOptions);

                UsersOptions.Insert(0, new ComboBoxItemDto { Text = @"- Χρήστης -", Value = -1 });
                SelectedUser = UsersOptions.FirstOrDefault();
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void InitializeLogTypes()
        {
            try
            {
                var logEventTypes = new List<ComboBoxItemDto>
                {
                    new ComboBoxItemDto { Text = @"- Όλα -", Value = -1 },
                    new ComboBoxItemDto {Text = @"Ίχνος", Value = 1},
                    new ComboBoxItemDto {Text = @"Προσοχή", Value = 2},
                    new ComboBoxItemDto {Text = @"Σφάλμα", Value = 3}
                };

                if (LogTypes != null)
                {
                    LogTypes.Clear();
                    logEventTypes.ForEach(x => LogTypes.Add(x));
                }
                else LogTypes = new ObservableCollection<ComboBoxItemDto>(logEventTypes);

                SelectedLogType = LogTypes.FirstOrDefault();
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void InitializeLogEventsPerUser()
        {
            try
            {
                var userLogEvents = Svc.LoggerService
                    .ListByCriteria(new LogEventCriteria())
                    .ToList();
                var userLogEventUsers = userLogEvents
                    .GroupBy(x => x.User)
                    .Select(x => x.Key.ToInt32())
                    .ToList();
                var userNames = Svc.UsersService
                    .ListByCriteria(new UserCriteria {Ids = userLogEventUsers})
                    .GetDtos()
                    .ToList();
                var logEventsPerUser = userLogEvents
                    .GroupBy(x => x.User)
                    .Select(x => new KeyValuePair<string, int>(
                        x.Key.Equals(@"0")
                            ? @"Χωρίς χρήστη"
                            : userNames.FirstOrDefault(y => y.Id == x.Key.ToInt32())?.Username ?? @"Διεγραμμένος Χρήστης",
                        x.Count()))
                    .ToList();

                if (LogEventsPerUser != null)
                {
                    LogEventsPerUser.Clear();
                    logEventsPerUser.ForEach(x => LogEventsPerUser.Add(x));
                }
                else LogEventsPerUser = new ObservableCollection<KeyValuePair<string, int>>(logEventsPerUser);
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void InitializeLogEventTypes()
        {
            try
            {
                var logEventTypes = _logEventItems
                    .GroupBy(x => x.Color)
                    .Select(x => new KeyValuePair<string, int>(
                        x.Key.Equals(@"black")
                            ? $"Info ({x.Count()})"
                            : x.Key.Equals(@"yellow")
                                ? $"Warning ({x.Count()})"
                                : x.Key.Equals(@"red")
                                    ? $"Error ({x.Count()})"
                                    : $"Info ({x.Count()})",
                        x.Count()))
                    .ToList();

                if (LogEventTypes != null)
                {
                    LogEventTypes.Clear();
                    logEventTypes.ForEach(x => LogEventTypes.Add(x));
                }
                else LogEventTypes = new ObservableCollection<KeyValuePair<string, int>>(logEventTypes);
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void InitializeLogEventsInTime()
        {
            try
            {
                var logEventsInTime = _logEventItems
                    .GroupBy(x => x.DateCreated.Date)
                    .Select(x => new KeyValuePair<string, int>(x.Key.ToString("dd/MM/yyyy"), x.Count()))
                    .ToList();

                if (LogEventsInTime != null)
                {
                    LogEventsInTime.Clear();
                    logEventsInTime.ForEach(x => LogEventsInTime.Add(x));
                }
                else LogEventsInTime = new ObservableCollection<KeyValuePair<string, int>>(logEventsInTime);
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void PreviewLogEvent()
        {
            try
            {
                if (SelectedLogEvent == null)
                    return;

                SelectedLogEventColor = SelectedLogEvent?.Color ?? string.Empty;
                SelectedLogEventApplication = SelectedLogEvent?.Application ?? string.Empty;
                SelectedLogEventType = SelectedLogEvent?.Type ?? string.Empty;
                SelectedLogEventSource = SelectedLogEvent?.Source ?? string.Empty;
                SelectedLogEventMessage = SelectedLogEvent?.Message ?? string.Empty;
                SelectedLogEventRawMessage = SelectedLogEvent?.RawMessage ?? string.Empty;
                SelectedLogEventUser = SelectedLogEvent?.User ?? string.Empty;
                SelectedLogEventDateCreated = SelectedLogEvent?.DateCreated.ToString("dd/MM/yyyy");
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void Search()
        {
            try
            {
                var seachDtos = _logEventItems
                    .Where(x => x.Message.ToLower().Contains(SearchTerm.ToLower()))
                    .ToList();

                LogEvents.Clear();
                seachDtos.ForEach(x => LogEvents.Add(x));
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }

        private void SearchByFilters()
        {
            try
            {
                var logEventTypeId = SelectedLogType?.Value.ToInt32() ?? 0;
                var userId = SelectedUser?.Value.ToInt32() ?? 0;

                var logEvents = _logEventItems;

                if (logEventTypeId == 1)
                    logEvents = _logEventItems
                        .Where(x => x.Color.Equals(@"black"))
                        .ToList();
                if (logEventTypeId == 2)
                    logEvents = _logEventItems
                        .Where(x => x.Color.Equals(@"yellow"))
                        .ToList();
                if (logEventTypeId == 3)
                    logEvents = _logEventItems
                        .Where(x => x.Color.Equals(@"red"))
                        .ToList();
                if (logEventTypeId < 1)
                    logEvents = _logEventItems;
                if (userId > 0)
                    logEvents = logEvents
                        .Where(x => x.UserId == userId);

                LogEvents.Clear();
                logEvents.ToList().ForEach(x => LogEvents.Add(x));
            }
            catch (Exception x)
            {
                x.ShowException();
            }
        }
        #endregion

        public sealed override void RefreshInputs()
        {
            InitializeLogEventsAndUserOptions();
            InitializeLogTypes();
            InitializeLogEventTypes();
            InitializeLogEventsPerUser();
            InitializeLogEventsInTime();
        }
    }
}
