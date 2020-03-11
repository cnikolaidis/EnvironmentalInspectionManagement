namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models.Exceptions;
    using Models.Criterias;
    using Models.Entities;
    using System.Linq;
    using Models.Dtos;
    using System;
    using Core;
    #endregion

    public interface ITaxOfficesService : IBaseEntityService<TaxOffice, TaxOfficeCriteria, TaxOfficeDto> { }

    public class TaxOfficesService : BaseEntityService<TaxOffice, TaxOfficeCriteria, TaxOfficeDto>, ITaxOfficesService
    {
        private readonly EiDbContext _dbContext;

        public TaxOfficesService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<TaxOffice> ListByCriteria(TaxOfficeCriteria criteria)
        {
            var query = _dbContext.TaxOffices.AsQueryable()
                .Include(x => x.City);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.PostalCode != null)
                query = query.Where(x => x.PostalCode == criteria.PostalCode);
            if (criteria.CityId != null)
                query = query.Where(x => x.CityId == criteria.CityId);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));
            
            if (!string.IsNullOrEmpty(criteria.Description))
                query = query.Where(x => x.Description.Equals(criteria.Description));
            if (!string.IsNullOrEmpty(criteria.Address))
                query = query.Where(x => x.Address.Equals(criteria.Address));
            if (!string.IsNullOrEmpty(criteria.PhoneNo))
                query = query.Where(x => x.PhoneNo.Equals(criteria.PhoneNo));

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
            if (criteria.CityIds != null)
                query = query.Where(x => criteria.CityIds.Contains(x.CityId));

            return query;
        }

        public override int Create(TaxOfficeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new TaxOfficeCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new TaxOffice
            {
                Name = dto.Name,
                PostalCode = dto.PostalCode,
                Description = dto.Description,
                Address = dto.Address,
                CityId = dto.CityId,
                PhoneNo = dto.PhoneNo,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.TaxOffices.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(TaxOfficeDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new TaxOfficeCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.PostalCode = dto.PostalCode;
            existingEntity.Description = dto.Description;
            existingEntity.Address = dto.Address;
            existingEntity.CityId = dto.CityId;
            existingEntity.PhoneNo = dto.PhoneNo;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new TaxOfficeCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.TaxOffices.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class TaxOfficesServiceExtensions
    {
        public static IEnumerable<TaxOfficeDto> GetDtos(this IEnumerable<TaxOffice> entities)
        {
            var entityDtos = entities
                .Select(x => new TaxOfficeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PostalCode = x.PostalCode,
                    Description = x.Description,
                    Address = x.Address,
                    CityId = x.CityId,
                    PhoneNo = x.PhoneNo,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    City = x.City?.Name ?? string.Empty
                });

            return entityDtos;
        }
    }
}
