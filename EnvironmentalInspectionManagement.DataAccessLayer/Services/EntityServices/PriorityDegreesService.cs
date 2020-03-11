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

    public interface IPriorityDegreesService : IBaseEntityService<PriorityDegree, PriorityDegreeCriteria, PriorityDegreeDto> { }

    public class PriorityDegreesService : BaseEntityService<PriorityDegree, PriorityDegreeCriteria, PriorityDegreeDto>, IPriorityDegreesService
    {
        private readonly EiDbContext _dbContext;

        public PriorityDegreesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<PriorityDegree> ListByCriteria(PriorityDegreeCriteria criteria)
        {
            var query = _dbContext.PriorityDegrees.AsQueryable()
                .Include(x => x.Indictments);

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

        public override int Create(PriorityDegreeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new PriorityDegreeCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new PriorityDegree
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.PriorityDegrees.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(PriorityDegreeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new PriorityDegreeCriteria { Id = dto.Id }).FirstOrDefault();

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

            var existingEntity = ListByCriteria(new PriorityDegreeCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.PriorityDegrees.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class PriorityDegreesServiceExtensions
    {
        public static IEnumerable<PriorityDegreeDto> GetDtos(this IEnumerable<PriorityDegree> entities)
        {
            var dtos = entities
                .Select(x => new PriorityDegreeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Indictments = x.Indictments.GetDtos()
                });

            return dtos;
        }
    }
}
