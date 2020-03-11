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

    public interface IAutopsyService : IBaseEntityService<Autopsy, AutopsyCriteria, AutopsyDto> { }

    public class AutopsyService : BaseEntityService<Autopsy, AutopsyCriteria, AutopsyDto>, IAutopsyService
    {
        private readonly EiDbContext _dbContext;

        public AutopsyService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Autopsy> ListByCriteria(AutopsyCriteria criteria)
        {
            var query = _dbContext.Autopsies.AsQueryable()
                .Include(x => x.AutopsyDocumentCategory);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.AutopsyDocumentCategoryId != null)
                query = query.Where(x => x.AutopsyDocumentCategoryId == criteria.AutopsyDocumentCategoryId);

            if (criteria.Fine != null)
                query = query.Where(x => x.Fine == criteria.Fine);
            if (criteria.FromFine != null)
                query = query.Where(x => x.Fine <= criteria.FromFine);
            if (criteria.ToFine != null)
                query = query.Where(x => x.Fine >= criteria.ToFine);

            if (!string.IsNullOrEmpty(criteria.AutopsyElements))
                query = query.Where(x => x.AutopsyElements.Equals(criteria.AutopsyElements));
            if (!string.IsNullOrEmpty(criteria.WantedElements))
                query = query.Where(x => x.WantedElements.Equals(criteria.WantedElements));
            if (!string.IsNullOrEmpty(criteria.SubmittedElements))
                query = query.Where(x => x.SubmittedElements.Equals(criteria.SubmittedElements));
            if (!string.IsNullOrEmpty(criteria.ProtocolNumber))
                query = query.Where(x => x.ProtocolNumber.Equals(criteria.ProtocolNumber));

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);
            if (criteria.DateUpdated != null)
                query = query.Where(x => x.DateUpdated == criteria.DateUpdated);

            if (criteria.DateStarted != null)
                query = query.Where(x => x.DateStarted == criteria.DateStarted);
            if (criteria.DateEnded != null)
                query = query.Where(x => x.DateEnded == criteria.DateEnded);

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

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));
            if (criteria.AutopsyDocumentCategoryIds != null)
                query = query.Where(x => criteria.AutopsyDocumentCategoryIds.Contains(x.AutopsyDocumentCategoryId));

            return query;
        }

        public override int Create(AutopsyDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.DateStarted == null)
                throw new EiEntityException(@"Date Started cannot be empty");

            var existingEntity = ListByCriteria(new AutopsyCriteria
            {
                DateStarted = dto.DateStarted
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Autopsy
            {
                DateStarted = dto.DateStarted.Value,
                DateEnded = dto.DateEnded,
                Fine = dto.Fine,
                AutopsyDocumentCategoryId = dto.AutopsyDocumentCategoryId,
                AutopsyElements = dto.AutopsyElements,
                ProtocolNumber = dto.ProtocolNumber,
                SubmittedElements = dto.SubmittedElements,
                WantedElements = dto.WantedElements,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Autopsies.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(AutopsyDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.DateStarted == null)
                throw new EiEntityException(@"Date Started cannot be empty");

            var existingEntity = ListByCriteria(new AutopsyCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.DateStarted = dto.DateStarted.Value;
            existingEntity.DateEnded = dto.DateEnded;
            existingEntity.Fine = dto.Fine;
            existingEntity.AutopsyDocumentCategoryId = dto.AutopsyDocumentCategoryId;
            existingEntity.AutopsyElements = dto.AutopsyElements;
            existingEntity.ProtocolNumber = dto.ProtocolNumber;
            existingEntity.SubmittedElements = dto.SubmittedElements;
            existingEntity.WantedElements = dto.WantedElements;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new AutopsyCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Autopsies.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class AutopsyServiceExtensions
    {
        public static IEnumerable<AutopsyDto> GetDtos(this IEnumerable<Autopsy> entities)
        {
            var entityDtos = entities
                .Select(x => new AutopsyDto
                {
                    Id = x.Id,
                    DateStarted = x.DateStarted,
                    DateEnded = x.DateEnded,
                    Fine = x.Fine,
                    AutopsyDocumentCategory = x.AutopsyDocumentCategory?.Code ?? string.Empty,
                    AutopsyDocumentCategoryId = x.AutopsyDocumentCategoryId,
                    AutopsyElements = x.AutopsyElements,
                    ProtocolNumber = x.ProtocolNumber,
                    SubmittedElements = x.SubmittedElements,
                    WantedElements = x.WantedElements,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
