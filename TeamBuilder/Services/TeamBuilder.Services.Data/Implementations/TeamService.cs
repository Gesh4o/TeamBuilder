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
    using TeamBuilder.Services.Common.Utilities;
    using TeamBuilder.Services.Data.Contracts;

    public class TeamService : ITeamService
    {
        private readonly TeamBuilderContext context;

        private readonly IFileService fileService;

        public TeamService(TeamBuilderContext context, IFileService fileService)
        {
            this.context = context;
            this.fileService = fileService;
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

        public Team Add(TeamAddBindingModel teamBindingModel, string creatorId)
        {
            Team team = Mapper.Instance.Map<Team>(teamBindingModel);
            team.CreatorId = creatorId;
            if (teamBindingModel.Image != null)
            {
                team.ImageFileName = this.fileService.Upload(teamBindingModel.Image.InputStream);
            }

            this.context.Teams.Add(team);
            this.context.SaveChanges();

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
