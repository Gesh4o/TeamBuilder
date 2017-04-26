namespace TeamBuilder.Services.Data.Contracts
{
    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common.Contracts;

    public interface ITeamService : IService
    {
        bool IsTeamExisting(TeamAddBindingModel team);

        TTeamProjection Find<TTeamProjection>(int id);

        Team Add(TeamAddBindingModel teamBindingModel, string creatorId);

        void Edit(TeamEditBindingModel teamBindingModel);

        void Disband(int id);

        string GetPictureAsBase64(string filePath);
    }
}
