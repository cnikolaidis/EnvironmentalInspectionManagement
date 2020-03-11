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

    public interface IOrganizationsService : IBaseEntityService<Organization, OrganizationCriteria, OrganizationDto> { }

    public class OrganizationsService : BaseEntityService<Organization, OrganizationCriteria, OrganizationDto>, IOrganizationsService
    {
        private readonly EiDbContext _dbContext;

        public OrganizationsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Organization> ListByCriteria(OrganizationCriteria criteria)
        {
            var query = _dbContext.Organizations.AsQueryable()
                .Include(x => x.Activities)
                .Include(x => x.LegalEntityCategory)
                .Include(x => x.RegionalUnity)
                .Include(x => x.Region)
                .Include(x => x.TaxOffice);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);


            if (criteria.RegionId != null)
                query = query.Where(x => x.RegionId == criteria.RegionId);
            if (criteria.TaxOfficeId != null)
                query = query.Where(x => x.TaxOfficeId == criteria.TaxOfficeId);
            if (criteria.RegionalUnityId != null)
                query = query.Where(x => x.RegionalUnityId == criteria.RegionalUnityId);
            if (criteria.LegalEntityCategoryId != null)
                query = query.Where(x => x.LegalEntityCategoryId == criteria.LegalEntityCategoryId);

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

            if (criteria.Ids != null && criteria.Ids.Any())
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            if (criteria.RegionIds != null)
                query = query.Where(x => criteria.RegionIds.Contains(x.RegionId));
            if (criteria.TaxOfficeIds != null)
                query = query.Where(x => criteria.TaxOfficeIds.Contains(x.TaxOfficeId));
            if (criteria.RegionalUnityIds != null)
                query = query.Where(x => criteria.RegionalUnityIds.Contains(x.RegionalUnityId));
            if (criteria.LegalEntityCategoryIds != null)
                query = query.Where(x => criteria.LegalEntityCategoryIds.Contains(x.LegalEntityCategoryId));

            return query;
        }

        public override int Create(OrganizationDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (dto.RegionId < 1)
                throw new EiEntityException(@"Organization must have a valid Region");
            if (dto.TaxOfficeId < 1)
                throw new EiEntityException(@"Organization must have a valid Tax Office");
            if (dto.RegionalUnityId < 1)
                throw new EiEntityException(@"Organization must have a valid Municipality");
            if (dto.LegalEntityCategoryId < 1)
                throw new EiEntityException(@"Organization must have a valid Legal Entity Category");
            if (string.IsNullOrEmpty(dto.TaxationNo))
                throw new EiEntityException(@"Activity must have a Taxation Number");

            var existingEntity = ListByCriteria(new OrganizationCriteria
            {
                Name = dto.Name,
                TaxationNo = dto.TaxationNo,
                LegalEntityCategoryId = dto.LegalEntityCategoryId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Organization
            {
                Name = dto.Name,
                Address = dto.Address,
                TaxationNo = dto.TaxationNo,
                PhoneNo1 = dto.PhoneNo1,
                PhoneNo2 = dto.PhoneNo2,
                RegionId = dto.RegionId,
                TaxOfficeId = dto.TaxOfficeId,
                RegionalUnityId = dto.RegionalUnityId,
                LegalEntityCategoryId = dto.LegalEntityCategoryId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Organizations.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(OrganizationDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (dto.RegionId < 1)
                throw new EiEntityException(@"Organization must have a valid Region");
            if (dto.TaxOfficeId < 1)
                throw new EiEntityException(@"Organization must have a valid Tax Office");
            if (dto.RegionalUnityId < 1)
                throw new EiEntityException(@"Organization must have a valid Municipality");
            if (dto.LegalEntityCategoryId < 1)
                throw new EiEntityException(@"Organization must have a valid Legal Entity Category");
            if (string.IsNullOrEmpty(dto.TaxationNo))
                throw new EiEntityException(@"Activity must have a Taxation Number");

            var existingEntity = ListByCriteria(new OrganizationCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.Address = dto.Address;
            existingEntity.TaxationNo = dto.TaxationNo;
            existingEntity.PhoneNo1 = dto.PhoneNo1;
            existingEntity.PhoneNo2 = dto.PhoneNo2;
            existingEntity.RegionId = dto.RegionId;
            existingEntity.TaxOfficeId = dto.TaxOfficeId;
            existingEntity.LegalEntityCategoryId = dto.LegalEntityCategoryId;
            existingEntity.RegionalUnityId = dto.RegionalUnityId;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new OrganizationCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Organizations.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class OrganizationsServiceExtensions
    {
        public static IEnumerable<OrganizationDto> GetDtos(this IEnumerable<Organization> entities)
        {
            var entityDtos = entities
                .Select(x => new OrganizationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    TaxationNo = x.TaxationNo,
                    PhoneNo1 = x.PhoneNo1,
                    PhoneNo2 = x.PhoneNo2,
                    TaxOfficeId = x.TaxOfficeId,
                    RegionId = x.RegionId,
                    RegionalUnityId = x.RegionalUnityId,
                    LegalEntityCategoryId = x.LegalEntityCategoryId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Region = x.Region?.Name ?? string.Empty,
                    TaxOffice = x.TaxOffice?.Name ?? string.Empty,
                    RegionalUnity = x.RegionalUnity?.Name ?? string.Empty,
                    LegalEntityCategory = x.LegalEntityCategory?.Name ?? string.Empty,
                    Activities = x.Activities.GetDtos()
                });

            return entityDtos;
        }
    }
}
