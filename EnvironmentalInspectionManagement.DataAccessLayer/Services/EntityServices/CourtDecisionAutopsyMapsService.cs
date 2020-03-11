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

    public interface ICourtDecisionAutopsyMapsService : IBaseEntityService<CourtDecisionAutopsyMap, CourtDecisionAutopsyMapCriteria, CourtDecisionAutopsyMapDto> { }

    public class CourtDecisionAutopsyMapsService : BaseEntityService<CourtDecisionAutopsyMap, CourtDecisionAutopsyMapCriteria, CourtDecisionAutopsyMapDto>, ICourtDecisionAutopsyMapsService
    {
        private readonly EiDbContext _dbContext;

        public CourtDecisionAutopsyMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<CourtDecisionAutopsyMap> ListByCriteria(CourtDecisionAutopsyMapCriteria criteria)
        {
            var query = _dbContext.CourtDecisionAutopsyMaps.AsQueryable()
                .Include(x => x.CourtDecision);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.AutopsyId != null)
                query = query.Where(x => x.AutopsyId == criteria.AutopsyId);
            if (criteria.CourtDecisionId != null)
                query = query.Where(x => x.CourtDecisionId == criteria.CourtDecisionId);

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

            if (criteria.CourtDecisionIds != null)
                query = query.Where(x => criteria.CourtDecisionIds.Contains(x.CourtDecisionId));
            if (criteria.AutopsyIds != null)
                query = query.Where(x => criteria.AutopsyIds.Contains(x.AutopsyId));

            return query;
        }

        public override int Create(CourtDecisionAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.CourtDecisionId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new CourtDecisionAutopsyMapCriteria
            {
                AutopsyId = dto.AutopsyId,
                CourtDecisionId = dto.CourtDecisionId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new CourtDecisionAutopsyMap
            {
                CourtDecisionId = dto.CourtDecisionId,
                AutopsyId = dto.AutopsyId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.CourtDecisionAutopsyMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(CourtDecisionAutopsyMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.AutopsyId < 1)
                throw new EiEntityException(@"Valid Autopsy must be selected");
            if (dto.CourtDecisionId < 1)
                throw new EiEntityException(@"Valid Additional Action must be selected");

            var existingEntity = ListByCriteria(new CourtDecisionAutopsyMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.CourtDecisionId = dto.CourtDecisionId;
            existingEntity.AutopsyId = dto.AutopsyId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new CourtDecisionAutopsyMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.CourtDecisionAutopsyMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class CourtDecisionAutopsyMapsServiceExtensions
    {
        public static IEnumerable<CourtDecisionAutopsyMapDto> GetDtos(this IEnumerable<CourtDecisionAutopsyMap> entities)
        {
            var entityDtos = entities
                .Select(x => new CourtDecisionAutopsyMapDto
                {
                    Id = x.Id,
                    CourtDecisionId = x.CourtDecisionId,
                    AutopsyId = x.AutopsyId,
                    CourtDecision = x.CourtDecision?.Name ?? string.Empty,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
