namespace TeamBuilder.Web.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using TeamBuilder.Clients.Infrastructure.Identity;
    using TeamBuilder.Clients.Models.Team;
    using TeamBuilder.Data.Common.Contracts;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Data.Contracts;

    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        private readonly IInvitationRepository invitationRepository;

        private ApplicationUserManager userManager;

        public TeamController(ITeamService teamService, IInvitationRepository invitationRepository)
        {
            this.teamService = teamService;
            this.invitationRepository = invitationRepository;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
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

                return this.RedirectToAction("Details", new { id = team.Name });
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

                return this.RedirectToAction("Details", new { id = teamBindingModel.Name });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProcessUserRequest(UserRequestBindingModel model)
        {
            Team team = this.teamService.Find(model.TeamId);
            if (team == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new JsonResult
                {
                    ContentType = "application/json",
                    Data = new { error = "Team does not exist!" }
                };
            }

            ApplicationUser user = this.UserManager.Users.FirstOrDefault(u => u.Id == model.UserId);

            if (user == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new JsonResult
                {
                    ContentType = "application/json",
                    Data = new { error = "User does not exist!" }
                };
            }

            Invitation invitation =
                this.invitationRepository.SingleOrDefault(
                    i => i.InvitedUserId == model.UserId && i.TeamId == model.TeamId && i.IsActive);
            invitation.IsActive = false;
            this.invitationRepository.Update(invitation);

            if (model.Result == "accept")
            {
                this.teamService.AddUserToTeam(model.UserId, model.TeamId);

                return new JsonResult { ContentType = "application/json", Data = new { message = "Request approved!", username = user.UserName } };
            }

            return new JsonResult { ContentType = "application/json", Data = new { message = "Request declined!", username = user.UserName } };
        }

        // GET: Team/Details/5
        [AllowAnonymous]
        [Route("teams/{teamName}")]
        public ActionResult Details(string teamName, string section = "")
        {
            if (string.IsNullOrEmpty(teamName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeamDetailsViewModel model = this.teamService.GetTeamDetails(teamName, section, this.User.Identity.GetUserId());
            return this.View(model);
        }
    }
}
