namespace TeamBuilder.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Data.Contracts;

    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        private readonly IFileService fileService;

        public TeamController(ITeamService teamService, IFileService fileService)
        {
            this.teamService = teamService;
            this.fileService = fileService;
        }

        // GET: Team/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeamDetailsViewModel teamViewModel = this.teamService.Find<TeamDetailsViewModel>(id.Value);
            if (teamViewModel == null)
            {
                return this.HttpNotFound();
            }

            teamViewModel.ImageContent = this.fileService.GetPictureAsBase64(teamViewModel.ImageFileName);

            return this.View(teamViewModel);
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Acronym,Image")] TeamAddBindingModel teamBindingModel)
        {
            if (ModelState.IsValid)
            {
                if (this.teamService.IsTeamNameTaken(teamBindingModel.Name))
                {
                    ModelState.AddModelError("Name", "Team with same name already exists!");
                    return this.View(teamBindingModel);
                }

                string userId = this.User.Identity.GetUserId();
                Team team = this.teamService.Add(teamBindingModel, userId);
                this.teamService.AddUserToTeam(userId, team.Id);
                
                return this.RedirectToAction("Details", new { id = team.Id });
            }

            return this.View(teamBindingModel);
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeamEditBindingModel team = this.teamService.Find<TeamEditBindingModel>(id.Value);
            if (team == null)
            {
                return this.HttpNotFound();
            }

            return this.View(team);
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Acronym")] TeamEditBindingModel teamBindingModel)
        {
            if (ModelState.IsValid)
            {
                this.teamService.Edit(teamBindingModel);

                return this.RedirectToAction("Details", new { id = teamBindingModel.Id });
            }

            return this.View(teamBindingModel);
        }

        // GET: Team/Disband/5
        public ActionResult Disband(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Team team = this.teamService.Find(id.Value);
            if (team == null)
            {
                return this.HttpNotFound();
            }

            return this.View(team);
        }

        // POST: Team/Disband/5
        [HttpPost, ActionName("Disband")]
        [ValidateAntiForgeryToken]
        public ActionResult DisbandConfirmed(int id)
        {
            Team team = this.teamService.Find(id);
            if (team == null)
            {
                return this.HttpNotFound();
            }

            this.teamService.Disband(id);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
