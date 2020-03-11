namespace EnvironmentalInspectionManagement.DataAccessLayer.Services
{
    #region Usings
    using System.Data;
    using System;
    #endregion

    public interface IBaseDbReaderService : IDisposable
    {
        DataTable ExecuteQuery(string query);
        DataTable GetTables();
    }

    public abstract class BaseDatabaseReaderService : IBaseDbReaderService
    {
        public abstract DataTable ExecuteQuery(string query);

        public abstract DataTable GetTables();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                Dispose();
        }
    }
}
