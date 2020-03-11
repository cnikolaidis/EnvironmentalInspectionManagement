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

    public interface IFindingsService : IBaseEntityService<Finding, FindingCriteria, FindingDto> { }

    public class FindingsService : BaseEntityService<Finding, FindingCriteria, FindingDto>, IFindingsService
    {
        private readonly EiDbContext _dbContext;

        public FindingsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Finding> ListByCriteria(FindingCriteria criteria)
        {
            var query = _dbContext.Findings.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.DocumentId != null)
                query = query.Where(x => x.DocumentId == criteria.DocumentId);

            if (!string.IsNullOrEmpty(criteria.Description))
                query = query.Where(x => x.Description.Equals(criteria.Description));

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

            if (criteria.DocumentIds != null)
                query = query.Where(x => criteria.DocumentIds.Contains(x.DocumentId));

            return query;
        }

        public override int Create(FindingDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Description))
                throw new EiEntityException(@"Description cannot be empty");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Valid Document must be selected");

            var existingEntity = ListByCriteria(new FindingCriteria
            {
                DocumentId = dto.DocumentId,
                Description = dto.Description
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Finding
            {
                DocumentId = dto.DocumentId,
                Description = dto.Description,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Findings.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(FindingDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Description))
                throw new EiEntityException(@"Description cannot be empty");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Valid Document must be selected");

            var existingEntity = ListByCriteria(new FindingCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.DocumentId = dto.DocumentId;
            existingEntity.Description = dto.Description;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new FindingCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Findings.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class FindingsServiceExtensions
    {
        public static IEnumerable<FindingDto> GetDtos(this IEnumerable<Finding> entities)
        {
            var dtosList = entities
                .Select(x => new FindingDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    DocumentId = x.DocumentId
                });

            return dtosList;
        }
    }
}
