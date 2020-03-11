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
    using Utilities;
    using System;
    using Core;
    #endregion

    public interface IUsersService : IBaseEntityService<User, UserCriteria, UserDto> { }

    public class UsersService : BaseEntityService<User, UserCriteria, UserDto>, IUsersService
    {
        private readonly EiDbContext _dbContext;

        public UsersService()
        {
            _dbContext = new EiDbContext();
        }

        public override IEnumerable<User> ListByCriteria(UserCriteria criteria)
        {
            var query = _dbContext.Users.AsQueryable()
                .Include(x => x.UserGroup);

            if (criteria.Id != null)
                query = query.Where(x => x.Id == criteria.Id);
            if (criteria.GroupId != null)
                query = query.Where(x => x.GroupId == criteria.GroupId);

            if (!string.IsNullOrEmpty(criteria.FirstName))
                query = query.Where(x => x.FirstName.Equals(criteria.FirstName));
            if (!string.IsNullOrEmpty(criteria.LastName))
                query = query.Where(x => x.LastName.Equals(criteria.LastName));
            if (!string.IsNullOrEmpty(criteria.Address))
                query = query.Where(x => x.Address.Equals(criteria.Address));
            if (!string.IsNullOrEmpty(criteria.Username))
                query = query.Where(x => x.Username.Equals(criteria.Username));
            if (!string.IsNullOrEmpty(criteria.Password))
            {
                var pw = criteria.Password.ToSha256();
                query = query.Where(x => x.Password.Equals(pw));
            }

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

            if (criteria.FromBirthDate != null)
                query = query.Where(x => x.BirthDate <= criteria.FromBirthDate);
            if (criteria.ToBirthDate != null)
                query = query.Where(x => x.BirthDate >= criteria.ToBirthDate);

            if (criteria.Ids != null)
                query = query.Where(x => criteria.Ids.Contains(x.Id));
            if (criteria.GroupIds != null)
                query = query.Where(x => criteria.GroupIds.Contains(x.GroupId));

            return query;
        }

        public override int Create(UserDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName))
                throw new EiEntityException(@"First Name or Last Name cannot be empty");
            if (dto.BirthDate == null)
                throw new EiEntityException(@"Birthdate cannot be empty");
            if (dto.GroupId < 1)
                throw new EiEntityException(@"User must belong to a valid User Group");

            var existingEntity = ListByCriteria(new UserCriteria
            {
                FirstName = dto.FirstName,
                LastName= dto.LastName
            }).FirstOrDefault();

            if (existingEntity != null)
                throw new EiEntityException(@"Entity already exists");

            var pw = dto.Password.ToSha256();
            var entityToSave = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Username = dto.Username,
                Password = pw,
                GroupId = dto.GroupId,
                BirthDate = dto.BirthDate,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            var savedEntity = _dbContext.Users.Add(entityToSave);
            _dbContext.SaveChanges();

            return savedEntity?.Id ?? 0;
        }

        public override void Update(UserDto dto)
        {
            if (dto == null)
                throw new EiEntityException(@"Entity cannot be null");
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName))
                throw new EiEntityException(@"First Name or Last Name cannot be empty");
            if (dto.BirthDate == null)
                throw new EiEntityException(@"Birthdate cannot be empty");
            if (dto.GroupId < 1)
                throw new EiEntityException(@"User must belong to a valid User Group");

            var existingEntity = ListByCriteria(new UserCriteria { Id = dto.Id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException(@"Entity does not exist. Nothing to update.");

            existingEntity.FirstName = dto.FirstName;
            existingEntity.LastName = dto.LastName;
            existingEntity.Address = dto.Address;
            existingEntity.GroupId = dto.GroupId;
            existingEntity.BirthDate = dto.BirthDate;
            existingEntity.Username = dto.Username;
            existingEntity.Password = dto.Password.ToSha256();
            existingEntity.DateUpdated = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public override void Delete(int id)
        {
            if (id == 0)
                throw new EiEntityException(@"Entity cannot have Id equal to 0");

            var existingEntity = ListByCriteria(new UserCriteria { Id = id }).FirstOrDefault();

            if (existingEntity == null)
                throw new EiEntityException($"Error getting entity with Id {id}");

            _dbContext.Users.Remove(existingEntity);
            _dbContext.SaveChanges();
        }
    }

    public static class UsersServiceExtensions
    {
        public static IEnumerable<UserDto> GetDtos(this IEnumerable<User> entities)
        {
            var entityDtos = entities
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    Username = x.Username,
                    BirthDate = x.BirthDate,
                    GroupId = x.GroupId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    GroupName = x.UserGroup.Name,
                    Roles = new[] {x.UserGroup.Name}
                });

            return entityDtos;
        }
    }
}
