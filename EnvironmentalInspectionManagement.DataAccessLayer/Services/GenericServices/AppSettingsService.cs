namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.GenericServices
{
    #region Usings
    using System.Xml.Linq;
    using System.IO;
    using Core;
    #endregion

    public interface IAppSettingsService : IBaseGenericService
    {
        string Get(string name);
        void Set(string name, string value);
        void Delete(string name);
    }

    public class AppSettingsService : BaseGenericService, IAppSettingsService
    {
        private readonly string _settingsPath;
        private readonly XDocument _settingsDoc;

        public AppSettingsService()
        {
            _settingsPath = Vars.AppSettingsFileLocal;
            _settingsDoc = GetSettingsXmlDocument();
        }

        public string Get(string name)
        {
            var docRoot = _settingsDoc.Root;
            if (docRoot == null)
                return string.Empty;

            var element = docRoot.Element(name);

            return element?.Value ?? string.Empty;
        }

        public void Set(string name, string value)
        {
            var docRoot = _settingsDoc.Root;
            if (docRoot == null)
                return;

            var element = docRoot.Element(name);
            if (element == null)
                docRoot.Add(new XElement(name) { Value = value });
            else element.Value = value;

            _settingsDoc.Save(_settingsPath);
        }

        public void Delete(string name)
        {
            var docRoot = _settingsDoc.Root;

            var element = docRoot?.Element(name);
            if (element == null)
                return;

            element.Remove();
            _settingsDoc.Save(_settingsPath);
        }

        private XDocument GetSettingsXmlDocument()
        {
            var xDoc = !File.Exists(_settingsPath)
                ? CreateSettingsFile()
                : XDocument.Load(_settingsPath);

            return xDoc;
        }

        private XDocument CreateSettingsFile()
        {
            var xDoc = new XDocument(new XElement(@"Settings"));
            xDoc.Save(_settingsPath);

            return xDoc;
        }
    }
}
