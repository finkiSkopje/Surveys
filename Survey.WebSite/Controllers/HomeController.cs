using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;
using System.Web.Security;
using Survey.Domain;
using Survey.DAL;
using Survey.Sync;
using Survey.WebSite.Models;

namespace Survey.WebSite.Controllers
{
    [AuthorizeUser(AccessLevel = AppUserTypes.admin)]
    public class HomeController : Controller
    {
        private UnitOfWork uiw;
        public IFunctions Functions;

        public HomeController(IFunctions Functions)
        {
            uiw = new UnitOfWork();
            this.Functions = Functions;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report1()
        {
            var dt = DateTime.Now;
            int semester = 0, year = 0;
            Functions.GetCurrentSemester(dt, out semester, out year);
            var model = new Report1Model { list = new List<Report1Entity>() };

            populateSemTypes(semester);
            populateYears(year);

            return View(model);
        }

        [HttpPost]
        public ActionResult Report1(Report1Model model)
        {
            int semester = model.SemesterTypes ?? 0, year = model.Years ?? 0;
            int type = semester == 1 ? 1 : 0;

            var list = new List<Report1Entity>();
			//CODE OMMITED FOR SECURITY REASONS


            model.list = list;
            populateSemTypes(model.SemesterTypes);
            populateYears(model.Years);

            return View(model);
        }

        private void populateSemTypes(int? selected = null)
        {
            var list = new List<SelectEntity>();
            list.Add(new SelectEntity { Id = 1, Name = "зимски" });
            list.Add(new SelectEntity { Id = 2, Name = "летен" });
            ViewBag.SemesterTypes = new SelectList(list, "ID", "Name", selected);
        }

        private void populateYears(int? selected = null)
        {
            var list = new List<SelectEntity>();
            var dt = DateTime.Now;
            for (int i = dt.Year - 3; i < dt.Year + 3; i++)
            {
                list.Add(new SelectEntity { Id = i, Name = i.ToString() });
            }
            ViewBag.Years = new SelectList(list, "ID", "Name", selected);
        }
    }
}