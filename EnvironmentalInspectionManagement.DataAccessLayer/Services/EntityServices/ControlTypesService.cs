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

    public interface IControlTypesService : IBaseEntityService<ControlType, ControlTypeCriteria, ControlTypeDto> { }

    public class ControlTypesService : BaseEntityService<ControlType, ControlTypeCriteria, ControlTypeDto>, IControlTypesService
    {
        private readonly EiDbContext _dbContext;

        public ControlTypesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<ControlType> ListByCriteria(ControlTypeCriteria criteria)
        {
            var query = _dbContext.ControlTypes.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));

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

        public override int Create(ControlTypeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new ControlTypeCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new ControlType
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.ControlTypes.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(ControlTypeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new ControlTypeCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new ControlTypeCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.ControlTypes.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class ControlTypesServiceExtensions
    {
        public static IEnumerable<ControlTypeDto> GetDtos(this IEnumerable<ControlType> entities)
        {
            var dtosList = entities
                .Select(x => new ControlTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return dtosList;
        }
    }
}
