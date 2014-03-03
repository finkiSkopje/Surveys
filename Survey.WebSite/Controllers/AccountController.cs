using Survey.DAL;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Survey.Repositories;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Survey.Sync;
using Survey.Domain;

namespace Survey.WebSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public UnitOfWork uiw;

        public IFunctions Functions;

        public AccountController(IFunctions Functions)
        {
            uiw = new UnitOfWork();
            this.Functions = Functions;
            //ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Index(string returnUrl, bool error = false, string errMsg = "")
        {
            if (this.User.Identity.IsAuthenticated && 
                (TempData["redirectionCounter"] == null || 
                (TempData["redirectionCounter"] != null && (int)TempData["redirectionCounter"] != 1)))
            {
                TempData["redirectionCounter"] = 1;
                return RedirectToAction("Index", GetRedirectionUrl(Convert.ToInt32(this.User.Identity.GetRoleId())));
            }
            var cookie = new HttpCookie("returnUrl", returnUrl);
            Response.SetCookie(cookie);

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.error = error;
            ViewBag.errMsg = errMsg;
            ViewBag.casLogin = Services.cas + "login?service=" + Services.service;

            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginCAS(string ticket)
        {
            var type = AppUserTypes.anonymous;
            User user = null;
            string err = "Неуспешна најава";
            try
            {
                Functions.AuthenticateCAS(ticket, out type, out user);
            }
            catch (Exception er) { err = er.ToString(); }

            if (type == AppUserTypes.anonymous || user == null)
            {
                return RedirectToAction("Index", "Account", new { error = true, errMsg = err });
            }

            SignIn(user);

            return RedirectToAction("Index", GetRedirectionUrl(user.RoleId ?? 0));
        }

        private string GetRedirectionUrl(int roleId)
        {
            if (roleId == 1)
                return "Teachers";
            else if (roleId == 2)
                return "Students";
            if (roleId == 3)
                return "Home";
            else
                return string.Empty;
        }

        private void SignIn(User user)
        {
            List<Claim> claims = new List<Claim>{
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Username),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id.ToString()),
            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Surveys")
        };
            ClaimsIdentity identity = new System.Security.Claims.ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Redirect(Services.cas + "logout");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";



        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}