namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using System.Linq;
    using Models.Dtos;
    using System;
    using Core;
    #endregion

    public interface INaturaRegionsService : IBaseEntityService<NaturaRegion, NaturaRegionCriteria, NaturaRegionDto> { }

    public class NaturaRegionsService : BaseEntityService<NaturaRegion, NaturaRegionCriteria, NaturaRegionDto>, INaturaRegionsService
    {
        private readonly EiDbContext _dbContext;

        public NaturaRegionsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<NaturaRegion> ListByCriteria(NaturaRegionCriteria criteria)
        {
            var query = _dbContext.NaturaRegions.AsQueryable();

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

        public override int Create(NaturaRegionDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new NaturaRegionCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new NaturaRegion
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.NaturaRegions.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(NaturaRegionDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new NaturaRegionCriteria { Id = dto.Id }).FirstOrDefault();

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

            var existingEntity = ListByCriteria(new NaturaRegionCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.NaturaRegions.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class NaturaRegionsServiceExtensions
    {
        public static IEnumerable<NaturaRegionDto> GetDtos(this IEnumerable<NaturaRegion> entities)
        {
            var entityDtos = entities
                .Select(x => new NaturaRegionDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });

            return entityDtos;
        }
    }
}
