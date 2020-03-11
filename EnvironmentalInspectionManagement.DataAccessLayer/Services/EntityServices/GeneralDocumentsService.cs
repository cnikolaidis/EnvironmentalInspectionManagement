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

    public interface IGeneralDocumentsService : IBaseEntityService<GeneralDocument, GeneralDocumentCriteria, GeneralDocumentDto> { }

    public class GeneralDocumentsService : BaseEntityService<GeneralDocument, GeneralDocumentCriteria, GeneralDocumentDto>, IGeneralDocumentsService
    {
        private readonly EiDbContext _dbContext;

        public GeneralDocumentsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<GeneralDocument> ListByCriteria(GeneralDocumentCriteria criteria)
        {
            var query = _dbContext.GeneralDocuments.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.Description))
                query = query.Where(x => x.Name.Equals(criteria.Description));
            if (!string.IsNullOrEmpty(criteria.ProtocolNo))
                query = query.Where(x => x.ProtocolNo.Equals(criteria.ProtocolNo));

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

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            return query;
        }

        public override int Create(GeneralDocumentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Description))
                throw new EiEntityException(@"Description cannot be empty");

            var existingEntity = ListByCriteria(new GeneralDocumentCriteria
            {
                Name = dto.Name,
                Description = dto.Description,
                ProtocolNo = dto.ProtocolNo
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new GeneralDocument
            {
                Name = dto.Name,
                Description = dto.Description,
                ProtocolNo = dto.ProtocolNo,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.GeneralDocuments.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(GeneralDocumentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Description))
                throw new EiEntityException(@"Description cannot be empty");

            var existingEntity = ListByCriteria(new GeneralDocumentCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.ProtocolNo = dto.ProtocolNo;
            existingEntity.Description = dto.Description;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new GeneralDocumentCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.GeneralDocuments.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class GeneralDocumentsServiceExtensions
    {
        public static IEnumerable<GeneralDocumentDto> GetDtos(this IEnumerable<GeneralDocument> entities)
        {
            var dtosList = entities
                .Select(x => new GeneralDocumentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ProtocolNo = x.ProtocolNo
                });

            return dtosList;
        }
    }
}
