namespace TeamBuilder.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using TeamBuilder.Clients.Models.Home;
    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common.Contracts;

    public interface ITeamService : IService
    {
        bool IsTeamNameTaken(string teamName);

        TTeamProjection Find<TTeamProjection>(int id);

        TTeamProjection Find<TTeamProjection>(string teamName);

        Team Find(int id);

        IEnumerable<TTeamProjection> Filter<TTeamProjection>(Expression<Func<Team, bool>> filter, string include = "");

        Team Add(TeamAddBindingModel teamBindingModel, string creatorId);

        void Edit(TeamEditBindingModel teamBindingModel);

        void Disband(int id);

        void AddUserToTeam(string userId, int teamId, TeamRole teamRole = TeamRole.Member);

        OverviewViewModel GetOverviewModel(string teamName);

        RequestsViewModel GetInvitationsViewModel(string teamName);

        UserJoinRequestsViewModel GetUserJoinRequestsViewModel(string teamName);

        SettingsViewModel GetSettingsViewModel(string teamName);

        TeamDetailsViewModel GetTeamDetails(string teamName, string section, string currentUserId);

        void SendJoinRequest(int modelId, string userId);

        IEnumerable<TeamViewModel> GetAllTeamsByCreatorId(string userId);
    }
}
