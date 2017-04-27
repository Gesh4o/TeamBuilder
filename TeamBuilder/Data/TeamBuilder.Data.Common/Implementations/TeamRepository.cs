namespace TeamBuilder.Data.Common.Implementations
{
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;

    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(TeamBuilderContext context)
            : base(context)
        {
        }
    }
}
