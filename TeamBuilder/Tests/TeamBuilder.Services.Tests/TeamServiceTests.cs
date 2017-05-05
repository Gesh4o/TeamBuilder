namespace TeamBuilder.Services.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;

    using NUnit.Framework;

    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Common.Implementations;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Data.Contracts;
    using TeamBuilder.Services.Data.Implementations;
    using TeamBuilder.Services.Tests.Mocks;
    using TeamBuilder.Tests.Common;

    [TestFixture]
    public class TeamServiceTests
    {
        private const int NameLength = 5;

        private const int AcronymLength = 3;

        private const int DescriptionLength = 5;

        private const int UserIdLength = 10;

        private ITeamService teamService;

        private ITeamRepository repository;

        [OneTimeSetUp]
        public void Init()
        {
            this.repository = new TeamRepositoryMock();
            this.teamService = new TeamService(this.repository, new InvitationRepositoryMock(), new FileServiceMock());

            // Edit and Disband
            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<TeamAddBindingModel, Team>();
                        cfg.CreateMap<Team, TeamAddBindingModel>();
                        cfg.CreateMap<Team, TeamEditBindingModel>();
                    });
        }

        [Test]
        public void AddTeam_WithProperData_ShouldAddTeam()
        {
            TeamAddBindingModel expectedTeam = new TeamAddBindingModel
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                Description = Utilities.GenerateRandomString(DescriptionLength)
            };

            string userId = Utilities.GenerateRandomString(UserIdLength);
            Team actualTeam = this.teamService.Add(expectedTeam, userId);

            Assert.IsNotNull(actualTeam);

            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(expectedTeam.Description, actualTeam.Description);
            Assert.AreEqual(userId, actualTeam.CreatorId);

            Assert.IsNull(actualTeam.ImageFileName);

            Assert.AreEqual(1, this.repository.GetAll(t => t.Name == actualTeam.Name).Count());
        }

        [Test]
        public void AddTeam_WithIvalidData_ShouldThrowException()
        {
            TeamAddBindingModel expectedTeam = new TeamAddBindingModel
            {
                Name = Utilities.GenerateRandomString(0),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                Description = Utilities.GenerateRandomString(DescriptionLength)
            };

            string userId = Utilities.GenerateRandomString(UserIdLength);

            ValidationException exception = Assert.Throws<ValidationException>(
                () =>
                    {
                        this.teamService.Add(expectedTeam, userId);
                    });

            Assert.That(exception.Message, Is.EqualTo("Team not valid."));
        }

        [Test]
        public void AddTeam_WithTakenName_ShouldThrowException()
        {
            Team firstTeam = this.AddTeam();
            Team secondTeam = new Team
            {
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                Description = Utilities.GenerateRandomString(DescriptionLength)
            };

            TeamAddBindingModel teamToAdd = Mapper.Instance.Map<TeamAddBindingModel>(secondTeam);
            teamToAdd.Name = firstTeam.Name;

            InvalidOperationException ioe = Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        this.teamService.Add(teamToAdd, Utilities.GenerateRandomString(UserIdLength));
                    });

            Assert.That(ioe.Message, Is.EqualTo("Team with same name already exists."));
        }

        [Test]
        public void IsTeamNameTaken_OnAlreadyExistingTeam_ShouldReturnTrue()
        {
            Team expectedTeam = this.AddTeam();

            bool isExisting = this.teamService.IsTeamNameTaken(expectedTeam.Name);

            Assert.IsTrue(isExisting);
        }

        [Test]
        public void IsTeamNameTaken_OnAlreadyExistingButDeletedTeam_ShouldReturnFalse()
        {
            Team expectedTeam = this.AddTeam();
            expectedTeam.IsDeleted = true;

            bool isExisting = this.teamService.IsTeamNameTaken(expectedTeam.Name);

            Assert.IsFalse(isExisting);
        }

        [Test]
        public void FindTeam_ByName_OnAlreadyExistingTeam_ShouldReturnTeam()
        {
            string userId = Utilities.GenerateRandomString(UserIdLength);
            Team expectedTeam = new Team
            {
                Id = Utilities.GenerateUniqueRandomInteger(),
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                Description = Utilities.GenerateRandomString(DescriptionLength),
                CreatorId = userId
            };

            this.repository.Add(expectedTeam);

            TeamAddBindingModel actualTeam = this.teamService.Find<TeamAddBindingModel>(expectedTeam.Id);

            Assert.IsNotNull(actualTeam);

            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(expectedTeam.Description, actualTeam.Description);
            Assert.IsNull(actualTeam.Image);
        }

        [Test]
        public void FindTeam_WithDuplicatedId_ShouldThrowException()
        {
            int id = Utilities.GenerateUniqueRandomInteger();
            Team firstTeam = this.AddTeam();
            firstTeam.Id = id;

            Team secondTeam = this.AddTeam();
            secondTeam.Id = id;

            Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        this.teamService.Find<TeamAddBindingModel>(firstTeam.Id);
                    });
        }

        [Test]
        public void FindTeam_WithNonExistingId_ShouldReturnNull()
        {
            TeamAddBindingModel teamModel = this.teamService.Find<TeamAddBindingModel>(-1);

            Assert.IsNull(teamModel);
        }

        [Test]
        public void EditTeam_Name_ShouldChangeTeamName()
        {
            Team expectedTeam = this.AddTeam();
            expectedTeam.Id = Utilities.GenerateUniqueRandomInteger();

            TeamEditBindingModel editedTeam = Mapper.Map<TeamEditBindingModel>(expectedTeam);
            editedTeam.Name = "DifferentName";

            this.teamService.Edit(editedTeam);

            Team actualTeam = this.repository.SingleOrDefault(t => t.Name == editedTeam.Name);

            Assert.IsNotNull(actualTeam);

            Assert.AreEqual(editedTeam.Name, actualTeam.Name);
            Assert.AreEqual(editedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(editedTeam.Description, actualTeam.Description);
            Assert.AreEqual(editedTeam.Acronym, actualTeam.Acronym);
        }

        [Test]
        public void EditTeam_Name_WithAlreadyExistingName_ShouldThrowException()
        {
            Team firstTeam = this.AddTeam();
            firstTeam.Id = Utilities.GenerateUniqueRandomInteger();

            Team secondTeam = this.AddTeam();
            secondTeam.Id = Utilities.GenerateUniqueRandomInteger();

            TeamEditBindingModel editedTeam = Mapper.Map<TeamEditBindingModel>(secondTeam);
            editedTeam.Name = firstTeam.Name;

            InvalidOperationException ioe = Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        this.teamService.Edit(editedTeam);
                    });

            Assert.That(ioe.Message, Is.EqualTo("Team with same name already exists."));
        }

        [Test]
        public void EditTeam_Description_WithInvalidData_ShouldThrowException()
        {
            Team team = this.AddTeam();
            team.Id = Utilities.GenerateUniqueRandomInteger();

            TeamEditBindingModel editedTeam = Mapper.Instance.Map<TeamEditBindingModel>(team);
            editedTeam.Description = string.Empty;

            ValidationException va = Assert.Throws<ValidationException>(
                () =>
                    {
                        this.teamService.Edit(editedTeam);
                    });

            Assert.That(va.Message, Is.EqualTo("Team not valid."));
        }

        [Test]
        public void DisbandTeam_WhichIsExisting_ShouldMarkItAsDeleted()
        {
            Team team = this.AddTeam();
            team.Id = Utilities.GenerateUniqueRandomInteger();

            this.teamService.Disband(team.Id);

            Assert.IsTrue(team.IsDeleted);
        }

        [Test]
        public void DisbandTeam_WhichIsNotExisting_ShouldThrowException()
        {
            InvalidOperationException ioe = Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        this.teamService.Disband(-1);
                    });

            Assert.That(ioe.Message, Is.EqualTo("Team not found."));
        }

        private Team AddTeam()
        {
            string userId = Utilities.GenerateRandomString(UserIdLength);
            Team team = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                Description = Utilities.GenerateRandomString(DescriptionLength),
                CreatorId = userId
            };

            this.repository.Add(team);
            return team;
        }
    }
}
