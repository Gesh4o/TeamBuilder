namespace TeamBuilder.Services.Data.Implementations
{
    using System;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common;
    using TeamBuilder.Services.Data.Contracts;

    public class TeamService : ITeamService
    {
        private readonly TeamBuilderContext context;

        public TeamService(TeamBuilderContext context)
        {
            this.context = context;
        }

        public bool IsTeamExisting(TeamAddBindingModel team)
        {
            return this.context.Teams.Any(t => t.Name == team.Name);
        }

        public TTeamProjection Find<TTeamProjection>(int id)
        {
            TTeamProjection team = this.context.Teams
                .Where(t => t.Id == id)
                .AsQueryable()
                .ProjectTo<TTeamProjection>()
                .SingleOrDefault();

            return team;
        }

        public Team Add(TeamAddBindingModel teamBindingModel)
        {
            Team team = Mapper.Instance.Map<Team>(teamBindingModel);

            return team;
        }

        public void Edit(TeamEditBindingModel teamBindingModel)
        {
            Team team = this.context.Teams.Find(teamBindingModel.Id);

            if (team == null)
            {
                throw new ArgumentNullException(ServicesConstants.TeamNotFound);
            }

            team.Name = teamBindingModel.Name;
            team.Acronym = teamBindingModel.Acronym;
            team.Description = teamBindingModel.Description;

            this.context.SaveChanges();
        }

        public void Disband(int id)
        {
            Team team = this.context.Teams.Find(id);

            if (team == null)
            {
                throw new ArgumentNullException(ServicesConstants.TeamNotFound);
            }

            team.IsDeleted = true;

            this.context.SaveChanges();
        }
    }
}
