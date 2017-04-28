namespace TeamBuilder.Data.Tests.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;

    using NUnit.Framework;

    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Common.Implementations;
    using TeamBuilder.Data.Models;

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
                Name = GenerateRandomString(NameLength),
                Acronym = GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);

            Team actualTeam = this.teamRepository.SingleOrDefault(t => t.Name == expectedTeam.Name);

            Assert.IsNotNull(actualTeam, "Addition of team failed - team not added.");

            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(expectedTeam.CreatorId, actualTeam.CreatorId);
            Assert.AreEqual(expectedTeam.Description, actualTeam.Description);
        }

        [Test]
        public void AddTeam_WithProperData_Should_ReturnSameEntityAsResult()
        {
            Team expectedTeam = new Team
            {
                Name = GenerateRandomString(NameLength),
                Acronym = GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            Team returnedTeam = this.teamRepository.Add(expectedTeam);

            Assert.IsNotNull(returnedTeam, "The returned team is not valid!");

            Assert.AreEqual(expectedTeam.Name, returnedTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, returnedTeam.Acronym);
            Assert.AreEqual(expectedTeam.CreatorId, returnedTeam.CreatorId);
            Assert.AreEqual(expectedTeam.Description, returnedTeam.Description);
        }

        [Test]
        public void AddTeam_WithProperData_Should_AddSameEntityInDatabase()
        {
            Team expectedTeam = new Team
            {
                Name = GenerateRandomString(NameLength),
                Acronym = GenerateRandomString(AcronymLength),
                CreatorId = this.currentUser.Id,
                Description = GenerateRandomString(DescriptionLength),
                ImageFileName = null
            };

            this.teamRepository.Add(expectedTeam);

            Team actualTeam = this.context.Teams.SingleOrDefault(t => t.Name == expectedTeam.Name);

            Assert.IsNotNull(actualTeam, "Addition of team failed - team not added.");

            Assert.AreEqual(expectedTeam.Name, actualTeam.Name);
            Assert.AreEqual(expectedTeam.Acronym, actualTeam.Acronym);
            Assert.AreEqual(expectedTeam.CreatorId, actualTeam.CreatorId);
            Assert.AreEqual(expectedTeam.Description, actualTeam.Description);
        }

        [Test]
        public void EditTeam_Should_UpdateEntity()
        {
        }

        [Test]
        public void DeleteTeam_Should_UpdateEntity()
        {
        }

        private static string GenerateRandomString(int length)
        {
            string result = Guid.NewGuid().ToString().Substring(0, length);
            return result;
        }
    }
}
