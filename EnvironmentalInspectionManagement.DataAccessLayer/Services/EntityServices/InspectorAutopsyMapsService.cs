namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using System.Linq;
    using Models.Dtos;
    using System;
    using Core;
    #endregion

    public interface IInspectorAutopsyMapsService : IBaseEntityService<InspectorAutopsyMap, InspectorAutopsyMapCriteria, InspectorAutopsyMapDto> { }

    public class InspectorAutopsyMapsService : BaseEntityService<InspectorAutopsyMap, InspectorAutopsyMapCriteria, InspectorAutopsyMapDto>, IInspectorAutopsyMapsService
    {
        private readonly EiDbContext _dbContext;

        public InspectorAutopsyMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<InspectorAutopsyMap> ListByCriteria(InspectorAutopsyMapCriteria criteria)
        {
            var query = _dbContext.InspectorAutopsyMaps.AsQueryable()
                .Include(x => x.Autopsy)
                .Include(x => x.Inspector);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.AutopsyId != null)
                query = query.Where(x => x.AutopsyId == criteria.AutopsyId);
            if (criteria.InspectorId != null)
                query = query.Where(x => x.InspectorId == criteria.InspectorId);

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

            if (criteria.InspectorIds != null)
                query = query.Where(x => criteria.InspectorIds.Contains(x.InspectorId));
            if (criteria.AutopsyIds != null)
                query = query.Where(x => criteria.AutopsyIds.Contains(x.AutopsyId));

            return query;
        }

        public override int Create(InspectorAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new InspectorAutopsyMapCriteria
            {
                AutopsyId = dto.AutopsyId,
                InspectorId = dto.InspectorId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new InspectorAutopsyMap
            {
                InspectorId = dto.InspectorId,
                AutopsyId = dto.AutopsyId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.InspectorAutopsyMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(InspectorAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new InspectorAutopsyMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.InspectorId = dto.InspectorId;
            existingEntity.AutopsyId = dto.AutopsyId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new InspectorAutopsyMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.InspectorAutopsyMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class InspectorAutopsyMapsServiceExtensions
    {
        public static IEnumerable<InspectorAutopsyMapDto> GetDtos(this IEnumerable<InspectorAutopsyMap> entities)
        {
            var entityDtos = entities
                .Select(x => new InspectorAutopsyMapDto
                {
                    Id = x.Id,
                    InspectorId = x.InspectorId,
                    AutopsyId = x.AutopsyId,
                    FirstName = x.Inspector?.FirstName ?? string.Empty,
                    LastName = x.Inspector?.LastName ?? string.Empty,
                    Specialty = x.Inspector?.Specialty ?? string.Empty,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
