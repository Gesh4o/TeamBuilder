namespace TeamBuilder.Data.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper.QueryableExtensions;

    using TeamBuilder.Data.Common.Contracts;

    public abstract class Repository<T> : IRepository<T>
        where T : class
    {
        protected Repository(TeamBuilderContext context)
        {
            this.Context = context;
        }

        protected TeamBuilderContext Context { get; }

        public TProjection SingleOrDefault<TProjection>(Expression<Func<T, bool>> condition, string include = "")
        {
            IQueryable<T> query = this.BuildQuery(condition, include);

            return query.ProjectTo<TProjection>().SingleOrDefault();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> condition, string include = "")
        {
            IQueryable<T> query = this.BuildQuery(condition, include);

            return query.SingleOrDefault();
        }

        public IEnumerable<TProjection> GetAll<TProjection>(Expression<Func<T, bool>> condition, string include = "")
        {
            IQueryable<T> query = this.BuildQuery(condition, include);

            return query.ProjectTo<TProjection>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> condition, string include = "")
        {
            IQueryable<T> query = this.BuildQuery(condition, include);

            return query;
        }

        public bool IsExisting(Expression<Func<T, bool>> condition)
        {
            return this.Context.Set<T>().Any(condition);
        }

        public T Add(T entity)
        {
            if (!this.IsEntityValid(entity))
            {
               throw new ValidationException(ValidationConstants.ModelInvalidErrorMessage);
            }

            this.Context.Set<T>().Add(entity);
            this.Context.SaveChanges();

            return entity;
        }

        public bool Update(T entity)
        {
            if (this.IsEntityValid(entity))
            {
                this.Context.Entry(entity).State = EntityState.Modified;
                this.Context.SaveChanges();

                return true;
            }

            return false;
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            T removedEntity = this.Context.Set<T>().Remove(entity);
            this.Context.SaveChanges();

            return removedEntity;
        }

        public bool IsEntityValid(T entity)
        {
            return this.Context.Entry(entity).GetValidationResult().IsValid;
        }

        private IQueryable<T> BuildQuery(Expression<Func<T, bool>> condition, string include)
        {
            IQueryable<T> query = this.Context.Set<T>().Where(condition);

            if (!string.IsNullOrEmpty(include))
            {
                foreach (string property in include.Split(',').Select(prop => prop.Trim()))
                {
                    query = query.Include(property);
                }
            }

            return query;
        }
    }
}
