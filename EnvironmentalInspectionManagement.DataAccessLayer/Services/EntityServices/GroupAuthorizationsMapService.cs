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

    public interface IGroupAuthorizationsMapService : IBaseEntityService<GroupAuthorizationMap, GroupAuthorizationMapCriteria, GroupAuthorizationMapDto> { }

    public class GroupAuthorizationsMapService : BaseEntityService<GroupAuthorizationMap, GroupAuthorizationMapCriteria, GroupAuthorizationMapDto>, IGroupAuthorizationsMapService
    {
        private readonly EiDbContext _dbContext;

        public GroupAuthorizationsMapService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<GroupAuthorizationMap> ListByCriteria(GroupAuthorizationMapCriteria criteria)
        {
            var query = _dbContext.GroupAuthorizationMaps.AsQueryable()
                .Include(x => x.UserGroup)
                .Include(x => x.Authorization);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);

            if (criteria.GroupId != null)
                query = query.Where(x => x.GroupId == criteria.GroupId);
            if (criteria.AuthorizationId != null)
                query = query.Where(x => x.AuthorizationId == criteria.AuthorizationId);

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

            if (criteria.C != null)
                query = query.Where(x => x.C == criteria.C);
            if (criteria.R != null)
                query = query.Where(x => x.R == criteria.R);
            if (criteria.U != null)
                query = query.Where(x => x.U == criteria.U);
            if (criteria.D != null)
                query = query.Where(x => x.D == criteria.D);

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));

            return query;
        }

        public override int Create(GroupAuthorizationMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.GroupId == null || dto.AuthorizationId == null)
                throw new EiEntityException(@"This mapping is not valid");

            var existingEntity = ListByCriteria(new GroupAuthorizationMapCriteria
            {
                GroupId = dto.GroupId,
                AuthorizationId = dto.AuthorizationId,
                C = dto.C,
                R = dto.R,
                U = dto.U,
                D = dto.D
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new GroupAuthorizationMap
            {
                GroupId = dto.GroupId.Value,
                AuthorizationId = dto.AuthorizationId.Value,
                C = dto.C ?? false,
                R = dto.R ?? false,
                U = dto.U ?? false,
                D = dto.D ?? false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.GroupAuthorizationMaps.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(GroupAuthorizationMapDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (dto.GroupId == null || dto.AuthorizationId == null)
                throw new EiEntityException(@"This mapping is not valid");

            var existingEntity = ListByCriteria(new GroupAuthorizationMapCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.GroupId = dto.GroupId.Value;
            existingEntity.AuthorizationId = dto.AuthorizationId.Value;
            existingEntity.C = dto.C ?? false;
            existingEntity.R = dto.R ?? false;
            existingEntity.U = dto.U ?? false;
            existingEntity.D = dto.D ?? false;
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new GroupAuthorizationMapCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.GroupAuthorizationMaps.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class GroupAuthorizationsMapServiceExtensions
    {
        public static IEnumerable<GroupAuthorizationMapDto> GetDtos(this IEnumerable<GroupAuthorizationMap> entities)
        {
            var entityDtos = entities
                .Select(x => new GroupAuthorizationMapDto
                {
                    Id = x.Id,
                    GroupId = x.GroupId,
                    AuthorizationId = x.AuthorizationId,
                    C = x.C,
                    R = x.R,
                    U = x.U,
                    D = x.D,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Authorization = x.Authorization?.Name ?? string.Empty,
                    Group = x.UserGroup?.Name ?? string.Empty
                });

            return entityDtos;
        }
    }
}
