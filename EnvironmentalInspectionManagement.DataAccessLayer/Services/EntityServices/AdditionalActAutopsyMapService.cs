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

    public interface IAdditionalActAutopsyMapService : IBaseEntityService<AdditionalActAutopsyMap, AdditionalActAutopsyMapCriteria, AdditionalActAutopsyMapDto> { }

    public class AdditionalActAutopsyMapService : BaseEntityService<AdditionalActAutopsyMap, AdditionalActAutopsyMapCriteria, AdditionalActAutopsyMapDto>, IAdditionalActAutopsyMapService
    {
        private readonly EiDbContext _dbContext;

        public AdditionalActAutopsyMapService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<AdditionalActAutopsyMap> ListByCriteria(AdditionalActAutopsyMapCriteria criteria)
        {
            var query = _dbContext.AdditionalActsAutopsyMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.AutopsyId != null)
                query = query.Where(x => x.AutopsyId == criteria.AutopsyId);
            if (criteria.AdditionalActionId != null)
                query = query.Where(x => x.AdditionalActionId == criteria.AdditionalActionId);

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

            if (criteria.Ids != null && criteria.Ids.Any())
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            if (criteria.AdditionalActionIds != null)
                query = query.Where(x => criteria.AdditionalActionIds.Contains(x.AdditionalActionId));
            if (criteria.AutopsyIds != null)
                query = query.Where(x => criteria.AutopsyIds.Contains(x.AutopsyId));

            return query;
        }

        public override int Create(AdditionalActAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.AdditionalActionId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new AdditionalActAutopsyMapCriteria
            {
                AutopsyId = dto.AutopsyId,
                AdditionalActionId = dto.AdditionalActionId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new AdditionalActAutopsyMap
            {
                AdditionalActionId = dto.AdditionalActionId,
                AutopsyId = dto.AutopsyId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.AdditionalActsAutopsyMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(AdditionalActAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.AdditionalActionId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new AdditionalActAutopsyMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.AdditionalActionId = dto.AdditionalActionId;
            existingEntity.AutopsyId = dto.AutopsyId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new AdditionalActAutopsyMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.AdditionalActsAutopsyMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class AdditionalActAutopsyMapServiceExtensions
    {
        public static IEnumerable<AdditionalActAutopsyMapDto> GetDtos(this IEnumerable<AdditionalActAutopsyMap> entities)
        {
            var entityDtos = entities
                .Select(x => new AdditionalActAutopsyMapDto
                {
                    Id = x.Id,
                    AdditionalActionId = x.AdditionalActionId,
                    AutopsyId = x.AutopsyId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
