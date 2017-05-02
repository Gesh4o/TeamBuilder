namespace TeamBuilder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using TeamBuilder.Clients.Common;
    using TeamBuilder.Clients.Infrastructure.Identity;
    using TeamBuilder.Clients.Models.Home;
    using TeamBuilder.Services.Data.Contracts;

    public class HomeController : Controller
    {
        private readonly ITeamService teamService;

        private readonly ApplicationUserManager userManager;

        public HomeController(ITeamService teamService, ApplicationUserManager userManager)
        {
            this.teamService = teamService;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                ViewBag.Activities = new[]
                {
                    "Noxus joined League of Legends", "Gesh4o commented about League of Legends",
                    "Gesh4o created Noxus", "League of Legends published an article.",
                    "Gesh4o created League of Legends"
                };

                IEnumerable<SearchType> searchTypes = Enum.GetValues(typeof(SearchType)).Cast<SearchType>();

                SelectList searchTypesList = new SelectList(
                    searchTypes.Select(s => s.ToString()));

                ViewBag.SearchTypes = searchTypesList;

                ViewBag.Events = new[]
                {
                    new EventViewModel { Id = 1, Name = "League of Legends", StartDate = "28/02/2017" },
                    new EventViewModel { Id = 2, Name = "Gala Night", StartDate = "11/04/2017" },
                    new EventViewModel { Id = 3, Name = "Aniventure", StartDate = "03/11/2017" }
                };

                ViewBag.Teams = new[]
                {
                    new TeamViewModel { Id = 1, Name = "Noxus" },
                    new TeamViewModel { Id = 2, Name = "Demacia" },
                    new TeamViewModel { Id = 3, Name = "Ionia" }
                };

                return this.View();
            }

            return this.View("Index-Guest");
        }

        [Route("search")]
        public ActionResult Search(string query, string type)
        {
            if (type == SearchType.Users.ToString())
            {
                return this.View("Search-Users", this.userManager.Users.Where(u => u.UserName.Contains(query)).ProjectTo<UserViewModel>());
            }

            if (type == SearchType.Teams.ToString())
            {
                return this.View("Search-Teams", this.teamService.Filter<TeamViewModel>(t => t.Name.Contains(query)));
            }

            if (type == SearchType.Events.ToString())
            {
                // TODO: Change service.
                return this.View("Search-Events", this.teamService.Filter<EventViewModel>(t => t.Name.Contains(query)));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ServerConstants.ErrorMessages.SearchTypeNotSupported);
        }
    }
}