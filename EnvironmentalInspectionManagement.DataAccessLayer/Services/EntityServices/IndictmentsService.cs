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

    public interface IIndictmentsService : IBaseEntityService<Indictment, IndictmentCriteria, IndictmentDto> { }

    public class IndictmentsService : BaseEntityService<Indictment, IndictmentCriteria, IndictmentDto>, IIndictmentsService
    {
        private readonly EiDbContext _dbContext;

        public IndictmentsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Indictment> ListByCriteria(IndictmentCriteria criteria)
        {
            var query = _dbContext.Indictments.AsQueryable()
                .Include(x => x.IndicteeActivity)
                .Include(x => x.ViolationRegionalUnity)
                .Include(x => x.IndictmentRegionalUnity)
                .Include(x => x.ViolationRegion)
                .Include(x => x.IndictmentRegion)
                .Include(x => x.ViolationCity)
                .Include(x => x.IndictmentCity);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.ProtocolNo))
                query = query.Where(x => x.ProtocolNo.Equals(criteria.ProtocolNo));
            if (!string.IsNullOrEmpty(criteria.IndictorName))
                query = query.Where(x => x.IndictorName.Equals(criteria.IndictorName));
            if (!string.IsNullOrEmpty(criteria.IndicteeName))
                query = query.Where(x => x.IndicteeName.Equals(criteria.IndicteeName));
            if (!string.IsNullOrEmpty(criteria.ViolationRegionDescription))
                query = query.Where(x => x.ViolationRegionDescription.Equals(criteria.ViolationRegionDescription));
            if (!string.IsNullOrEmpty(criteria.IndictmentRegionDescription))
                query = query.Where(x => x.IndictmentRegionDescription.Equals(criteria.IndictmentRegionDescription));

            if (criteria.IndicteeActivityId != null)
                query = query.Where(x => x.IndicteeActivityId == criteria.IndicteeActivityId);
            if (criteria.ViolationRegionalUnityId != null)
                query = query.Where(x => x.ViolationRegionalUnityId == criteria.ViolationRegionalUnityId);
            if (criteria.ViolationRegionId != null)
                query = query.Where(x => x.ViolationRegionId == criteria.ViolationRegionId);
            if (criteria.ViolationCityId != null)
                query = query.Where(x => x.ViolationCityId == criteria.ViolationCityId);
            if (criteria.IndictmentRegionalUnityId != null)
                query = query.Where(x => x.IndictmentRegionalUnityId == criteria.IndictmentRegionalUnityId);
            if (criteria.IndictmentRegionId != null)
                query = query.Where(x => x.IndictmentRegionId == criteria.IndictmentRegionId);
            if (criteria.IndictmentCityId != null)
                query = query.Where(x => x.IndictmentCityId == criteria.IndictmentCityId);

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);
            if (criteria.DateUpdated != null)
                query = query.Where(x => x.DateUpdated == criteria.DateUpdated);
            if (criteria.IndictmentDate != null)
                query = query.Where(x => x.IndictmentDate == criteria.IndictmentDate);

            if (criteria.FromDateCreated != null)
                query = query.Where(x => x.DateCreated <= criteria.FromDateCreated);
            if (criteria.ToDateCreated != null)
                query = query.Where(x => x.DateCreated >= criteria.ToDateCreated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated <= criteria.FromDateUpdated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated >= criteria.ToDateUpdated);
            if (criteria.IndictmentDateFrom != null)
                query = query.Where(x => x.IndictmentDate <= criteria.IndictmentDateFrom);
            if (criteria.IndictmentDateTo != null)
                query = query.Where(x => x.IndictmentDate >= criteria.IndictmentDateTo);

            if (criteria.Ids != null && criteria.Ids.Any())
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            if (criteria.IndicteeActivityIds != null)
                query = query.Where(x => criteria.IndicteeActivityIds.Contains(x.IndicteeActivityId));
            if (criteria.ViolationRegionalUnityIds != null)
                query = query.Where(x => criteria.ViolationRegionalUnityIds.Contains(x.ViolationRegionalUnityId));
            if (criteria.ViolationRegionIds != null)
                query = query.Where(x => criteria.ViolationRegionIds.Contains(x.ViolationRegionId));
            if (criteria.ViolationCityIds != null)
                query = query.Where(x => criteria.ViolationCityIds.Contains(x.ViolationCityId));
            if (criteria.IndictmentRegionalUnityIds != null)
                query = query.Where(x => criteria.IndictmentRegionalUnityIds.Contains(x.IndictmentRegionalUnityId));
            if (criteria.IndictmentRegionIds != null)
                query = query.Where(x => criteria.IndictmentRegionIds.Contains(x.IndictmentRegionId));
            if (criteria.IndictmentCityIds != null)
                query = query.Where(x => criteria.IndictmentCityIds.Contains(x.IndictmentCityId));

            return query;
        }

        public override int Create(IndictmentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.IndictorName))
                throw new EiEntityException(@"Indictor Name cannot be empty");
            if (string.IsNullOrEmpty(dto.IndicteeName))
                throw new EiEntityException(@"Indictee Name cannot be empty");
            if (dto.IndicteeActivityId < 1)
                throw new EiEntityException(@"Valid Indictee Activity must be selected");
            if (dto.ViolationRegionalUnityId < 1)
                throw new EiEntityException(@"Valid Violation Regional Unity must be selected");
            if (dto.ViolationRegionId < 1)
                throw new EiEntityException(@"Valid Violation Region must be selected");
            if (dto.ViolationCityId < 1)
                throw new EiEntityException(@"Valid Violation City must be selected");
            if (dto.IndictmentRegionalUnityId < 1)
                throw new EiEntityException(@"Valid Indictment Regional Unity must be selected");
            if (dto.IndictmentRegionId < 1)
                throw new EiEntityException(@"Valid Indictment Region must be selected");
            if (dto.IndictmentCityId < 1)
                throw new EiEntityException(@"Valid Indictment City must be selected");
            if (dto.IndictmentDate == null)
                throw new EiEntityException(@"Indictment Date cannot be empty");
            if (dto.PriorityDegreeId < 1)
                throw new EiEntityException(@"Indictment Priority Degree must be valid");
            if (dto.SafetyDegreeId < 1)
                throw new EiEntityException(@"Indictment Safety Degree must be valid");
            if (string.IsNullOrEmpty(dto.IndictmentRegionDescription))
                throw new EiEntityException(@"Indictment Region Description must not be empty");
            if (string.IsNullOrEmpty(dto.IndictmentContent))
                throw new EiEntityException(@"Indictment Content must not be empty");
            if (string.IsNullOrEmpty(dto.IndictmentSubject))
                throw new EiEntityException(@"Indictment Subject must not be empty");

            var existingEntity = ListByCriteria(new IndictmentCriteria
            {
                ProtocolNo = dto.ProtocolNo,
                IndictorName = dto.IndictorName,
                IndicteeName = dto.IndicteeName,
                ViolationRegionalUnityId = dto.ViolationRegionalUnityId,
                ViolationRegionId = dto.ViolationRegionId,
                ViolationCityId = dto.ViolationCityId,
                IndictmentRegionalUnityId = dto.IndictmentRegionalUnityId,
                IndictmentRegionId = dto.IndictmentRegionId,
                IndictmentCityId = dto.IndictmentCityId,
                IndictmentDate = dto.IndictmentDate
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Indictment
            {
                ProtocolNo = dto.ProtocolNo,
                IndictorName = dto.IndictorName,
                IndicteeName = dto.IndicteeName,
                ViolationRegionalUnityId = dto.ViolationRegionalUnityId,
                ViolationRegionId = dto.ViolationRegionId,
                ViolationCityId = dto.ViolationCityId,
                IndictmentRegionalUnityId = dto.IndictmentRegionalUnityId,
                IndictmentRegionId = dto.IndictmentRegionId,
                IndictmentCityId = dto.IndictmentCityId,
                IndictmentDate = dto.IndictmentDate.Value,
                IndicteeActivityId = dto.IndicteeActivityId,
                IndictmentContent = dto.IndictmentContent,
                IndictmentRegionDescription = dto.IndictmentRegionDescription,
                IndictmentSubject = dto.IndictmentSubject,
                ViolationRegionDescription = dto.ViolationRegionDescription,
                SafetyDegreeId = dto.SafetyDegreeId,
                PriorityDegreeId = dto.PriorityDegreeId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Indictments.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(IndictmentDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.IndictorName))
                throw new EiEntityException(@"Indictor Name cannot be empty");
            if (string.IsNullOrEmpty(dto.IndicteeName))
                throw new EiEntityException(@"Indictee Name cannot be empty");
            if (dto.IndicteeActivityId < 1)
                throw new EiEntityException(@"Valid Indictee Activity must be selected");
            if (dto.ViolationRegionalUnityId < 1)
                throw new EiEntityException(@"Valid Violation Regional Unity must be selected");
            if (dto.ViolationRegionId < 1)
                throw new EiEntityException(@"Valid Violation Region must be selected");
            if (dto.ViolationCityId < 1)
                throw new EiEntityException(@"Valid Violation City must be selected");
            if (dto.IndictmentRegionalUnityId < 1)
                throw new EiEntityException(@"Valid Indictment Regional Unity must be selected");
            if (dto.IndictmentRegionId < 1)
                throw new EiEntityException(@"Valid Indictment Region must be selected");
            if (dto.IndictmentCityId < 1)
                throw new EiEntityException(@"Valid Indictment City must be selected");
            if (dto.IndictmentDate == null)
                throw new EiEntityException(@"Indictment Date cannot be empty");
            if (dto.PriorityDegreeId < 1)
                throw new EiEntityException(@"Indictment Priority Degree must be valid");
            if (dto.SafetyDegreeId < 1)
                throw new EiEntityException(@"Indictment Safety Degree must be valid");
            if (string.IsNullOrEmpty(dto.IndictmentRegionDescription))
                throw new EiEntityException(@"Indictment Region Description must not be empty");
            if (string.IsNullOrEmpty(dto.IndictmentContent))
                throw new EiEntityException(@"Indictment Content must not be empty");
            if (string.IsNullOrEmpty(dto.IndictmentSubject))
                throw new EiEntityException(@"Indictment Subject must not be empty");

            var existingEntity = ListByCriteria(new IndictmentCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.ProtocolNo = dto.ProtocolNo;
            existingEntity.IndictorName = dto.IndictorName;
            existingEntity.IndicteeName = dto.IndicteeName;
            existingEntity.ViolationRegionalUnityId = dto.ViolationRegionalUnityId;
            existingEntity.ViolationRegionId = dto.ViolationRegionId;
            existingEntity.ViolationCityId = dto.ViolationCityId;
            existingEntity.IndictmentRegionalUnityId = dto.IndictmentRegionalUnityId;
            existingEntity.IndictmentRegionId = dto.IndictmentRegionId;
            existingEntity.IndictmentCityId = dto.IndictmentCityId;
            existingEntity.IndictmentDate = dto.IndictmentDate.Value;
            existingEntity.IndicteeActivityId = dto.IndicteeActivityId;
            existingEntity.IndictmentContent = dto.IndictmentContent;
            existingEntity.IndictmentRegionDescription = dto.IndictmentRegionDescription;
            existingEntity.IndictmentSubject = dto.IndictmentSubject;
            existingEntity.ViolationRegionDescription = dto.ViolationRegionDescription;
            existingEntity.SafetyDegreeId = dto.SafetyDegreeId;
            existingEntity.PriorityDegreeId = dto.PriorityDegreeId;
            existingEntity.DateCreated = DateTime.Now;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new IndictmentCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Indictments.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class IndictmentsServiceExtensions
    {
        public static IEnumerable<IndictmentDto> GetDtos(this IEnumerable<Indictment> entities)
        {
            var dtosList = entities
                .Select(x => new IndictmentDto
                {
                    Id = x.Id,
                    ProtocolNo = x.ProtocolNo,
                    ViolationRegionalUnityId = x.ViolationRegionalUnityId,
                    IndicteeActivityId = x.IndicteeActivityId,
                    IndictmentCityId = x.IndictmentCityId,
                    IndictmentRegionId = x.IndictmentRegionId,
                    ViolationRegionId = x.ViolationRegionId,
                    ViolationCityId = x.ViolationCityId,
                    IndictmentRegionalUnityId = x.IndictmentRegionalUnityId,
                    IndictmentDate = x.IndictmentDate,
                    IndicteeName = x.IndicteeName,
                    IndictorName = x.IndictorName,
                    IndictmentRegionDescription = x.IndictmentRegionDescription,
                    ViolationRegionDescription = x.ViolationRegionDescription,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ViolationRegionalUnity = x.ViolationRegionalUnity?.Name ?? string.Empty,
                    IndicteeActivityName = x.IndicteeActivity?.Name ?? string.Empty,
                    IndictmentCity = x.IndictmentCity?.Name ?? string.Empty,
                    IndictmentRegion = x.IndictmentRegion?.Name ?? string.Empty,
                    IndictmentRegionalUnity = x.IndictmentRegionalUnity?.Name ?? string.Empty,
                    ViolationCity = x.ViolationCity?.Name ?? string.Empty,
                    ViolationRegion = x.ViolationRegion?.Name ?? string.Empty
                });

            return dtosList;
        }
    }
}
