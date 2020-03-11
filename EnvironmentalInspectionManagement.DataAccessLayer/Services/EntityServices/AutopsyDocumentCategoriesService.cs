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

    public interface IAutopsyDocumentCategoriesService : IBaseEntityService<AutopsyDocumentCategory, AutopsyDocumentCategoryCriteria, AutopsyDocumentCategoryDto> { }

    public class AutopsyDocumentCategoriesService : BaseEntityService<AutopsyDocumentCategory, AutopsyDocumentCategoryCriteria, AutopsyDocumentCategoryDto>, IAutopsyDocumentCategoriesService
    {
        private readonly EiDbContext _dbContext;

        public AutopsyDocumentCategoriesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<AutopsyDocumentCategory> ListByCriteria(AutopsyDocumentCategoryCriteria criteria)
        {
            var query = _dbContext.AutopsyDocumentCategories.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.Code))
                query = query.Where(x => x.Code.Equals(criteria.Code));
            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.Version))
                query = query.Where(x => x.Version.Equals(criteria.Version));

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

        public override int Create(AutopsyDocumentCategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Code))
                throw new EiEntityException(@"Code cannot be empty");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new AutopsyDocumentCategoryCriteria
            {
                Code = dto.Code,
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new AutopsyDocumentCategory
            {
                Code = dto.Code,
                Name = dto.Name,
                Version = dto.Version,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.AutopsyDocumentCategories.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(AutopsyDocumentCategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Code))
                throw new EiEntityException(@"Code cannot be empty");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new AutopsyDocumentCategoryCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Code = dto.Code;
            existingEntity.Name = dto.Name;
            existingEntity.Version = dto.Version;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new AutopsyDocumentCategoryCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.AutopsyDocumentCategories.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class AutopsyDocumentCategoriesServiceExtensions
    {
        public static IEnumerable<AutopsyDocumentCategoryDto> GetDtos(this IEnumerable<AutopsyDocumentCategory> entities)
        {
            var entityDtos = entities
                .Select(x => new AutopsyDocumentCategoryDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Version = x.Version,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
