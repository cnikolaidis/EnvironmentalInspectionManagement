namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using System.Linq;
    using Models.Dtos;
    using System;
    using Core;
    #endregion

    public interface IDocumentCaseMapsService : IBaseEntityService<DocumentCaseMap, DocumentCaseMapCriteria, DocumentCaseMapDto> { }

    public class DocumentCaseMapsService : BaseEntityService<DocumentCaseMap, DocumentCaseMapCriteria, DocumentCaseMapDto>, IDocumentCaseMapsService
    {
        private readonly EiDbContext _dbContext;

        public DocumentCaseMapsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<DocumentCaseMap> ListByCriteria(DocumentCaseMapCriteria criteria)
        {
            var query = _dbContext.DocumentCaseMaps.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.CaseId != null)
                query = query.Where(x => x.CaseId == criteria.CaseId);
            if (criteria.DocumentId != null)
                query = query.Where(x => x.DocumentId == criteria.DocumentId);

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
            if (criteria.DocumentIds != null)
                query = query.Where(x => criteria.DocumentIds.Contains(x.DocumentId));

            return query;
        }

        public override int Create(DocumentCaseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Valid Case must be selected");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Valid Document must be selected");

            var existingEntity = ListByCriteria(new DocumentCaseMapCriteria
            {
                CaseId = dto.CaseId,
                DocumentId = dto.DocumentId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new DocumentCaseMap
            {
                CaseId = dto.CaseId,
                DocumentId = dto.DocumentId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.DocumentCaseMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(DocumentCaseMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Valid Case must be selected");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Valid Document must be selected");

            var existingEntity = ListByCriteria(new DocumentCaseMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.CaseId = dto.CaseId;
            existingEntity.DocumentId = dto.DocumentId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new DocumentCaseMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.DocumentCaseMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class DocumentCaseMapsServiceExtensions
    {
        public static IEnumerable<DocumentCaseMapDto> GetDtos(this IEnumerable<DocumentCaseMap> entities)
        {
            var entityDtos = entities
                .Select(x => new DocumentCaseMapDto
                {
                    Id = x.Id,
                    CaseId = x.CaseId,
                    DocumentId = x.DocumentId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
