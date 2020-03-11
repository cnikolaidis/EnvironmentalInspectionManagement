namespace EnvironmentalInspectionManagement.DataAccessLayer.Services.EntityServices
{
    #region Usings
    using System.Data.Entity.Infrastructure;
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

    public interface IAuthorizationsService : IBaseEntityService<Authorization, AuthorizationCriteria, AuthorizationDto> { }

    public class AuthorizationsService : BaseEntityService<Authorization, AuthorizationCriteria, AuthorizationDto>, IAuthorizationsService
    {
        private readonly EiDbContext _dbContext;

        public AuthorizationsService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<Authorization> ListByCriteria(AuthorizationCriteria criteria)
        {
            var query = _dbContext.Authorizations.AsQueryable()
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

        public override int Create(AuthorizationDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new AuthorizationCriteria
            {
                Name = dto.Name
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var entityToSave = new Authorization
            {
                Name = dto.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Authorizations.Add(entityToSave);
            _dbContext.SaveChanges();
            var savedEntityId = savedEntity?.Id ?? 0;

            //*RULE*: Add Authorization (without CRUD) to every UserGroup
            var userGroups = Svc.GroupAuthorizationsMapsService
                .ListByCriteria(new GroupAuthorizationMapCriteria())
                .Where(x => x.AuthorizationId != savedEntityId)
                .Select(x => x.GroupId)
                .Distinct()
                .ToList();
            userGroups.ForEach(x =>
            {
                Svc.GroupAuthorizationsMapsService.Create(new GroupAuthorizationMapDto
                {
                    GroupId = x,
                    AuthorizationId = savedEntityId,
                    C = false,
                    R = false,
                    U = false,
                    D = false
                });
            });

            return savedEntityId;
        }

        public override void Update(AuthorizationDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.Name))
                throw new EiEntityException(@"Name cannot be empty");

            var existingEntity = ListByCriteria(new AuthorizationCriteria { Id = dto.Id }).FirstOrDefault();

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

            var existingEntity = ListByCriteria(new AuthorizationCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            //*RULE*: If at least 1 GroupAuth Mapping exists with a C/R/U/D right given, do not delete
            var userGroupAuths = Svc.GroupAuthorizationsMapsService
                .ListByCriteria(new GroupAuthorizationMapCriteria())
                .Where(x => x.AuthorizationId == existingEntity.Id)
                .FirstOrDefault(x => x.C || x.R || x.U || x.D);

            if (userGroupAuths != null)
                throw new EiEntityException(@"Authorization is currently enabled for one UserGroup");

            //*RULE*: Remove all Authorization mappings with UserGroups
            var userGroupMaps = Svc.GroupAuthorizationsMapsService
                .ListByCriteria(new GroupAuthorizationMapCriteria {AuthorizationId = existingEntity.Id})
                .GetDtos().ToList();

            foreach (var ug in userGroupMaps)
                Svc.GroupAuthorizationsMapsService.Delete(ug.Id);

            _dbContext.Authorizations.Remove(existingEntity);

            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException cx)
                {
                    saveFailed = true;
                    cx.Entries.Single().Reload();
                }
            } while (saveFailed);
        }
    }

    public static class AuthorizationsServiceExtensions
    {
        public static IEnumerable<AuthorizationDto> GetDtos(this IEnumerable<Authorization> entities)
        {
            var entityDtos = entities
                .Select(x => new AuthorizationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    GroupAuthorizations = x.GroupAuthorizations.GetDtos()
                });

            return entityDtos;
        }
    }
}
