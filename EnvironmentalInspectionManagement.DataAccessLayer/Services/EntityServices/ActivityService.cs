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

    public interface IActivityService : IBaseEntityService<Activity, ActivityCriteria, ActivityDto> { }

    public class ActivityService : BaseEntityService<Activity, ActivityCriteria, ActivityDto>, IActivityService
    {
        private readonly EiDbContext _dbContext;

        public ActivityService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Activity> ListByCriteria(ActivityCriteria criteria)
        {
            var query = _dbContext.Activities.AsQueryable()
                .Include(x => x.Organization)
                .Include(x => x.RegionalUnity)
                .Include(x => x.TaxOffice)
                .Include(x => x.WorkCategory)
                .Include(x => x.WorkSubcategory)
                .Include(x => x.NaceCodeSector)
                .Include(x => x.NaceCode);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.OrganizationId != null)
                query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
            if (criteria.TaxOfficeId != null)
                query = query.Where(x => x.TaxOfficeId == criteria.TaxOfficeId);
            if (criteria.RegionalUnityId != null)
                query = query.Where(x => x.RegionalUnityId == criteria.RegionalUnityId);
            if (criteria.WorkCategoryId != null)
                query = query.Where(x => x.WorkCategoryId == criteria.WorkCategoryId);
            if (criteria.WorkSubcategoryId != null)
                query = query.Where(x => x.WorkSubcategoryId == criteria.WorkSubcategoryId);
            if (criteria.NaceCodeSectorId != null)
                query = query.Where(x => x.NaceCodeSectorId == criteria.NaceCodeSectorId);
            if (criteria.NaceCodeId != null)
                query = query.Where(x => x.NaceCodeId == criteria.NaceCodeId);

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(x => x.Name.Equals(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.Description))
                query = query.Where(x => x.Description.Equals(criteria.Description));
            if (!string.IsNullOrEmpty(criteria.TaxNumber))
                query = query.Where(x => x.TaxNumber.Equals(criteria.TaxNumber));
            if (!string.IsNullOrEmpty(criteria.Address))
                query = query.Where(x => x.Address.Equals(criteria.Address));
            if (!string.IsNullOrEmpty(criteria.PhoneNo1))
                query = query.Where(x => x.PhoneNo1.Equals(criteria.PhoneNo1));
            if (!string.IsNullOrEmpty(criteria.PhoneNo2))
                query = query.Where(x => x.PhoneNo2.Equals(criteria.PhoneNo2));

            if (!string.IsNullOrEmpty(criteria.FaxNo))
                query = query.Where(x => x.FaxNo.Equals(criteria.FaxNo));
            if (!string.IsNullOrEmpty(criteria.MailAddress))
                query = query.Where(x => x.MailAddress.Equals(criteria.MailAddress));
            if (!string.IsNullOrEmpty(criteria.PlaceName))
                query = query.Where(x => x.PlaceName.Equals(criteria.PlaceName));
            if (!string.IsNullOrEmpty(criteria.ManagerName))
                query = query.Where(x => x.ManagerName.Equals(criteria.ManagerName));
            if (!string.IsNullOrEmpty(criteria.OtaName))
                query = query.Where(x => x.OtaName.Equals(criteria.OtaName));

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

            if (criteria.CoordX != null)
                query = query.Where(x => x.CoordX == criteria.CoordX);
            if (criteria.CoordY != null)
                query = query.Where(x => x.CoordY == criteria.CoordY);

            if (criteria.Ids != null && criteria.Ids.Any())
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            if (criteria.OrganizationIds != null)
                query = query.Where(x => criteria.OrganizationIds.Contains(x.OrganizationId));
            if (criteria.TaxOfficeIds != null)
                query = query.Where(x => criteria.TaxOfficeIds.Contains(x.TaxOfficeId));
            if (criteria.RegionalUnityIds != null)
                query = query.Where(x => criteria.RegionalUnityIds.Contains(x.RegionalUnityId));
            if (criteria.WorkCategoryIds != null)
                query = query.Where(x => criteria.WorkCategoryIds.Contains(x.WorkCategoryId));
            if (criteria.WorkSubcategoryIds != null)
                query = query.Where(x => criteria.WorkSubcategoryIds.Contains(x.WorkSubcategoryId));
            if (criteria.NaceCodeSectorIds != null)
                query = query.Where(x => criteria.NaceCodeSectorIds.Contains(x.NaceCodeSectorId));
            if (criteria.NaceCodeIds != null)
                query = query.Where(x => criteria.NaceCodeIds.Contains(x.NaceCodeId));

            return query;
        }

        public override int Create(ActivityDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (dto.OrganizationId < 1)
                throw new EiEntityException(@"Activity must belong to a valid Organization");
            if (dto.TaxOfficeId < 1)
                throw new EiEntityException(@"Activity must have a valid Tax Office");
            if (dto.RegionalUnityId < 1)
                throw new EiEntityException(@"Activity must have a valid Municipality");
            if (dto.WorkCategoryId < 1)
                throw new EiEntityException(@"Activity must have a valid Work Category");
            if (dto.WorkSubcategoryId < 1)
                throw new EiEntityException(@"Activity must have a valid Work Subcategory");
            if (dto.NaceCodeSectorId < 1)
                throw new EiEntityException(@"Activity must have a valid Nace Code Sector");
            if (dto.NaceCodeId < 1)
                throw new EiEntityException(@"Activity must have a valid Nace Code");
            if (string.IsNullOrEmpty(dto.TaxNumber))
                throw new EiEntityException(@"Activity must have a Taxation Number");

            var existingEntity = ListByCriteria(new ActivityCriteria
            {
                Name = dto.Name,
                OrganizationId = dto.OrganizationId
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Activity
            {
                Name = dto.Name,
                OrganizationId = dto.OrganizationId,
                TaxOfficeId = dto.TaxOfficeId,
                RegionalUnityId = dto.RegionalUnityId,
                WorkCategoryId = dto.WorkCategoryId,
                WorkSubcategoryId = dto.WorkSubcategoryId,
                NaceCodeSectorId = dto.NaceCodeSectorId,
                NaceCodeId = dto.NaceCodeId,
                Description = dto.Description,
                TaxNumber = dto.TaxNumber,
                Address = dto.Address,
                PhoneNo1 = dto.PhoneNo1,
                PhoneNo2 = dto.PhoneNo2,
                FaxNo = dto.FaxNo,
                MailAddress = dto.MailAddress,
                PlaceName = dto.PlaceName,
                ManagerName = dto.ManagerName,
                OtaName = dto.OtaName,
                CoordX = dto.CoordX,
                CoordY = dto.CoordY,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Activities.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(ActivityDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");
            if (dto.OrganizationId < 1)
                throw new EiEntityException(@"Activity must belong to a valid Organization");
            if (dto.TaxOfficeId < 1)
                throw new EiEntityException(@"Activity must have a valid Tax Office");
            if (dto.RegionalUnityId < 1)
                throw new EiEntityException(@"Activity must have a valid Municipality");
            if (dto.WorkCategoryId < 1)
                throw new EiEntityException(@"Activity must have a valid Work Category");
            if (dto.WorkSubcategoryId < 1)
                throw new EiEntityException(@"Activity must have a valid Work Subcategory");
            if (dto.NaceCodeSectorId < 1)
                throw new EiEntityException(@"Activity must have a valid Nace Code Sector");
            if (dto.NaceCodeId < 1)
                throw new EiEntityException(@"Activity must have a valid Nace Code");
            if (string.IsNullOrEmpty(dto.TaxNumber))
                throw new EiEntityException(@"Activity must have a Taxation Number");

            var existingEntity = ListByCriteria(new ActivityCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.Name = dto.Name;
            existingEntity.OrganizationId = dto.OrganizationId;
            existingEntity.TaxOfficeId = dto.TaxOfficeId;
            existingEntity.RegionalUnityId = dto.RegionalUnityId;
            existingEntity.WorkCategoryId = dto.WorkCategoryId;
            existingEntity.WorkSubcategoryId = dto.WorkSubcategoryId;
            existingEntity.NaceCodeSectorId = dto.NaceCodeSectorId;
            existingEntity.NaceCodeId = dto.NaceCodeId;
            existingEntity.Description = dto.Description;
            existingEntity.TaxNumber = dto.TaxNumber;
            existingEntity.Address = dto.Address;
            existingEntity.PhoneNo1 = dto.PhoneNo1;
            existingEntity.PhoneNo2 = dto.PhoneNo2;
            existingEntity.FaxNo = dto.FaxNo;
            existingEntity.MailAddress = dto.MailAddress;
            existingEntity.PlaceName = dto.PlaceName;
            existingEntity.ManagerName = dto.ManagerName;
            existingEntity.OtaName = dto.OtaName;
            existingEntity.CoordX = dto.CoordX;
            existingEntity.CoordY = dto.CoordY;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new ActivityCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Activities.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class ActivityServiceExtensions
    {
        public static IEnumerable<ActivityDto> GetDtos(this IEnumerable<Activity> entities)
        {
            var entityDtos = entities
                .Select(x => new ActivityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    OrganizationId = x.OrganizationId,
                    TaxOfficeId = x.TaxOfficeId,
                    RegionalUnityId = x.RegionalUnityId,
                    WorkCategoryId = x.WorkCategoryId,
                    WorkSubcategoryId = x.WorkSubcategoryId,
                    NaceCodeSectorId = x.NaceCodeSectorId,
                    NaceCodeId = x.NaceCodeId,
                    Description = x.Description,
                    TaxNumber = x.TaxNumber,
                    Address = x.Address,
                    PhoneNo1 = x.PhoneNo1,
                    PhoneNo2 = x.PhoneNo2,
                    ManagerName = x.ManagerName,
                    FaxNo = x.FaxNo,
                    PlaceName = x.PlaceName,
                    OtaName = x.OtaName,
                    MailAddress = x.MailAddress,
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    WorkCategory = x.WorkCategory?.Name ?? string.Empty,
                    RegionalUnity = x.RegionalUnity?.Name ?? string.Empty,
                    TaxOffice = x.TaxOffice?.Name ?? string.Empty,
                    Organization = x.Organization?.Name ?? string.Empty,
                    NaceCodeSector = $"{x.NaceCodeSector?.Sector ?? string.Empty} - {x.NaceCodeSector?.Name ?? string.Empty}",
                    WorkSubcategory = x.WorkSubcategory?.Name ?? string.Empty,
                    NaceCode = $"{x.NaceCode?.Class ?? string.Empty} - {x.NaceCode?.Name ?? string.Empty}",
                });

            return entityDtos;
        }
    }
}
