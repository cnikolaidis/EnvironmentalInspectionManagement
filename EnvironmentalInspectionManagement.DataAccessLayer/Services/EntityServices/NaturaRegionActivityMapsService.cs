namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using Models.Exceptions;
    using Models.Entities;
    using Models.Criterias;
    using System.Linq;
    using Models.Dtos;
    using System;
    using Core;
    #endregion

    public interface INaturaRegionActivityMapsService : IBaseEntityService<NaturaRegionActivityMap, NaturaRegionActivityMapCriteria, NaturaRegionActivityMapDto> { }

    public class NaturaRegionActivityMapsService : BaseEntityService<NaturaRegionActivityMap, NaturaRegionActivityMapCriteria, NaturaRegionActivityMapDto>, INaturaRegionActivityMapsService
    {
        private readonly EiDbContext _dbContext;

        public NaturaRegionActivityMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<NaturaRegionActivityMap> ListByCriteria(NaturaRegionActivityMapCriteria criteria)
        {
            var query = _dbContext.NaturaRegionActivityMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.ActivityId != null)
                query = query.Where(x => x.ActivityId == criteria.ActivityId);
            if (criteria.NaturaRegionId != null)
                query = query.Where(x => x.NaturaRegionId == criteria.NaturaRegionId);

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

            if (criteria.ActivityIds != null)
                query = query.Where(x => criteria.ActivityIds.Contains(x.ActivityId));
            if (criteria.NaturaRegionIds != null)
                query = query.Where(x => criteria.NaturaRegionIds.Contains(x.NaturaRegionId));

            return query;
        }

        public override int Create(NaturaRegionActivityMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Valid Activity must be selected");
            if (dto.NaturaRegionId < 1)
                throw new EiEntityException(@"Valid Natura Region must be selected");

            var existingEntity = ListByCriteria(new NaturaRegionActivityMapCriteria
            {
                ActivityId = dto.ActivityId,
                NaturaRegionId = dto.NaturaRegionId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new NaturaRegionActivityMap
            {
                NaturaRegionId = dto.NaturaRegionId,
                ActivityId = dto.ActivityId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.NaturaRegionActivityMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(NaturaRegionActivityMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Valid Activity must be selected");
            if (dto.NaturaRegionId < 1)
                throw new EiEntityException(@"Valid Natura Region must be selected");

            var existingEntity = ListByCriteria(new NaturaRegionActivityMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.NaturaRegionId = dto.NaturaRegionId;
            existingEntity.ActivityId = dto.ActivityId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new NaturaRegionActivityMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.NaturaRegionActivityMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class NaturaRegionActivityMapsServiceExtensions
    {
        public static IEnumerable<NaturaRegionActivityMapDto> GetDtos(this IEnumerable<NaturaRegionActivityMap> entities)
        {
            var entityDtos = entities
                .Select(x => new NaturaRegionActivityMapDto
                {
                    Id = x.Id,
                    ActivityId = x.ActivityId,
                    NaturaRegionId = x.NaturaRegionId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
