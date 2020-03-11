namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using Models.Dtos;
    using System.Linq;
    using System;
    using Core;
    #endregion

    public interface ILicensesService : IBaseEntityService<License, LicenseCriteria, LicenseDto> { }

    public class LicensesService : BaseEntityService<License, LicenseCriteria, LicenseDto>, ILicensesService
    {
        private readonly EiDbContext _dbContext;

        public LicensesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<License> ListByCriteria(LicenseCriteria criteria)
        {
            var query = _dbContext.Licenses.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.LicenseNo))
                query = query.Where(x => x.LicenseNo.Equals(criteria.LicenseNo));
            if (!string.IsNullOrEmpty(criteria.LicensedBy))
                query = query.Where(x => x.LicensedBy.Equals(criteria.LicensedBy));
            if (!string.IsNullOrEmpty(criteria.Notes))
                query = query.Where(x => x.Notes.Equals(criteria.Notes));

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);
            if (criteria.DateUpdated != null)
                query = query.Where(x => x.DateUpdated == criteria.DateUpdated);

            if (criteria.DateLicensed != null)
                query = query.Where(x => x.DateLicensed == criteria.DateLicensed);
            if (criteria.DateExpiring != null)
                query = query.Where(x => x.DateExpiring == criteria.DateExpiring);

            if (criteria.FromDateCreated != null)
                query = query.Where(x => x.DateCreated <= criteria.FromDateCreated);
            if (criteria.ToDateCreated != null)
                query = query.Where(x => x.DateCreated >= criteria.ToDateCreated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated <= criteria.FromDateUpdated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated >= criteria.ToDateUpdated);

            if (criteria.DateLicensedFrom != null)
                query = query.Where(x => x.DateLicensed <= criteria.DateLicensedFrom);
            if (criteria.DateLicensedTo != null)
                query = query.Where(x => x.DateUpdated >= criteria.DateLicensedTo);
            if (criteria.DateExpiringFrom != null)
                query = query.Where(x => x.DateUpdated <= criteria.DateExpiringFrom);
            if (criteria.DateExpiringTo != null)
                query = query.Where(x => x.DateExpiring >= criteria.DateExpiringTo);

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            return query;
        }

        public override int Create(LicenseDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.LicenseNo))
                throw new EiEntityException(@"License No cannot be empty");
            if (string.IsNullOrEmpty(dto.LicensedBy))
                throw new EiEntityException(@"Licensed By cannot be empty");
            if (dto.DateLicensed == null)
                throw new EiEntityException(@"Date Licensed must not be empty");
            if (dto.DateExpiring == null)
                throw new EiEntityException(@"Date Expiring must not be empty");

            var existingEntity = ListByCriteria(new LicenseCriteria
            {
                LicenseNo = dto.LicenseNo,
                LicensedBy = dto.LicensedBy
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new License
            {
                LicenseNo = dto.LicenseNo,
                LicensedBy = dto.LicensedBy,
                Notes = dto.Notes,
                DateLicensed = dto.DateLicensed.Value,
                DateExpiring = dto.DateExpiring.Value,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Licenses.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(LicenseDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.LicenseNo))
                throw new EiEntityException(@"License No cannot be empty");
            if (string.IsNullOrEmpty(dto.LicensedBy))
                throw new EiEntityException(@"Licensed By cannot be empty");
            if (dto.DateLicensed == null)
                throw new EiEntityException(@"Date Licensed must not be empty");
            if (dto.DateExpiring == null)
                throw new EiEntityException(@"Date Expiring must not be empty");

            var existingEntity = ListByCriteria(new LicenseCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.LicenseNo = dto.LicenseNo;
            existingEntity.LicensedBy = dto.LicensedBy;
            existingEntity.Notes = dto.Notes;
            existingEntity.DateLicensed = dto.DateLicensed.Value;
            existingEntity.DateExpiring = dto.DateExpiring.Value;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new LicenseCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Licenses.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class LicensesServiceExtensions
    {
        public static IEnumerable<LicenseDto> GetDtos(this IEnumerable<License> entities)
        {
            var dtos = entities
                .Select(x => new LicenseDto
                {
                    Id = x.Id,
                    DateExpiring = x.DateExpiring,
                    DateLicensed = x.DateLicensed,
                    LicenseNo = x.LicenseNo,
                    LicensedBy = x.LicensedBy,
                    Notes = x.Notes
                });

            return dtos;
        }
    }
}
