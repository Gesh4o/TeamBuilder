namespace TeamBuilder.Data.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        T SingleOrDefault(Expression<Func<T, bool>> condition, string include = "");

        TProjection SingleOrDefault<TProjection>(Expression<Func<T, bool>> condition, string include = "");

        IEnumerable<T> GetAll(Expression<Func<T, bool>> condition, string include = "");

        IEnumerable<TProjection> GetAll<TProjection>(Expression<Func<T, bool>> condition, string include = "");

        bool IsExisting(Expression<Func<T, bool>> condition);

        T Add(T entity);

        T Delete(T entity);

        bool Update(T entity);

        bool IsEntityValid(T entity);
    }
}
