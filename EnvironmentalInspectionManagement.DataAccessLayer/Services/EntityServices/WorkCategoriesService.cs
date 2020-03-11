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

    public interface IWorkCategoriesService : IBaseEntityService<WorkCategory, WorkCategoryCriteria, WorkCategoryDto> { }

    public class WorkCategoriesService : BaseEntityService<WorkCategory, WorkCategoryCriteria, WorkCategoryDto>, IWorkCategoriesService
    {
        private readonly EiDbContext _dbContext;

        public WorkCategoriesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<WorkCategory> ListByCriteria(WorkCategoryCriteria criteria)
        {
            var query = _dbContext.WorkCategories.AsQueryable()
                .Include(x => x.WorkSubcategories);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.LibraryNumber))
                query = query.Where(x => x.Name.Equals(criteria.LibraryNumber));

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
            if (criteria.LibraryNumbers != null)
                query = query.Where(x => criteria.LibraryNumbers.Contains(x.LibraryNumber));

            return query;
        }

        public override int Create(WorkCategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");

            var existingEntity = ListByCriteria(new WorkCategoryCriteria
            {
                Name = dto.Name,
                LibraryNumber = dto.LibraryNumber
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new WorkCategory
            {
                Name = dto.Name,
                LibraryNumber = dto.LibraryNumber,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.WorkCategories.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(WorkCategoryDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");

            var existingEntity = ListByCriteria(new WorkCategoryCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.LibraryNumber = dto.LibraryNumber;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new WorkCategoryCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.WorkCategories.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class WorkCategoriesServiceExtensions
    {
        public static IEnumerable<WorkCategoryDto> GetDtos(this IEnumerable<WorkCategory> entities)
        {
            var entityDtos = entities
                .Select(x => new WorkCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LibraryNumber = x.LibraryNumber,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    WorkSubcategories = x.WorkSubcategories.GetDtos()
                });

            return entityDtos;
        }
    }
}
