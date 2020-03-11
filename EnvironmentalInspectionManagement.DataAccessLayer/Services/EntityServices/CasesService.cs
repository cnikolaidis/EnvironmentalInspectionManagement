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

    public interface ICasesService : IBaseEntityService<Case, CaseCriteria, CaseDto> { }

    public class CasesService : BaseEntityService<Case, CaseCriteria, CaseDto>, ICasesService
    {
        private readonly EiDbContext _dbContext;

        public CasesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Case> ListByCriteria(CaseCriteria criteria)
        {
            var query = _dbContext.Cases.AsQueryable()
                .Include(x => x.DocumentCaseMaps)
                .Include(x => x.Activity)
                .Include(x => x.ControlProgress);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.ActivityId != null)
                query = query.Where(x => x.ActivityId == criteria.ActivityId);
            if (criteria.ControlTriggerId != null)
                query = query.Where(x => x.ControlTriggerId == criteria.ControlTriggerId);
            if (criteria.ControlProgressId != null)
                query = query.Where(x => x.ControlProgressId == criteria.ControlProgressId);

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

            if (criteria.FromDateCreated != null)
                query = query.Where(x => x.DateCreated <= criteria.FromDateCreated);
            if (criteria.ToDateCreated != null)
                query = query.Where(x => x.DateCreated >= criteria.ToDateCreated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated <= criteria.FromDateUpdated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated >= criteria.ToDateUpdated);

            if (criteria.FromDateStarted != null)
                query = query.Where(x => x.DateStarted <= criteria.FromDateStarted);
            if (criteria.ToDateStarted != null)
                query = query.Where(x => x.DateStarted >= criteria.ToDateStarted);
            if (criteria.FromDateEnded != null)
                query = query.Where(x => x.DateEnded <= criteria.FromDateEnded);
            if (criteria.ToDateEnded != null)
                query = query.Where(x => x.DateEnded >= criteria.ToDateEnded);

            if (criteria.Ids != null && criteria.Ids.Any())
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            if (criteria.ActivityIds != null)
                query = query.Where(x => criteria.ActivityIds.Contains(x.ActivityId));
            if (criteria.ControlTriggerIds != null)
                query = query.Where(x => criteria.ControlTriggerIds.Contains(x.ControlTriggerId));
            if (criteria.ControlProgressIds != null)
                query = query.Where(x => criteria.ControlProgressIds.Contains(x.ControlProgressId));

            return query;
        }

        public override int Create(CaseDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Case must have a valid Activity");

            var existingEntity = ListByCriteria(new CaseCriteria
            {
                ActivityId = dto.ActivityId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Case
            {
                ControlTriggerId = dto.ControlTriggerId,
                ActivityId = dto.ActivityId,
                ControlProgressId = dto.ControlProgressId,
                DateStarted = DateTime.Now,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Cases.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(CaseDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Case must have a valid Activity");

            var existingEntity = ListByCriteria(new CaseCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.DateStarted = dto.DateStarted;
            existingEntity.DateEnded = dto.DateEnded;
            existingEntity.ControlTriggerId = dto.ControlTriggerId;
            existingEntity.ActivityId = dto.ActivityId;
            existingEntity.ControlProgressId = dto.ControlProgressId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new CaseCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Cases.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class CasesServiceExtensions
    {
        public static IEnumerable<CaseDto> GetDtos(this IEnumerable<Case> entities)
        {
            var entityDtos = entities
                .Select(x => new CaseDto
                {
                    Id = x.Id,
                    DateStarted = x.DateStarted,
                    DateEnded = x.DateEnded,
                    ControlTriggerId = x.ControlTriggerId,
                    ActivityId = x.ActivityId,
                    ControlProgressId = x.ControlProgressId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ActivityName = x.Activity?.Name ?? string.Empty,
                    CaseProgress = x.ControlProgress?.Name ?? @"Άγνωστη",
                    DocumentsCount = x.DocumentCaseMaps.Count
                });

            return entityDtos;
        }
    }
}
