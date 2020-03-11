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

    public interface IWorkSubcategoriesService : IBaseEntityService<WorkSubcategory, WorkSubcategoryCriteria, WorkSubcategoryDto> { }

    public class WorkSubcategoriesService : BaseEntityService<WorkSubcategory, WorkSubcategoryCriteria, WorkSubcategoryDto>, IWorkSubcategoriesService
    {
        private readonly EiDbContext _dbContext;

        public WorkSubcategoriesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<WorkSubcategory> ListByCriteria(WorkSubcategoryCriteria criteria)
        {
            var query = _dbContext.WorkSubcategories.AsQueryable()
                .Include(x => x.WorkCategory);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);
            if (criteria.WorkCategoryId != null)
                query = query.Where(x => x.WorkCategoryId == criteria.WorkCategoryId);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.LibraryNumber))
                query = query.Where(x => x.LibraryNumber.Equals(criteria.LibraryNumber));

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
            if (criteria.WorkCategoryIds != null)
                query = query.Where(x => criteria.WorkCategoryIds.Contains(x.WorkCategoryId));
            if (criteria.LibraryNumbers != null)
                query = query.Where(x => criteria.LibraryNumbers.Contains(x.LibraryNumber));

            return query;
        }

        public override int Create(WorkSubcategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");

            var existingEntity = ListByCriteria(new WorkSubcategoryCriteria
            {
                Name = dto.Name,
                LibraryNumber = dto.LibraryNumber
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new WorkSubcategory
            {
                Name = dto.Name,
                WorkCategoryId = dto.WorkCategoryId,
                LibraryNumber = dto.LibraryNumber,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.WorkSubcategories.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(WorkSubcategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");

            var existingEntity = ListByCriteria(new WorkSubcategoryCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.LibraryNumber = dto.LibraryNumber;
            existingEntity.WorkCategoryId = dto.WorkCategoryId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new WorkSubcategoryCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.WorkSubcategories.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class WorkSubcategoriesServiceExtensions
    {
        public static IEnumerable<WorkSubcategoryDto> GetDtos(this IEnumerable<WorkSubcategory> entities)
        {
            var entityDtos = entities
                .Select(x => new WorkSubcategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    WorkCategoryId = x.WorkCategoryId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    LibraryNumber = x.LibraryNumber,
                    WorkCategory = x.WorkCategory?.Name ?? string.Empty,
                    WorkCategoryLibraryNumber = x.WorkCategory?.LibraryNumber ?? string.Empty
                });

            return entityDtos;
        }
    }
}
