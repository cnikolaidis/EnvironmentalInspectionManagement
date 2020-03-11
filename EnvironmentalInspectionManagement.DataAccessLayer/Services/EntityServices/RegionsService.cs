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

    public interface IRegionsService : IBaseEntityService<Region, RegionCriteria, RegionDto> { }

    public class RegionsService : BaseEntityService<Region, RegionCriteria, RegionDto>, IRegionsService
    {
        private readonly EiDbContext _dbContext;

        public RegionsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Region> ListByCriteria(RegionCriteria criteria)
        {
            var query = _dbContext.Regions.AsQueryable()
                .Include(x => x.RegionalUnities);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.IsNorth != null)
                query = query.Where(x => x.IsNorth == criteria.IsNorth);
            if (criteria.IsSouth != null)
                query = query.Where(x => x.IsSouth == criteria.IsSouth);

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

        public override int Create(RegionDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");
            if (dto.IsNorth == dto.IsSouth)
                throw new EiEntityException(@"North / South region must be selected");

            var existingEntity = ListByCriteria(new RegionCriteria
            {
                Name = dto.Name,
                LibraryNumber = dto.LibraryNumber
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Region
            {
                Name = dto.Name,
                LibraryNumber = dto.LibraryNumber,
                IsNorth = dto.IsNorth,
                IsSouth = dto.IsSouth,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Regions.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(RegionDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.LibraryNumber))
                throw new EiEntityException(@"Library Number cannot be empty");
            if (dto.IsNorth == dto.IsSouth)
                throw new EiEntityException(@"North / South region must be selected");

            var existingEntity = ListByCriteria(new RegionCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.LibraryNumber = dto.LibraryNumber;
            existingEntity.IsNorth = dto.IsNorth;
            existingEntity.IsSouth = dto.IsSouth;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new RegionCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Regions.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class RegionsServiceExtensions
    {
        public static IEnumerable<RegionDto> GetDtos(this IEnumerable<Region> entities)
        {
            var entityDtos = entities
                .Select(x => new RegionDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LibraryNumber = x.LibraryNumber,
                    IsNorth = x.IsNorth,
                    IsSouth = x.IsSouth,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
