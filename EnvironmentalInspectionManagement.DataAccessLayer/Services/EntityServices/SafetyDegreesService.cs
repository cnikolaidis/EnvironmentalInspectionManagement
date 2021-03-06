﻿namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models.Criterias;
    using Models.Exceptions;
    using Models.Entities;
    using Models.Dtos;
    using System.Linq;
    using System;
    using Core;
    #endregion

    public interface ISafetyDegreesService : IBaseEntityService<SafetyDegree, SafetyDegreeCriteria, SafetyDegreeDto> { }

    public class SafetyDegreesService : BaseEntityService<SafetyDegree, SafetyDegreeCriteria, SafetyDegreeDto>, ISafetyDegreesService
    {
        private readonly EiDbContext _dbContext;

        public SafetyDegreesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<SafetyDegree> ListByCriteria(SafetyDegreeCriteria criteria)
        {
            var query = _dbContext.SafetyDegrees.AsQueryable()
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

        public override int Create(SafetyDegreeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new SafetyDegreeCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new SafetyDegree
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.SafetyDegrees.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(SafetyDegreeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new SafetyDegreeCriteria { Id = dto.Id }).FirstOrDefault();

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

            var existingEntity = ListByCriteria(new SafetyDegreeCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.SafetyDegrees.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class SafetyDegreesServiceExtensions
    {
        public static IEnumerable<SafetyDegreeDto> GetDtos(this IEnumerable<SafetyDegree> entities)
        {
            var dtos = entities
                .Select(x => new SafetyDegreeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Indictments = x.Indictments.GetDtos()
                });

            return dtos;
        }
    }
}
