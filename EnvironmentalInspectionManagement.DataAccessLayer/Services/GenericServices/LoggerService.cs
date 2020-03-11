namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.GenericServices
{
    #region Usings
    using System.Collections.Generic;
    using Models.Criterias;
    using System.Xml.Linq;
    using Models.Dtos;
    using System.Linq;
    using System.IO;
    using System;
    using Models;
    using Core;
    #endregion

    public interface ILoggerService : IBaseGenericService
    {
        IEnumerable<LogEventDto> ListByCriteria(LogEventCriteria criteria);
        void Trace(string message, object obj = null);
        void Warning(string message, object obj = null);
        void Error(string message, object obj = null);
    }

    public class LoggerService : BaseGenericService, ILoggerService
    {
        private readonly string _logPath;
        private readonly XDocument _logDoc;
        private readonly int _userId;

        public LoggerService()
        {
            _logPath = Vars.LoggerFileLocal;
            _logDoc = GetLoggerXmlDocument();

            var currentPrincipal = System.Threading.Thread.CurrentPrincipal as BasePrincipal;
            _userId = currentPrincipal?.Identity.User?.Id ?? 0;
        }

        public IEnumerable<LogEventDto> ListByCriteria(LogEventCriteria criteria)
        {
            var query = GetLogs();

            if (!string.IsNullOrEmpty(criteria.Color))
                query = query.Where(x => x.Color.Equals(criteria.Color));
            if (!string.IsNullOrEmpty(criteria.Application))
                query = query.Where(x => x.Application.Equals(criteria.Application));
            if (!string.IsNullOrEmpty(criteria.Type))
                query = query.Where(x => x.Type.Equals(criteria.Type));
            if (!string.IsNullOrEmpty(criteria.Source))
                query = query.Where(x => x.Source.Equals(criteria.Source));
            if (!string.IsNullOrEmpty(criteria.Message))
                query = query.Where(x => x.Message.Equals(criteria.Message));
            if (!string.IsNullOrEmpty(criteria.RawMessage))
                query = query.Where(x => x.RawMessage.Equals(criteria.RawMessage));
            if (!string.IsNullOrEmpty(criteria.User))
                query = query.Where(x => x.User.Equals(criteria.User));

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);

            if (criteria.DateCreatedFrom != null)
                query = query.Where(x => x.DateCreated <= criteria.DateCreatedFrom);
            if (criteria.DateCreatedTo != null)
                query = query.Where(x => x.DateCreated >= criteria.DateCreatedTo);

            return query;
        }

        public void Trace(string message, object obj = null) => PassLog(@"black", message, @"Trace", obj);

        public void Warning(string message, object obj = null) => PassLog(@"yellow", message, @"Warning", obj);

        public void Error(string message, object obj = null) => PassLog(@"red", message, @"Error", obj);

        private void PassLog(string clr, string msg, string type, object obj = null)
        {
            if (obj is Exception)
            {
                var xLogsList = GetExceptions((Exception)obj);
                Log(clr, xLogsList);
            }
            else
            {
                var jsonObj = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
                var logEvent = new LogEventDto
                {
                    Message = msg,
                    RawMessage = jsonObj,
                    Type = type,
                    User = $"{_userId}",
                    DateCreated = DateTime.Now
                };
                Log(clr, logEvent);
            }
        }

        private void Log(string color, IEnumerable<LogEventDto> logEvents)
        {
            foreach (var logEvent in logEvents)
                Log(color, logEvent);
        }

        private void Log(string color, LogEventDto logEvent)
        {
            var docRoot = _logDoc.Root;
            if (docRoot == null)
                return;

            var logElement = new XElement(@"Log");
            logElement.SetAttributeValue(@"color", color);
            logElement.Add(new XElement(@"Application") { Value = logEvent.Application ?? string.Empty });
            logElement.Add(new XElement(@"Message") { Value = logEvent.Message ?? string.Empty });
            logElement.Add(new XElement(@"RawMessage") { Value = logEvent.RawMessage ?? string.Empty });
            logElement.Add(new XElement(@"Source") { Value = logEvent.Source ?? string.Empty });
            logElement.Add(new XElement(@"Type") { Value = logEvent.Type ?? string.Empty });
            logElement.Add(new XElement(@"User") { Value = logEvent.User ?? string.Empty });
            logElement.Add(new XElement(@"DateCreated") { Value = logEvent.DateCreated.ToString("dd/MM/yyyy HH:mm:ss") });

            docRoot.Add(logElement);
            _logDoc.Save(_logPath);
        }

        private XDocument GetLoggerXmlDocument()
        {
            var logDoc = !File.Exists(_logPath)
                ? CreateLoggerFile()
                : XDocument.Load(_logPath);

            return logDoc;
        }

        private XDocument CreateLoggerFile()
        {
            var xDoc = new XDocument(new XElement(@"Logs"));
            xDoc.Save(_logPath);

            return xDoc;
        }

        private IEnumerable<LogEventDto> GetExceptions(Exception x)
        {
            var xList = new List<Exception> { x };
            while (x.InnerException != null)
            {
                x = x.InnerException;
                xList.Add(x);
            }

            var xLogsList = new List<LogEventDto>();
            xList.ForEach(e =>
            {
                var logEvent = new LogEventDto
                {
                    Application = e.Source,
                    Message = e.Message,
                    RawMessage = e.StackTrace,
                    Source = $"{e.TargetSite}",
                    Type = $"{e.GetType()}",
                    User = $"{_userId}",
                    DateCreated = DateTime.Now
                };
                xLogsList.Add(logEvent);
            });

            return xLogsList.AsEnumerable();
        }

        private IEnumerable<LogEventDto> GetLogs()
        {
            var logs = _logDoc.Root?.Nodes()
                .Select(x => new LogEventDto
                {
                    Color = (x as XElement)?.Attribute(@"color")?.Value ?? string.Empty,
                    Application = (x as XElement)?.Element(@"Application")?.Value ?? string.Empty,
                    Message = (x as XElement)?.Element(@"Message")?.Value ?? string.Empty,
                    RawMessage = (x as XElement)?.Element(@"RawMessage")?.Value ?? string.Empty,
                    Source = (x as XElement)?.Element(@"Source")?.Value ?? string.Empty,
                    Type = (x as XElement)?.Element(@"Type")?.Value ?? string.Empty,
                    User = (x as XElement)?.Element(@"User")?.Value ?? string.Empty,
                    DateCreated = Convert.ToDateTime((x as XElement)?.Element(@"DateCreated")?.Value)
                });

            return logs;
        }
    }
}
