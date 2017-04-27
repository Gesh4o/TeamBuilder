namespace TeamBuilder.Services.Data.Implementations
{
    using System;

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

        public bool IsTeamExisting(TeamAddBindingModel team)
        {
            return this.teamRepository.IsExisting(t => t.Name == team.Name);
        }

        public TTeamProjection Find<TTeamProjection>(int id)
        {
            TTeamProjection team = this.teamRepository
                .SingleOrDefault<TTeamProjection>(t => t.Id == id);

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

            this.teamRepository.Add(team);

            return team;
        }

        public void Edit(TeamEditBindingModel teamBindingModel)
        {
            Team team = this.Find<Team>(teamBindingModel.Id);

            if (team == null)
            {
                throw new ArgumentNullException(ServicesConstants.TeamNotFound);
            }

            team.Name = teamBindingModel.Name;
            team.Acronym = teamBindingModel.Acronym;
            team.Description = teamBindingModel.Description;

            this.teamRepository.Update(team);
        }

        public void Disband(int id)
        {
            Team team = this.Find<Team>(id);

            if (team == null)
            {
                throw new ArgumentNullException(ServicesConstants.TeamNotFound);
            }

            team.IsDeleted = true;
            this.teamRepository.Update(team);
        }

        public string GetPictureAsBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }

            return FileUtilities.ConvertByteArrayToImageUrl(this.fileService.Download(filePath));
        }
    }
}
