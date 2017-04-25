namespace TeamBuilder.Services.Data.Contracts
{
    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common;

    public interface ITeamService : IService
    {
        bool IsTeamExisting(TeamAddBindingModel team);

        TTeamProjection Find<TTeamProjection>(int id);

        Team Add(TeamAddBindingModel teamBindingModel);

        void Edit(TeamEditBindingModel teamBindingModel);

        void Disband(int id);
    }
}
