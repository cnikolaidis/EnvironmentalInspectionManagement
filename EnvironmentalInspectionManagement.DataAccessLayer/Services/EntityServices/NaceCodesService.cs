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

    public interface INaceCodesService : IBaseEntityService<NaceCode, NaceCodeCriteria, NaceCodeDto> { }

    public class NaceCodesService : BaseEntityService<NaceCode, NaceCodeCriteria, NaceCodeDto>, INaceCodesService
    {
        private readonly EiDbContext _dbContext;

        public NaceCodesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<NaceCode> ListByCriteria(NaceCodeCriteria criteria)
        {
            var query = _dbContext.NaceCodes.AsQueryable()
                .Include(x => x.NaceCodeSector);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.SectorId != null)
                query = query.Where(x => x.SectorId == criteria.SectorId);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.Class))
                query = query.Where(x => x.Class.Equals(criteria.Class));

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
            if (criteria.SectorIds != null)
                query = query.Where(x => criteria.SectorIds.Contains(x.SectorId));

            return query;
        }

        public override int Create(NaceCodeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Class))
                throw new EiEntityException(@"Class cannot be empty");
            if (dto.SectorId < 1)
                throw new EiEntityException(@"Nace Code must have a valid Sector");

            var existingEntity = ListByCriteria(new NaceCodeCriteria
            {
                Name = dto.Name,
                Class = dto.Class,
                SectorId = dto.SectorId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new NaceCode
            {
                Name = dto.Name,
                Class = dto.Class,
                SectorId = dto.SectorId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.NaceCodes.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(NaceCodeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Class))
                throw new EiEntityException(@"Class cannot be empty");
            if (dto.SectorId < 1)
                throw new EiEntityException(@"Nace Code must have a valid Sector");

            var existingEntity = ListByCriteria(new NaceCodeCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.SectorId = dto.SectorId;
            existingEntity.Class = dto.Class;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new NaceCodeCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.NaceCodes.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class NaceCodesServiceExtensions
    {
        public static IEnumerable<NaceCodeDto> GetDtos(this IEnumerable<NaceCode> entities)
        {
            var entityDtos = entities
                .Select(x => new NaceCodeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Class = x.Class,
                    SectorId = x.SectorId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Sector = x.NaceCodeSector?.Name ?? string.Empty
                });

            return entityDtos;
        }
    }
}
