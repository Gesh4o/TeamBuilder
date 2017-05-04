namespace TeamBuilder.Web.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using FluentValidation.Results;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    using TeamBuilder.Clients.Infrastructure.Identity;
    using TeamBuilder.Clients.Models.Account.Validation;
    using TeamBuilder.Clients.Models.Manage;
    using TeamBuilder.Data.Models;
    using TeamBuilder.Services.Data.Contracts;
    using TeamBuilder.Services.Data.Implementations;

    [Authorize]
    public class ManageController : Controller
    {
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private readonly IFileService fileService;

        private ApplicationSignInManager signInManager;

        private ApplicationUserManager userManager;

        public ManageController()
        {
            // TODO: Use Ninject.
            this.fileService = new DropboxService();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : this()
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,

            ChangePasswordSuccess,

            SetTwoFactorSuccess,

            SetPasswordSuccess,

            RemoveLoginSuccess,

            RemovePhoneSuccess,

            Error
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        public JsonResult ChangeProfilePicture()
        {
            ChangeProfilePictureBindingModel model = new ChangeProfilePictureBindingModel();
            model.NewProfilePicture = this.Request.Files.Count != 0 ? this.Request.Files.Get(0) : null;

            UpdateProfilePictureValidator validator = new UpdateProfilePictureValidator();
            ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                string errorMessage = validationResult.Errors.FirstOrDefault().ErrorMessage;
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new JsonResult
                {
                    ContentType = "application/json",
                    Data = new { error = errorMessage }
                };
            }

            string profilePicturePath = this.fileService.Upload(model.NewProfilePicture.InputStream);

            string currentUserId = this.User.Identity.GetUserId();
            ApplicationUser user = this.UserManager.Users.FirstOrDefault(u => u.Id == currentUserId);

            user.ProfilePicturePath = profilePicturePath;

            this.UserManager.Update(user);

            return new JsonResult
            {
                Data = new { message = "Profile successfully updated!" },
                ContentType = "application/json"
            };
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordViewModel model)
        {
            string errorMessage;
            if (!ModelState.IsValid)
            {
                errorMessage = GetFirstErrorMessageFromModelState();

                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new JsonResult
                {
                    Data = new { error = errorMessage },
                    ContentType = "application/json"
                };
            }

            var result = UserManager.ChangePasswordAsync(
                             User.Identity.GetUserId(),
                             model.OldPassword,
                             model.NewPassword);
            result.Wait();

            if (result.Result.Succeeded)
            {
                var user = UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.Wait();

                if (user.Result != null)
                {
                    var signIn = SignInManager.SignInAsync(user.Result, isPersistent: false, rememberBrowser: false);

                    signIn.Wait();
                }

                return new JsonResult
                {
                    Data = new { message = "Password successfully changed." },
                    ContentType = "application/json"
                };
            }

            errorMessage = result.Result.Errors.FirstOrDefault();
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return new JsonResult
            {
                Data = new { error = errorMessage },
                ContentType = "application/json"
            };
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Manage");
        }

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess
                                        ? "Your password has been changed."
                                        : message == ManageMessageId.SetPasswordSuccess
                                            ? "Your password has been set."
                                            : message == ManageMessageId.SetTwoFactorSuccess
                                                ? "Your two-factor authentication provider has been set."
                                                : message == ManageMessageId.Error
                                                    ? "An error has occurred."
                                                    : message == ManageMessageId.AddPhoneSuccess
                                                        ? "Your phone number was added."
                                                        : message == ManageMessageId.RemovePhoneSuccess
                                                            ? "Your phone number was removed."
                                                            : string.Empty;

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered =
                                    await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(
                provider,
                Url.Action("LinkLoginCallback", "Manage"),
                User.Identity.GetUserId());
        }

        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }

            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded
                       ? RedirectToAction("ManageLogins")
                       : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.RemoveLoginSuccess
                                        ? "The external login was removed."
                                        : message == ManageMessageId.Error ? "An error has occurred." : string.Empty;
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins =
                AuthenticationManager.GetExternalAuthenticationTypes()
                    .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
                    .ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel { CurrentLogins = userLogins, OtherLogins = otherLogins });
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(
                             User.Identity.GetUserId(),
                             new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    ContentType = "application/json",
                    Data = new { error = "Email not valid." }
                };
            }

            string userId = this.User.Identity.GetUserId();
            ApplicationUser user = this.UserManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user.Email == model.Email)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new JsonResult
                {
                    ContentType = "application/json",
                    Data = new { error = "Please choose different email." }
                };
            }

            user.Email = model.Email;
            this.UserManager.Update(user);

            return new JsonResult
            {
                ContentType = "application/json",
                Data = new { message = "Email successfully updated." }
            };
        }

        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);

            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null
                       ? View("Error")
                       : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePhoneNumberAsync(
                             User.Identity.GetUserId(),
                             model.PhoneNumber,
                             model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "Failed to verify phone");
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private string GetFirstErrorMessageFromModelState()
        {
            return this.ModelState
                .Values
                .FirstOrDefault(v => v.Errors.Any())
                .Errors.FirstOrDefault()
                .ErrorMessage;
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }

            return false;
        }
    }
}