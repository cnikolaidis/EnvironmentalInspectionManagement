namespace EnvironmentalInspectionManagement.DataAccessLayer.Services
{
    #region Usings
    using System.Collections.Generic;
    using System;
    using Models;
    #endregion

    public interface IBaseEntityService<out TBaseEntity, in TBaseCriteria, in TBaseDto> : IDisposable
        where TBaseEntity : class, IBaseEntity, new()
        where TBaseCriteria : class, IBaseCriteria, new()
        where TBaseDto : class, IBaseDto, new()
    {
        IEnumerable<TBaseEntity> ListByCriteria(TBaseCriteria criteria);
        int Create(TBaseDto dto);
        void Update(TBaseDto dto);
        void Delete(int id);
    }

    public abstract class BaseEntityService<TBaseEntity, TBaseCriteria, TBaseDto> : IBaseEntityService<TBaseEntity, TBaseCriteria, TBaseDto>
        where TBaseEntity : class, IBaseEntity, new()
        where TBaseCriteria : class, IBaseCriteria, new()
        where TBaseDto : class, IBaseDto, new()
    {
        public abstract IEnumerable<TBaseEntity> ListByCriteria(TBaseCriteria criteria);

        public abstract int Create(TBaseDto dto);

        public abstract void Update(TBaseDto dto);

        public abstract void Delete(int id);

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
