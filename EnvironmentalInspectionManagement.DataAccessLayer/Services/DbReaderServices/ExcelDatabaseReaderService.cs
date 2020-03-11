namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.DbReaderServices
{
    #region Usings
    using System.Data.OleDb;
    using System.Data;
    #endregion

    public interface IExcelDatabaseService : IBaseDbReaderService { }

    public class ExcelDatabaseReaderService : BaseDatabaseReaderService, IExcelDatabaseService
    {
        #region Properties
        private readonly OleDbConnection _dbConnection;
        #endregion

        #region Constructor
        public ExcelDatabaseReaderService(string filePath)
        {
            _dbConnection = new OleDbConnection(Core.Vars.DbStrings.ExcelJetDb(filePath));
            _dbConnection.Open();
        }
        #endregion

        public override DataTable GetTables() => _dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        public override DataTable ExecuteQuery(string query)
        {
            var dataTable = new DataTable();

            using (var oleDbCmd = new OleDbCommand { CommandText = query, Connection = _dbConnection })
            using (var dataReader = oleDbCmd.ExecuteReader())
                if (dataReader != null)
                    dataTable.Load(dataReader);

            return dataTable;
        }
    }
}
