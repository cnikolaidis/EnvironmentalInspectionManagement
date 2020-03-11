namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region
    using System.Collections.Generic;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using Models.Dtos;
    using System.Linq;
    using System;
    using Core;
    #endregion

    public interface IInspectorsService : IBaseEntityService<Inspector, InspectorCriteria, InspectorDto> { }

    public class InspectorsService : BaseEntityService<Inspector, InspectorCriteria, InspectorDto>, IInspectorsService
    {
        private readonly EiDbContext _dbContext;

        public InspectorsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Inspector> ListByCriteria(InspectorCriteria criteria)
        {
            var query = _dbContext.Inspectors.AsQueryable();

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.FirstName))
                query = query.Where(x => x.FirstName.Equals(criteria.FirstName));
            if (!string.IsNullOrEmpty(criteria.LastName))
                query = query.Where(x => x.LastName.Equals(criteria.LastName));
            if (!string.IsNullOrEmpty(criteria.Specialty))
                query = query.Where(x => x.Specialty.Equals(criteria.Specialty));

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

        public override int Create(InspectorDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.FirstName))
                throw new EiEntityException(@"First Name must not be empty");
            if (string.IsNullOrEmpty(dto.LastName))
                throw new EiEntityException(@"Last Name must not be empty");

            var existingEntity = ListByCriteria(new InspectorCriteria
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Inspector
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialty = dto.Specialty,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Inspectors.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(InspectorDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.FirstName))
                throw new EiEntityException(@"First Name must not be empty");
            if (string.IsNullOrEmpty(dto.LastName))
                throw new EiEntityException(@"Last Name must not be empty");

            var existingEntity = ListByCriteria(new InspectorCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.FirstName = dto.FirstName;
            existingEntity.LastName = dto.LastName;
            existingEntity.Specialty = dto.Specialty;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new InspectorCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Inspectors.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class InspectorsServiceExtensions
    {
        public static IEnumerable<InspectorDto> GetDtos(this IEnumerable<Inspector> entities)
        {
            var entityDtos = entities
                .Select(x => new InspectorDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Specialty = x.Specialty,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
