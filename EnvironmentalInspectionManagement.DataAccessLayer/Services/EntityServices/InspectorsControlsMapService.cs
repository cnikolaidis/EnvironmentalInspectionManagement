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

    public interface IInspectorsControlsMapService : IBaseEntityService<InspectorsControlsMap, InspectorsControlsMapCriteria, InspectorsControlsMapDto> { }

    public class InspectorsControlsMapService : BaseEntityService<InspectorsControlsMap, InspectorsControlsMapCriteria, InspectorsControlsMapDto>, IInspectorsControlsMapService
    {
        private readonly EiDbContext _dbContext;

        public InspectorsControlsMapService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<InspectorsControlsMap> ListByCriteria(InspectorsControlsMapCriteria criteria)
        {
            var query = _dbContext.InspectorsControlsMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.ControlId != null)
                query = query.Where(x => x.ControlId == criteria.ControlId);
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

            if (criteria.ControlIds != null)
                query = query.Where(x => criteria.ControlIds.Contains(x.ControlId));
            if (criteria.InspectorIds != null)
                query = query.Where(x => criteria.InspectorIds.Contains(x.InspectorId));

            return query;
        }

        public override int Create(InspectorsControlsMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ControlId < 1)
                throw new EiEntityException(@"Valid Control must be selected");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Inspector must be selected");

            var existingEntity = ListByCriteria(new InspectorsControlsMapCriteria
            {
                ControlId = dto.ControlId,
                InspectorId = dto.InspectorId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new InspectorsControlsMap
            {
                ControlId = dto.ControlId,
                InspectorId = dto.InspectorId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.InspectorsControlsMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(InspectorsControlsMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ControlId < 1)
                throw new EiEntityException(@"Valid Control must be selected");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Inspector must be selected");

            var existingEntity = ListByCriteria(new InspectorsControlsMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.ControlId = dto.ControlId;
            existingEntity.InspectorId = dto.InspectorId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new InspectorsControlsMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.InspectorsControlsMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class InspectorsControlsMapServiceExtensions
    {
        public static IEnumerable<InspectorsControlsMapDto> GetDtos(this IEnumerable<InspectorsControlsMap> entities)
        {
            var dtosList = entities
                .Select(x => new InspectorsControlsMapDto
                {
                    Id = x.Id,
                    ControlId = x.ControlId,
                    InspectorId = x.InspectorId
                });

            return dtosList;
        }
    }
}
