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

    public interface IControlsService : IBaseEntityService<Control, ControlCriteria, ControlDto> { }

    public class ControlsService : BaseEntityService<Control, ControlCriteria, ControlDto>, IControlsService
    {
        private readonly EiDbContext _dbContext;

        public ControlsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Control> ListByCriteria(ControlCriteria criteria)
        {
            var query = _dbContext.Controls.AsQueryable()
                .Include(x => x.ControlTrigger)
                .Include(x => x.ControlType);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);
            if (criteria.ControlTriggerId != null)
                query = query.Where(x => x.ControlTriggerId == criteria.ControlTriggerId);
            if (criteria.ControlTypeId != null)
                query = query.Where(x => x.ControlTypeId == criteria.ControlTypeId);

            if (!string.IsNullOrEmpty(criteria.ControlManagerName))
                query = query.Where(x => x.ControlManagerName.Equals(criteria.ControlManagerName));
            if (!string.IsNullOrEmpty(criteria.ControlManagerFaculty))
                query = query.Where(x => x.ControlManagerFaculty.Equals(criteria.ControlManagerFaculty));

            if (criteria.DateCreated != null)
                query = query.Where(x => x.DateCreated == criteria.DateCreated);
            if (criteria.DateUpdated != null)
                query = query.Where(x => x.DateUpdated == criteria.DateUpdated);
            if (criteria.ControlDate != null)
                query = query.Where(x => x.ControlDate == criteria.ControlDate);

            if (criteria.FromDateCreated != null)
                query = query.Where(x => x.DateCreated <= criteria.FromDateCreated);
            if (criteria.ToDateCreated != null)
                query = query.Where(x => x.DateCreated >= criteria.ToDateCreated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated <= criteria.FromDateUpdated);
            if (criteria.FromDateUpdated != null)
                query = query.Where(x => x.DateUpdated >= criteria.ToDateUpdated);

            if (criteria.ControlDateFrom != null)
                query = query.Where(x => x.ControlDate <= criteria.ControlDateFrom);
            if (criteria.ControlDateTo != null)
                query = query.Where(x => x.ControlDate >= criteria.ControlDateTo);

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));
            if (criteria.ControlTriggerIds != null)
                query = query.Where(x => criteria.ControlTriggerIds.Contains(x.ControlTriggerId));
            if (criteria.ControlTypeIds != null)
                query = query.Where(x => criteria.ControlTypeIds.Contains(x.ControlTypeId));

            return query;
        }

        public override int Create(ControlDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ControlTypeId < 1)
                throw new EiEntityException(@"Valid Control Type must be selected");
            if (dto.ControlTriggerId < 1)
                throw new EiEntityException(@"Valid Control Trigger must be selected");
            if (string.IsNullOrEmpty(dto.ControlManagerName))
                throw new EiEntityException(@"Control Manager name cannot be empty");
            if (string.IsNullOrEmpty(dto.ControlManagerFaculty))
                throw new EiEntityException(@"Control Manager faculty cannot be empty");

            var existingEntity = ListByCriteria(new ControlCriteria
            {
                ControlTriggerId = dto.ControlTriggerId,
                ControlTypeId = dto.ControlTypeId,
                ControlManagerName = dto.ControlManagerName,
                ControlManagerFaculty = dto.ControlManagerFaculty,
                ControlDate = dto.ControlDate
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Control
            {
                ControlTriggerId = dto.ControlTriggerId,
                ControlTypeId = dto.ControlTypeId,
                ControlManagerName = dto.ControlManagerName,
                ControlManagerFaculty = dto.ControlManagerFaculty,
                ControlDate = dto.ControlDate,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Controls.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(ControlDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.ControlTypeId < 1)
                throw new EiEntityException(@"Valid Control Type must be selected");
            if (dto.ControlTriggerId < 1)
                throw new EiEntityException(@"Valid Control Trigger must be selected");
            if (string.IsNullOrEmpty(dto.ControlManagerName))
                throw new EiEntityException(@"Control Manager name cannot be empty");
            if (string.IsNullOrEmpty(dto.ControlManagerFaculty))
                throw new EiEntityException(@"Control Manager faculty cannot be empty");

            var existingEntity = ListByCriteria(new ControlCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.ControlTriggerId = dto.ControlTriggerId;
            existingEntity.ControlTypeId = dto.ControlTypeId;
            existingEntity.ControlManagerName = dto.ControlManagerName;
            existingEntity.ControlManagerFaculty = dto.ControlManagerFaculty;
            existingEntity.ControlDate = dto.ControlDate;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new ControlCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Controls.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class ControlsServiceExtensions
    {
        public static IEnumerable<ControlDto> GetDtos(this IEnumerable<Control> entities)
        {
            var dtosList = entities
                .Select(x => new ControlDto
                {
                    Id = x.Id,
                    ControlManagerFaculty = x.ControlManagerFaculty,
                    ControlManagerName = x.ControlManagerName,
                    ControlTriggerId = x.ControlTriggerId,
                    ControlTypeId = x.ControlTypeId,
                    ControlDate = x.ControlDate,
                    ControlType = x.ControlType?.Name ?? string.Empty,
                    ControlTrigger = x.ControlTrigger?.Name ?? string.Empty
                });

            return dtosList;
        }
    }
}
