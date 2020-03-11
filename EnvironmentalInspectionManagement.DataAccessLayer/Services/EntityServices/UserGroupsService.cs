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

    public interface IUserGroupsService : IBaseEntityService<UserGroup, UserGroupCriteria, UserGroupDto> { }

    public class UserGroupsService : BaseEntityService<UserGroup, UserGroupCriteria, UserGroupDto>, IUserGroupsService
    {
        private readonly EiDbContext _dbContext;

        public UserGroupsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<UserGroup> ListByCriteria(UserGroupCriteria criteria)
        {
            var query = _dbContext.UserGroups.AsQueryable()
                .Include(x => x.Users)
                .Include(x => x.GroupAuthorizations);

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

        public override int Create(UserGroupDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new UserGroupCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new UserGroup
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.UserGroups.Add(entityToSave);
            _dbContext.SaveChanges();
            var savedEntityId = savedEntity?.Id ?? 0;

            //*RULE*: Add every Authorization existing for New Group
            var authorizations = Svc.AuthorizationService
                .ListByCriteria(new AuthorizationCriteria())
                .GetDtos()
                .ToList();
            authorizations.ForEach(x =>
            {
                Svc.GroupAuthorizationsMapsService.Create(new GroupAuthorizationMapDto
                {
                    AuthorizationId = x.Id,
                    GroupId = savedEntityId,
                    C = false,
                    R = false,
                    U = false,
                    D = false
                });
            });

            return savedEntityId;
        }

        public override void Update(UserGroupDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new UserGroupCriteria { Id = dto.Id }).FirstOrDefault();

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

            var existingEntity = ListByCriteria(new UserGroupCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            //*RULE*: If at least 1 User belongs to this UserGroup, do not delete
            var groupUsers = Svc.UsersService.ListByCriteria(new UserCriteria
            {
                GroupId = existingEntity.Id
            }).FirstOrDefault();

            if (groupUsers != null)
                throw new EiEntityException(@"Users still belong to this Group");

            //*RULE*: Remove all Authorization mappings for UserGroup
            var authorizations = Svc.GroupAuthorizationsMapsService.ListByCriteria(new GroupAuthorizationMapCriteria
            {
                GroupId = existingEntity.Id
            }).GetDtos().ToList();
            authorizations.ForEach(x =>
            {
                Svc.GroupAuthorizationsMapsService.Delete(x.Id);
            });

            _dbContext.UserGroups.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class UserGroupsServiceExtensions
    {
        public static IEnumerable<UserGroupDto> GetDtos(this IEnumerable<UserGroup> entities)
        {
            var entityDtos = entities
                .Select(x => new UserGroupDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Users = x.Users.GetDtos(),
                    GroupAuthorizations = x.GroupAuthorizations.GetDtos()
                });

            return entityDtos;
        }
    }
}
