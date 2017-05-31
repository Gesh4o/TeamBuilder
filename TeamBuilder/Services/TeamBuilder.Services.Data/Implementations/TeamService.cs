namespace TeamBuilder.Services.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper;

    using TeamBuilder.Clients.Models.Home;
    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Common;
    using TeamBuilder.Services.Data.Contracts;

    public class TeamService : ITeamService
    {
        private readonly IFileService fileService;

        private readonly ITeamRepository teamRepository;

        private readonly IInvitationRepository invitationRepository;

        public TeamService(ITeamRepository teamRepository, IInvitationRepository invitationRepository, IFileService fileService)
        {
            this.invitationRepository = invitationRepository;
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

        public TTeamProjection Find<TTeamProjection>(string teamName)
        {
            return this.teamRepository.SingleOrDefault<TTeamProjection>(t => t.Name == teamName);
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

        public void AddUserToTeam(string userId, int teamId, TeamRole role = TeamRole.Member)
        {
            // TODO: Check userId.
            Team team = this.teamRepository.SingleOrDefault(t => t.Id == teamId);
            team.Members.Add(new UserTeam
            {
                TeamId = teamId,
                UserId = userId,
                Role = role
            });

            this.teamRepository.Update(team);
        }

        public OverviewViewModel GetOverviewModel(string teamName)
        {
            Team team = this.teamRepository.SingleOrDefault(t => t.Name == teamName, "Members");

            OverviewViewModel model = new OverviewViewModel
            {
                Description = team.Description,
                Members = team.Members
                    .Select(us => us.User)
                    .Select(
                        u =>
                            new MemberViewModel
                            {
                                Username = u.UserName,
                                ProfilePictureLink = u.ProfilePicturePath
                            })
                    .ToList()
            };

            foreach (MemberViewModel member in model.Members)
            {
                member.ProfilePictureLink = this.fileService.GetPictureAsBase64(member.ProfilePictureLink);
            }

            return model;
        }

        public UserJoinRequestsViewModel GetUserJoinRequestsViewModel(string teamName)
        {
            Team team = this.teamRepository.SingleOrDefault(t => t.Name == teamName, "Invitations,Invitations.InvitedUser");

            UserJoinRequestsViewModel model = new UserJoinRequestsViewModel
            {
                TeamId = team.Id,
                Users = team.Invitations
                .Where(i => i.IsActive)
                .Select(
                    i =>
                    new UserRequestViewModel
                    {
                        Id = i.InvitedUserId,
                        Username = i.InvitedUser.UserName
                    })
                    .ToList()
            };

            return model;
        }

        // TODO: Update
        public RequestsViewModel GetInvitationsViewModel(string teamName)
        {
            RequestsViewModel model = new RequestsViewModel();

            return model;
        }

        public SettingsViewModel GetSettingsViewModel(string teamName)
        {
            Team team = this.teamRepository.SingleOrDefault(t => t.Name == teamName, "Members,Members.User");
            SettingsViewModel model = new SettingsViewModel();
            model.TeamId = team.Id;
            model.Members = team.Members.Select(ut => new MemberWithRoleViewModel() { Id = ut.User.Id, Username = ut.User.UserName, Role = ut.Role.ToString() }).ToList();

            return model;
        }

        public TeamDetailsViewModel GetTeamDetails(string teamName, string section, string currentUserId)
        {
            TeamDetailsViewModel teamViewModel = this.teamRepository.GetAll(t => t.Name == teamName)
                .ToList()
                .Select(
                t =>
                    new TeamDetailsViewModel
                    {
                        Name = t.Name,
                        Acronym = t.Acronym,
                        ImageFileName = t.ImageFileName,
                        CanSentJoinRequest = this.GetCanSentJoinRequest(currentUserId, t),
                        SendJoinRequestViewModel = new SendJoinRequestViewModel
                        {
                            TeamId = t.Id,
                            UserId = currentUserId
                        },
                        TeamProfileViewModel = new TeamProfileViewModel
                        {
                            IsAuthenticated = t.CreatorId == currentUserId
                        }
                    })
                    .FirstOrDefault();

            if (teamViewModel == null)
            {
                // TODO: Fix this.
                return null;
            }

            if (string.IsNullOrEmpty(section) || section == "overview")
            {
                teamViewModel.TeamProfileViewModel.ViewData = this.GetOverviewModel(teamName);
                teamViewModel.TeamProfileViewModel.View = "Overview";
            }
            else if (section == "invitations")
            {
                teamViewModel.TeamProfileViewModel.View = "Invitations";
                teamViewModel.TeamProfileViewModel.ViewData = this.GetInvitationsViewModel(teamName);
            }
            else if (section == "requests")
            {
                teamViewModel.TeamProfileViewModel.View = "Requests";
                teamViewModel.TeamProfileViewModel.ViewData = this.GetUserJoinRequestsViewModel(teamName);
            }
            else if (section == "settings")
            {
                teamViewModel.TeamProfileViewModel.View = "Settings";
                teamViewModel.TeamProfileViewModel.ViewData = this.GetSettingsViewModel(teamName);
            }
            else
            {
                return null;
            }

            teamViewModel.LogoUrl = this.fileService.GetPictureAsBase64(teamViewModel.ImageFileName);

            teamViewModel.TeamProfileViewModel.Section = section;
            teamViewModel.TeamProfileViewModel.TeamName = teamName;

            return teamViewModel;
        }

        public void SendJoinRequest(int teamId, string userId)
        {
            Invitation invitation = new Invitation
            {
                InvitedUserId = userId,
                SenderUserId = userId,
                TeamId = teamId
            };

            this.invitationRepository.Add(invitation);
        }

        public IEnumerable<TeamViewModel> GetAllTeamsByCreatorId(string userId)
        {
            return this.teamRepository.GetAll<TeamViewModel>(t => t.CreatorId == userId);
        }

        private bool GetCanSentJoinRequest(string userId, Team team)
        {
            return !string.IsNullOrEmpty(userId) &&
                   team.Members.All(m => m.UserId != userId) &&
                   !this.invitationRepository.IsExisting(
                        i => i.IsActive &&
                        i.InvitedUserId == userId &&
                        i.TeamId == team.Id);
        }
    }
}
