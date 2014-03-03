using Survey.DAL;
using Survey.Domain;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Repositories;
using Microsoft.AspNet.Identity;
using Survey.WebSite.Models;
using Survey.Sync;

namespace Survey.WebSite.Controllers
{
    [AuthorizeUser(AccessLevel = AppUserTypes.student)]
    public class StudentsController : Controller
    {
        private UnitOfWork uiw;
        public IFunctions Functions;

        public StudentsController(IFunctions Functions)
        {
            uiw = new UnitOfWork();
            this.Functions = Functions;
        }
        //
        // GET: /Students/
        public ActionResult Index()
        {
            var dt = DateTime.Now;
            int semester = 0, year = 0;
            Functions.GetLastSemester(dt, out semester, out year);
            var surveys = uiw.Repository<SurveyCours>().GetByStudentId(Convert.ToInt32(User.Identity.GetUserId()),
                year, semester);

            return View(surveys);
        }

        public ActionResult Survey(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var dt = DateTime.Now;
            int semester = 0, year = 0;
            Functions.GetLastSemester(dt, out semester, out year);
            var item = uiw.Repository<SurveyCours>()
                .GetByIdAndStudentIdRefactored(id.Value, Convert.ToInt32(User.Identity.GetUserId()),
                semester, year);

            if (item == null)
            {
                return HttpNotFound();
            }

            var assistants = uiw.Repository<User>().GetAssistantsByCourse(item.SemesterCours.CourseCode);

            if (item == null)
            {
                return HttpNotFound();
            }
            var model = new StudentsSurveyModel()
            {
                SurveyCourse = item,
                Assistants = assistants,
                SurveyId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Survey(StudentsSurveyModel model)
        {
            var dt = DateTime.Now;
            int semester = 0, year = 0;
            Functions.GetLastSemester(dt, out semester, out year);
            var item = uiw.Repository<SurveyCours>()
                .GetByIdAndStudentIdRefactored(model.SurveyId ?? 0, Convert.ToInt32(User.Identity.GetUserId()),
                semester, year);

            if (item == null)
            {
                return HttpNotFound();
            }

            try
            {
                if (model.UserAnswers != null)
                {
                    foreach (var a in model.UserAnswers)
                    {
                        var answer = new UserAnswer
                        {
                            AnswerId = a,
                            SurveyCourseId = item.Id,
                            RoleId = 1
                        };
                        uiw.Repository<UserAnswer>().Insert(answer);
                    }
                }

                if (model.UserAnswersString != null)
                {
                    foreach (var a in model.UserAnswersString)
                    {
                        var answer = new UserAnswer
                        {
                            AnswerId = a.Id,
                            SurveyCourseId = item.Id,
                            Value = a.Text,
                            RoleId = 1
                        };
                        uiw.Repository<UserAnswer>().Insert(answer);
                    }
                }

                if (model.UserAnswersString != null || model.UserAnswers != null)
                {
                    var us = new UserSurvey
                    {
                        UserId = Convert.ToInt32(User.Identity.GetUserId()),
                        SurveyCourseId = item.Id,
                        DateOfEvent=DateTime.Now
                    };
                    uiw.Repository<UserSurvey>().Insert(us);
                }

                //Assistant
                if ((model.UserAnswersSecondUser != null && model.TeacherId != null) || model.UserAnswersString != null)
                {
                    var sitem = uiw.Repository<SurveyCours>().Get(filter: x => x.SemesterCours.CourseCode == item.SemesterCours.CourseCode &&
                        x.SemesterCours.UserId == model.TeacherId &&
                        (x.SemesterCours.ProgrammeCode == item.SemesterCours.RevisionName || 1 == 1) &&
                        (x.SemesterCours.ProgrammeCode == item.SemesterCours.ProgrammeCode || 1 == 1)).FirstOrDefault();

                    if (sitem != null)
                    {
                        if (model.UserAnswersSecondUser != null)
                        {
                            foreach (var a in model.UserAnswersSecondUser)
                            {
                                var answer = new UserAnswer
                                {
                                    AnswerId = a,
                                    SurveyCourseId = sitem.Id,
                                    RoleId = 4
                                };
                                uiw.Repository<UserAnswer>().Insert(answer);
                            }
                        }

                        if (model.UserAnswersString != null)
                        {
                            foreach (var a in model.UserAnswersString)
                            {
                                var answer = new UserAnswer
                                {
                                    AnswerId = a.Id,
                                    SurveyCourseId = sitem.Id,
                                    Value = a.Text,
                                    RoleId = 4
                                };
                                uiw.Repository<UserAnswer>().Insert(answer);
                            }
                        }

                        if (model.UserAnswersSecondUser != null || model.UserAnswersString != null)
                        {
                            var us = new UserSurvey
                            {
                                UserId = Convert.ToInt32(User.Identity.GetUserId()),
                                SurveyCourseId = sitem.Id,
                                DateOfEvent = DateTime.Now
                            };
                            uiw.Repository<UserSurvey>().Insert(us);
                        }
                    }
                }

                uiw.Save();
            }
            catch { }

            return RedirectToAction("Index");
        }
    }
}