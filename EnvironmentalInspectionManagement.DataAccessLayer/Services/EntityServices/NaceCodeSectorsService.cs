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

    public interface INaceCodeSectorsService : IBaseEntityService<NaceCodeSector, NaceCodeSectorCriteria, NaceCodeSectorDto> { }

    public class NaceCodeSectorsService : BaseEntityService<NaceCodeSector, NaceCodeSectorCriteria, NaceCodeSectorDto>, INaceCodeSectorsService
    {
        private readonly EiDbContext _dbContext;

        public NaceCodeSectorsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<NaceCodeSector> ListByCriteria(NaceCodeSectorCriteria criteria)
        {
            var query = _dbContext.NaceCodeSectors.AsQueryable()
                .Include(x => x.NaceCodes);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            if (!string.IsNullOrEmpty(criteria.Sector))
                query = query.Where(x => x.Sector.Equals(criteria.Sector));

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

        public override int Create(NaceCodeSectorDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Sector))
                throw new EiEntityException(@"Sector cannot be empty");

            var existingEntity = ListByCriteria(new NaceCodeSectorCriteria
            {
                Name = dto.Name,
                Sector = dto.Sector
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new NaceCodeSector
            {
                Name = dto.Name,
                Sector = dto.Sector,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.NaceCodeSectors.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(NaceCodeSectorDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (string.IsNullOrEmpty(dto.Sector))
                throw new EiEntityException(@"Sector cannot be empty");

            var existingEntity = ListByCriteria(new NaceCodeSectorCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.Sector = dto.Sector;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new NaceCodeSectorCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.NaceCodeSectors.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class NaceCodeSectorsServiceExtensions
    {
        public static IEnumerable<NaceCodeSectorDto> GetDtos(this IEnumerable<NaceCodeSector> entities)
        {
            var entityDtos = entities
                .Select(x => new NaceCodeSectorDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sector = x.Sector,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    NaceCodes = x.NaceCodes.GetDtos()
                });

            return entityDtos;
        }
    }
}
