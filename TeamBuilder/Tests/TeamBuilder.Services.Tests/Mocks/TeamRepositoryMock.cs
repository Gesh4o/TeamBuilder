namespace TeamBuilder.Services.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using AutoMapper.QueryableExtensions;

    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;

    public class TeamRepositoryMock : ITeamRepository
    {
        private readonly List<Team> repository;

        public TeamRepositoryMock() : this(new List<Team>())
        {
        }

        public TeamRepositoryMock(List<Team> teams)
        {
            this.repository = teams;
        }

        public Team Add(Team entity)
        {
            this.repository.Add(entity);
            return entity;
        }

        public Team Delete(Team entity)
        {
            this.repository.Remove(entity);
            return entity;
        }

        public IEnumerable<Team> GetAll(Expression<Func<Team, bool>> condition, string include = "")
        {
            return this.repository
                .AsQueryable()
                .Where(condition);
        }

        public IEnumerable<TProjection> GetAll<TProjection>(
            Expression<Func<Team, bool>> condition, string include = "")
        {
            return this.repository.AsQueryable().Where(condition).ProjectTo<TProjection>();
        }

        public bool IsEntityValid(Team entity)
        {
            var attributes = typeof(Team)
                .GetProperties()
                .Select(p => new
                {
                    Property = p,
                    ValidationAttributes = p.GetCustomAttributes<ValidationAttribute>()
                })
                .Where(p => p.ValidationAttributes.Any())
                .ToList();

            return attributes.All(p => p.ValidationAttributes.All(a => a.IsValid(p.Property.GetValue(entity))));
        }

        public bool IsExisting(Expression<Func<Team, bool>> condition)
        {
            return this.repository
                .AsQueryable()
                .Any(condition);
        }

        public Team SingleOrDefault(Expression<Func<Team, bool>> condition, string include = "")
        {
            return this.repository
                .AsQueryable()
                .SingleOrDefault(condition);
        }

        public TProjection SingleOrDefault<TProjection>(Expression<Func<Team, bool>> condition, string include = "")
        {
            return this.repository
                .AsQueryable()
                .Where(condition)
                .ProjectTo<TProjection>()
                .SingleOrDefault();
        }

        public bool Update(Team entity)
        {
            return entity != null && this.IsEntityValid(entity);
        }
    }
}
