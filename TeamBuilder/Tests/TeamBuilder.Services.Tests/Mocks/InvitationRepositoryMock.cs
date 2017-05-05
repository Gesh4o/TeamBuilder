namespace TeamBuilder.Services.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;

    public class InvitationRepositoryMock : IInvitationRepository
    {
        public Invitation SingleOrDefault(Expression<Func<Invitation, bool>> condition, string include = "")
        {
            throw new NotImplementedException();
        }

        public TProjection SingleOrDefault<TProjection>(Expression<Func<Invitation, bool>> condition, string include = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Invitation> GetAll(Expression<Func<Invitation, bool>> condition, string include = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TProjection> GetAll<TProjection>(Expression<Func<Invitation, bool>> condition, string include = "")
        {
            throw new NotImplementedException();
        }

        public bool IsExisting(Expression<Func<Invitation, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Invitation Add(Invitation entity)
        {
            throw new NotImplementedException();
        }

        public Invitation Delete(Invitation entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Invitation entity)
        {
            throw new NotImplementedException();
        }

        public bool IsEntityValid(Invitation entity)
        {
            throw new NotImplementedException();
        }
    }
}
