namespace TeamBuilder.Services.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using AutoMapper;

    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common;
    using TeamBuilder.Services.Common.Utilities;
    using TeamBuilder.Services.Data.Contracts;

    public class TeamService : ITeamService
    {
        private readonly IFileService fileService;

        private readonly ITeamRepository teamRepository;

        public TeamService(ITeamRepository teamRepository, IFileService fileService)
        {
            this.teamRepository = teamRepository;
            this.fileService = fileService;
        }

        public bool IsTeamNameTaken(string teamName)
        {
            return this.teamRepository.IsExisting(t => t.Name == teamName && t.IsDeleted == false);
        }

        public TTeamProjection Find<TTeamProjection>(int id)
        {
            TTeamProjection team = this.teamRepository
                .SingleOrDefault<TTeamProjection>(t => t.Id == id);

            return team;
        }

        public Team Find(int id)
        {
            Team team = this.teamRepository
                .SingleOrDefault(t => t.Id == id);

            return team;
        }

        public Team Add(TeamAddBindingModel teamBindingModel, string creatorId)
        {
            Team team = Mapper.Instance.Map<Team>(teamBindingModel);
            team.CreatorId = creatorId;
            if (teamBindingModel.Image != null)
            {
                team.ImageFileName = this.fileService.Upload(teamBindingModel.Image.InputStream);
            }

            if (this.IsTeamNameTaken(team.Name))
            {
                throw new InvalidOperationException(string.Format(ServicesConstants.EntityAlreadyExists, "Team", "name"));
            }

            if (!this.teamRepository.IsEntityValid(team))
            {
                throw new ValidationException(string.Format(ServicesConstants.EntityNotValid, "Team"));
            }

            this.teamRepository.Add(team);

            return team;
        }

        public void Edit(TeamEditBindingModel teamBindingModel)
        {
            Team team = this.teamRepository.SingleOrDefault(t => t.Id == teamBindingModel.Id);

            if (team == null)
            {
                throw new InvalidOperationException(string.Format(ServicesConstants.EntityNotFound, "Team"));
            }

            // If name is about to be changed.
            if (team.Name != teamBindingModel.Name)
            {
                if (this.IsTeamNameTaken(teamBindingModel.Name))
                {
                    throw new InvalidOperationException(string.Format(ServicesConstants.EntityAlreadyExists, "Team", "name"));
                }
            }

            team.Name = teamBindingModel.Name;
            team.Acronym = teamBindingModel.Acronym;
            team.Description = teamBindingModel.Description;

            if (!this.teamRepository.IsEntityValid(team))
            {
                throw new ValidationException(string.Format(ServicesConstants.EntityNotValid, "Team"));
            }

            bool hasUpdated = this.teamRepository.Update(team);

            if (!hasUpdated)
            {
                throw new InvalidOperationException(string.Format(ServicesConstants.EntityAlreadyExists, "Team", "name"));
            }
        }

        public void Disband(int id)
        {
            Team team = this.teamRepository.SingleOrDefault(t => t.Id == id);

            if (team == null)
            {
                throw new InvalidOperationException(string.Format(ServicesConstants.EntityNotFound, "Team"));
            }

            team.IsDeleted = true;
            this.teamRepository.Update(team);
        }

        public IEnumerable<TTeamProjection> Filter<TTeamProjection>(Expression<Func<Team, bool>> filter, string include = "")
        {
            return this.teamRepository.GetAll<TTeamProjection>(filter, include);
        }
    }
}
