namespace EnvironmentalInspectionManagement.DataAccessLayer.Core
{
    public static class Vars
    {
        public static string AppSettingsFile = string.Empty;
        public static string AppSettingsFileLocal = @"appSettings.xml";

        public static string LoggerFile = string.Empty;
        public static string LoggerFileLocal = @"logs.xml";

        public class DbStrings
        {
            //SQLite
            public static string SqLiteDb(string filePath) => $"DataSource={filePath};{DbProps.SqLiteDbExtendedProperties}";

            //Excel
            public static string ExcelJetDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.JetDbExtendedProperties}";
            public static string ExcelJetOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.JetDbExtendedProperties}";

            //Excel ACE
            public static string ExcelAceDb(string filePath) => $"{DbProps.AceDbProvider}Data Source={filePath};{DbProps.AceDbExtendedProperties}";
            public static string ExcelAceOleDb(string filePath) => $"{DbProps.AceOleDbProvider}Data Source={filePath};{DbProps.AceDbExtendedProperties}";

            //Paradox
            public static string ParadoxDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.ParadoxDbExtendedProperties}";
            public static string ParadoxOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.ParadoxDbExtendedProperties}";

            //Text
            public static string TextDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.TextDbExtendedProperties}";
            public static string TextOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.TextDbExtendedProperties}";

            //Access (StandardSecurity)
            public static string AccessDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.AccessStandardSecurity}";
            public static string AccessOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.AccessStandardSecurity}";

            //Access (DbProps Security)
            public static string AccessSecureDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.AccessDatabasePassword}";
            public static string AccessSecureOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.AccessDatabasePassword}";

            //Html
            public static string HtmlDb(string filePath) => $"{DbProps.JetDbProvider}Data Source={filePath};{DbProps.HtmlDbExtendedProperties}";
            public static string HtmlOleDb(string filePath) => $"{DbProps.JetOleDbProvider}Data Source={filePath};{DbProps.HtmlDbExtendedProperties}";

            /// <summary>
            /// HDR     ::      First row contains column names, no data [Yes / No]
            /// IMEX    ::      Read Intermixed columns as text [1 / 0]
            /// FMT     ::      (Edit Registry) Column length
            /// </summary>
            protected static class DbProps
            {
                //Provider Strings
                public static readonly string JetDbProvider = @"Provider=Microsoft.Jet.OLEDB.4.0;";
                public static readonly string JetOleDbProvider = @"OLEDB;Provider=Microsoft.Jet.OLEDB.4.0;";
                public static readonly string AceDbProvider = @"Provider=Microsoft.ACE.OLEDB.12.0;";
                public static readonly string AceOleDbProvider = @"OLEDB;Provider=Microsoft.ACE.OLEDB.12.0;";

                //DbProps Extended Properties Strings
                public static readonly string JetDbExtendedProperties = @"Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';";
                public static readonly string AceDbExtendedProperties = @"Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'";
                public static readonly string SqLiteDbExtendedProperties = @"Version=3;Foreign Keys=true;FailIfMissing=True;";
                public static readonly string ParadoxDbExtendedProperties = @"Extended Properties=Paradox 5.x;";
                public static readonly string TextDbExtendedProperties = @"Extended Properties='text;HDR=Yes;FMT=Delimited';";
                public static readonly string AccessStandardSecurity = @"User Id=admin;Password=;";
                public static readonly string AccessDatabasePassword = @"Jet OLEDB:Database Password=[PASSWORD];";
                public static readonly string HtmlDbExtendedProperties = @"Extended Properties='HTML Import;HDR=YES;IMEX=1';";
            }
        }
    }
}
