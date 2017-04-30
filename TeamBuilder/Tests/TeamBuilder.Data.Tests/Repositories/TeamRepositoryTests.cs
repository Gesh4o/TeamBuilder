namespace TeamBuilder.Data.Tests.Repositories
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;

    using NUnit.Framework;

    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Common.Implementations;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Tests.Common;

    [TestFixture]
    public class TeamRepositoryTests
    {
        private const int AcronymLength = 3;
        private const int NameLength = 10;
        private const int DescriptionLength = 15;

        private TeamBuilderContext context;

        private ITeamRepository teamRepository;

        private ApplicationUser currentUser;

        [OneTimeSetUp]
        public void SetUp()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TeamBuilderContext>());
            this.context = new TeamBuilderContext();
            this.teamRepository = new TeamRepository(this.context);

            this.currentUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "TestUser",
                PasswordHash = "Some_Useless_Hash",
                Email = "test_user@test.com"
            };
            this.context.Users.Add(this.currentUser);
            this.context.SaveChanges();

            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<Team, Team>();
                    });
        }
        
        [Test]
        public void AddTeam_WithProperData_Should_AddSameEntityInRepository()
        {
            Team expectedTeam = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);

            Team actualTeam = this.teamRepository.SingleOrDefault(t => t.Name == expectedTeam.Name);

            Assert.IsNotNull(actualTeam, "Addition of team failed - team not added.");

            CompareTeams(expectedTeam, actualTeam);
        }

        [Test]
        public void AddTeam_WithProperData_Should_ReturnSameEntityAsResult()
        {
            Team expectedTeam = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            Team actualTeam = this.teamRepository.Add(expectedTeam);

            Assert.IsNotNull(actualTeam, "The returned team is not valid!");
            CompareTeams(expectedTeam, actualTeam);
        }

        [Test]
        public void AddTeam_WithProperData_Should_AddSameEntityInDatabase()
        {
            Team expectedTeam = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);

            Team actualTeam = this.context.Teams.SingleOrDefault(t => t.Name == expectedTeam.Name);

            Assert.IsNotNull(actualTeam, "Addition of team failed - team not added.");

            CompareTeams(expectedTeam, actualTeam);
        }

        [Test]
        public void AddTeam_WithInvalidName_Should_ThrowException()
        {
            Team team = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength + 5),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            Assert.Throws<ValidationException>(
                () =>
                    {
                        this.teamRepository.Add(team);
                    });
        }

        [Test]
        public void EditTeam_Should_ReturnUpdatedEntity_And_UpdateEntityInDatabase()
        {
            Team expectedTeam = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);

            expectedTeam.IsDeleted = true;
            bool hasDeleted = this.teamRepository.Update(expectedTeam);
            Team actualTeam = this.teamRepository.SingleOrDefault(t => t.Id == expectedTeam.Id);
            
            Assert.IsTrue(hasDeleted);
            Assert.IsNotNull(actualTeam);
            Assert.IsTrue(actualTeam.IsDeleted);

            CompareTeams(expectedTeam, actualTeam);
        }

        [Test]
        public void DeleteTeam_Should_ReturnSameEntity_And_DeleteIt()
        {
            Team expectedTeam = new Team
            {
                Name = Utilities.GenerateRandomString(NameLength),
                Acronym = Utilities.GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = Utilities.GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);
            Team actualTeam = this.teamRepository.Delete(expectedTeam);

            Team reloadedTEam = this.teamRepository.SingleOrDefault(t => t.Id == expectedTeam.Id);

            Assert.IsNotNull(actualTeam);
            Assert.IsNull(reloadedTEam);

            CompareTeams(expectedTeam, actualTeam);
        }

        private static void CompareTeams(Team expectedTeam, Team actualTeam)
        {
            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(expectedTeam.CreatorId, actualTeam.CreatorId);
            Assert.AreEqual(expectedTeam.Description, actualTeam.Description);
        }
    }
}
