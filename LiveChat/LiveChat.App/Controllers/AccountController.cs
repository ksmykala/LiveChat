using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using LiveChat.Domain.Infrastructure.Interfaces;
using LiveChat.Domain.Models.EntityClasses;
using System;
using System.Linq;
using System.Web.Security;
using LiveChat.Domain.Models.EntityExtensions;
using WebGrease.Css.Extensions;
using WebMatrix.WebData;

namespace LiveChat.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<webpages_Roles> _rolesRepository;

        public AccountController(IUserRepository repository, IRepository<webpages_Roles> rolesRepository)
        {
            _userRepository = repository;
            _rolesRepository = rolesRepository;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { model.Nickname });
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Shoutbox", "Chat");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterNewUser(ManageAccountsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.RegisterModel.UserName, model.RegisterModel.Password,
                        new { model.RegisterModel.Nickname });
                    return RedirectToAction("ManageAccounts");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, ErrorCodeToString(e.StatusCode));
                }
            }
            if (model.Users == null)
                model.Users = GetUsers();

            return View("ManageAccounts", model);
        }

        public ViewResult ManageAccounts(ManageAccountsViewModel model)
        {
            if (model.Users == null)
                model.Users = GetUsers();

            return View(model);
        }

        private IEnumerable<User> GetUsers()
        {
            var result = _userRepository.GetAll().ToList();
            result.ForEach(x => x.UserRolesCount = x.webpages_Roles.Count);

            return result;
        }

        [Authorize(Roles = "Administrator, User")]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : string.Empty;
            ViewBag.ReturnUrl = Url.Action("Manage");

            var user = _userRepository.GetAll().Single(x => x.UserId == WebSecurity.CurrentUserId);
            var model = new EditUserViewModel
            {
                UserId = user.UserId,
                Nickname = user.Nickname
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(EditUserViewModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (ModelState.IsValid)
            {
                var changePasswordSucceeded = ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }

                ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            return View(model);
        }

        private bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            bool changePasswordSucceeded;
            try
            {
                changePasswordSucceeded = WebSecurity.ChangePassword(userName, oldPassword, newPassword);
            }
            catch (Exception)
            {
                changePasswordSucceeded = false;
            }
            return changePasswordSucceeded;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult ChangeNickname(EditUserViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Nickname))
            {
                var user = _userRepository.GetAll().Single(x => x.UserId == model.UserId);
                user.Nickname = model.Nickname;
                _userRepository.Save(user);
            }

            ModelState.ForEach(x => x.Value.Errors.Clear());

            return RedirectToAction("Manage");
        }

        public ActionResult AddUserRole(int userId, int roleId)
        {
            var user = _userRepository.GetAll().Single(x => x.UserId == userId);
            var roleToAdd = _rolesRepository.GetAll().Single(x => x.RoleId == roleId);

            if (!Roles.IsUserInRole(user.UserName, roleToAdd.RoleName))
                Roles.AddUserToRole(user.UserName, roleToAdd.RoleName);

            var model = new RoleViewModel
            {
                RoleId = roleToAdd.RoleId,
                RoleName = roleToAdd.RoleName,
                UserId = user.UserId,
                UserName = user.UserName
            };

            return PartialView("_RemoveRole", model);
        }

        public PartialViewResult RemoveUserRole(int userId, int roleId)
        {
            var user = _userRepository.GetAll().Single(x => x.UserId == userId);
            var roleToRemove = user.webpages_Roles.Single(x => x.RoleId == roleId);

            if (Roles.IsUserInRole(user.UserName, roleToRemove.RoleName))
                Roles.RemoveUserFromRole(user.UserName, roleToRemove.RoleName);

            var model = new RoleViewModel
            {
                RoleId = roleToRemove.RoleId,
                RoleName = roleToRemove.RoleName,
                UserId = user.UserId,
                UserName = user.UserName
            };

            return PartialView("_AddRole", model);
        }

        public HttpStatusCodeResult RemoveUser(int userId)
        {
            var userToRemove = _userRepository.GetAll().Single(x => x.UserId == userId);
            _userRepository.Delete(userToRemove);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public JsonResult GetUserRolesCount(int userId)
        {
            var count = _userRepository.GetAll().Single(x => x.UserId == userId).webpages_Roles.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public ViewResult EditUser(int userId)
        {
            var user = _userRepository.GetAll().Single(x => x.UserId == userId);
            var userRolesIds = user.webpages_Roles.Select(x => x.RoleId);
            var rolesToAdd = _rolesRepository.GetAll().Where(x => !userRolesIds.Contains(x.RoleId));
            var result = new EditUserAdminViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Nickname = user.Nickname,
                Roles = user.webpages_Roles,
                RolesToAdd = rolesToAdd
            };

            return View(result);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetAll().Single(x => x.UserId == model.UserId);
                user.Nickname = model.Nickname;
                _userRepository.Save(user);

                var changePasswordSucceeded = ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
                if (changePasswordSucceeded)
                {
                    return RedirectToAction("EditUser", new { userId = model.UserId });
                }

                ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            return RedirectToAction("EditUser", new { userId = model.UserId });
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Shoutbox", "Chat");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
    }
}
