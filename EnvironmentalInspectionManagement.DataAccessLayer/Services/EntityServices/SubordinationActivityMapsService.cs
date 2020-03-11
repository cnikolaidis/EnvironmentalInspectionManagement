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

    public interface ISubordinationActivityMapsService : IBaseEntityService<SubordinationActivityMap, SubordinationActivityMapCriteria, SubordinationActivityMapDto> { }

    class SubordinationActivityMapsService : BaseEntityService<SubordinationActivityMap, SubordinationActivityMapCriteria, SubordinationActivityMapDto>, ISubordinationActivityMapsService
    {
        private readonly EiDbContext _dbContext;

        public SubordinationActivityMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<SubordinationActivityMap> ListByCriteria(SubordinationActivityMapCriteria criteria)
        {
            var query = _dbContext.SubordinationActivityMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.SubordinationId != null)
                query = query.Where(x => x.SubordinationId == criteria.SubordinationId);
            if (criteria.ActivityId != null)
                query = query.Where(x => x.ActivityId == criteria.ActivityId);

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

            if (criteria.SubordinationIds != null)
                query = query.Where(x => criteria.SubordinationIds.Contains(x.SubordinationId));
            if (criteria.ActivityIds != null)
                query = query.Where(x => criteria.ActivityIds.Contains(x.ActivityId));

            return query;
        }

        public override int Create(SubordinationActivityMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.SubordinationId < 1)
                throw new EiEntityException(@"Valid Subordination must be selected");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Valid Activity Action must be selected");

            var existingEntity = ListByCriteria(new SubordinationActivityMapCriteria
            {
                SubordinationId = dto.SubordinationId,
                ActivityId = dto.ActivityId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new SubordinationActivityMap
            {
                SubordinationId = dto.SubordinationId,
                ActivityId = dto.ActivityId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.SubordinationActivityMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(SubordinationActivityMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.SubordinationId < 1)
                throw new EiEntityException(@"Valid Subordination must be selected");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Valid Activity Action must be selected");

            var existingEntity = ListByCriteria(new SubordinationActivityMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.SubordinationId = dto.SubordinationId;
            existingEntity.ActivityId = dto.ActivityId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new SubordinationActivityMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.SubordinationActivityMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class SubordinationActivityMapsServiceExtensions
    {
        public static IEnumerable<SubordinationActivityMapDto> GetDtos(this IEnumerable<SubordinationActivityMap> entities)
        {
            var entityDtos = entities
                .Select(x => new SubordinationActivityMapDto
                {
                    Id = x.Id,
                    SubordinationId = x.SubordinationId,
                    ActivityId = x.ActivityId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
