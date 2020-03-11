namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using Models.Dtos;
    using System.Linq;
    using System;
    using Core;
    #endregion

    public interface IAutopsyLicenseMapsService : IBaseEntityService<AutopsyLicenseMap, AutopsyLicenseMapCriteria, AutopsyLicenseMapDto> { }

    public class AutopsyLicenseMapsService : BaseEntityService<AutopsyLicenseMap, AutopsyLicenseMapCriteria, AutopsyLicenseMapDto>, IAutopsyLicenseMapsService
    {
        private readonly EiDbContext _dbContext;

        public AutopsyLicenseMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<AutopsyLicenseMap> ListByCriteria(AutopsyLicenseMapCriteria criteria)
        {
            var query = _dbContext.AutopsyLicenseMaps.AsQueryable()
                .Include(x => x.Autopsy)
                .Include(x => x.License);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.AutopsyId != null)
                query = query.Where(x => x.AutopsyId == criteria.AutopsyId);
            if (criteria.LicenseId != null)
                query = query.Where(x => x.LicenseId == criteria.LicenseId);

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);
            if (criteria.DateUpdated != null)
                query = query.Where(x => x.DateUpdated == criteria.DateUpdated);

            if (criteria.FromDateCreated != null)
                query = query.Where(x => x.DateCreated <= criteria.FromDateCreated);
            if (criteria.ToDateCreated != null)
                query = query.Where(x => x.DateCreated >= criteria.ToDateCreated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated <= criteria.FromDateUpdated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated >= criteria.ToDateUpdated);

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));
            if (criteria.AutopsyIds != null)
                query = query.Where(x => criteria.AutopsyIds.Contains(x.AutopsyId));
            if (criteria.LicenseIds != null)
                query = query.Where(x => criteria.LicenseIds.Contains(x.LicenseId));

            return query;
        }

        public override int Create(AutopsyLicenseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId == null || dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.LicenseId == null || dto.LicenseId < 1)
                throw new EiEntityException(@"Valid License must be selected");

            var existingEntity = ListByCriteria(new AutopsyLicenseMapCriteria
            {
                AutopsyId = dto.AutopsyId,
                LicenseId = dto.LicenseId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new AutopsyLicenseMap
            {
                AutopsyId = dto.AutopsyId.Value,
                LicenseId = dto.LicenseId.Value,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.AutopsyLicenseMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(AutopsyLicenseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId == null || dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.LicenseId == null || dto.LicenseId < 1)
                throw new EiEntityException(@"Valid License must be selected");

            var existingEntity = ListByCriteria(new AutopsyLicenseMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");
            
            existingEntity.AutopsyId = dto.AutopsyId.Value;
            existingEntity.LicenseId = dto.LicenseId.Value;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new AutopsyLicenseMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.AutopsyLicenseMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class AutopsyLicenseMapsServiceExtensions
    {
        public static IEnumerable<AutopsyLicenseMapDto> GetDtos(this IEnumerable<AutopsyLicenseMap> entities)
        {
            var dtos = entities
                .Select(x => new AutopsyLicenseMapDto
                {
                    Id = x.Id,
                    AutopsyId = x.AutopsyId,
                    LicenseId = x.LicenseId,
                    DateExpiring = x.License.DateExpiring,
                    DateLicensed = x.License.DateLicensed,
                    LicenseNo = x.License.LicenseNo ?? string.Empty,
                    LicenseNotes = x.License.Notes ?? string.Empty,
                    LicensedBy = x.License.LicensedBy ?? string.Empty
                });

            return dtos;
        }
    }
}
