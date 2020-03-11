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

    public interface IInspectorCaseMapsService : IBaseEntityService<InspectorCaseMap, InspectorCaseMapCriteria, InspectorCaseMapDto> { }

    public class InspectorCaseMapsService : BaseEntityService<InspectorCaseMap, InspectorCaseMapCriteria, InspectorCaseMapDto>, IInspectorCaseMapsService
    {
        private readonly EiDbContext _dbContext;

        public InspectorCaseMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<InspectorCaseMap> ListByCriteria(InspectorCaseMapCriteria criteria)
        {
            var query = _dbContext.InspectorCaseMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.InspectorId != null)
                query = query.Where(x => x.InspectorId == criteria.InspectorId);
            if (criteria.CaseId != null)
                query = query.Where(x => x.CaseId == criteria.CaseId);

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

            if (criteria.CaseIds != null)
                query = query.Where(x => criteria.CaseIds.Contains(x.CaseId));
            if (criteria.InspectorIds != null)
                query = query.Where(x => criteria.InspectorIds.Contains(x.InspectorId));

            return query;
        }

        public override int Create(InspectorCaseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new InspectorCaseMapCriteria
            {
                CaseId = dto.CaseId,
                InspectorId = dto.InspectorId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new InspectorCaseMap
            {
                InspectorId = dto.InspectorId,
                CaseId = dto.CaseId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.InspectorCaseMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(InspectorCaseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.InspectorId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new InspectorCaseMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.CaseId = dto.CaseId;
            existingEntity.InspectorId = dto.InspectorId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new InspectorCaseMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.InspectorCaseMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class InspectorCaseMapsServiceExtensions
    {
        public static IEnumerable<InspectorCaseMapDto> GetDtos(this IEnumerable<InspectorCaseMap> entities)
        {
            var entityDtos = entities
                .Select(x => new InspectorCaseMapDto
                {
                    Id = x.Id,
                    InspectorId = x.InspectorId,
                    CaseId = x.CaseId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
