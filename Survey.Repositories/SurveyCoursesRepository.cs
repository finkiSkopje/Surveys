using Survey.DAL;
using Survey.Domain;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Repositories
{
    public static class SurveyCoursesRepository
    {
        public static IEnumerable<SurveyCoursesEntity> GetByStudentId(
            this IRepository<SurveyCours> surveyCoursesRepository,
            int studentId, int year, int semester)
        {
            var dt = DateTime.Now;
            return surveyCoursesRepository.GetQuery()
                .Where(x => x.SemesterCours.StudentCourses
                    .Count(y => y.UserId == studentId && y.Semester == semester && y.Year == year)
                    > 0 &&
                    x.UserSurveys.Count(y => y.UserId == studentId) == 0
                    && DbFunctions.TruncateTime(x.DateFrom) <= dt.Date && DbFunctions.TruncateTime(x.DateTo) >= dt.Date
                       && x.Semester == semester && x.Year == year)
                .OrderBy(x => x.SemesterCours.CourseName)
                .Select(x => new SurveyCoursesEntity
                {
                    Id = x.Id,
                    Name = x.Survey.Name,
                    IsRequired = x.IsRequired,
                    CourseName = x.SemesterCours.CourseName,
                    TeacherNameSurname = x.SemesterCours.User.Name + " " + x.SemesterCours.User.Surname
                })
                .ToList();
        }

        public static SurveyCours GetByIdAndStudentIdRefactored(
            this IRepository<SurveyCours> surveyCoursesRepository,
            int surveyCourseId, int studentId, int semester, int year)
        {
            var dt = DateTime.Now;
            return surveyCoursesRepository.GetQuery()
                .Where(x => x.SemesterCours.StudentCourses
                    .Count(y => y.UserId == studentId && y.Semester == semester && y.Year == year)
                    > 0 &&
                    x.UserSurveys.Count(y => y.UserId == studentId) == 0 &&
                        x.Id == surveyCourseId &&
                        x.DateFrom <= dt.Date && x.DateTo >= dt.Date
                        && x.Semester == semester && x.Year == year)
                .FirstOrDefault();
        }
    }
}
