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

    public interface IDocumentsService : IBaseEntityService<Document, DocumentCriteria, DocumentDto> { }

    public class DocumentsService : BaseEntityService<Document, DocumentCriteria, DocumentDto>, IDocumentsService
    {
        private readonly EiDbContext _dbContext;

        public DocumentsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Document> ListByCriteria(DocumentCriteria criteria)
        {
            var query = _dbContext.Documents.AsQueryable()
                .Include(x => x.DocumentType);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.CaseId != null)
                query = query.Where(x => x.CaseId == criteria.CaseId);
            if (criteria.DocumentId != null)
                query = query.Where(x => x.DocumentId == criteria.DocumentId);
            if (criteria.DocumentTypeId != null)
                query = query.Where(x => x.DocumentTypeId == criteria.DocumentTypeId);
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

            if (criteria.CaseIds != null)
                query = query.Where(x => criteria.CaseIds.Contains(x.CaseId));
            if (criteria.DocumentIds != null)
                query = query.Where(x => criteria.DocumentIds.Contains(x.DocumentId));
            if (criteria.DocumentTypeIds != null)
                query = query.Where(x => criteria.DocumentTypeIds.Contains(x.DocumentTypeId));
            if (criteria.ActivityIds != null)
                query = query.Where(x => criteria.ActivityIds.Contains(x.ActivityId));

            return query;
        }

        public override int Create(DocumentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Not valid Document selected");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Not valid Case selected");
            if (dto.DocumentTypeId < 1)
                throw new EiEntityException(@"Not valid Document Type selected");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Not valid Activity selected");

            var existingEntity = ListByCriteria(new DocumentCriteria
            {
                DocumentId = dto.DocumentId,
                CaseId = dto.CaseId,
                ActivityId = dto.ActivityId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Document
            {
                DocumentId = dto.DocumentId,
                CaseId = dto.CaseId,
                ActivityId = dto.ActivityId,
                DocumentTypeId = dto.DocumentTypeId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Documents.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(DocumentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.DocumentId < 1)
                throw new EiEntityException(@"Not valid Document selected");
            if (dto.CaseId < 1)
                throw new EiEntityException(@"Not valid Case selected");
            if (dto.DocumentTypeId < 1)
                throw new EiEntityException(@"Not valid Document Type selected");
            if (dto.ActivityId < 1)
                throw new EiEntityException(@"Not valid Activity selected");

            var existingEntity = ListByCriteria(new DocumentCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.DocumentId = dto.DocumentId;
            existingEntity.CaseId = dto.CaseId;
            existingEntity.ActivityId = dto.ActivityId;
            existingEntity.DocumentTypeId = dto.DocumentTypeId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new DocumentCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Documents.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class DocumentsServiceExtensions
    {
        public static IEnumerable<DocumentDto> GetDtos(this IEnumerable<Document> entities)
        {
            var entityDtos = entities
                .Select(x => new DocumentDto
                {
                    Id = x.Id,
                    DocumentId = x.DocumentId,
                    CaseId = x.CaseId,
                    ActivityId = x.ActivityId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentType = x.DocumentType?.Name ?? @"Άγνωστο",
                    LibraryIdentity = string.Empty,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
