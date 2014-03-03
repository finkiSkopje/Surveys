using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Survey.DAL;
using System.Security.Principal;
using Survey.Domain;

namespace Survey.WebSite
{
    public static class Extensions
    {
        public static UnitOfWork uiw;
        public static string GetRoleId(this IIdentity identity)
        {
            try
            {
                uiw = new UnitOfWork();
                var user = uiw.Repository<User>().GetById(Convert.ToInt32(identity.GetUserId()));
                return user.RoleId.ToString();
            }
            catch
            {
                return "0";
            }
        }
        public static AppUserTypes GetUserType(int? roleId)
        {
            if (roleId == 1)
            {
                return AppUserTypes.teacher;
            }
            else if (roleId == 2)
            {
                return AppUserTypes.student;
            }
            else if (roleId == 3)
            {
                return AppUserTypes.admin;
            }
            else
                return AppUserTypes.anonymous;
        }
    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public AppUserTypes AccessLevel { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            try
            {
                var uiw = new UnitOfWork();
                var user = uiw.Repository<User>().GetById(Convert.ToInt32(httpContext.User.Identity.GetUserId()));
                return AccessLevel == Extensions.GetUserType(user.RoleId);
            }
            catch
            {
                return false;
            }
        }
    }
}