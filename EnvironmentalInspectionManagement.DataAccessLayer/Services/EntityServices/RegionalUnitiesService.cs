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

    public interface IRegionalUnitiesService : IBaseEntityService<RegionalUnity, RegionalUnityCriteria, RegionalUnityDto> { }

    public class RegionalUnitiesService : BaseEntityService<RegionalUnity, RegionalUnityCriteria, RegionalUnityDto>, IRegionalUnitiesService
    {
        private readonly EiDbContext _dbContext;

        public RegionalUnitiesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<RegionalUnity> ListByCriteria(RegionalUnityCriteria criteria)
        {
            var query = _dbContext.RegionalUnities.AsQueryable()
                .Include(x => x.Region);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

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
            if (criteria.LibraryNumbers != null)
                query = query.Where(x => criteria.LibraryNumbers.Contains(x.LibraryNumber));

            return query;
        }

        public override int Create(RegionalUnityDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");
            if (dto.RegionId < 0)
                throw new EiEntityException(@"Valid Region must be selected");

            var existingEntity = ListByCriteria(new RegionalUnityCriteria
            {
                Name = dto.Name,
                RegionId = dto.RegionId,
                LibraryNumber = dto.LibraryNumber
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new RegionalUnity
            {
                Name = dto.Name,
                RegionId = dto.RegionId,
                LibraryNumber = dto.LibraryNumber,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.RegionalUnities.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(RegionalUnityDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");
            if (dto.RegionId < 0)
                throw new EiEntityException(@"Valid Region must be selected");

            var existingEntity = ListByCriteria(new RegionalUnityCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.RegionId = dto.RegionId;
            existingEntity.LibraryNumber = dto.LibraryNumber;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new RegionalUnityCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.RegionalUnities.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class RegionalUnitiesServiceExtensions
    {
        public static IEnumerable<RegionalUnityDto> GetDtos(this IEnumerable<RegionalUnity> entities)
        {
            var entityDtos = entities
                .Select(x => new RegionalUnityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    RegionId = x.RegionId,
                    LibraryNumber = x.LibraryNumber,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Region = x.Region?.Name ?? string.Empty,
                    RegionLibraryNumber = x.Region?.LibraryNumber ?? string.Empty
                });

            return entityDtos;
        }
    }
}
