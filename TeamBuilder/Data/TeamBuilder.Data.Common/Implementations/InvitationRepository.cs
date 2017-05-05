namespace TeamBuilder.Data.Common.Implementations
{
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;

    public class InvitationRepository : Repository<Invitation>, IInvitationRepository
    {
        public InvitationRepository(TeamBuilderContext context)
            : base(context)
        {
        }
    }
}
